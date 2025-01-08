using SunwaysFactoryProgram.StaticSource;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// ParaSetView.xaml 的交互逻辑
    /// </summary>
    public partial class ParaSetView : Window
    {
        private string INIPath123 = System.AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\BurnParaSet.ini";
        public ParaSetView()
        {
            InitializeComponent();
            Load();
        }

        private void Load()
        {
            try 
            {
                tbPVTime.Text = Methods.INIRead("老化参数", "PV老化时间", INIPath123);
                tbBatteryTime.Text = Methods.INIRead("老化参数", "电池老化时间", INIPath123);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (tbBatteryTime.Text == "" || tbPVTime.Text == "")
            {
                MessageBox.Show("参数输入为空!");
                return;
            }

            try
            {
                if (File.Exists(INIPath123))
                {
                    Methods.INIWrite("老化参数", "老化房", "SUN-ROOM-001", INIPath123);
                    Methods.INIWrite("老化参数", "PV老化时间", Convert.ToInt32(tbPVTime.Text).ToString(), INIPath123);
                    Methods.INIWrite("老化参数", "电池老化时间", Convert.ToInt32(tbBatteryTime.Text).ToString(), INIPath123);
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
    }
    
}
