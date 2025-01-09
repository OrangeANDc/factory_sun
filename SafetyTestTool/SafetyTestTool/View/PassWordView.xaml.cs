using SafetyTestTool.Protocol;
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
    /// SafetyView.xaml 的交互逻辑
    /// </summary>
    public partial class PassWordView : Window
    {
        public string PassWord = "";
        public PassWordView()
        {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbPassWord.Text))
            {
                MessageBox.Show("Please Select Safety!");
                return;
            }

            PassWord = tbPassWord.Text;
            this.DialogResult = true;
            this.Close();
        }
    }
}
