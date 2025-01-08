using Prism.Events;
using Prism.Ioc;
using SunwaysFactoryProgram.DBModel;
using SunwaysFactoryProgram.Messenager;
using SunwaysFactoryProgram.Protocol;
using SunwaysFactoryProgram.StaticSource;
using SunwaysFactoryProgram.Views.DataViews;
using SunwaysFactoryProgram.Views.FuncViews;
using SupportProject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
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

namespace SunwaysFactoryProgram.Views
{
    /// <summary>
    /// PackView.xaml 的交互逻辑
    /// </summary>
    public partial class PackView : UserControl
    {
        private IEventAggregator _eventAggregator;
        private IContainerProvider _containerProvider;
        private SerialDevice _serialDevice;
        private IFreeSql fsql = DBHelper.SqlEntity;
        //private Dictionary<string, ushort> _safetyDic;
        private Dictionary<string, ushort> _languageDic;
        private bool _safetyChecked;
        private bool _lanChecked;
        private bool _isLowDevice;
        private bool _e2Checked;
        private bool _burnModeChecked;
        private bool _batteryIdChecked;
        private bool _batteryTypeChecked;
        private string _safetyStr;
        private string _lanStr;
        private int _burnMode;
        private int _batteryId;
        private int _batteryType;

        public string ST_LogDictionary = "D:\\TestLog\\Inverter\\PackTest";
        public string ST_LogDayDictionary = "D:\\TestLog\\Inverter\\PackDay";
        public string ST_ServerDictionary =  "\\\\" + "192.168.10.99" + "\\TestLog\\Inverter\\PackTest";
        public PackView(IEventAggregator ea, IContainerProvider containerProvider)
        {
            InitializeComponent();
            Load();
            _eventAggregator = ea;
            _containerProvider = containerProvider;
            _serialDevice = _containerProvider.Resolve<SerialDevice>();
        }

        private void Load()
        {
            InitSource();
            cbSafety.ItemsSource = Variable._safetyDic.Select(x => x.Key).ToList();
            cbLanguage.ItemsSource = _languageDic.Select(x => x.Key).ToList();
        }

       
        private void CreateLogFile(string sn, List<string> list, TBS_PackTest obj)
        {
            string fileName1 = FileLogRecord.GetFileName(this.ST_LogDictionary, sn, "PACK", obj.TestResult);
            string fileName2 = FileLogRecord.GetFileName(this.ST_ServerDictionary, sn, "PACK", obj.TestResult);
            string fileNameDay = FileLogRecord.GetFileNameDay(this.ST_LogDayDictionary, "PACK");
            WriteTestInfo(fileName1, fileNameDay, list, obj);
            try
            {
                File.Copy(fileName1, fileName2);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void WriteTestInfo(string oneFilePath, string dayFilePath, List<string> list, TBS_PackTest obj)
        {
            FileStream fileStream = new FileStream(oneFilePath, FileMode.Create);
            StreamWriter streamWriter1 = new StreamWriter((Stream)fileStream);
            StreamWriter streamWriter2 = new StreamWriter(dayFilePath, true);
            string str1 = "测试时间:" + obj.TestTime.ToString();
            streamWriter1.WriteLine(str1);
            string str2 = "测试结果:" + obj.TestResult;
            streamWriter1.WriteLine(str2);
            if (obj.ErrorMsg != "")
            {
                string str3 = "错误信息:" + obj.ErrorMsg; 
                streamWriter1.WriteLine(str3);
            }
            streamWriter1.WriteLine("");
            streamWriter2.WriteLine("")                                                  ;
            string str4 = "测试日志:";
            streamWriter1.WriteLine(str4);
            for (int index = 0; index < list.Count; ++index)
            {
                streamWriter1.WriteLine(list[index]);
                streamWriter2.WriteLine(list[index]);
            }
            streamWriter1.Close();
            fileStream.Close();
            streamWriter2.Close();
        }

        private void InitSource()
        {
           
            // Language
            _languageDic = new Dictionary<string, ushort>()
            {
               { "中文"    ,0 },
               { "英语"    ,1 },
               { "西班牙语",2 },
               { "葡萄牙语",3  },
               { "波兰语"  ,4  },
               { "意大利语",5  },
               { "德语/捷克语",  6  },
               { "乌克兰语",7  }
            };
        }

        private void ButtonTodayInf_Click(object sender, RoutedEventArgs e)
        {
            PackTestDataView packTestDataView = new PackTestDataView(true);
            packTestDataView.ShowDialog();
        }

        private void ButtonInf_Click(object sender, RoutedEventArgs e)
        {
            PackTestDataView packTestDataView = new PackTestDataView();
            packTestDataView.ShowDialog();
        }                                                                                                                                                                                                                                                                                                                                                                                                                                                            

        private void SendInfo(string msg)
        {
            _eventAggregator.GetEvent<RecordLogEvent>().Publish(msg);
            Variable._packTestResult.ErrorMsg = msg;
        }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      

        private bool IsE2LimitModel(string sn, ref ushort limitValue)
        {
            string str1 = sn.Substring(11, 2);
            string str2 = sn.Substring(14, 1);
            if (str1 == "01" && str2 == "4")
            {
                limitValue = (ushort)49;
                return true;
            }
            if (str1 == "03" && str2 == "3")
            {
                limitValue = (ushort)99;
                return true;
            }
            if (!(str1 == "12") || !(str2 == "4"))
                return false;
            limitValue = (ushort)149;
            return true;
        }

        public void TestPrepare(string sn, string user, string stationName)
        {
            if (!_serialDevice.GetStatus())
            {
                MessageBox.Show("请连接串口!");
                return;
            }

            _safetyChecked = cbSafetyChecked.IsChecked == true ? true : false;
            _lanChecked = cbLanguageChecked.IsChecked == true ? true : false;
            _isLowDevice = cbIsLow.IsChecked == true ? true : false;
            _e2Checked = cbE2LimitChecked.IsChecked == true ? true : false;
            _burnModeChecked = cbBurnModeChecked.IsChecked == true ? true : false;
            _batteryIdChecked = cbBatteryIdChecked.IsChecked == true ? true : false;
            _batteryTypeChecked = cbBatteryTypeChecked.IsChecked == true ? true : false;
            _safetyStr = cbSafety.Text;
            _lanStr = cbLanguage.Text;
            _burnMode = cbBurnMode.SelectedIndex;
            _batteryId = cbBatteryId.SelectedIndex;
            _batteryType = cbBatteryType.SelectedIndex;

            if (_safetyChecked && string.IsNullOrEmpty(_safetyStr))
            {
                MessageBox.Show("请选择安规!");
                return;
            }
            if (_lanChecked && string.IsNullOrEmpty(_lanStr))
            {
                MessageBox.Show("请选择语言!");
                return;
            }

            if (_lanChecked == true)
            {
                if (sn.Substring(1, 1) == "0")
                {
                    if (_lanStr != "中文")
                    {
                        MessageBox.Show("国内机器只能设置成中文!");
                        return;
                    }
                }
                else
                {
                    if (_lanStr == "中文")
                    {
                        MessageBox.Show("国外机器不能设置成中文!");
                        return;
                    }
                }
            }

            if (_burnModeChecked == true && string.IsNullOrEmpty(cbBurnMode.Text))
            {
                MessageBox.Show("请选择老化模式!");
                return;
            }
            if ((_batteryIdChecked == true ) && string.IsNullOrEmpty(cbBatteryId.Text))
            {
                MessageBox.Show("请选择Battery ID!");
                return;
            }
            if ((_batteryTypeChecked == true) && string.IsNullOrEmpty(cbBatteryType.Text))
            {
                MessageBox.Show("请选择Battery Type!");
                return;
            }


            // 初始化测试结果对象
            Variable._packTestResult = new TBS_PackTest();
            Variable._packTestResult.InitData();
            Variable._packTestResult.Tester = user;
            Variable._packTestResult.StationId = stationName;
            Variable._packTestResult.InverterSN = sn;

            StartTest(sn);
        }

        private  async void StartTest(string sn)
        {
            Variable._packLogList = new List<string>();          
            Task t = new Task(() => RunTest(sn));
            t.Start();
            await t;

        }

        private void RunTest(string sn)
        {
            try
            {
                _eventAggregator.GetEvent<TestStatusChangeEvent>().Publish(new TestStatusChange { Status = 1, Text = sn + " 测试中" });

                if (!Run(sn))
                {
                    Variable._packTestResult.TestResult = "FAIL";
                    _eventAggregator.GetEvent<TestStatusChangeEvent>().Publish(new TestStatusChange { Status = 3, Text = sn + " Fail", IsEnable = false });
                }
                else
                {
                    Variable._packTestResult.TestResult = "PASS";
                    Variable._packTestResult.ErrorMsg = "";
                    _eventAggregator.GetEvent<TestStatusChangeEvent>().Publish(new TestStatusChange { Status = 2, Text = sn + " Pass", IsEnable = false });
                }

                Variable._packTestResult.TestTime = DateTime.Now;

                App.Current.Dispatcher.Invoke(() =>
                {
                    _eventAggregator.GetEvent<TextFocusEvent>().Publish();
                });

                if (fsql.Insert<TBS_PackTest>(Variable._packTestResult).ExecuteAffrows() != 1)
                {
                    MessageBox.Show("保存测试结果到数据库失败,请重新测试!");
                }
                else
                {
                    //保存日志
                    CreateLogFile(sn, Variable._packLogList, Variable._packTestResult);
                }
            }
            catch (Exception ex)
            {
                _eventAggregator.GetEvent<TestStatusChangeEvent>().Publish(new TestStatusChange { Status = 3, Text = sn + " Fail", IsEnable = true });
                MessageBox.Show(ex.Message);
                Log.Error(ex.Message);
                return;
            }
        }

        private bool Run(string sn)
        {
            SendInfo("分隔符");
            string getMsg = "";
            if (!_serialDevice.GetFunc(GetCommandType.获取序列号, ref getMsg))
            {
                SendInfo("读取SN失败");
                return false;
            }
            else
            {
                SendInfo("逆变器序列号:" + getMsg);
                if (getMsg == sn)
                    SendInfo("序列号对比成功!");
                else
                {
                    SendInfo("逆变器序列号对比失败!");
                    return false;
                }

            }

            //
            EnCompany enCompany = Methods.GetEnCompany(sn);
            if (enCompany == EnCompany.Wattsonic || enCompany == EnCompany.Stromherz || enCompany == EnCompany.Nelumbo_Energy ||
                enCompany == EnCompany.Salicru || enCompany == EnCompany.Regitic || enCompany == EnCompany.M_Tec)
            {
                if (!_serialDevice.WriteSecondDomain(false))
                    return false;
            }

            if (enCompany == EnCompany.ETEK)
            {
                if (!_serialDevice.WriteSecondDomain(true))
                    return false;
            }

            if (enCompany == EnCompany.Salicru)
            {

                bool result = App.Current.Dispatcher.Invoke(() =>
                {
                    OdmSNView odmSNView = new OdmSNView();
                    if (odmSNView.ShowDialog() != true)
                    {
                        return false;
                    }
                    Variable._packTestResult.OdmSN = odmSNView.OdmSN;
                    return true;
                });


                if (!result)
                {
                    SendInfo("没有扫描 OEM SN!");
                    return false;
                }
                SendInfo("扫描的OEM SN:" + Variable._packTestResult.OdmSN);
                var record = fsql.Select<TBS_FuncTest>().Where(o => o.OdmSN == Variable._packTestResult.OdmSN).ToList().FirstOrDefault();
                if (record == null)
                {
                    SendInfo("没未找到功能测试站点对应的ODM序列号!");
                    return false;
                }
                else
                    SendInfo("OEM序列号对比成功!");

            }
            else
            {
                string checkCode = string.Empty;
                bool result = App.Current.Dispatcher.Invoke(() =>
                {
                    CheckCodeView checkCodeView = new CheckCodeView();
                    if (checkCodeView.ShowDialog() != true)
                    {
                        return false;
                    }
                    checkCode = checkCodeView.CheckCode;
                    return true;
                });

                if (!result)
                {
                    SendInfo("没有扫描 CheckCode!");
                    return false;
                }
                SendInfo("扫描的CheckCode:" + checkCode);

                if (!_serialDevice.GetFunc(GetCommandType.获取CheckCode, ref getMsg))
                {
                    SendInfo("读取CheckCode失败");
                    return false;
                }
                else
                {
                    SendInfo("逆变器CheckCode:" + getMsg);
                    if (getMsg == checkCode)
                        SendInfo("CheckCode对比成功!");
                    else
                    {
                        SendInfo("逆变器CheckCode对比失败!");
                        return false;
                    }
                }
            }

            // 读取软件版本
            if (!_serialDevice.ReadFirmwareVersion()) 
                return false;


            // 读取RTC
            if (!_serialDevice.ResetTime())   
                return false;

            if (!_isLowDevice)
            {
                // 数据清零
                if (!_serialDevice.ClearFactoryData())
                    return false;
            }
            


            //salicru && 西班牙
            if (enCompany == EnCompany.Salicru && sn.Substring(2, 1) == "1")
            {
                if (_safetyStr == "ES:TED749(西班牙)")
                {
                    if (!_serialDevice.SetBatModel(1))
                    {
                        Variable._packTestResult.ClearOK = "FAIL";
                        return false;
                    }
                    if (!_serialDevice.SetBatProtocol(1))
                    {
                        Variable._packTestResult.ClearOK = "FAIL";
                        return false;
                    }
                }
                else
                {
                    if (!_serialDevice.SetBatModel(14))
                    {
                        Variable._packTestResult.ClearOK = "FAIL";
                        return false;
                    }
                    if (!_serialDevice.SetBatProtocol(0))
                    {
                        Variable._packTestResult.ClearOK = "FAIL";
                        return false;
                    }
                }
                SendInfo("Salicru 参数设置成功!");
            }


                //设置安规
                if (_safetyChecked == true)
                {
                    if (!_serialDevice.SetSafety(_safetyStr, Variable._safetyDic[_safetyStr]))
                        return false;
                }

                //设置语言
                if (_lanChecked == true)
                {
                    if (!_serialDevice.SetLanguage(_lanStr, _languageDic[_lanStr]))
                        return false;
                }

                if (_e2Checked == true)
                {
                    ushort limitValue = 10000;
                    if (IsE2LimitModel(sn, ref limitValue))
                    {
                        ushort safetyCode = Variable._safetyDic[_safetyStr];
                        if (safetyCode == 13 || safetyCode == 14 || safetyCode == 46 || safetyCode == 48)
                        {
                            if (!_serialDevice.SetPowerE2Limit(limitValue))
                                return false;
                        }
                    }
                }

            if (_isLowDevice)
            {
                if (_burnModeChecked == true)
                {
                    if (!_serialDevice.SetBurnMode((ushort)_burnMode))
                        return false;
                }

                if (_batteryIdChecked == true)
                {
                    int value = _batteryId + 1;

                    if (!_serialDevice.SetBatteryID((ushort)value))
                        return false;
                }

                if (_batteryTypeChecked == true)
                {
                    if (!_serialDevice.SetBatteryType((ushort)_batteryType))
                        return false;
                }

                // 数据清零
                if (!_serialDevice.ClearFactoryData())
                    return false;
                SendInfo("机器待机等待中....!");
                Thread.Sleep(10000);
            }

           
            SendInfo("包装测试成功!");
            return true;
        }

        private void cbIsLow_Checked(object sender, RoutedEventArgs e)
        {
            gbLowTextItem.IsEnabled = true;
        }

        private void cbIsLow_Unchecked(object sender, RoutedEventArgs e)
        {
            gbLowTextItem.IsEnabled = false;
        }
    }
}
