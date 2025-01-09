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
    public partial class SafetyView : Window
    {
        public string SafetyName = "";
        public SafetyView()
        {
            InitializeComponent();
            cbSafety.ItemsSource = new List<string>()
            {
/*                "50HzDefault",
                "60HzDefault",
                "VDE4105",
                "ES:RD1699",
                "EN50549",
                "Italy",
                "EN50549(CZ)",
                "EN50549(TR)",
                "EN50549(IE)",
                "EN50549(SE)",
                "Poland",
                "EN50549(HR)",
                "Belgium",
                "VDE0126",*/
                "Austria",
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(cbSafety.Text))
            {
                MessageBox.Show("Please Select Safety!");
                return;
            }

            SafetyName = cbSafety.Text;
            this.DialogResult = true;
            this.Close();
        }
    }
}
