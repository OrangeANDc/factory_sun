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

namespace SunwaysFactoryProgram.Views.BurnInViews
{
    /// <summary>
    /// ExportType.xaml 的交互逻辑
    /// </summary>
    public partial class ExportTypeView : Window
    {
        public List<string> exportTypes;
        public ExportTypeView()
        {
            exportTypes = new List<string>();
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (cbNormal.IsChecked == true)
                exportTypes.Add("正常");
            if (cbPass.IsChecked == true)
                exportTypes.Add("PASS");
            if (cbAbnormal.IsChecked == true)
                exportTypes.Add("异常");
            if (cbOffline.IsChecked == true)
                exportTypes.Add("离线");
            if (cbFail.IsChecked == true)
                exportTypes.Add("FAIL");

            this.DialogResult = true;
            this.Close();
        }

        private void cbAll_Checked(object sender, RoutedEventArgs e)
        {
            cbNormal.IsChecked      = true;
            cbPass.IsChecked        = true;
            cbAbnormal.IsChecked    = true;
            cbOffline.IsChecked     = true;
            cbFail.IsChecked        = true;
        }

        private void cbAll_Unchecked(object sender, RoutedEventArgs e)
        {
            cbNormal.IsChecked      = false;
            cbPass.IsChecked        = false;
            cbAbnormal.IsChecked    = false;
            cbOffline.IsChecked     = false;
            cbFail.IsChecked        = false;
        }
    }


}
