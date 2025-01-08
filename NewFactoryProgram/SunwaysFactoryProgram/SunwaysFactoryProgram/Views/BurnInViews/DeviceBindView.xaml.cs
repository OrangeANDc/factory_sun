using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using SunwaysFactoryProgram.DBModel;
using SunwaysFactoryProgram.StaticSource;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
using System.Reflection;
using SupportProject;
using Prism.Events;
using Microsoft.Win32;
using System.IO;

namespace SunwaysFactoryProgram.Views
{
    /// <summary>
    /// DeviceBindView.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceBindView : Window
    {
        private IFreeSql fsql = DBHelper.SqlEntity;    
        private ObservableCollection<ST_BurnInPosition> _bindedList = new ObservableCollection<ST_BurnInPosition>();
        public DeviceBindView()
        {
            InitializeComponent();
            RefreshDataGrid();
        }

        private void RefreshDataGrid()
        {
            _bindedList = Methods.ConvertList(fsql.Select<ST_BurnInPosition>().Where(x => x.DataStatus == "1").
                OrderBy(x => x.BurnInCar).OrderBy(x => x.InverterSN).ToList());
            gdBindedList.ItemsSource = _bindedList;
        }

        private void gdBindedList_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void gdBindedList_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var result = e.PropertyName;
            var p = (e.PropertyDescriptor as PropertyDescriptor)?.ComponentType.GetProperties().FirstOrDefault(x => x.Name == e.PropertyName);

            if (p != null)
            {
                var found = p.GetCustomAttribute<DisplayAttribute>();
                if (found != null) result = found.Name;
            }

            if (e.PropertyName == "DataStatus")
                e.Column.Visibility = Visibility.Collapsed;
            else
            {
                e.Column.Header = result;
                e.Column.IsReadOnly = true;
            }
        }

        private void tbRoom_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Return)
                return;
            string input = tbRoom.Text.Trim().ToUpper();
            if (input.Length == 12 && input.IndexOf("SUN-ROOM-") >= 0)
            {
                tbRoom.Text = input;
                return;
            }
            tbRoom.Text = "";
            return;
        }

        private void tbCar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Return)
                return;
            string input = tbCar.Text.Trim().ToUpper();
            if (input.Length == 11 && input.IndexOf("SUN-CAR-") >= 0)
            {
                tbCar.Text = input;
                if (tbStatus.Text == "解绑中...")
                {
                    fsql.Delete<ST_BurnInPosition>().Where(x => x.BurnInCar == input).ExecuteAffrows();
                    RefreshDataGrid();
                }

                return;
            }
            tbCar.Text = "";
            return;
        }

        private void tbInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Return)
                return;
            string input = tbInput.Text.Trim().ToUpper();
            if (input == "")
                return;

            if (input == "BIND")
            {
                tbStatus.Text = "绑定中...";
                tbStatus.Background = new SolidColorBrush(Colors.Green);
                tbInput.Text = "";
                return;
            }
            else if (input == "CLEAR")
            {
                tbCar.Text = "";
                tbStatus.Text = "解绑中...";
                tbStatus.Background = new SolidColorBrush(Colors.Yellow);
                tbInput.Text = "";
                return;
            }
            else if (input == "EXCHANGE")
            {
                tbCar.Text = "";
                tbStatus.Text = "更换充电桩...";
                tbStatus.Background = new SolidColorBrush(Colors.Orange);
                tbInput.Text = "";
            }
            else if (input == "END")
            {
                tbStatus.Text = "空 闲";
                tbStatus.Background = new SolidColorBrush(Colors.Gray);
                tbInput.Text = "";
                return;
            }

            if (tbStatus.Text == "绑定中..." && input.Length == 16)
            {
                tbInput.Text = "";
                if (string.IsNullOrEmpty(tbRoom.Text) || string.IsNullOrEmpty(tbCar.Text))
                {
                    MessageBox.Show("老化车或老化房未输入！");
                    return;
                }
                int count = 0;
                foreach (var a in _bindedList)
                {
                    if(a.BurnInCar == tbCar.Text)
                        count++;
                }
                if (count >= 12)
                {
                    MessageBox.Show($"老化车:{tbCar.Text}最多绑定12个序列号！");
                    return;
                }

                if (!BindOneSN(input))
                    return;
                RefreshDataGrid();
            }

            if (tbStatus.Text == "解绑中...")
            {
                if (input.Length == 16)
                {
                    if (_bindedList.Count == 0)
                        return;
                    fsql.Delete<ST_BurnInPosition>().Where(x => x.InverterSN == input).ExecuteAffrows();
                    RefreshDataGrid();
                }             
            }

            if (tbStatus.Text == "更换充电桩..."  && input.Length == 16)
            {
                if (string.IsNullOrEmpty(tbRoom.Text) || string.IsNullOrEmpty(tbCar.Text))
                {
                    MessageBox.Show("老化车或老化房未输入！");
                    return;
                }

                bool isExist = false;
                foreach (var a in _bindedList)
                {
                    if (a.InverterSN == input)
                    {
                        isExist = true;
                        break;
                    }
                        
                }

                if (!isExist)
                {
                    MessageBox.Show("当前列表中没有该序列号！");
                    return;
                }

                fsql.Delete<ST_BurnInPosition>().Where(x => x.InverterSN == input).ExecuteAffrows();
                if (!BindOneSN(input))
                    return;
                RefreshDataGrid();
                tbInput.Text = "";
            }

           
        }

        private bool BindOneSN(string sn)
        {
            try
            {
                ST_BurnInPosition sT_BurnInPosition = new ST_BurnInPosition()
                {
                    BurnInRoom = tbRoom.Text,
                    BurnInCar = tbCar.Text,
                    InverterSN = sn,
                    CreateTime = DateTime.Now,
                    DataStatus = "1"
                };
                var isReport = fsql.Select<ST_BurnInPosition>().Where(x => x.InverterSN == sn).ToList();
                if (isReport != null && isReport.Count > 0)
                {
                    MessageBox.Show("序列号已绑定,请重新输入!");
                    return true;
                }
                fsql.Insert<ST_BurnInPosition>(sT_BurnInPosition).ExecuteAffrows();

                return true;
            }
            catch (Exception ex) 
            {
                MessageBox.Show("绑定失败");
                Log.Error(ex.Message);
                return false;
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Variable.BurninService.ChangeBurninView();
            Variable.BurninService.ChangeBurninData();
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            if (tbStatus.Text != "绑定中...")
            {
                MessageBox.Show("请设置为绑定模式");
                return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "*.txt;|*.txt;";
            if (openFileDialog.ShowDialog() != true)
            {
                return;
            }
            else
            {
                foreach (string readAllLine in File.ReadAllLines(openFileDialog.FileName))
                {
                    string strInverter = readAllLine.ToUpper().Trim();
                    if (strInverter.Length == 16)
                    {
                        BindOneSN(strInverter);
                    }
                }
            }

            RefreshDataGrid();
        }
    }
}
