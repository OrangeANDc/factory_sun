using CANDeviceUpgrade.Protocol;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace CANDeviceUpgrade
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        
        private Device _device = new Device();
        public MainWindow()
        {
            InitializeComponent();
            SetFuncStatus(true);
            tbProcess.Text = "刷写进度:";

            Variable._delegateService.OnAddInfo += AddMessage;
            Variable._delegateService.OnSetProcess += SetProcess;
        }

        private void SetFuncStatus(bool status)
        {
            cbChannel.IsEnabled = status;
            btnConnect.IsEnabled = status;
            btnClose.IsEnabled = !status;
            btnStartCAN.IsEnabled = !status;
            btnResetCAN.IsEnabled = !status;
            gbUpgradeSet.IsEnabled = !status;

            if (status)
            {
                tbStatus.Text = "未连接";
                tbStatus.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                tbStatus.Text = "已连接";
                tbStatus.Foreground = new SolidColorBrush(Colors.Green);
            }
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            if (cbChannel.SelectedIndex < 0)
            {
                MessageBox.Show("清选择设备通道号");
                return;
            }
            _device._canChannel = (uint)cbChannel.SelectedIndex;

            if (_device.Connect())
                SetFuncStatus(false);
            else
                return;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (_device.Disconnect())
                SetFuncStatus(true);
            else
                return;
        }

        private void btnStartCAN_Click(object sender, RoutedEventArgs e)
        {
            _device.StartCAN(); 
        }

        private void btnResetCAN_Click(object sender, RoutedEventArgs e)
        {
            _device.ResetCAN();
        }

        private void btnChoose_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;      //该值确定是否可以选择多个文件
            dialog.Title = "请选择需要升级的文件";     //弹窗的标题
            dialog.InitialDirectory = tbChooseFile.Text;       //默认打开的文件夹的位置
            dialog.Filter = "Bin文件(*.bin)|*.bin";       //筛选文件

            if (dialog.ShowDialog() == true)
            {
                tbChooseFile.Text = dialog.FileName;
                _fileName = dialog.SafeFileName;
            }       
        }

        private string _fileName;
        private async void btnUpgrade_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbChooseFile.Text))
            {
                MessageBox.Show("清选择升级文件!");
                return;
            }

            ListMessage = null;
            SetProcess(0);

            byte[] binChar = new byte[] { }; //读取bin文件数组
            int fileLen = 0;
            using (var fileStream = new FileStream(tbChooseFile.Text, FileMode.Open, FileAccess.Read))
            {
                BinaryReader br = new BinaryReader(fileStream, Encoding.UTF8);
                fileLen = (int)fileStream.Length;
                binChar = br.ReadBytes(fileLen);
            }

            var task = new Task(() => UpgradeBMS(binChar));
            task.Start();
            await task;
        }

        private void UpgradeBMS(byte[] binChar)
        {

            ushort packNum;
            if ((binChar.Length % 128) == 0)
                packNum = (ushort)(binChar.Length / 128);
            else
                packNum = (ushort)(binChar.Length / 128 + 1);



            if (!_device.SendFileCheck(_fileName.Substring(1, 8)))
            {
                AddMessage("发送文件名校验失败!");
                return;
            }
            AddMessage("发送文件名校验成功");

            if (!_device.SendUpgradeJump(packNum))
            {
                AddMessage("发送升级跳转指令失败");
                return;
            }
            AddMessage("发送升级跳转指令成功");

            if (!_device.SendUpgradeClean())
            {
                MessageBox.Show("发送升级擦除指令失败");
                return;
            }
            AddMessage("发送升级擦除指令成功");


            int total = (int)((100.0 / packNum) * 100);
            double percent = total / 100.0;
            byte[] firstFrame = new byte[8];
            firstFrame[0] = 0xAA;
            firstFrame[1] = 0x55;
            firstFrame[2] = 0x80;
            //升级过程
            for (int i = 0; i < packNum; i++)
            {
                byte checkSum = 0;
                if (i != (packNum - 1))
                {

                    // 计算checkSum data1~128和
                    for (int j = 0; j < 128; j++)
                    {
                        checkSum += binChar[i * 128 + j];
                    }
                    firstFrame[7] = checkSum;

                    // 首帧
                    // byte4 ~7 第1包为0，第2包128，…，第N包为128*(N-1)
                    Array.Copy(BitConverter.GetBytes((uint)(128 * i)).Reverse().ToArray(), 0, firstFrame, 3, 4);
                    //数据帧 长度128 
                    byte[] sendDatas = new byte[128];
                    Array.Copy(binChar, i * 128, sendDatas, 0, 128);

                    //发送一包数据
                    if (!_device.UpgradeOnePack(firstFrame, sendDatas))
                    {
                        AddMessage($"发送第{i + 1}包数据失败");
                        return;
                    }
                    AddMessage($"发送第{i + 1}包数据成功");
                    SetProcess(percent * (i + 1));
                }
                else //尾包
                {
                    // 计算checkSum
                    for (int p = i * 128; p < binChar.Length; p++)
                    {
                        checkSum += binChar[p];
                    }
                    firstFrame[7] = checkSum;
                    // 首帧
                    // byte4 ~7 第1包为0，第2包128，…，第N包为128*(N-1)
                    Array.Copy(BitConverter.GetBytes((uint)(128 * i)).Reverse().ToArray(), 0, firstFrame, 3, 4);

                    //尾包数据 不足128 0xff 填充
                    byte[] tailFrame = new byte[128];
                    for (int k = 0; k < 128; k++)
                    {
                        tailFrame[k] = 0xff;
                    }
                    Array.Copy(binChar, i * 128, tailFrame, 0, binChar.Length - i * 128);

                    //发送一包数据
                    if (!_device.UpgradeOnePack(firstFrame, tailFrame))
                    {
                        AddMessage($"发送第{i + 1}包数据失败");
                        return;
                    }
                    AddMessage($"发送第{i + 1}包数据成功");
                    SetProcess(percent * (i + 1));
                }
            }




            if (!_device.SendUpgradeFinish())
            {
                AddMessage("发送升级完成指令失败");
                return;
            }
            AddMessage("发送升级完成指令成功");
            SetProcess(100);
            AddMessage("BMS升级完成");

        }

        private void SetProcess(double value)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                if (value == 0)
                {
                    pbProcess.Value = 0;
                    tbProcess.Text = "升级进度:";
                    return;
                }
                pbProcess.Value = value;
                tbProcess.Text = "升级进度:" + value + "%";
            }));
        }

        private ObservableCollection<Message> ListMessage;
        public void AddMessage(string s)
        {
            
            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                if (ListMessage == null)
                {
                    ListMessage = new ObservableCollection<Message>();
                    gd1.ItemsSource = ListMessage;
                }
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

    public class Message
    {
        public string Time { get; set; }
        public string Information { get; set; }
    }

    public delegate void AddInfoDelegate(string info);
    public delegate void SetProcessDelegate(double process);

    public class DelegateService
    {
        public event AddInfoDelegate OnAddInfo;
        public event SetProcessDelegate OnSetProcess;

        public void AddInfo(string info)
        {
            if(OnAddInfo != null)
                OnAddInfo(info);
        }

        public void SetPorcess(double process)
        {
            if(OnSetProcess != null)
                OnSetProcess(process);
        }
    }


}
