using Prism.DryIoc;
using Prism.Events;
using Prism.Ioc;
using Prism.Regions;
using SunwaysFactoryProgram.Messenager;
using SunwaysFactoryProgram.Protocol;
using SunwaysFactoryProgram.StaticSource;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace SunwaysFactoryProgram.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        private IContainerProvider _containerProvider;
        private event ExitEventHandler _exitHander;
        private IEventAggregator _eventAggregator;
        private IRegionManager _regionManager;
        private Type _packType;
        public MainView(IContainerProvider containerProvider,IEventAggregator ea, IRegionManager region)
        {          
            InitializeComponent();
            _eventAggregator = ea;
            _eventAggregator.GetEvent<Messenge_MainViewInf_Event>().Subscribe(SetViewInf);
            _eventAggregator.GetEvent<TextFocusEvent>().Subscribe(FocusTextCode);
            _containerProvider = containerProvider;
            _regionManager = region;
            this._exitHander += Window_Closing;
            tbCode.Focus();
        }


        private void SetViewInf(List<string> inf)
        {
            tbUser.Text = "当前用户:" + inf[0];
            tbProduce.Text = inf[1];
            tbStationName.Text = "测试站点:" + inf[2];          
            // IRegion sas = _regionManager.Regions["ContentRegion"];

        }

        private void FocusTextCode()
        {
            tbCode.Focus();
        }


        private void Window_Closing(object? sender, ExitEventArgs e)
        {
            MessageBoxResult msgBoxResult;
            if (e.Msg == "注销系统")
            {
                msgBoxResult = MessageBox.Show("确定注销系统?", "注销系统", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (msgBoxResult == MessageBoxResult.No)
                {
                    e.Cancel = true;
                   
                    
                    return;
                }
                else
                {
                    _containerProvider.Resolve<LoginView>().Show();
                    this.Hide();
                    _eventAggregator.GetEvent<LogoutInitEvent>().Publish();
                    cbPorts.Text = string.Empty;

                    e.Cancel = false;
                }
            }
            else
            {
                msgBoxResult = MessageBox.Show("确定退出系统?", "退出系统", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (msgBoxResult == MessageBoxResult.No)
                {
                    e.Cancel = true;
                    return;
                }
                else
                {
                    e.Cancel = false;
                    Environment.Exit(0);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this._exitHander != null)
            {
                ExitEventArgs exitEventArgs = new ExitEventArgs("注销系统");
                this._exitHander(this, exitEventArgs);
            }
            

        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {                 
                    string sn = tbCode.Text.Trim();
                    if (sn.Length == 0)
                    {
                        MessageBox.Show("请扫描序列号!");
                        return;
                    }
                    if (sn.Length < 16)
                    {
                        MessageBox.Show("序列号长度错误!");
                        return;
                    }
                    string str = sn.Substring(2,1);
                    if (str != "0" && str != "1" && str != "2")
                    {
                        MessageBox.Show("序列号格式错误!");
                        return;
                    }

                    var region = _regionManager.Regions["ContentRegion"].Views;
          
                    if (tbProduce.Text == "功能测试")
                    {
                        foreach (var view in region)
                        {
                            if (view.GetType() == typeof(FunctionView))
                            {
                                FunctionView? childview = view as FunctionView;
                                if (childview != null)
                                {
                                    childview.TestPrepare(sn, tbUser.Text, tbStationName.Text);
                                }
                            }
                        }
                        
                    }
                    else
                    {
                        foreach (var view in region)
                        {
                            if (view.GetType() == typeof(PackView))
                            {
                                PackView? childview = view as PackView;
                                if (childview != null)
                                {
                                    childview.TestPrepare(sn, tbUser.Text, tbStationName.Text);
                                }
                            }
                            else if (view.GetType() == typeof(Low_PackView))
                            {
                                Low_PackView? childview = view as Low_PackView;
                                if (childview != null)
                                {
                                    childview.TestPrepare(sn, tbUser.Text, tbStationName.Text);
                                }
                            }
                        }
                    }

                    tbCode.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "Error");
                }

            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbLog.ScrollToEnd();
        }
    }

    public delegate void ExitEventHandler(object? sender, ExitEventArgs e);
    public class ExitEventArgs : CancelEventArgs
    {
        public ExitEventArgs(string msg = "") 
        {
            Msg = msg;
        }

        public string Msg { get; set; }
    }
}
 