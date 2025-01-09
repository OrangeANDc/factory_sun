using Microsoft.Win32;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup.Localizer;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SystemTool.DelegateService;
using SystemTool.Messenager;
using SystemTool.Model;
using SystemTool.Protocol;
using SystemTool.StaticSource;
using SystemTool.ViewModels;

namespace SystemTool.Views
{
    /// <summary>
    /// DataMonitorView.xaml 的交互逻辑
    /// </summary>
    public partial class DataMonitorView : UserControl
    {
        private CancellationTokenSource _readDataCts = new CancellationTokenSource();
        private CancellationTokenSource _refreshViewCts = new CancellationTokenSource();
        private ManualResetEvent _mResetEvent = new ManualResetEvent(true);
        private SerialDevice _serialDevice;
        private Dictionary<string, ObservableCollection<ParaModel>> _viewSource;
        private Dictionary<string, ObservableCollection<ParaModel>> _viewSource2;
        private IContainerProvider _containerProvider;
        private IEventAggregator _eventAggregator;
        private bool _isTaskPause = false;


        public DataMonitorView(IContainerProvider containerProvider,IEventAggregator ea)
        {
            InitializeComponent();
            _serialDevice = containerProvider.Resolve<SerialDevice>();
            _containerProvider = containerProvider;
            _eventAggregator = ea;
            _eventAggregator.GetEvent<ControlTaskStatus>().Subscribe(SetReadTaskStatus);
            btnStopM.IsEnabled = false;

        }

        private void SetReadTaskStatus(bool status)
        {
            // 继续
            if (status)
            {
                _mResetEvent.Set();
                _isTaskPause = false;
            }
            else // 暂停
            { 
                _mResetEvent.Reset();
                _isTaskPause = true;
                Log.Error("father get pause meg" + DateTime.Now.ToString());
            }
        }

        private void ImportConfig_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.InitialDirectory = Environment.CurrentDirectory + @"\Resources";
                dialog.Title = "选择文件";
                dialog.Filter = "数据结构文件(*.Json)|*.Json";
                dialog.DefaultExt = "Json";
                dialog.RestoreDirectory = true;
                if (dialog.ShowDialog() == true)
                {

                    string? json = FileHelper.ReadJsonFile(dialog.FileName);
                    if (json != null)
                    {
                        List<ParaInfModel>? paraInfModels = JsonConvert.DeserializeObject<List<ParaInfModel>>(json);

                        ObservableCollection<ParaModel> models = new ObservableCollection<ParaModel>();
                        foreach (var data in paraInfModels)
                        {
                            ParaModel paraModel = new ParaModel()
                            {
                                DataAddress = data.DataAddress,
                                DataName = data.DataName,
                                Remark = data.Remark,
                                DataLength = data.DataLength,
                                DataGain = data.DataGain,
                                DataUnit = data.DataUnit,
                                IsSigned = data.IsSigned,
                            };
                            models.Add(paraModel);
                        }

                        BaseDataView baseDataView = new BaseDataView(_containerProvider, _eventAggregator);
                        baseDataView.SetBaseInf(dialog.SafeFileName.Split('.').First(), models);

                        foreach (var cld in wpView.Children)
                        {
                            if ((cld as BaseDataView).Header == baseDataView.Header)
                            {
                                MessageBox.Show("无法导入同名配置！");
                                return;
                            }
                        }
                        wpView.Children.Add(baseDataView);

                    }
                }


                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.Error(ex.Message);
                return;
            }

        }

        private void StartMonitor_Click(object sender, RoutedEventArgs e)
        {
            btnImport.IsEnabled = false;
            btnStartM.IsEnabled = false;
            btnStopM.IsEnabled = true;
            _isTaskPause = false;
            _readDataCts = new CancellationTokenSource();
            _refreshViewCts = new CancellationTokenSource();
            _viewSource = new Dictionary<string, ObservableCollection<ParaModel>>();
            if (wpView.Children.Count > 0)
            {
                for (int i = 0; i < wpView.Children.Count; i++)
                {
                    var view = wpView.Children[i] as BaseDataView;
                    if (view != null)
                    {
                        _viewSource.Add(view.Header, view.DataModels);
                    }
                }
            }

            _viewSource2 = _viewSource;


            Task task1 = Task.Run(() => ReadData(), _readDataCts.Token);
            Task task2 = Task.Run(() => RefreshChildView(), _refreshViewCts.Token);
            _mResetEvent.Set();


        }

        public void StopMonitor()
        {
            btnImport.IsEnabled = true;
            btnStartM.IsEnabled = true;
            btnStopM.IsEnabled = false;
            _readDataCts.Cancel();
            _refreshViewCts.Cancel();
        }

        private void StopMonitor_Click(object sender, RoutedEventArgs e)
        {
            StopMonitor();
        }

        private object _refreshLocker = new object();
        private void RefreshChildView()
        {
            while (!_refreshViewCts.IsCancellationRequested)
            {
                Thread.Sleep(1000);
                App.Current.Dispatcher.Invoke(() =>
                {

                    try
                    {
                        if (wpView.Children.Count > 0)
                        {
                            for (int i = 0; i < wpView.Children.Count; i++)
                            {
                                var view = wpView.Children[i] as BaseDataView;

                                lock (_refreshLocker)
                                {
                                    view.RefreshView(_viewSource2[view.Header]);
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.Message);
                        return;
                    }

                });
            }
        }


        private object _dataLocker = new object();
        // 读取寄存器数值任务
        private void ReadData()
        {
            while (!_readDataCts.IsCancellationRequested)
            {                 
                Thread.Sleep(100);

                //检测到子界面通知暂停标志后，等待上一次循环结束后通知子界面开始使用串口，防止同时调用数据错误
                if (_isTaskPause)
                {
                    Log.Error("father send" + DateTime.Now.ToString());
                    _eventAggregator.GetEvent<NotifyTaskStatus>().Publish(true);
                }
                //用来控制是否需要继续和暂停
                _mResetEvent.WaitOne();
                try
                {
                    if (_viewSource.Count > 0)
                    {
                        for (int i = 0; i < _viewSource.Count; i++)
                        {

                            var data = _viewSource.ElementAt(i).Value;

                            if (data.Count > 0)
                            {
                                ushort sAddr = data.First().DataAddress;
                                ushort eAddr = data.Last().DataAddress;
                                ushort len = data.Last().DataLength;
                                byte[] result = new byte[] { };
                                if (_serialDevice.GetStatus())
                                {
                                    if (_serialDevice.ReadData(BitConverter.GetBytes(sAddr).Reverse().ToArray(), (ushort)(eAddr - sAddr + len), ref result))
                                    {
                                        for (int j = 0; j < data.Count(); j++)
                                        {
                                            int index = data[j].DataAddress - sAddr;
                                            dynamic value = 0;


                                            // 数据长度筛选
                                            switch (data[j].DataLength)
                                            {
                                                case 1:
                                                    value = (result[index * 2] << 8) + result[index * 2 + 1];
                                                    if (data[j].IsSigned)
                                                        value = (short)value;
                                                    else
                                                        value = (ushort)value;
                                                    break;
                                                case 2:
                                                    value = (result[index * 2] << 24) + (result[index * 2 + 1] << 16) + (result[index * 2 + 2] << 8) + result[index * 2 + 3];
                                                    if (data[j].IsSigned)
                                                        value = (int)value;
                                                    else
                                                        value = (uint)value;
                                                    break;
                                                case 3:
                                                    value = (result[index * 2] << 40) + (result[index * 2 + 1] << 32) + (result[index * 2 + 2] << 24) +
                                                            (result[index * 2 + 3] << 16) + (result[index * 2 + 4] << 8) + result[index * 2 + 5];
                                                    if (data[j].IsSigned)
                                                        value = (long)value;
                                                    else
                                                        value = (ulong)value;
                                                    break;
                                                default:
                                                    break;

                                            }

                                            // 增益判断
                                            switch (Convert.ToDouble(data[j].DataGain))
                                            {
                                                case 0.05:
                                                    data[j].DataValue = (value * 20).ToString();
                                                    break;
                                                case 1:
                                                    data[j].DataValue = value.ToString();
                                                    break;
                                                case 10:
                                                    data[j].DataValue = (value * 0.1).ToString("0.0");
                                                    break;
                                                case 100:
                                                    data[j].DataValue = (value * 0.01).ToString("0.00");
                                                    break;
                                                case 1000:
                                                    data[j].DataValue = (value * 0.001).ToString("0.000");
                                                    break;
                                                default:
                                                    //MessageBox.Show("不支持该增益,请联系管理员!");
                                                    break;
                                            }


                                        }

                                        lock (_dataLocker)
                                        {
                                            _viewSource2[_viewSource.ElementAt(i).Key] = data;
                                        }

                                    }
                                }


                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                    return;
                }
            }
        }
    }
}
