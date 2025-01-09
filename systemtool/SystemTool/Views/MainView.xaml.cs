using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.IO.Ports;
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
using SystemTool.ViewModels;

namespace SystemTool.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        private SerialDevice _serialDevice;
        public MainView(IContainerProvider containerProvider)
        {
            InitializeComponent();
            _serialDevice = containerProvider.Resolve<SerialDevice>();
            cbPorts.ItemsSource = SerialPort.GetPortNames().ToList();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            cbPorts.ItemsSource = SerialPort.GetPortNames().ToList();
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            if (_serialDevice.GetStatus())
                return;
            try
            {


                if (!_serialDevice.Open(cbPorts.Text, Convert.ToInt32(cbBaudRate.Text)))
                {
                    MessageBox.Show("打开串口失败");
                    return;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            tbStatus.Text = "串口已连接";
            cbBaudRate.IsEnabled = cbPorts.IsEnabled = btnRefresh.IsEnabled = btnOpen.IsEnabled = false;

        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {

            if (!_serialDevice.GetStatus())
            {
                tbStatus.Text = "串口未连接";
                cbBaudRate.IsEnabled = cbPorts.IsEnabled = btnRefresh.IsEnabled = btnOpen.IsEnabled = true;
                return;
            }
            if (!_serialDevice.Close())
            {
                MessageBox.Show("关闭串口失败");
                return;
            }
            tbStatus.Text = "串口未连接";
            cbBaudRate.IsEnabled = cbPorts.IsEnabled = btnRefresh.IsEnabled = btnOpen.IsEnabled = true;
        }
    }
}
