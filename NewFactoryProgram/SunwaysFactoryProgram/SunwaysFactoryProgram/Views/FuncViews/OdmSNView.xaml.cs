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

namespace SunwaysFactoryProgram.Views.FuncViews
{
    /// <summary>
    /// OdmSNView.xaml 的交互逻辑
    /// </summary>
    public partial class OdmSNView : Window
    {
        public string OdmSN = "";
        public OdmSNView()
        {
            InitializeComponent();
            tbOdmSN.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string str = this.tbOdmSN.Text.Trim();
            if (string.IsNullOrEmpty(str))
            {
                MessageBox.Show("请输入ODM序列号");
                return;
            }

            if (str.Length != 12)
            {
                MessageBox.Show("ODM序列号长度不正确");
                return;
            }
            if (str.Substring(0, 4) != "5315")
            {
                MessageBox.Show("ODM序列号格式不正确");
                return;
            }

            this.OdmSN = str;
            this.DialogResult = true;
            this.Close();
        }

        private void tbOdmSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string str = this.tbOdmSN.Text.Trim();

                if (str.Length != 12)
                {
                    MessageBox.Show("ODM序列号长度不正确");
                    return;
                }
                if (str.Substring(0, 4) != "5315")
                {
                    MessageBox.Show("ODM序列号格式不正确");
                    return;
                }

                this.OdmSN = str;
                this.DialogResult = true;
                this.Close();
            }
        }
    }
}
