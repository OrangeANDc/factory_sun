using Prism.Events;
using Prism.Ioc;
using Prism.Services.Dialogs;
using SunwaysFactoryProgram.DBModel;
using SunwaysFactoryProgram.Messenager;
using SunwaysFactoryProgram.Protocol;
using SunwaysFactoryProgram.StaticSource;
using SunwaysFactoryProgram.Views.FuncViews;
using SupportProject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Printing;
using System.Text;
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
    /// FunctionView.xaml 的交互逻辑
    /// </summary>
    public partial class FunctionView : UserControl
    {
        private IEventAggregator _eventAggregator;
        private IContainerProvider _containerProvider;
        private SerialDevice _serialDevice;
        private IFreeSql fsql = DBHelper.SqlEntity;
        private bool _volCorrectChecked;
        private bool _exportLimitChecked;
        private bool _dredChecked;
        private string _exportLimitStr;

        public string ST_LogDictionary = "D:\\TestLog\\Inverter\\FuncTest";
        public string ST_LogDayDictionary = "D:\\TestLog\\Inverter\\FuncDay";
        public string ST_ServerDictionary =  "\\\\" + "192.168.10.99" + "\\TestLog\\Inverter\\FuncTest";

        public FunctionView(IEventAggregator ea, IContainerProvider containerProvider)
        {
            InitializeComponent();
            _eventAggregator = ea;
            _containerProvider = containerProvider;
            _serialDevice = _containerProvider.Resolve<SerialDevice>();
            cbSafety.ItemsSource = Variable._safetyDic.Select(x => x.Key).ToList();
        }

        private void CreateLogFile(string sn, List<string> list, TBS_FuncTest obj)
        {
            string fileName1 = FileLogRecord.GetFileName(this.ST_LogDictionary, sn, "FUNC", obj.TestResult);
            string fileName2 = FileLogRecord.GetFileName(this.ST_ServerDictionary, sn, "FUNC", obj.TestResult);
            string fileNameDay = FileLogRecord.GetFileNameDay(this.ST_LogDayDictionary, "FUNC");
            this.WriteTestInfo(fileName1, fileNameDay, list, obj);
            try
            {
                File.Copy(fileName1, fileName2);
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message);
            }
        }

        private void WriteTestInfo(string oneFilePath, string dayFilePath, List<string> list,  TBS_FuncTest obj)
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
            streamWriter2.WriteLine("");
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



        private void SendInfo(string msg)
        {
            _eventAggregator.GetEvent<RecordLogEvent>().Publish(msg);
            Variable._funcTestResult.ErrorMsg = msg;
        }

        private bool IsESInverter(string sn)
        {
            return sn.Substring(2, 1) == "1";
        }

        private bool IsSinglePhase(string sn)
        {
            string str = sn.Substring(11, 2);
            return str == "01" || str == "02" || str == "03";
        }

        private void SetButtonStatus(bool status)
        {
            this.Dispatcher.Invoke(() =>
            {
                gbTestSet.IsEnabled = status;
                gbParaSet.IsEnabled = status;
                gbLowDeviceParaSet.IsEnabled = status;

            /*    btnReadCT.IsEnabled = status;
                btnSetSwitch.IsEnabled = status;
                btnSetBurnMode.IsEnabled = status;
                btnSetWorkMode.IsEnabled = status;
                btnResetStatus.IsEnabled = status;
                btnSetSafety.IsEnabled = status;*/
            });
           
        }

        public void TestPrepare(string sn, string user, string stationName)
        {
            try
            {
                if (!_serialDevice.GetStatus())
                {
                    MessageBox.Show("请连接串口!");
                    return;
                }

                _volCorrectChecked = cbVolCorrect.IsChecked == true ? true : false;
                _exportLimitChecked = cbExportLimit.IsChecked == true ? true : false;
                _dredChecked = cbDred.IsChecked == true ? true : false;
                _exportLimitStr = tbExportLimit.Text;


                if (_exportLimitChecked)
                {
                    if (string.IsNullOrEmpty(_exportLimitStr))
                    {
                        MessageBox.Show("请设置上行功率百分比!");
                        return;
                    }
                    else
                        Convert.ToDouble(_exportLimitStr);
                }

                // 初始化测试结果对象
                Variable._funcTestResult = new TBS_FuncTest();
                Variable._funcTestResult.InitData();
                Variable._funcTestResult.Tester = user;
                Variable._funcTestResult.StationId = stationName;
                Variable._funcTestResult.InverterSN = sn;

                StartTest(sn);
            }
            catch (Exception ex)
            {           
                MessageBox.Show(ex.Message);
                Log.Error(ex.Message);
                return;
            }
        }

        private async void StartTest(string sn)
        {
            Variable._funcLogList = new List<string>();
            Task t = new Task(() => RunTest(sn));
            t.Start();
            await t;
        }

        private void RunTest(string sn)
        {
            try
            {              
                _eventAggregator.GetEvent<TestStatusChangeEvent>().Publish(new TestStatusChange { Status = 1, Text = sn + " 测试中" });
                SetButtonStatus(false);

                if (!Run(sn))
                {
                    Variable._funcTestResult.TestResult = "FAIL";
                    _eventAggregator.GetEvent<TestStatusChangeEvent>().Publish(new TestStatusChange { Status = 3, Text = sn + " Fail", IsEnable = false });
                }
                else
                {
                    Variable._funcTestResult.ErrorMsg = "";
                    Variable._funcTestResult.TestResult = "PASS";
                    _eventAggregator.GetEvent<TestStatusChangeEvent>().Publish(new TestStatusChange { Status = 2, Text = sn + " Pass", IsEnable = false });
                }

                Variable._funcTestResult.TestTime = DateTime.Now;

                App.Current.Dispatcher.Invoke(() =>
                {
                    _eventAggregator.GetEvent<TextFocusEvent>().Publish();
                });

               /* if (fsql.Insert<TBS_FuncTest>(Variable._funcTestResult).ExecuteAffrows() != 1)
                {
                    MessageBox.Show("保存测试结果到数据库失败,请重新测试!");
                }
                else
                {
                    //保存日志
                    CreateLogFile(sn, Variable._funcLogList, Variable._funcTestResult);
                }*/

                SetButtonStatus(true);
            }
            catch (Exception ex)
            {
                SetButtonStatus(true);
                _eventAggregator.GetEvent<TestStatusChangeEvent>().Publish(new TestStatusChange { Status = 3, Text = sn + " Fail", IsEnable = true });
                MessageBox.Show(ex.Message);
                Log.Error(ex.Message);
                return;
            }
        }

        private bool Run(string sn)
        {
            SendInfo("分隔符");

            bool viewResult = false;
            EnCompany enCompany = Methods.GetEnCompany(sn);

            if (enCompany == EnCompany.Salicru)
            {
                viewResult = App.Current.Dispatcher.Invoke(() =>
                {
                    OdmSNView odmSNView = new OdmSNView();
                    if (odmSNView.ShowDialog() != true)
                    {
                        return false;
                    }
                    Variable._funcTestResult.OdmSN = odmSNView.OdmSN;
                    return true;
                });

                if (!viewResult)
                {
                    SendInfo("没有扫描 OEM SN");
                    return false;
                }
            }

            // 写入校验SN
            if (!_serialDevice.WriteAndReadSN(sn))
                return false;

            // 设置第二域名
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

            // 读取软件版本
            if (!_serialDevice.ReadFirmwareVersion())
                return false;

            // 同步时间
            if (!_serialDevice.ResetTime())
                return false;

            //1.65v电压校正
            if (_volCorrectChecked)
            {
                if(!_serialDevice.CalibrateVol_165())
                    return false; 
            }

            //Warehouse安规设置
            if (!_serialDevice.SetWarehouseSafety())
                return false;

            //复位指令发送
            if (!_serialDevice.SetSpsReset())
                return false;

            if (IsESInverter(sn))
            {
                bool flag = false;
                for (int count = 0; count < 3; count++)
                {
                    flag = _serialDevice.SetArcbSwitch(false);
                    if (flag)
                        break;
                }
                if(!flag)
                    return false;

            }

            viewResult = App.Current.Dispatcher.Invoke(() =>
            {
                NoticeView noticeView = new NoticeView("请打开AC开关并确认发电功率是否正常!");
                return noticeView.ShowDialog() == true;
            });
            if (!viewResult)
            {
                Variable._funcTestResult.OnGridTest = "FAIL";
                SendInfo("确认发电功率是否正常时,手动判定为异常!");
                return false;
            }
            Variable._funcTestResult.OnGridTest = "PASS";

            //设置ct变比
            if (IsSinglePhase(sn))
            {
                if(!_serialDevice.SetCtRatio())
                    return false;
            }

            //防逆流设置以及手动判断发电功率
            if (_exportLimitChecked)
            {
                bool flag = false;
                bool judg_fail = false;
                for (int count = 0; count < 3; count++)
                {
                    Variable._funcTestResult.ExportLimitTest = "FAIL";
                    if(!_serialDevice.SetPowerPercent((ushort)(Convert.ToDouble(_exportLimitStr) * 10.0)))
                        return false;
                    if(!_serialDevice.SetArcbSwitch(true))
                        return false;

                    flag = App.Current.Dispatcher.Invoke(() =>
                    {
                        NoticeView noticeView = new NoticeView("请打开AC开关并确认发电功率是否正常!", true);
                        var viewResult1 = noticeView.ShowDialog();
                        if (viewResult1 == null)
                            return false;
                        else if (viewResult1 == false)
                        {
                            // 手动判断
                            judg_fail = true;
                            return false;
                        }
                        else 
                        {
                            if (!_serialDevice.SetArcbSwitch(true))
                                return false;

                            NoticeView noticeView2 = new NoticeView("请确认发电功率是否恢复!", true);
                            var viewResult2 = noticeView2.ShowDialog();

                            if (viewResult2 == null)
                                return false;
                            else if (viewResult2 == false)
                            {
                                judg_fail = true;
                                return false;
                            }
                            else
                            {
                                Variable._funcTestResult.ExportLimitTest = "PASS";
                                return true;
                            }
                        }
                    });

                    if (flag)
                        break;
                    if(judg_fail)
                        return false;
                    if (count == 2 && !flag)
                        return false;
                }
            }

            //DRED 测试
            if (_dredChecked)
            {
                bool flag = false;
                bool judg_fail = false;
                for (int count = 0; count < 3; count++)
                {
                    Variable._funcTestResult.DredTest = "FAIL";
                    if (!_serialDevice.SetAustriaSafety())
                        return false;

                    if (!_serialDevice.SetSpsReset())
                        return false;

                    flag = App.Current.Dispatcher.Invoke(() =>
                    {
                        NoticeView noticeView = new NoticeView("请打开AC开关并确认发电功率是否正常!", true);
                        var viewResult1 = noticeView.ShowDialog();
                        if (viewResult1 == null)
                            return false;
                        else if (viewResult1 == false)
                        {
                            // 手动判断
                            judg_fail = true;
                            return false;
                        }
                        else
                        {
                            Variable._funcTestResult.DredTest = "PASS";
                            return true;
                        }
                    });

                    if(flag)
                        break;
                    if (judg_fail) 
                        return false;
                    if(count == 2 && !flag)
                        return false;
                }
            }

            if(!_serialDevice.SetBurnInSafetyAndCheck())
                return false;

            return true;
        }

        private void btnSetSwitch_Click(object sender, RoutedEventArgs e)
        {
            if (!_serialDevice.GetStatus())
            {
                MessageBox.Show("请连接串口!");
                return;
            }

            if (string.IsNullOrEmpty(cbSwitch.Text))
            {
                MessageBox.Show("请选择开启或关闭防逆流!");
                return;
            }

            _serialDevice.SetArcbSwitch(cbSwitch.Text == "ON" ? true : false);
        }

        private void btnReadCT_Click(object sender, RoutedEventArgs e)
        {
            if (!_serialDevice.GetStatus())
            {
                MessageBox.Show("请连接串口!");
                return;
            }

            _serialDevice.ReadCtRatio();
        }

        
    }
}
