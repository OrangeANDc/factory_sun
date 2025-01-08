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

namespace SunwaysFactoryProgram.Views.FuncViews
{
    /// <summary>
    /// NoticeView.xaml 的交互逻辑
    /// </summary>
    public partial class NoticeView : Window
    {
        public NoticeView(string msg, bool retry = false)
        {
            InitializeComponent();
            tbNotice.Text = msg;
            if (retry)
            {
                btnRetry.Visibility = Visibility.Visible;
            }
            else
            {
                btnRetry.Visibility = Visibility.Collapsed;
            }
        }

        private void ButtonYes_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void ButtonNo_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void ButtonRetry_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = null;
            this.Close();
        }
    }
}
