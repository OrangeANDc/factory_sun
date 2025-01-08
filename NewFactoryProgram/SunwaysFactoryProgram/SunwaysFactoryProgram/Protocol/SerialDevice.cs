using Prism.Events;
using SunwaysFactoryProgram.DBModel;
using SunwaysFactoryProgram.Messenager;
using SunwaysFactoryProgram.StaticSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SunwaysFactoryProgram.Protocol
{
    internal class SerialDevice : SerialBase
    {
        private IEventAggregator _eventAggregator;
        public SerialDevice(IEventAggregator ea)
        {
            _eventAggregator = ea;
        }

        public void SendInfo(string msg)
        {
            _eventAggregator.GetEvent<RecordLogEvent>().Publish(msg);
            Variable._funcTestResult.ErrorMsg = msg;
            Variable._packTestResult.ErrorMsg = msg;
        }
    
        // 读取固件版本
        public bool ReadFirmwareVersion()
        {
            string msg = string.Empty;
            if (!GetFunc(GetCommandType.获取软件版本号, ref msg))
            {
                SendInfo("读取逆变器软件版本失败!");
                return false;
            }
            else
            {
                Variable._funcTestResult.Version = msg.Split(",")[0];
                Variable._funcTestResult.InternalVersion = msg.Split(",")[1];
                Variable._packTestResult.Version = msg.Split(",")[0];
                Variable._packTestResult.InternalVersion = msg.Split(",")[1];
                SendInfo("逆变器固件版本:" + msg.Split(",")[0]);
                SendInfo("逆变器内部版本:" + msg.Split(",")[1]);
                return true;
            }
        }


        //第二域名写入
        public bool WriteSecondDomain(bool isEtek)
        {
            string str = "";
            if (isEtek)
            {
                str = "5743,iot.etek-power.com";
            }
            else
            {
                str = "5743,iot.watt-sonic.com";
            }
            byte[] address = new byte[] { 0x4e, 0x4e };
            byte[] data = new byte[60];

            byte[] domain = Encoding.Default.GetBytes(str);
            Array.Copy(domain, 0, data, 0, domain.Length);
            if (WriteMulData(address, data))
            {
                if (isEtek)
                    SendInfo("ETEK第二域名写入成功!");
                else
                    SendInfo("第二域名写入成功!");
                return true;
            }
            else
            {
                if (isEtek)
                    SendInfo("ETEK第二域名写入失败!");
                else
                    SendInfo("第二域名写入失败!");
                return false;
            }

        }

        public bool ReadSN()
        {
            string result = "";
            if (!GetFunc(GetCommandType.获取序列号, ref result))
            {
                SendInfo("读取SN失败!");
                return false;
            }
            else
            {
                SendInfo("逆变器SN:" + result);
                return true;
            }
        }

        public bool WriteAndReadSN(string sn)
        {

            byte[] address = new byte[] { 0x13, 0x88 };
            //储能机是10000
            //byte[] address = new byte[] { 0x27, 0x10 };
            var bytes = Encoding.UTF8.GetBytes("0000000000000000");
            if (!WriteMulData(address, bytes))
            {
                SendInfo("序列号00写入失败!");
            }

            bytes = Encoding.UTF8.GetBytes(sn);
            if (!WriteMulData(address, bytes))
            {
                SendInfo("序列号" + sn + "写入失败!");
                return false;
            }
            else
            {
                SendInfo("序列号" + sn + "写入成功!");
                Thread.Sleep(5000);
                string result = "";
                if (!GetFunc(GetCommandType.获取序列号, ref result))
                {
                    SendInfo("读取SN失败!");
                    return false;
                }
                else
                {
                    SendInfo("逆变器SN:" + result);
                    if (result != sn)
                    {
                        SendInfo("逆变器SN与写入的SN不匹配!");
                        return false;
                    }
                }

            }
            return true;
        }

        public bool SetRTC()
        {
            //  校验时间 判断是否需要返修
            byte[] revTime = new byte[20];
            if (GetFunc(GetCommandType.获取RTC, ref revTime))
            {
                string timeStr = string.Format("{0:0000}-{1:00}-{2:00} {3:00}:{4:00}:{5:00}",
                    Convert.ToInt32(revTime[0] + 2000), Convert.ToInt32(revTime[1]), Convert.ToInt32(revTime[2]),
                    Convert.ToInt32(revTime[3]), Convert.ToInt32(revTime[4]), Convert.ToInt32(revTime[5]));
                SendInfo("当前逆变器RTC时间:" + timeStr);

                DateTime read = Convert.ToDateTime(timeStr);
                TimeSpan ts = DateTime.Now - read;
                if (Math.Abs(ts.TotalMinutes) >= 30)
                {
                    SendInfo("时间对比误差超出30分钟,请检修!");
                    return false;
                }
                SendInfo("时间对比误差符合范围!");
                
            }
            else
            {
                SendInfo("读取逆变器RTC时间失败!");
                return false;
            }

            if(!ResetTime())
                return false;

            return true;
        }

        public bool ResetTime()
        {
            byte[] address = new byte[] { 0x4e, 0x20 };

            byte year = Convert.ToByte(DateTime.Now.Year.ToString().Remove(0, 2));
            byte month = Convert.ToByte(DateTime.Now.Month.ToString());
            byte day = Convert.ToByte(DateTime.Now.Day.ToString());
            byte hour = Convert.ToByte(DateTime.Now.Hour.ToString());
            byte minute = Convert.ToByte(DateTime.Now.Minute.ToString());
            byte second = Convert.ToByte(DateTime.Now.Second.ToString());

            byte[] datas = new byte[6];
            datas[0] = year;
            datas[1] = month;
            datas[2] = day;
            datas[3] = hour;
            datas[4] = minute;
            datas[5] = second;

            if (WriteMulData(address, datas))
            {
                SendInfo("同步时间成功!");
                byte[] revData = new byte[20];
                if (GetFunc(GetCommandType.获取RTC, ref revData))
                {
                    string timeStr = string.Format("{0:0000}-{1:00}-{2:00} {3:00}:{4:00}:{5:00}",
                        Convert.ToInt32(revData[0] + 2000), Convert.ToInt32(revData[1]), Convert.ToInt32(revData[2]),
                        Convert.ToInt32(revData[3]), Convert.ToInt32(revData[4]), Convert.ToInt32(revData[5]));
                    SendInfo("当前逆变器RTC时间:" + timeStr);

                    DateTime read = Convert.ToDateTime(timeStr);
                    TimeSpan ts = DateTime.Now - read;
                    if (ts.TotalSeconds > 100)
                    {
                        SendInfo("时间对比失败!");
                        return false;
                    }
                    SendInfo("时间对比成功!");
                    return true;
                }
                else
                {
                    SendInfo("读取逆变器RTC时间失败!");
                    return false;
                }
            }
            else
            {
                SendInfo("同步时间失败!");
                return false;
            }
        }

        public bool CalibrateVol_165()
        {
            byte[] address = { 0x14, 0x03 };
            byte[] data = new byte[] { 0x00, 0x01 };

            if (!WriteSingleData(address, data))
            {
                Variable._funcTestResult.VolCalibrate = "FAIL";
                SendInfo("1.65V电压校正失败!");
                return false;
            }
            else
            {
                Variable._funcTestResult.VolCalibrate = "PASS";
                SendInfo("1.65V电压校正成功!");
                return true;
            }
        }

        public bool SetWarehouseSafety()
        {
            byte[] address = { 0x61, 0xA8 };
            byte[] data = new byte[] { 0x00, 0x00 };

            if (!WriteSingleData(address, data))
            {
                SendInfo("Warehouse安规设置失败!");
                return false;
            }
            else
            {
                SendInfo("Warehouse安规设置成功!");
                return true;
            }
        }

        public bool SetSpsReset()
        {
            byte[] address = { 0x61, 0xb1};
            byte[] data = new byte[] { 0x00, 0x01 };

            if (!WriteSingleData(address, data))
            {
                SendInfo("复位指令发送失败!");
                return false;
            }
            else
            {
                SendInfo("复位指令发送成功!");
                return true;
            }
        }

        public bool SetArcbSwitch(bool isOpen)
        {
            byte[] address = { 0x62, 0x0c };
            byte[] data = new byte[] { 0x00, 0x00 };

            if (isOpen)
            {
                data = new byte[] { 0x00, 0x01 };
            }

            if (!WriteSingleData(address, data))
            {
                if(isOpen)
                    SendInfo("防逆流打开失败!");
                else
                    SendInfo("防逆流关闭失败!");
                return false;
            }
            else
            {
                if (isOpen)
                    SendInfo("防逆流打开成功!");
                else
                    SendInfo("防逆流关闭成功!");
                return true;
            }
        }

        public bool ReadCtRatio()
        {
            byte[] address = { 0x61, 0xb2 };
            byte[] result = new byte[] { };
            if (!GetFunc(address, 1, ref result))
            {
                SendInfo("读CT变比失败!");
                return false;
            }
            else
            {
                int value = (result[0] << 8) + result[1];
                SendInfo($"CT变比为: {value}:1");
                return true;
            }
        }

        public bool SetCtRatio()
        {
            ushort ctRatio = 2000;
            byte[] address = { 0x61, 0xb2 };
            byte[] data = BitConverter.GetBytes(ctRatio).Reverse().ToArray();

            if (!WriteSingleData(address, data))
            {
                SendInfo("CT变比设置失败!");
                return false;
            }
            else
            {
                SendInfo("CT变比设置成功!");
                return true;
            }
        }

        public bool SetPowerPercent(ushort value)
        {
            byte[] address = { 0x62, 0x0f };
            byte[] data = BitConverter.GetBytes(value).Reverse().ToArray();

            if (!WriteSingleData(address, data))
            {
                SendInfo("功率限值设置失败!");
                return false;
            }
            else
            {
                SendInfo("功率限值设置成功!");
                return true;
            }
        }

        public bool SetAustriaSafety()
        {
            byte[] address = { 0x61, 0xA8 };
            byte[] data = { 0x00, 0x0d};

            if (!WriteSingleData(address, data))
            {
                SendInfo("澳洲安规设置失败!");
                return false;
            }
            else
            {
                SendInfo("澳洲安规设置成功!");
                return true;
            }
        }

        public bool SetBurnInSafetyAndCheck()
        {
            byte[] address = { 0x61, 0xA8 };
            byte[] data = { 0x00, 0x01 };
            byte[] readAddress = { 0x27, 0x78 };

            if (!WriteSingleData(address, data))
            {
                SendInfo("老化安规设置失败!");
                return false;
            }
            else
            {
                SendInfo("老化安规设置成功!");
                Thread.Sleep(5000);

                byte[] result = new byte[] { };
                if (!GetFunc(readAddress, 1, ref result))
                {
                    SendInfo("读取安规失败!");
                    return false;
                }
                else if (!Enumerable.SequenceEqual(result, data))
                {
                    SendInfo("当前安规不是老化安规,请重新设置老化安规!");
                    return false;
                }
                else
                {
                    Variable._funcTestResult.BurnInSafetyCode = 1;
                    SendInfo("老化安规确认成功!");
                    return true;
                }               
            }
        }

        public bool ClearFactoryData()
        {
            byte[] address = new byte[] { 0x13, 0x98 };
            byte[] data = new byte[] { 0x00, 0x01 };


            if (!WriteSingleData(address, data))
            {
                Variable._packTestResult.ClearOK = "FAIL";
                SendInfo("数据清零失败!");
                return false;
            }
            else
            {
                Variable._packTestResult.ClearOK = "PASS";
                SendInfo("数据清零成功!");
                return true;
            }
        }

        //电池型号配置
        public bool SetBatModel(ushort value)
        {
            byte[] address = { 0xcd, 0x14 };

            if (!WriteSingleData(address, BitConverter.GetBytes(value).Reverse().ToArray()))
            {
                SendInfo("电池型号设置失败!");
                return false;
            }
            else
            {
                SendInfo("电池型号设置成功!");
                return true;
            }
        }

        //电池协议配置
        public bool SetBatProtocol(ushort value)
        {
            byte[] address = { 0xcd, 0x15 };

            if (!WriteSingleData(address, BitConverter.GetBytes(value).Reverse().ToArray()))
            {
                SendInfo("电池协议设置失败!");
                return false;
            }
            else
            {
                SendInfo("电池协议设置成功!");
                return true;
            }
        }

        public bool SetSafety(string safety, ushort value)
        {
            byte[] address = { 0x61, 0xA8 };

            if (WriteSingleData(address, BitConverter.GetBytes(value).Reverse().ToArray()))
            {
                Variable._packTestResult.SafetyCode = value;
                Variable._packTestResult.SafetyName = safety;
                SendInfo(string.Format("安规代码:{0}({1})", safety, value));
                SendInfo("出厂安规设置成功!");
                return true;
            }
            else
            {
                SendInfo("出厂安规设置失败!");
                return false;
            }
        }

        public bool SetLanguage(string lan, ushort value)
        {
            byte[] address = { 0x4e, 0x2b };

            if (WriteSingleData(address, BitConverter.GetBytes(value).Reverse().ToArray()))
            {
                Variable._packTestResult.Language = lan;
                SendInfo(string.Format("逆变器语言代码:{0}({1})", lan, value));
                SendInfo("逆变器语言设置成功!");
                return true;
            }
            else
            {
                SendInfo("逆变器语言设置失败!");
                return false;
            }
        }

        public bool SetPowerE2Limit(ushort value)
        {
            byte[] address = { 0x61, 0xb3 };

            if (!WriteSingleData(address, BitConverter.GetBytes(value).Reverse().ToArray()))
            {
                SendInfo("设置有功E2限载失败!");
                return false;
            }
            else
            {
                Variable._packTestResult.E2Limit = value;
                SendInfo("设置有功E2限载成功!");
                return true;
            }
        }

        public bool SetWorkMode(ushort value, string mode)
        {
            byte[] address = new byte[] { 0x3A, 0xa5 };

            if (!WriteSingleData(address, BitConverter.GetBytes(value).Reverse().ToArray()))
            {
                SendInfo($"设置工作模式-{mode}失败!");
                return false;
            }
            else
            {
                SendInfo($"设置工作模式-{mode}成功!");
                return true;
            }
        }

        public bool SetOffLineMode(ushort value, string mode)
        {
            byte[] address = new byte[] { 0x3A, 0xc2 };

            if (!WriteSingleData(address, BitConverter.GetBytes(value).Reverse().ToArray()))
            {
                SendInfo($"设置工作模式-{mode}失败!");
                return false;
            }
            else
            {
                SendInfo($"设置工作模式-{mode}成功!");
                return true;
            }
        }

        public bool SetForceMode(ushort value, string mode)
        {
            byte[] address = new byte[] { 0x3A, 0xc3 };

            if (!WriteSingleData(address, BitConverter.GetBytes(value).Reverse().ToArray()))
            {
                SendInfo($"设置工作模式-{mode}失败!");
                return false;
            }
            else
            {
                SendInfo($"设置工作模式-{mode}成功!");
                return true;
            }
        }

        public bool SetBurnMode(ushort value)
        {
            byte[] address = new byte[] { 0xcd, 0x17 };

            if (!WriteSingleData(address, BitConverter.GetBytes(value).Reverse().ToArray()))
            {
                SendInfo("设置老化模式失败!");
                return false;
            }
            else
            {
                SendInfo("设置老化模式成功!");
                return true;
            }
        }

        public bool SetBatteryID(ushort value , string name = "")
        {
            byte[] address = new byte[] { 0x3a, 0xa6 };

            if (!WriteSingleData(address, BitConverter.GetBytes(value).Reverse().ToArray()))
            {
                SendInfo($"设置Battery ID-{name}失败!");
                return false;
            }
            else
            {
                SendInfo($"设置Battery ID-{name}成功!");
                return true;
            }
        }

        public bool SetBatteryType(ushort value,string type = "")
        {
            byte[] address = new byte[] { 0x3a, 0xc1 };

            if (!WriteSingleData(address, BitConverter.GetBytes(value).Reverse().ToArray()))
            {
                SendInfo($"设置Battery Type-{type}失败!");
                return false;
            }
            else
            {
                SendInfo($"设置Battery Type-{type}成功!");
                return true;
            }
        }

        public bool ResetStatus()
        {
            //byte[] address1 = new byte[] { 0xcd, 0x17 };
            byte[] address2 = new byte[] { 0x13, 0x98 };

          /*  if (!WriteSingleData(address1, BitConverter.GetBytes(0).Reverse().ToArray()))
            {
                SendInfo("恢复老化模式失败!");
                return false;
            }
            else
            {
                SendInfo("恢复老化模式成功!");
            }*/

            if (!WriteSingleData(address2, BitConverter.GetBytes((ushort)1).Reverse().ToArray()))
            {
                SendInfo("清零Total失败!");
                return false;
            }
            else
            {
                SendInfo("清零Total成功!");
                return true;
            }
        }

        public bool SetPriority(ushort value,string name)
        {
            // 52506
            byte[] address = new byte[] { 0xcd, 0x16 };

            if (!WriteSingleData(address, BitConverter.GetBytes(value).Reverse().ToArray()))
            {
                SendInfo($"设置Priority-{name}失败!");
                return false;
            }
            else
            {
                SendInfo($"设置Priority-{name}成功!");
                return true;
            }
        }

        public bool SetCTMode(ushort value,string name)
        {
            // 15012
            byte[] address = new byte[] { 0x3a, 0xa4 };

            if (!WriteSingleData(address, BitConverter.GetBytes(value).Reverse().ToArray()))
            {
                SendInfo($"设置CT Mode-{name}失败!");
                return false;
            }
            else
            {
                SendInfo($"设置CT Mode-{name}成功!");
                return true;
            }
        }

        public bool SetCT(ushort value, string name)
        {
            // 25010
            byte[] address = new byte[] { 0x61, 0xb2 };

            if (!WriteSingleData(address, BitConverter.GetBytes(value).Reverse().ToArray()))
            {
                SendInfo($"设置CT-{name}失败!");
                return false;
            }
            else
            {
                SendInfo($"设置CT-{name}成功!");
                return true;
            }
        }

        public bool SetOffGridSW(ushort value)
        {
            // 15015
            byte[] address = new byte[] { 0x3a, 0xa7 };
            string item = (value == 0 ? " OFF" : " ON");

            if (!WriteSingleData(address, BitConverter.GetBytes(value).Reverse().ToArray()))
            {
                SendInfo($"设置OffGrid SW{item}失败!");
                return false;
            }
            else
            {
                SendInfo($"设置OffGrid SW{item}成功!");
                return true;
            }
        }

        public bool SetOnOffGridAllow(ushort value)
        {
            // 52504
            byte[] address = new byte[] { 0xcd, 0x18 };
            string item = (value == 0 ? " OFF" : " ON");

            if (!WriteSingleData(address, BitConverter.GetBytes(value).Reverse().ToArray()))
            {
                SendInfo($"设置OnOffGrid Allow{item}失败!");
                return false;
            }
            else
            {
                SendInfo($"设置OnOffGrid Allow{item}成功!");
                return true;
            }
        }
    }
}
