using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
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
    public partial class CheckCodeView : Window
    {
        public string CheckCode = "";
        public CheckCodeView()
        {
            InitializeComponent();
            tbCheckCode.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string str = this.tbCheckCode.Text.Trim().ToUpper();
            if (string.IsNullOrEmpty(str))
            {
                MessageBox.Show("请输入CheckCode");
                return;
            }

            if (str.Length != 6)
            {
                MessageBox.Show("CheckCode长度不正确,请重新输入!");
                return;
            }


            this.CheckCode = str;
            this.DialogResult = true;
            this.Close();
        }


        private void tbCheckCode_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                string str = this.tbCheckCode.Text.Trim().ToUpper();
                if (str.Length != 6)
                {
                    MessageBox.Show("CheckCode长度不正确,请重新输入!");
                    return;
                }

                this.CheckCode = str;
                this.DialogResult = true;
                this.Close();
            }
        }
    }
}
