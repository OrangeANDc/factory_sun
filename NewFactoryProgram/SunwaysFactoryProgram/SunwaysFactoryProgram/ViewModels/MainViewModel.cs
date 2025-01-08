using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics;
using System.Text;
using System.Threading.Tasks;
using SunwaysFactoryProgram.Messenager;
using System.IO.Ports;
using Prism.Ioc;
using Prism.DryIoc;
using SunwaysFactoryProgram.Views;
using Prism.Commands;
using SunwaysFactoryProgram.Protocol;
using System.Windows;
using SunwaysFactoryProgram.StaticSource;
using System.Windows.Media;
using Prism.Regions;
using System.Windows.Interop;

namespace SunwaysFactoryProgram.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private IEventAggregator _eventAggregator;
        private IContainerProvider _containerProvider;
        private SerialDevice _serialDevice;
        public MainViewModel(IContainerProvider containerProvider, IEventAggregator ea)
        {
            _containerProvider = containerProvider;
            _eventAggregator = ea;
            _eventAggregator.GetEvent<RecordLogEvent>().Subscribe(SetRecordLog);
            _eventAggregator.GetEvent<TestStatusChangeEvent>().Subscribe(SetTestAndBackground);
            _eventAggregator.GetEvent<LogoutInitEvent>().Subscribe(LogoutInit);

            _serialDevice = _containerProvider.Resolve<SerialDevice>();
            MdColor.SetThemeColor("#FF2196F3");         
            Load();
        }

        private void Load()
        {
            Version = Variable.Version;
            IsFuncEnable = !_serialDevice.GetStatus();
            SetConnectCfg(!IsFuncEnable);
            PortLists = SerialPort.GetPortNames().ToList();
            SetTestAndBackground(new TestStatusChange { Status = 0, Text = "", IsEnable = false});

            OpenCommand = new DelegateCommand<string>(Open);
            CloseCommand = new DelegateCommand(Close);
            RefreshCommand = new DelegateCommand(Refresh);
            ClearInfoCommand = new DelegateCommand(ClearInfo);
            ReadParaCommand = new DelegateCommand(ReadPara);
        }

        // 标题名称字段
        private string _procedureName;
        public string ProcedureName
        {
            get => _procedureName;
            set => SetProperty(ref _procedureName, value);
        }

        // 用户
        private string _user;
        public string User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        // 站点名称
        private string _stationName;
        public string StationName
        {
            get => _stationName;
            set => SetProperty(ref _stationName, value);
        }

        //版本
        private string _version;
        public string Version
        {
            get => _version;
            set => SetProperty(ref _version, value);
        }

        //  连接设置
        private string _connectText;
        public string ConnectText
        {
            get => _connectText;
            set => SetProperty(ref _connectText, value);
        }

        private string _connectColor;
        public string ConnectColor
        {
            get => _connectColor;
            set => SetProperty(ref _connectColor, value);
        }

        private string _reocrdLog;
        public string RecordLog
        {
            get => _reocrdLog;
            set => SetProperty(ref _reocrdLog, value);
        }

        private string _buadRate;
        public string BuadRate
        {
            get => _buadRate;
            set => SetProperty(ref _buadRate, value);
        }
        private void SetRecordLog(string log)
        {
            if (log == "分隔符")
            {
                log = "---------------------------------------------------";
            }
            else
            {
                log = DateTime.Now.ToString() + " " + log;
            }

            Variable._funcLogList.Add(log);
            Variable._packLogList.Add(log);

            if (RecordLog == null)
                RecordLog = "";
            if (RecordLog.Length > 2000)
                RecordLog.Remove(0, 1000);


            RecordLog += (log + Environment.NewLine);
        }

        private void SetConnectCfg(bool status)
        {
            if (status)
            {
                ConnectText = "已连接";
                ConnectColor = "#00ff50"; //green
            }
            else
            {
                ConnectText = "未连接";
                ConnectColor = "#ff0000"; //red
            }
        }

        private string _testResult;
        public string TestResult
        {
            get => _testResult;
            set => SetProperty(ref _testResult, value);
        }

        private string _testResultBackground;
        public string TestResultBackground
        {
            get => _testResultBackground;
            set => SetProperty(ref _testResultBackground, value);
        }

        private bool _testEnable;
        public bool TestEnable
        {
            get => _testEnable;
            set => SetProperty(ref _testEnable, value);
        }

        private void SetTestAndBackground(TestStatusChange testStatusChange)
        {
            TestResult = testStatusChange.Text;
            TestEnable = testStatusChange.IsEnable;
            switch (testStatusChange.Status)
            {
                case 0:
                    TestResultBackground = "#000000";
                    break;
                case 1:
                    TestResultBackground = "#FF2196F3";
                    break;
                case 2:
                    TestResultBackground = "#00ff00";
                    break;
                case 3:
                    TestResultBackground = "#ff0000";
                    break;
            }
        }



        // 串口号列表
        private List<string> _portLists;
        public List<string> PortLists
        {
            get => _portLists;
            set => SetProperty(ref _portLists, value);
        }

        // 功能使能
        private bool _isFuncEnable;
        public bool IsFuncEnable
        {
            get => _isFuncEnable;
            set => SetProperty(ref _isFuncEnable, value);
        }


        public DelegateCommand<string> OpenCommand { get; set; }
        public DelegateCommand CloseCommand { get; set; }
        public DelegateCommand RefreshCommand { get; set; }
        public DelegateCommand ClearInfoCommand { get; set; }
        public DelegateCommand ReadParaCommand { get; set; }

        private void Open(string portName)
        {
            try
            {
                if (!_serialDevice.GetStatus())
                {
                    if (!_serialDevice.Open(portName, Convert.ToInt32(BuadRate)))
                    {
                        MessageBox.Show("打开串口失败");
                        return;
                    }
                    IsFuncEnable = false;
                    SetConnectCfg(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        private void Close()
        {
            if (!_serialDevice.Close())
            {
                MessageBox.Show("关闭串口失败");
                return;
            }
            IsFuncEnable = true;
            SetConnectCfg(false);
        }

        private void Refresh()
        {
            PortLists = SerialPort.GetPortNames().ToList();
        }

        private void ClearInfo()
        {
            RecordLog = "";
        }

        private void ReadPara()
        {
            if (!_serialDevice.GetStatus())
            {                     
                MessageBox.Show("请打开串口");
                return;
            }

            _serialDevice.ReadSN();
            _serialDevice.ReadFirmwareVersion();
            

        }

        private void LogoutInit()
        {
            this.Close();
            this.ClearInfo();
            TestResult = string.Empty;
            this.Refresh();
        }
    }
}
