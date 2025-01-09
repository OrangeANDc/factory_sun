using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using SystemTool.Protocol;
using SystemTool.StaticSource;

namespace SystemTool.Views.BaseControl
{
    /// <summary>
    /// AdvanceSettingView.xaml 的交互逻辑
    /// </summary>
    public partial class AdvanceSettingView : UserControl
    {
        private SerialDevice _serialDevice;
        public AdvanceSettingView(IContainerProvider containerProvider)
        {
            InitializeComponent();
            _serialDevice = containerProvider.Resolve<SerialDevice>();
            cbSafety.ItemsSource = Variable._safetyDic.Select(x => x.Value).ToList();
            cbLanguage.ItemsSource = Variable._lanDic.Select(x => x.Value).ToList();
        }

        // SN
        private void BtnSnRead_Click(object sender, RoutedEventArgs e)
        {
            if (!_serialDevice.GetStatus())
            {
                MessageBox.Show("Please Connect SerialPort!");
                return;
            }

            string recvMsg = string.Empty;
            if (_serialDevice.GetFunc(GetCommandType.获取序列号, ref recvMsg))
            {
                tbSn.Text = recvMsg;
                AddMessage("Read SN Succeed!");
                return;
            }
            else
            {
                AddMessage("Read SN Failed!");
                return;
            }
        }

        private void BtnSnSet_Click(object sender, RoutedEventArgs e)
        {
            if (!_serialDevice.GetStatus())
            {
                MessageBox.Show("Please Connect SerialPort!");
                return;
            }

            if (tbSn.Text.Trim().Length != 16)
            {
                MessageBox.Show("SN Length Error!");
                return;
            }

            var byte00 = Encoding.UTF8.GetBytes("0000000000000000");
            var bytes = Encoding.UTF8.GetBytes(tbSn.Text.Trim()).ToArray();

            // 5000
            byte[] address = new byte[] { 0x13, 0x88 };


            if (_serialDevice.WriteMulData(address, byte00) && _serialDevice.WriteMulData(address, bytes))
            {
                AddMessage("Write SN Succeed!");
                return;
            }
            else
            {
                AddMessage("Write SN Failed!");
                return;
            }
        }

        // 安规
        private void BtnSafetyRead_Click(object sender, RoutedEventArgs e)
        {
            if (!_serialDevice.GetStatus())
            {
                MessageBox.Show("Please Connect SerialPort!");
                return;
            }

            // 25000
            byte[] address = new byte[] { 0x61, 0xA8 };
            byte[] result = new byte[] { };

            if (_serialDevice.ReadData(address, 1, ref result))
            {
                ushort safeCode = ((ushort)((result[0] << 8) + result[1]));
                if (Variable._safetyDic.ContainsKey(safeCode))
                {
                    cbSafety.Text = Variable._safetyDic[safeCode];
                    AddMessage("Read Safety Succeed!");
                    return;
                }
                else
                {
                    AddMessage("No Key In Dictionary,Please contanct Admin");
                    return;
                }
            }
            else
            {
                AddMessage("Read Safety Failed!");
                return;
            }
        }

        private void BtnSafetySet_Click(object sender, RoutedEventArgs e)
        {
            if (!_serialDevice.GetStatus())
            {
                MessageBox.Show("Please Connect SerialPort!");
                return;
            }

            if (cbSafety.SelectedIndex < 0)
            {
                MessageBox.Show("Please Select Safety!");
                return;
            }

            // 25000
            byte[] address = new byte[] { 0x61, 0xA8 };
            ushort value = Variable._safetyDic.FirstOrDefault(x => x.Value == cbSafety.Text).Key;

            byte[] data = BitConverter.GetBytes(value).Reverse().ToArray();

            if (_serialDevice.WriteSingleData(address, data))
            {
                AddMessage("Write Safety Succeed!");
                return;
            }
            else
            {
                AddMessage("Write Safety Failed!");
                return;
            }
        }


        // 语言
        private void BtnLanguageRead_Click(object sender, RoutedEventArgs e)
        {
            if (!_serialDevice.GetStatus())
            {
                MessageBox.Show("Please Connect SerialPort!");
                return;
            }

            // 20011
            byte[] address = new byte[] { 0x4E, 0x2B };
            byte[] result = new byte[] { };

            if (_serialDevice.ReadData(address, 1, ref result))
            {
                ushort lanCode = ((ushort)((result[0] << 8) + result[1]));
                if (Variable._lanDic.ContainsKey(lanCode))
                {
                    cbLanguage.Text = Variable._lanDic[lanCode];
                    AddMessage("Read Language Succeed!");
                    return;
                }
                else
                {
                    AddMessage("No Key In Dictionary,Please contanct Administrator");
                    return;
                }
            }
            else
            {
                AddMessage("Read Language Failed!");
                return;
            }
        }

        private void BtnLanguageSet_Click(object sender, RoutedEventArgs e)
        {
            if (!_serialDevice.GetStatus())
            {
                MessageBox.Show("Please Connect SerialPort!");
                return;
            }

            if (cbLanguage.SelectedIndex < 0)
            {
                MessageBox.Show("Please Select Language!");
                return;
            }

            // 20011
            byte[] address = new byte[] { 0x4E, 0x2B };
            ushort value = Variable._lanDic.FirstOrDefault(x => x.Value == cbLanguage.Text).Key;

            byte[] data = BitConverter.GetBytes(value).Reverse().ToArray();

            if (_serialDevice.WriteSingleData(address, data))
            {
                AddMessage("Write Language Succeed!");
                return;
            }
            else
            {
                AddMessage("Write Language Failed!");
                return;
            }
        }

        // 防逆流
        private void BtnExportLimitRead_Click(object sender, RoutedEventArgs e)
        {
            if (!_serialDevice.GetStatus())
            {
                MessageBox.Show("Please Connect SerialPort!");
                return;
            }

            // 25100
            byte[] address = new byte[] { 0x62, 0x0c };
            byte[] result = new byte[] { };

            if (_serialDevice.ReadData(address, 1, ref result))
            {
                ushort value = ((ushort)((result[0] << 8) + result[1]));
                cbExportLimit.SelectedIndex = value;
                AddMessage("Read ExportLimit Succeed!");
            }
            else
            {
                AddMessage("Read ExportLimit Failed!");
                return;
            }
        }

        private void BtnExportLimitSet_Click(object sender, RoutedEventArgs e)
        {
            if (!_serialDevice.GetStatus())
            {
                MessageBox.Show("Please Connect SerialPort!");
                return;
            }

            if (cbExportLimit.SelectedIndex < 0)
            {
                MessageBox.Show("Please Select ExportLimit!");
                return;
            }

            // 25100
            byte[] address = new byte[] { 0x62, 0x0c };
            byte[] data = BitConverter.GetBytes((ushort)cbExportLimit.SelectedIndex).Reverse().ToArray();

            if (_serialDevice.WriteSingleData(address, data))
            {
                AddMessage("Write ExportLimit Succeed!");
                return;
            }
            else
            {
                AddMessage("Write ExportLimit Failed!");
                return;
            }
        }

        // 无功调度
        private void BtnReactiveAdjustRead_Click(object sender, RoutedEventArgs e)
        {
            if (!_serialDevice.GetStatus())
            {
                MessageBox.Show("Please Connect SerialPort!");
                return;
            }

            // 25124
            byte[] address = new byte[] { 0x62, 0x24 };
            byte[] result = new byte[] { };

            if (_serialDevice.ReadData(address, 1, ref result))
            {
                ushort value = ((ushort)((result[0] << 8) + result[1]));
                cbReactiveMode.SelectedIndex = value - 1;
                AddMessage("Read ReactiveMode Succeed!");
            }
            else
            {
                AddMessage("Read ReactiveMode Failed!");
                return;
            }
        }

        private void BtnReactiveAdjustSet_Click(object sender, RoutedEventArgs e)
        {
            if (!_serialDevice.GetStatus())
            {
                MessageBox.Show("Please Connect SerialPort!");
                return;
            }

            if (cbReactiveMode.SelectedIndex < 0)
            {
                MessageBox.Show("Please Select ReactiveMode!");
                return;
            }

            // 25124
            byte[] address = new byte[] { 0x62, 0x24 };
            byte[] data = BitConverter.GetBytes((ushort)cbReactiveMode.SelectedIndex + 1).Reverse().ToArray();

            if (_serialDevice.WriteSingleData(address, data))
            {
                AddMessage("Write ReactiveMode Succeed!");
                return;
            }
            else
            {
                AddMessage("Write ReactiveMode Failed!");
                return;
            }
        }


        private ObservableCollection<Message>? ListMessage;
        public void AddMessage(string s)
        {
            if (ListMessage == null)
            {
                ListMessage = new ObservableCollection<Message>();
                gd1.ItemsSource = ListMessage;
            }
            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                if (ListMessage.Count > 1000)
                {
                    ListMessage.RemoveAt(0);
                }
                ListMessage.Add(new Message { Time = DateTime.Now.ToString("HH:mm:ss"), Information = s });
                if (gd1.Items.Count > 3)
                {
                    var border = VisualTreeHelper.GetChild(gd1, 0) as Decorator;
                    if (border != null)
                    {
                        var scroll = border.Child as ScrollViewer;
                        if (scroll != null)
                        {
                            scroll.ScrollToEnd();
                        }
                    }
                }
            }));

        }

        
    }

    public class Message
    {
        public string Time { get; set; }
        public string Information { get; set; }
    }
}
