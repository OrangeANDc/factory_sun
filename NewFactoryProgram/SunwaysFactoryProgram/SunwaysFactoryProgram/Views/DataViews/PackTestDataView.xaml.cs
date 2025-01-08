using SunwaysFactoryProgram.DBModel;
using SunwaysFactoryProgram.StaticSource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Data;
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
using ImTools;

namespace SunwaysFactoryProgram.Views.DataViews
{
    /// <summary>
    /// PackTestDataView.xaml 的交互逻辑
    /// </summary>
    public partial class PackTestDataView : Window
    {
        private IFreeSql fsql = DBHelper.SqlEntity;
        public PackTestDataView(bool isToday = false)
        {
            InitializeComponent();
            if (isToday)
            {
                dpStart.SelectedDate = DateTime.Today;
                dpEnd.SelectedDate = DateTime.Today.AddDays(1);
                List<TBS_PackTest> datas = new List<TBS_PackTest>();
                datas = fsql.Select<TBS_PackTest>().Where(x => x.TestTime.Between(
                       (DateTime)dpStart.SelectedDate, (DateTime)dpEnd.SelectedDate)).ToList();

                Load(datas);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<TBS_PackTest> datas = new List<TBS_PackTest>();

            if (dpStart.SelectedDate != null && dpEnd.SelectedDate != null)
            {
                if (DateTime.Compare((DateTime)dpStart.SelectedDate, (DateTime)dpEnd.SelectedDate) >= 0)
                {
                    MessageBox.Show("开始时间必须早于结束时间");
                    return;
                }

                if (string.IsNullOrEmpty(tbNum.Text))
                {
                    datas = fsql.Select<TBS_PackTest>().Where(x => x.TestTime.Between(
                        (DateTime)dpStart.SelectedDate, (DateTime)dpEnd.SelectedDate)).ToList();
                }
                else
                    datas = fsql.Select<TBS_PackTest>().Where(x => x.TestTime.Between(
                        (DateTime)dpStart.SelectedDate, (DateTime)dpEnd.SelectedDate) && x.InverterSN == tbNum.Text).ToList();

            }
            else
            {
                if (string.IsNullOrEmpty(tbNum.Text))
                    return;
                else
                    datas = fsql.Select<TBS_PackTest>().Where(x=> x.InverterSN == tbNum.Text).ToList();
            }
            Load(datas);
        }

        private void Load(List<TBS_PackTest> datas)
        {
            dgPackTest.ItemsSource = datas;
            if (datas.Count == 0)
            {
                tbAll.Text = "";
                tbFail.Text = "";
                tbSuccess.Text = "";
                tbPercent.Text = "";
            }
            else
            {
                int failCount = 0;
                foreach (var d in datas)
                {
                    if (d.TestResult == "FAIL")
                        failCount++;
                }

                tbAll.Text = datas.Count.ToString();
                tbFail.Text = failCount.ToString();
                tbSuccess.Text = (datas.Count - failCount).ToString();
                double percent = ((double)(datas.Count - failCount)) / (double)datas.Count;
                tbPercent.Text = percent.ToString("0.00");
            }
        }

        private void dgPackTest_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
            TBS_PackTest? tBS_PackTest = e.Row.Item as TBS_PackTest;
            if (tBS_PackTest != null)
            {
                if (tBS_PackTest.TestResult == "FAIL")
                {
                    e.Row.Foreground = new SolidColorBrush(Colors.Red);
                }
            }

                      
        }

        private void dgPackTest_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var result = e.PropertyName;
            var p = (e.PropertyDescriptor as PropertyDescriptor)?.ComponentType.GetProperties().FirstOrDefault(x => x.Name == e.PropertyName);

            if (p != null)
            {
                var found = p.GetCustomAttribute<DisplayAttribute>();
                if (found != null) result = found.Name;
            }

            if (e.PropertyName == "Tester" || e.PropertyName == "StationId" || e.PropertyName == "OdmSN")
                e.Column.Visibility = Visibility.Collapsed;         
            else
            {
                e.Column.Header = result;
                e.Column.IsReadOnly = true;
            }
        }
    }
}
