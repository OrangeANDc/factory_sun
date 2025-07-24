using ConfigTool;
using ConfigTool.Protocol;
using FlashTool.Protocol;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Ports;
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
using static MaterialDesignThemes.Wpf.Theme;

namespace FlashTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private ISerial _serial;
        public MainWindow()
        {
            InitializeComponent();
            cbPort.ItemsSource = SerialPort.GetPortNames();
            _serial = new SerialDevice();
            tbProcess.Text = "刷写进度:";
            // _testDevice = new TestDevice();
            SetViewStatus(true);
            TimeHelper.TimeCheck();


        }

        private void OpenBinFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;      //该值确定是否可以选择多个文件
            dialog.Title = "请选择需要升级的文件";     //弹窗的标题
            dialog.InitialDirectory = OpenFilePath.Text;       //默认打开的文件夹的位置
            dialog.Filter = "Bin文件(*.bin)|*.bin";       //筛选文件

            if (dialog.ShowDialog() == true)
            {
                OpenFilePath.Text = dialog.FileName;
            }
        }

        private void SetViewStatus(bool status)
        {
            cbPort.IsEnabled = status;
            cbBaudRate.IsEnabled = status;
            btnOpen.IsEnabled = status;

            btnClose.IsEnabled = !status;
            btnWrite.IsEnabled = !status;
        }

        private void OpenSerial_Click(object sender, RoutedEventArgs e)
        {

            if (_serial.GetStatus())
            {
                return;
            }
            else
            {
                if (string.IsNullOrEmpty(cbPort.Text) || string.IsNullOrEmpty(cbBaudRate.Text))
                {
                    AddMessage("请选择串口号和波特率");
                    return;
                }

                if (_serial.Open(cbPort.Text, cbBaudRate.Text))
                {
                    SetViewStatus(false);
                    AddMessage("串口打开成功");
               
                    return;
                }
                else
                {
                    AddMessage("串口打开失败");
                    return;
                }
            }
        }

        private void CloseSerial_Click(object sender, RoutedEventArgs e)
        {

            if (!_serial.GetStatus())
            {
                return;
            }
            else
            {
                if (_serial.Close())
                {
                    SetViewStatus(true);
                    AddMessage("串口关闭成功");
                    return;
                }
                else
                {
                    AddMessage("串口关闭失败");
                    return;
                }
            }
        }



        private async void StartFlash_Click(object sender, RoutedEventArgs e)
        {
            if(ListMessage != null)
                ListMessage.Clear();
            byte[] binChar = new byte[] { }; //读取bin文件数组
            int fileLen;
            //读取bin文件
            if (string.IsNullOrEmpty(OpenFilePath.Text))
            {
                MessageBox.Show("请选择文件!");
                return;
            }

            using (var fileStream = new FileStream(OpenFilePath.Text, FileMode.Open, FileAccess.Read))
            {
                BinaryReader br = new BinaryReader(fileStream, Encoding.UTF8);
                fileLen = (int)fileStream.Length;
                binChar = br.ReadBytes(fileLen);
            }

            if (binChar[2] == 0x4c)
            {
                Task t = new Task(() => FuncMain_Datalog(binChar, fileLen));
                t.Start();
                await t;
            }
            else if (binChar[2] == 0x4d)
            {
                Task t = new Task(() => FuncMain_Arm(binChar, fileLen));
                t.Start();
                await t;
            }
            else
            {
                // dsp升级
                Task t = new Task(() => FuncMain_Arm(binChar, fileLen));
                t.Start();
                await t;
            }
            //await Task<byte[],int>.Factory.StartNew(FuncMain(binChar, fileLen));      
        }

        private void FuncMain_Datalog(byte[] binChar, int fileLen)
        {       
            string recvMsg = string.Empty;
            if (_serial.GetFunc("获取序列号", ref recvMsg))
            {
                AddMessage("SN:" + recvMsg);
            }
            else
            {
                AddMessage("获取SN失败!");
                SetProcess(0);
                return;
            }

            if (_serial.GetFunc("获取软件版本号", ref recvMsg))
            {
                AddMessage("软件版本号:" + recvMsg);
            }
            else
            {
                AddMessage("获取软件版本号失败!");
                SetProcess(0);
                return;
            }

            //发送升级指令
            byte[] header = binChar.Take(32).ToArray();

            if (_serial.FlashHead_Datalog(header))
            {
                AddMessage("发送文件头成功");
            }
            else
            {
                AddMessage("发送文件头失败");
                SetProcess(0);
                return;
            }

            // 发送实际数据
            byte[] data = new byte[fileLen - 32];
            Array.Copy(binChar, 32, data, 0, fileLen - 32);
            int packNum = (data.Length / 256) + 1;
            byte[] singlePack;
            int total = (int)((100.0 / packNum) * 100);
            double percent = total / 100.0;

            for (int i = 0; i < packNum; i++)
            {
                if (i != packNum - 1)
                {
                    singlePack = new byte[256];
                    Array.Copy(data, i * 256, singlePack, 0, 256);
                }
                else
                {
                    singlePack = new byte[data.Length - 256 * (packNum - 1)];
                    Array.Copy(data, i * 256, singlePack, 0, singlePack.Length);
                }

                if (_serial.Flash_Datalog(singlePack))
                {
                    AddMessage($"写入第{i}包数据成功");
                    SetProcess(percent * (i + 1));
                }
                else
                {
                    int j = 2;
                    while (j < 4)
                    {
                        if (_serial.Flash_Datalog(singlePack))
                        {
                            AddMessage($"第{j}次写入第{i}包数据成功");
                            SetProcess(percent * (i + 1));
                            break;
                        }
                        else
                        {
                            AddMessage($"第{j}次写入第{i}包数据失败");
                        }
                        j++;
                    }

                    if (j == 4)
                    {
                        SetProcess(0);
                        MessageBox.Show("刷写失败");
                        return;
                    }
                    }
                }

            AddMessage($"刷写数据成功!");
            SetProcess(100.0);
        }

        // ARM烧录流程
        private void FuncMain_Arm(byte[] binChar, int fileLen)
        {
            string recvMsg = string.Empty;
            if (_serial.GetFunc("获取序列号", ref recvMsg))
            {
                AddMessage("SN:" + recvMsg);
            }
            else
            {
                AddMessage("获取SN失败!");
                SetProcess(0);
                return;
            }

            //发送文件头
            byte[] header = binChar.Take(32).ToArray();

            if (_serial.FlashHead_Arm(header))
            {
                AddMessage("发送文件头成功");
            }
            else
            {
                AddMessage("发送文件头失败");
                SetProcess(0);
                return;
            }

            // 发送实际数据
            byte[] data = new byte[fileLen - 32];
            Array.Copy(binChar, 32, data, 0, fileLen - 32);
            int packNum = (data.Length / 512) + 1;
            byte[] singlePack = new byte[512];

            int total = (int)((100.0 / packNum) * 100);
            double percent = total / 100.0;
            for (int i = 0; i < packNum; i++)
            {
                if (i != packNum - 1)
                {
                    Array.Copy(data, i * 512, singlePack, 0, 512);
                }
                else
                {
                    singlePack = new byte[data.Length - 512 * (packNum - 1)];
                    Array.Copy(data, i * 512, singlePack, 0, singlePack.Length);
                }

                byte[] count = BitConverter.GetBytes((ushort)i).Reverse().ToArray();
                if (_serial.Flash_Arm(singlePack, count))
                {
                    AddMessage($"写入第{i + 1}包数据成功");
                    SetProcess(percent * (i + 1));
                }
                else
                {
                    AddMessage($"第1次写入第{i + 1}包数据失败");
                    int j = 2;
                    while (j < 4)
                    {
                        if (_serial.Flash_Arm(singlePack, count))
                        {
                            AddMessage($"第{j}次写入第{i + 1}包数据成功");
                            SetProcess(percent * (i + 1));
                            break;
                        }
                        else
                        {
                            AddMessage($"第{j}次写入第{i + 1}包数据失败");
                        }
                        j++;
                    }

                    if (j == 4)
                    {
                        SetProcess(0);
                        MessageBox.Show("刷写失败");
                        return;
                    }
                }
            }

            AddMessage($"刷写数据成功!");
            SetProcess(100.0);
        }

        /*// DSP烧录流程
        private void FuncMain_Dsp(byte[] binChar, int fileLen)
        {
            string recvMsg = string.Empty;
            if (_serial.GetFunc("获取序列号", ref recvMsg))
            {
                AddMessage("SN:" + recvMsg);
            }
            else
            {
                AddMessage("获取SN失败!");
                SetProcess(0);
                return;
            }

            //发送文件头
            byte[] header = binChar.Take(32).ToArray();

            if (_serial.FlashRequset_Arm(header))
            {
                AddMessage("发送文件头成功");
            }
            else
            {
                AddMessage("发送文件头失败");
                SetProcess(0);
                return;
            }

            // 发送实际数据
            byte[] data = new byte[fileLen - 32];
            Array.Copy(binChar, 32, data, 0, fileLen - 32);
            int packNum = (data.Length / 512) + 1;
            byte[] singlePack = new byte[512];
            double percent = Math.Round((101.0 / packNum), 2);
            for (int i = 0; i < packNum; i++)
            {
                if (i != packNum - 1)
                {
                    Array.Copy(data, i * 512, singlePack, 0, 512);
                }
                else
                {
                    singlePack = new byte[data.Length - 512 * (packNum - 1)];
                    Array.Copy(data, i * 512, singlePack, 0, singlePack.Length);
                }

                byte[] count = BitConverter.GetBytes((ushort)i).Reverse().ToArray();
                if (_serial.Flash_Arm(singlePack, count))
                {
                    AddMessage($"写入第{i + 1}包数据成功");
                    SetProcess(percent * (i + 1));
                }
                else
                {
                    AddMessage($"第1次写入第{i + 1}包数据失败");
                    int j = 2;
                    while (j < 4)
                    {
                        if (_serial.Flash_Arm(singlePack, count))
                        {
                            AddMessage($"第{j}次写入第{i + 1}包数据成功");
                            //SetProcess(percent * (i + 1));
                            break;
                        }
                        else
                        {
                            AddMessage($"第{j}次写入第{i + 1}包数据失败");
                        }
                        j++;
                    }

                    if (j == 4)
                    {
                        SetProcess(0);
                        MessageBox.Show("刷写失败");
                        return;
                    }
                }
            }

            AddMessage($"刷写数据成功!");
            SetProcess(100.0);
        }*/



        private void ClearLog_Click(object sender, RoutedEventArgs e)
        {
            ListMessage.Clear();
        }

        private void SetProcess(double value)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                if (value == 0)
                {
                    pbValue.Value = 0;
                    tbProcess.Text = "刷写进度:";
                    return;
                }
                pbValue.Value = value;
                tbProcess.Text = "刷写进度:" + value + "%";
            }));          
        }

        private ObservableCollection<Message> ListMessage;
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
                if (gd1.Items.Count > 8)
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
}
