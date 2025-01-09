using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using SystemTool.Protocol;
using SystemTool.StaticSource;

namespace SystemTool.Views
{
    /// <summary>
    /// BaseControlView.xaml 的交互逻辑
    /// </summary>
    public partial class BaseControlView : UserControl
    {
        private byte[] _baseInf1;
        private byte[] _baseInf2;
        private byte[] _baseInf3;
        private SerialDevice _serialDevice;
        private Task _task1;
        private Task _task2;
        private bool _isTaskRun = false;
        public BaseControlView(IContainerProvider containerProvider)
        {
            InitializeComponent();
            _serialDevice = containerProvider.Resolve<SerialDevice>();
            _baseInf1 = new byte[100];
            _baseInf2 = new byte[40];
            _baseInf3 = new byte[200];
            
        }

        public void ResetStatus()
        {
            _isTaskRun = false;
            btnRead.IsEnabled = true;
            btnStop.IsEnabled = false;
        }

        private void ReadAllData_Click(object sender, RoutedEventArgs e)
        { 
            if (!_serialDevice.GetStatus())
            {
                MessageBox.Show("请连接串口");
                return;
            }
            _isTaskRun = true;
            _task1 = new Task(() => ReadData());
            _task2 = new Task(() => Refresh());
            _task1.Start();
            _task2.Start();
            btnRead.IsEnabled = false;
            btnStop.IsEnabled = true;
        }

        private void StopRead_Click(object sender, RoutedEventArgs e)
        {
            _isTaskRun = false;
            btnRead.IsEnabled = true;
            btnStop.IsEnabled = false;
        }

        private void ReadData()
        {
            while (_isTaskRun)
            {
                try
                {
                    if (_serialDevice.GetStatus())
                    {
                        _serialDevice.ReadData(BitConverter.GetBytes((ushort)10000).Reverse().ToArray(), 50, ref _baseInf1);
                        _serialDevice.ReadData(BitConverter.GetBytes((ushort)10100).Reverse().ToArray(), 20, ref _baseInf2);
                        //_serialDevice.ReadData(BitConverter.GetBytes((ushort)11000).Reverse().ToArray(), 100, ref _baseInf3);

                        Thread.Sleep(500);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);

                }
            }
        }

        private object _locker = new object();
        private void Refresh()
        {
            Thread.Sleep(1000);
            while (_isTaskRun)
            {
                Thread.Sleep(500);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    lock (_locker)
                    { 
                        DataMatch1();
                        DataMatch2(); 
                    }
                });
            }
        }

        private void DataMatch1()
        {
            try
            {
                byte[] sn = new byte[16];
                Array.Copy(_baseInf1, sn, 16);
                tbSn.Text = Encoding.ASCII.GetString(sn);

                //机种信息
                string flag1 = ASCIIEncoding.ASCII.GetString(sn,11,2);
                //Array.Copy(_baseInf1, 17, type, 0, 1);
                string flag2 = ASCIIEncoding.ASCII.GetString(sn, 13, 2);
                tbSInf.Text = Variable._machineType[flag1][flag2];

                byte[] data = new byte[8];
                // 输出方式
                Array.Copy(_baseInf1, 18, data, 0, 2);
                tbOutputMethod.Text = (ushort)((data[0] << 8) + data[1]) == 0 ? "三相四线制" : "三相三线制";
                data.Initialize();

                //版本号
                Array.Copy(_baseInf1, 22, data, 0, 8);
                tbHw.Text = string.Format("Ver-{0:00}.{1:00}.{2:00}.{3:00}", data[0], data[1], data[2], data[3]);
                tbSw.Text = string.Format("Ver-{0:00}.{1:00}.{2:00}.{3:00}", data[4], data[5], data[6], data[7]);
                data.Initialize();

                //通信方式
                Array.Copy(_baseInf1, 30, data, 0, 2);
                switch ((ushort)((data[0] << 8) + data[1]))
                {
                    case 1:
                        tbCMethod.Text = "RS485";
                        break;
                    case 2:
                        tbCMethod.Text = "WIFI";
                        break;
                    case 3:
                        tbCMethod.Text = "GPRS";
                        break;
                    case 4:
                        tbCMethod.Text = "LAN";
                        break;
                    case 5:
                        tbCMethod.Text = "4G";
                        break;
                    default:
                        break;
                }
                data.Initialize();

                //校验码
                byte[] checkCode = new byte[6];
                Array.Copy(_baseInf1, 92, checkCode, 0, 6);
                tbCheckCode.Text = Encoding.ASCII.GetString(checkCode);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                    return;
            }

        }

        private void DataMatch2()
        {
            byte[] RTC = new byte[6];
            Array.Copy(_baseInf2, RTC, 6);
            tbRTC.Text = "20" + Modbus.BytetoString2(RTC[0]) + "-" + Convert.ToString(RTC[1], 10) + "-" + Convert.ToString(RTC[2], 10) + " " +
                                Modbus.BytetoString2(RTC[3]) + ":" + Modbus.BytetoString2(RTC[4]) + ":" + Modbus.BytetoString2(RTC[5]);

            byte[] data = new byte[4];
            // RSSI
            Array.Copy(_baseInf2, 6, data, 0, 2);
            tbRSSI.Text = ((ushort)((data[0] << 8) + data[1])).ToString(); 
            data.Initialize();

            //安规
            Array.Copy(_baseInf2, 8, data, 0, 2);
            ushort value = (ushort)((data[0] << 8) + data[1]);
            if (Variable._safetyDic.ContainsKey(value))
            {
                tbSafety.Text = Variable._safetyDic[value];
            }
            data.Initialize();

            //工作状态
            Array.Copy(_baseInf2, 10, data, 0, 2);
            switch ((ushort)((data[0] << 8) + data[1]))
            {
                case 0: 
                    tbWorkStatus.Text = "待机";
                    break;
                case 1:
                    tbWorkStatus.Text = "自检";
                    break;
                case 2:
                    tbWorkStatus.Text = "正常并网";
                    break;
                case 3:
                    tbWorkStatus.Text = "故障";
                    break;
                case 4:
                    tbWorkStatus.Text = "FW Updating";
                    break;
                default:
                    break;
            }
            data.Initialize();

            //运行状态1
            Array.Copy(_baseInf2, 12, data, 0, 4);
            tbRunStatus1.Text = ((uint)((data[0] << 24) + (data[1] << 16) + (data[2] << 8) + data[3])).ToString();
            data.Initialize();

          /*  Array.Copy(_baseInf2, 16, data, 0, 4);
            tbRunStatus2.Text = ((uint)((data[0] << 24) + (data[1] << 16) + (data[2] << 8) + data[3])).ToString();
            data.Initialize();

            Array.Copy(_baseInf2, 20, data, 0, 4);
            tbRunStatus3.Text = ((uint)((data[0] << 24) + (data[1] << 16) + (data[2] << 8) + data[3])).ToString();
            data.Initialize();*/

            //故障
            Array.Copy(_baseInf2, 24, data, 0, 4);
            uint fault1 = ((uint)((data[0] << 24) + (data[1] << 16) + (data[2] << 8) + data[3]));
            data.Initialize();

            Array.Copy(_baseInf2, 28, data, 0, 4);
            uint fault2 = ((uint)((data[0] << 24) + (data[1] << 16) + (data[2] << 8) + data[3]));
            data.Initialize();

            tbFault1.Text = "";
            //fault1`
            if ((fault1 & 0x800000) > 0)
                tbFault1.Text += "Boost温度传感器故障" + Environment.NewLine;

            if ((fault1 & 0x400000) > 0)
                tbFault1.Text += "逆变温度传感器故障" + Environment.NewLine;

            if ((fault1 & 0x200000) > 0)
                tbFault1.Text += "Boost过温" + Environment.NewLine;

            if ((fault1 & 0x100000) > 0)
                tbFault1.Text += "设备着火" + Environment.NewLine;

            if ((fault1 & 0x80000) > 0)
                tbFault1.Text += "输出功率异常" + Environment.NewLine;

            if ((fault1 & 0x40000) > 0)
                tbFault1.Text += "直流电压检测异常" + Environment.NewLine;

            if ((fault1 & 0x20000) > 0)
                tbFault1.Text += "电网电压不平衡" + Environment.NewLine;

            if ((fault1 & 0x10000) > 0)
                tbFault1.Text += "输出电流不平衡" + Environment.NewLine;

            if ((fault1 & 0x8000) > 0)
                tbFault1.Text += "母线短路故障" + Environment.NewLine;

            if ((fault1 & 0x4000) > 0)
                tbFault1.Text += "正负半母线过压" + Environment.NewLine;

            if ((fault1 & 0x2000) > 0)
                tbFault1.Text += "缺相故障" + Environment.NewLine;

            if ((fault1 & 0x1000) > 0)
                tbFault1.Text += "继电器2短路故障" + Environment.NewLine;

            if ((fault1 & 0x800) > 0)
                tbFault1.Text += "继电器1短路故障" + Environment.NewLine;

            if ((fault1 & 0x400) > 0)
                tbFault1.Text += "逆变硬件过流" + Environment.NewLine;

            if ((fault1 & 0x200) > 0)
                tbFault1.Text += "PV硬件过流" + Environment.NewLine;

            if ((fault1 & 0x100) > 0)
                tbFault1.Text += "逆变温度超限" + Environment.NewLine;
            
            if ((fault1 & 0x80) > 0)
                tbFault1.Text += "母线电压超限" + Environment.NewLine;
            
            if ((fault1 & 0x40) > 0)
                tbFault1.Text += "输入电压超限" + Environment.NewLine;
            
            if ((fault1 & 0x20) > 0)
                tbFault1.Text += "漏电流超限" + Environment.NewLine;
            
            if ((fault1 & 0x10) > 0)
                tbFault1.Text += "绝缘阻抗超限" + Environment.NewLine;
            
            if ((fault1 & 0x08) > 0)
                tbFault1.Text += "直流分量超限" + Environment.NewLine;
            
            if ((fault1 & 0x04) > 0)
                tbFault1.Text += "电网频率异常" + Environment.NewLine;
            
            if ((fault1 & 0x02) > 0)
                tbFault1.Text += "电压电压异常" + Environment.NewLine;
            
            if ((fault1 & 0x01) > 0)
                tbFault1.Text += "电网丢失" + Environment.NewLine;


            tbFault2.Text = "";
            //fault2
            if ((fault2 & 0x20000) > 0)
                tbFault2.Text += "母线电压不平衡" + Environment.NewLine;

            if ((fault2 & 0x10000) > 0)
                tbFault2.Text += "母线电压低" + Environment.NewLine;

            if ((fault2 & 0x8000) > 0)
                tbFault2.Text += "boost2短路故障" + Environment.NewLine;

            if ((fault2 & 0x4000) > 0)
                tbFault2.Text += "boost1短路故障" + Environment.NewLine;

            if ((fault2 & 0x2000) > 0)
                tbFault2.Text += "boost2开路故障" + Environment.NewLine;

            if ((fault2 & 0x1000) > 0)
                tbFault2.Text += "boost1开路故障" + Environment.NewLine;


            if ((fault2 & 0x100) > 0)
                tbFault2.Text += "DC电流传感器故障" + Environment.NewLine;

            if ((fault2 & 0x80) > 0)
                tbFault2.Text += "外部风扇故障" + Environment.NewLine;       
            
            if ((fault2 & 0x40) > 0)          
                tbFault2.Text += "内部风扇故障" + Environment.NewLine;
            
            if ((fault2 & 0x20) > 0)        
                tbFault2.Text += "Relay开路故障" + Environment.NewLine;
            
            if ((fault2 & 0x10) > 0)         
                tbFault2.Text += "AC电流传感器故障" + Environment.NewLine;
            
            if ((fault2 & 0x08) > 0)       
                tbFault2.Text += "GFCI传感器故障" + Environment.NewLine;
            
            if ((fault2 & 0x04) > 0)
                tbFault2.Text += "E2故障" + Environment.NewLine;
            
            if ((fault2 & 0x02) > 0)
                tbFault2.Text += "SPI通讯故障" + Environment.NewLine;
            
            if ((fault2 & 0x01) > 0)
                tbFault2.Text += "SCI通讯故障" + Environment.NewLine;
            
        }

        

        private Dictionary<ushort, ReadDataModel> _deviceParas = new Dictionary<ushort, ReadDataModel>
        {
            {11000, new ReadDataModel{Length = 2, Gain = 1000, isSigned = true } },
            {11002, new ReadDataModel{Length = 2, Gain = 100,  } },
            {11004, new ReadDataModel{Length = 2, Gain = 100,  } },
            {11006, new ReadDataModel{Length = 1, Gain = 10,  } },
            {11007, new ReadDataModel{Length = 1, Gain = 10,  } },
            {11008, new ReadDataModel{Length = 1, Gain = 10,  } },
            {11009, new ReadDataModel{Length = 1, Gain = 10,  } },
            {11010, new ReadDataModel{Length = 1, Gain = 10,  } },
            {11011, new ReadDataModel{Length = 1, Gain = 10,  } },
            {11012, new ReadDataModel{Length = 1, Gain = 10,  } },
            {11013, new ReadDataModel{Length = 1, Gain = 10,  } },
            {11014, new ReadDataModel{Length = 1, Gain = 10,  } },
            {11015, new ReadDataModel{Length = 1, Gain = 100,  } },
            {11016, new ReadDataModel{Length = 2, Gain = 1000, isSigned = true  } },
            {11018, new ReadDataModel{Length = 2, Gain = 10,  } },
            {11020, new ReadDataModel{Length = 2, Gain = 10,  } },
            {11022, new ReadDataModel{Length = 2, Gain = 1,  } },
            {11024, new ReadDataModel{Length = 2, Gain = 1000,  } },
            {11026, new ReadDataModel{Length = 2, Gain = 1000, isSigned = true  } },
            {11028, new ReadDataModel{Length = 2, Gain = 100,  } },
            {11030, new ReadDataModel{Length = 1, Gain = 1000, isSigned = true} },
            {11031, new ReadDataModel{Length = 1, Gain = 100, } },
            {11032, new ReadDataModel{Length = 1, Gain = 10, isSigned = true} },
            {11033, new ReadDataModel{Length = 1, Gain = 10, isSigned = true} },
            {11034, new ReadDataModel{Length = 1, Gain = 10, isSigned = true} },
            {11035, new ReadDataModel{Length = 1, Gain = 10, isSigned = true} },
            {11036, new ReadDataModel{Length = 1, Gain = 10, } },
            {11037, new ReadDataModel{Length = 1, Gain = 10, } },
            {11038, new ReadDataModel{Length = 1, Gain = 10, } },
            {11039, new ReadDataModel{Length = 1, Gain = 10, } },
            {11040, new ReadDataModel{Length = 1, Gain = 10, } },
            {11041, new ReadDataModel{Length = 1, Gain = 10, } },
            {11042, new ReadDataModel{Length = 1, Gain = 10, } },
            {11043, new ReadDataModel{Length = 1, Gain = 10, } },
            {11044, new ReadDataModel{Length = 1, Gain = 10, } },
            {11045, new ReadDataModel{Length = 1, Gain = 10, } },
            {11046, new ReadDataModel{Length = 1, Gain = 10, } },
            {11047, new ReadDataModel{Length = 1, Gain = 10, } },
            {11048, new ReadDataModel{Length = 1, Gain = 10, } },
            {11049, new ReadDataModel{Length = 1, Gain = 10, } },
            {11050, new ReadDataModel{Length = 1, Gain = 10, } },
            {11051, new ReadDataModel{Length = 1, Gain = 10, } },
            {11052, new ReadDataModel{Length = 1, Gain = 10, } },
            {11053, new ReadDataModel{Length = 1, Gain = 10, } },
            {11054, new ReadDataModel{Length = 1, Gain = 10, } },
            {11055, new ReadDataModel{Length = 1, Gain = 10, } },
            {11056, new ReadDataModel{Length = 1, Gain = 10, } },
            {11057, new ReadDataModel{Length = 1, Gain = 10, } },
            {11058, new ReadDataModel{Length = 1, Gain = 10, } },
            {11059, new ReadDataModel{Length = 1, Gain = 10, } },
            {11060, new ReadDataModel{Length = 1, Gain = 10, } },
            {11061, new ReadDataModel{Length = 1, Gain = 10, } },
            {11062, new ReadDataModel{Length = 2, Gain = 1000, } },
            {11064, new ReadDataModel{Length = 2, Gain = 1000, } },
            {11066, new ReadDataModel{Length = 2, Gain = 1000, } },
            {11068, new ReadDataModel{Length = 2, Gain = 1000, } },
            {11070, new ReadDataModel{Length = 2, Gain = 1000, } },
            {11072, new ReadDataModel{Length = 2, Gain = 1000, } },
            {11074, new ReadDataModel{Length = 1, Gain = 10, } },
            {11075, new ReadDataModel{Length = 1, Gain = 10, } },
            {11076, new ReadDataModel{Length = 1, Gain = 10, } },
            {11077, new ReadDataModel{Length = 1, Gain = 10, } },
            {11078, new ReadDataModel{Length = 1, Gain = 10, } },
            {11079, new ReadDataModel{Length = 1, Gain = 10, } },
            {11080, new ReadDataModel{Length = 1, Gain = 10, } },
            {11081, new ReadDataModel{Length = 1, Gain = 10, } },
            {11082, new ReadDataModel{Length = 1, Gain = 10, } },
            {11083, new ReadDataModel{Length = 1, Gain = 10, } },
            {11084, new ReadDataModel{Length = 1, Gain = 10, } },
            {11085, new ReadDataModel{Length = 1, Gain = 10, } },
            {11086, new ReadDataModel{Length = 1, Gain = 10, } },
            {11087, new ReadDataModel{Length = 1, Gain = 10, } },
            {11088, new ReadDataModel{Length = 1, Gain = 10, } },
            {11089, new ReadDataModel{Length = 1, Gain = 10, } },
            {11090, new ReadDataModel{Length = 2, Gain = 1000, } },
            {11092, new ReadDataModel{Length = 2, Gain = 1000, } },
            {11094, new ReadDataModel{Length = 2, Gain = 1000, } },
            {11096, new ReadDataModel{Length = 2, Gain = 1000, } },
        };
    }

    public class ReadDataModel
    {
        public string Value { get; set; }
        public int Length { get; set; }
        public int Gain { get; set; }
        public bool isSigned { get; set; } = false;
    }
}
