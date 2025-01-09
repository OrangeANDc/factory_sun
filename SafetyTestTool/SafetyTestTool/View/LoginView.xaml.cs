using SafetyTestTool.ViewModel;
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
using System.Windows.Shapes;

namespace SafetyTestTool.View
{
    /// <summary>
    /// LoginView.xaml 的交互逻辑
    /// </summary>
    public partial class LoginView : Window
    {
        private LoginViewModel viewModel = new LoginViewModel();
        public LoginView()
        {
            InitializeComponent();         
            this.DataContext = viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if ((viewModel.PassWord)?.ToUpper() != "SUN123")
            {
                MessageBox.Show("Invalid PassWord!");
                return;
            }
            else
            {
                MainView mainView = new MainView();
                mainView.Show();
                this.Close();
            }
        }
    }
}
