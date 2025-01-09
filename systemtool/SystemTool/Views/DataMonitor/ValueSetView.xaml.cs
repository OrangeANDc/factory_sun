using Prism.Events;
using Prism.Ioc;
using System;
using System.Collections.Generic;
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
using SystemTool.Messenager;
using SystemTool.Model;
using SystemTool.Protocol;
using SystemTool.StaticSource;

namespace SystemTool.Views.DataMonitor
{
    /// <summary>
    /// ValueSetView.xaml 的交互逻辑
    /// </summary>
    public partial class ValueSetView : Window
    {
        private DataModel _dataModel;
        private SerialDevice _serialDevice;
        private IEventAggregator _eventAggregator;
        private bool _isTaskPause;
         

        public ValueSetView( IContainerProvider containerProvider, IEventAggregator ea)
        {
            InitializeComponent();
            

            _serialDevice = containerProvider.Resolve<SerialDevice>();
            _eventAggregator = ea;
            _eventAggregator.GetEvent<NotifyTaskStatus>().Subscribe(TaskPauseFlag);

        }

        public void  SetViewConfig(DataModel dataModel)
        {
            _dataModel = dataModel;
            tbDataAddress.Text = dataModel.DataAddress.ToString();
            tbDataGain.Text = dataModel.DataGain.ToString();
            tbDataName.Text = dataModel.DataName.ToString();
            tbDataLength.Text = dataModel.DataLength.ToString();

            tbDataType.Text = dataModel.IsSigned ? "有符号" : "无符号";

            if (dataModel.Remark == null)
                tbDataRemark.Text = "无";
            else
                tbDataRemark.Text = dataModel.Remark?.ToString();
        }

        private void TaskPauseFlag(bool isPause)
        {
            _isTaskPause = isPause;
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbValueSet.Text))
                {
                    MessageBox.Show("请输入需要写入的数值!");
                    return;
                }

                double value = Convert.ToDouble(tbValueSet.Text);
                byte[] valueArray = null;
                double gain = Convert.ToDouble(_dataModel.DataGain);
                //有符号判断
                if (_dataModel.IsSigned)
                {

                    switch (_dataModel.DataLength)
                    {
                        case 1:
                            value = (short)(value * gain);
                            valueArray = BitConverter.GetBytes((short)value).Reverse().ToArray();
                            break;
                        case 2:
                            value = (int)(value * gain);
                            valueArray = BitConverter.GetBytes((int)value).Reverse().ToArray();
                            break;
                        case 3:
                            value = (long)(value * gain);
                            valueArray = BitConverter.GetBytes((long)value).Reverse().ToArray();
                            break;
                        default:
                            MessageBox.Show("不支持长度3以上的数据,请联系管理员!");
                            return;
                    }
                }
                else
                {

                    switch (_dataModel.DataLength)
                    {
                        case 1:
                            value = (ushort)(value * gain);
                            valueArray = BitConverter.GetBytes((ushort)value).Reverse().ToArray();
                            break;
                        case 2:
                            value = (uint)(value * gain);
                            valueArray = BitConverter.GetBytes((uint)value).Reverse().ToArray();
                            break;
                        case 3:
                            value = (ulong)(value * gain);
                            valueArray = BitConverter.GetBytes((ulong)value).Reverse().ToArray();
                            break;
                        default:
                            MessageBox.Show("不支持长度3以上的数据,请联系管理员!");
                            return;
                    }
                }

                byte[] address = BitConverter.GetBytes(_dataModel.DataAddress).Reverse().ToArray();

                Log.Error("child start: " + DateTime.Now.ToString());
                _isTaskPause = false;
                _eventAggregator.GetEvent<ControlTaskStatus>().Publish(false);

                int ticks = 0;
                while (!_isTaskPause)
                {
                    Thread.Sleep(50);
                    ticks++;
                    if (ticks == 100) 
                    {
                        MessageBox.Show("超时，不支持在非监控状态下设置值!");
                        return;
                    }
                }

                if (!_serialDevice.WriteMulData(address, valueArray))
                {
                    MessageBox.Show("写入失败");
                    return;
                }

                _eventAggregator.GetEvent<ControlTaskStatus>().Publish(true);

                this.DialogResult = true;
                this.Close();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

        }
    }
}
