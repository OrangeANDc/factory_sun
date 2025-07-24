using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace CANDeviceUpgrade.Protocol
{
    public class Device
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DeviceType"></param>
        /// <param name="DeviceInd"></param>
        /// <param name="Reserved"></param>
        /// <returns></returns>
        /*------------兼容ZLG的函数描述---------------------------------*/
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_OpenDevice(UInt32 DeviceType, UInt32 DeviceInd, UInt32 Reserved);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_CloseDevice(UInt32 DeviceType, UInt32 DeviceInd);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_InitCAN(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_INIT_CONFIG pInitConfig);

        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_ReadBoardInfo(UInt32 DeviceType, UInt32 DeviceInd, ref VCI_BOARD_INFO pInfo);

        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_GetReceiveNum(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_ClearBuffer(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd);

        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_StartCAN(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_ResetCAN(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd);

        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_Transmit(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_CAN_OBJ pSend, UInt32 Len);

        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_Receive(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_CAN_OBJ pReceive, UInt32 Len, Int32 WaitTime);

        /*------------其他函数描述---------------------------------*/

        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_ConnectDevice(UInt32 DevType, UInt32 DevIndex);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_UsbDeviceReset(UInt32 DevType, UInt32 DevIndex, UInt32 Reserved);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_FindUsbDevice2(ref VCI_BOARD_INFO pInfo);
        /*------------函数描述结束---------------------------------*/


        private UInt32 _deviceType = 4; //USBCAN2 USBCAN-2A USBCAN-2C CANalyst-II系列 MiniPCIe-CAN 汽车OBDII专用版     
        private UInt32 _deviceIndex = 0;  //Can 索引号  第一个设备
        private VCI_CAN_OBJ[] m_recobj = new VCI_CAN_OBJ[1000];
        public UInt32 _canChannel { get; set; } = 0;  //Can 通道号

        private Task _task;
        private ManualResetEvent _resetEvent = new ManualResetEvent(true);
        public Device()
        {
            _task = new Task(()=> { 
               ReceiveDataTask();
            });
            _task.Start();

            _resetEvent.Reset();
        }

        public bool Disconnect()
        {
            if (VCI_CloseDevice(_deviceType, _deviceIndex) != 1)
            {
                MessageBox.Show("关闭设备失败", "错误",
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            _resetEvent.Reset();
            return true;
        }

        public bool Connect()
        {
            try
            {
                if (VCI_OpenDevice(_deviceType, _deviceIndex, 0) != 1)
                {
                    MessageBox.Show("打开设备失败,请检查设备类型和设备索引号是否正确", "错误",
                            MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return false;
                }

                VCI_INIT_CONFIG config = new VCI_INIT_CONFIG();
                config.AccCode = 0xa4180000;
                config.AccMask = 0xffffffff; // accCode 和 accMask这样设置代表所有帧都能接收,见文档说明


                config.Timing0 = 0;
                config.Timing1 = 0x1c;       // 波特率500kbps  参考文档设置
                config.Filter = 3;           // 1:接收所有类型  2:只接受标准帧 3:只接受扩展帧 
                config.Mode = 0;

                if (VCI_InitCAN(_deviceType, _deviceIndex, _canChannel, ref config) != 1)
                {
                    Variable._delegateService.AddInfo("初始化Can设备失败");
                    return false;
                }
                Variable._delegateService.AddInfo("初始化Can设备成功");


                return true;
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public void StartCAN()
        {
            if (VCI_StartCAN(_deviceType, _deviceIndex, _canChannel) != 1)
            {
                Variable._delegateService.AddInfo("打开Can设备失败");
                return;
            }
            Variable._delegateService.AddInfo("打开Can设备成功");
            _resetEvent.Set();
        }

        public void ResetCAN()
        {
            if (VCI_ResetCAN(_deviceType, _deviceIndex, _canChannel) != 1)
            {
                Variable._delegateService.AddInfo("重置Can设备失败");
                return;
            }
            Variable._delegateService.AddInfo("重置Can设备成功");
            _resetEvent.Set();
        }

        public bool SendFileCheck(string filecheck)
        {
            byte[] data = Encoding.ASCII.GetBytes(filecheck);
            uint canId = 0x14831201;

            if(!SendData(data, canId))
            {
                return false;
            }

            return true;
        }

        unsafe public bool SendUpgradeJump(ushort packNum)
        {
            uint upgradejumpId = 0x14831201;
            byte[] upgradejumpDatas = new byte[8] { 0xAA, 0x55, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00 };
            Array.Copy(BitConverter.GetBytes(packNum).Reverse().ToArray(), 0, upgradejumpDatas, 4, 2);

            _receiveStruct = null;
            if (!SendData(upgradejumpDatas, upgradejumpId))
            {
                return false;
            }

            int tickCountLast = Environment.TickCount;
            while (_receiveStruct == null)
            {
                Thread.Sleep(100);
                if (Environment.TickCount - tickCountLast > 2000)
                {
                    Log.Debug("数据接收超时");
                    return false;

                }
            }

            foreach (var data in _receiveStruct)
            {
                if (data.ID == 0x14830112)
                {
                    Log.Debug("0x" + data.ID.ToString("x2") + " recv :" + BytePointerToSting(data));
                    if (data.Data[2] == 0x01)
                    {                       
                        return true;
                    }
                    else
                        return false;
                }
            }

            return false;
        }

        unsafe public bool SendUpgradeClean()
        {
            uint cleanId = 0x14841201;
            byte[] cleanDatas = new byte[8] { 0xAA, 0x55, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00 };

            _receiveStruct = null;
            if (!SendData(cleanDatas, cleanId))
            {
                return false;
            }

            int tickCountLast = Environment.TickCount;
            while (_receiveStruct == null)
            {
                Thread.Sleep(100);
                if (Environment.TickCount - tickCountLast > 2000)
                {
                    Log.Debug("数据接收超时");
                    return false;

                }
            }

            foreach (var data in _receiveStruct)
            {
                if (data.ID == 0x14840112)
                {
                    Log.Debug("0x" + data.ID.ToString("x2") + " recv :" +  BytePointerToSting(data));
                    if (data.Data[2] == 0x01)
                    {
                        return true;
                    }
                    else
                        return false;
                }
            }

            return false;
        }

        unsafe public bool UpgradeOnePack(byte[] firstFrame, byte[] sendDatas)
        {
            _receiveStruct = null;
            uint upgradeId = 0x14861201;

            //发送首帧
            if (!SendData(firstFrame, upgradeId))
                return false;

            //发送数据帧
            if (!SendUpgradeData(sendDatas, upgradeId))
            {

                return false;
            }

            // 每包等待500ms
            Thread.Sleep(100);
            int tickCountLast = Environment.TickCount;
            while (_receiveStruct == null)
            {
                Thread.Sleep(100);
                if (Environment.TickCount - tickCountLast > 2000)
                {
                    Log.Debug("数据接收超时");
                    return false;

                }
            }

            //接收校验
            foreach (var data in _receiveStruct)
            {
                if (data.ID == 0x14860112)
                {
                    Log.Debug("0x" + data.ID.ToString("x2") + " recv :" + BytePointerToSting(data));
                    if (data.Data[2] == 0x01)
                    {
                        break;
                    }
                    else
                        return false;
                }
            }
            Log.Debug(Environment.NewLine);
            return true;
        }

        // 128长度的数据包 连续发送
        private bool SendUpgradeData(byte[] datas , uint id)
        {
            if (datas.Length != 128)
            {
                Log.Error("数据包长度不等于128");
                return false;
            }

            byte[] sendDatas = new byte[8];
            for (int index = 0; index < 16; index++)
            {
                Array.Copy(datas, index * 8, sendDatas, 0, 8);
                if (!SendData(sendDatas, id))
                {
                    return false;
                }
            }

            return true;

        }


        unsafe public bool SendUpgradeFinish()
        {
            uint upgradeFinishId = 0x14871201;
            byte[] upgradeFinish = new byte[8];
            upgradeFinish[0] = 0xAA;
            upgradeFinish[1] = 0x55;
            upgradeFinish[2] = 0x45;
            upgradeFinish[3] = 0x4E;
            upgradeFinish[4] = 0x44;
            upgradeFinish[5] = (byte)(upgradeFinish[0] + upgradeFinish[1] + upgradeFinish[2] + upgradeFinish[3] + upgradeFinish[4]);

            // 接收清空
            _receiveStruct = null;
            if (!SendData(upgradeFinish, upgradeFinishId))
            {
                return false;
            }

            int tickCountLast = Environment.TickCount;
            while (_receiveStruct == null)
            {
                Thread.Sleep(100);
                if (Environment.TickCount - tickCountLast > 2000)
                {
                    Log.Debug("数据接收超时");
                    return false;

                }
            }

            foreach (var data in _receiveStruct)
            {
                if (data.ID == 0x14870112)
                {
                    Log.Debug("0x" + data.ID.ToString("x2") + " recv :" + BytePointerToSting(data));
                    if (data.Data[2] == 0x01)
                    {
                        return true;
                    }
                    else
                        return false;
                }
            }

            return false;
        }



        unsafe private bool SendData(byte[] data,UInt32 canId, byte idType = 1, byte frameType = 0)
        {
            VCI_CAN_OBJ sendobj = new VCI_CAN_OBJ();
            sendobj.RemoteFlag = frameType;
            sendobj.ExternFlag = idType;
            sendobj.ID = canId;
            sendobj.DataLen = 8;

            if (data.Length != 8)
            {
                MessageBox.Show("发送数据长度错误!");
                    return false;
            }

            for (int i = 0; i < 8; i++)
            {
                sendobj.Data[i] = data[i];
            }

            if (VCI_Transmit(_deviceType, _deviceIndex, _canChannel, ref sendobj, 1) != 1)
            {
                MessageBox.Show("发送失败", "错误",
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            Log.Debug("0x" + sendobj.ID.ToString("x2") + " send :" +  BytePointerToSting(sendobj));
            return true;
        }

        private VCI_CAN_OBJ[] _receiveStruct;
        private object _dataLocker = new object();
        unsafe private void ReceiveDataTask()
        {
            while (true)
            {
                _resetEvent.WaitOne();

               
                var res = VCI_Receive(_deviceType, _deviceIndex, _canChannel, ref m_recobj[0], 1000, 100);
                if (res == 0xFFFFFFFF || res == 0) res = 0;//当设备未初始化时，返回0xFFFFFFFF，不进行列表显示。

                if (res > 0)
                {
                    lock (_dataLocker)
                    {
                        _receiveStruct = m_recobj;
                    }
                }

                Task.Delay(100);
            }
        }

        unsafe private string BytePointerToSting(VCI_CAN_OBJ obj)
        {
            string msg = "";
            for (int i = 0; i < obj.DataLen; i++)
            { 
                msg += (obj.Data[i].ToString("x2") + " ");
            }
            return msg;
        }


    }

    

    //1.ZLGCAN系列接口卡信息的数据类型。
    unsafe public struct VCI_BOARD_INFO//使用不安全代码
    {
        public UInt16 hw_Version;
        public UInt16 fw_Version;
        public UInt16 dr_Version;
        public UInt16 in_Version;
        public UInt16 irq_Num;
        public byte can_Num;

        public fixed byte str_Serial_Num[20];
        public fixed byte str_hw_Type[40];
        public fixed byte Reserved[8];
    }

    /////////////////////////////////////////////////////
    //2.定义CAN信息帧的数据类型。
    unsafe public struct VCI_CAN_OBJ  //使用不安全代码
    {
        public uint ID;
        public uint TimeStamp;        //时间标识
        public byte TimeFlag;         //是否使用时间标识
        public byte SendType;         //发送标志。保留，未用
        public byte RemoteFlag;       //是否是远程帧
        public byte ExternFlag;       //是否是扩展帧
        public byte DataLen;          //数据长度
        public fixed byte Data[8];    //数据
        public fixed byte Reserved[3];//保留位

    }

    //3.定义初始化CAN的数据类型
    public struct VCI_INIT_CONFIG
    {
        public UInt32 AccCode;
        public UInt32 AccMask;
        public UInt32 Reserved;
        public byte Filter;   //0或1接收所有帧。2标准帧滤波，3是扩展帧滤波。
        public byte Timing0;  //波特率参数，具体配置，请查看二次开发库函数说明书。
        public byte Timing1;
        public byte Mode;     //模式，0表示正常模式，1表示只听模式,2自测模式
    }

    /*------------其他数据结构描述---------------------------------*/
    //4.USB-CAN总线适配器板卡信息的数据类型1，该类型为VCI_FindUsbDevice函数的返回参数。
    public struct VCI_BOARD_INFO1
    {
        public UInt16 hw_Version;
        public UInt16 fw_Version;
        public UInt16 dr_Version;
        public UInt16 in_Version;
        public UInt16 irq_Num;
        public byte can_Num;
        public byte Reserved;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)] public byte[] str_Serial_Num;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] str_hw_Type;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] str_Usb_Serial;
    }

    /*------------数据结构描述完成---------------------------------*/
}
