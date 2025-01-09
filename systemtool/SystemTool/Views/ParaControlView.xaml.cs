using System;
using System.Collections.Generic;
using System.Linq;
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
using SystemTool.Model;
using SystemTool.DelegateService;
using SystemTool.Protocol;
using Prism.Ioc;

namespace SystemTool.Views
{
    /// <summary>
    /// ParaControlView.xaml 的交互逻辑
    /// </summary>
    public partial class ParaControlView : UserControl
    {
        private SerialDevice _serialDevice;
        public ParaControlView(IContainerProvider containerProvider)
        {
            InitializeComponent();
            _serialDevice = containerProvider.Resolve<SerialDevice>();
            DelegateInfo._refreshTabView.OnRefresh += Refresh;
            
        }

        private void dgInf_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void Refresh()
        {
            tbcView.Items.Refresh();
        }

        private void ButtonRead_Click(object sender, RoutedEventArgs e)
        {
            if (!_serialDevice.GetStatus())
            {
                MessageBox.Show("串口未打开!");
                return;
            }

            ParaModel para = (ParaModel)((Button)e.Source).DataContext;
            if (para.DataLength == 0 || para.DataAddress == 0 )
            {
                MessageBox.Show("地址/长度 不能为0");
                return;
            }

            byte[] addr = BitConverter.GetBytes(para.DataAddress).Reverse().ToArray();
            byte[] result = new byte[] { };
            if (_serialDevice.ReadData(addr, para.DataLength, ref result))
            {
                dynamic value = 0;
                for (int i = 0; i < para.DataLength * 2; i++)
                {
                    value += result[i] << (8 * (para.DataLength * 2 - (i + 1)));
                }
                //有符号判断
                if (para.IsSigned)
                {
                    switch (para.DataLength)
                    {
                        case 1:
                            value = unchecked((short)value);
                            break;
                        case 2:
                            value = unchecked((int)value);
                            break;
                        case 3:
                            value = unchecked((long)value);
                            break;
                        default:
                            MessageBox.Show("不支持长度3以上的数据,请联系管理员!");
                            return;
                    }
                }
                else
                {
                    switch (para.DataLength)
                    {
                        case 1:
                           value = (ushort)value;
                            break;
                        case 2:
                            value = (uint)value;
                            break;
                        case 3:
                            value = (ulong)value;
                            break;
                        default:
                            MessageBox.Show("不支持长度3以上的数据,请联系管理员!");
                            return;
                    }
                }

                switch (Convert.ToDouble(para.DataGain))
                {
                    case 0.05:
                        para.DataValue = (value * 20).ToString();
                        break;
                    case 1:
                        para.DataValue = value.ToString();
                        break;
                    case 10:
                        para.DataValue = (value * 0.1).ToString("0.0");
                        break;
                    case 100:
                        para.DataValue = (value * 0.01).ToString("0.00");
                        break;
                    case 1000:
                        para.DataValue = (value * 0.001).ToString("0.000");
                        break;
                    default:
                        MessageBox.Show("不支持该增益,请联系管理员!");
                        return;

                }
                para.CommandInf = DateTime.Now.ToString("hh:mm:ss ") + "Read data succeed.";

            }
            else
            {
                para.DataValue = "";
                para.CommandInf = DateTime.Now.ToString("hh:mm:ss ") + "Read data failed.";
            }

            Refresh();



        }

        private void ButtonWrite_Click(object sender, RoutedEventArgs e)
        {
            if (!_serialDevice.GetStatus())
            {
                MessageBox.Show("串口未打开!");
                return;
            }

            ParaModel para = (ParaModel)((Button)e.Source).DataContext;
            if (para.DataLength == 0 || para.DataAddress == 0 )
            {
                MessageBox.Show("地址/长度");
                return;
            }

            if (string.IsNullOrEmpty(para.DataValue))
            {
                MessageBox.Show("请输入需要写入的数值!");
                return;
            }

            try
            {
                double value = Convert.ToDouble(para.DataValue);
                byte[] valueArray = null;
                double gain = Convert.ToDouble(para.DataGain);

                if (gain < 1)
                {
                    if (gain != 0.05)
                    {
                        MessageBox.Show("不支持该增益,请联系管理员!");
                        return;
                    }
                }
                //有符号判断
                if (para.IsSigned)
                {
                    
                    switch (para.DataLength)
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
                    
                    switch (para.DataLength)
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

                byte[] address = BitConverter.GetBytes(para.DataAddress).Reverse().ToArray();

                if (_serialDevice.WriteMulData(address, valueArray))
                {
                    para.CommandInf = DateTime.Now.ToString("hh:mm:ss ") + "Write data succeed.";
                }
                else
                {
                    para.CommandInf = DateTime.Now.ToString("hh:mm:ss ") + "Write data failed.";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            Refresh();
        }


 
    }
}
