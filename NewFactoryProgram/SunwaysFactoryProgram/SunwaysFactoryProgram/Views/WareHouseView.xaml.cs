using MaterialDesignThemes.Wpf;
using SunwaysFactoryProgram.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
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
using SunwaysFactoryProgram.DBModel;
using SunwaysFactoryProgram.StaticSource;
using SunwaysFactoryProgram.Views.FuncViews;
using System.Collections.ObjectModel;
using SupportProject;
using Microsoft.Win32;

namespace SunwaysFactoryProgram.Views
{
    /// <summary>
    /// WareHouseView.xaml 的交互逻辑
    /// </summary>
    public partial class WareHouseView : Window
    {
        private ObservableCollection<ST_OutBound> _ReadyNumList = new ObservableCollection<ST_OutBound>();
        private IFreeSql fsql = DBHelper.SqlEntity;
        private WareHouseViewModel _wareHouseSource;
        public WareHouseView()
        {        
            InitializeComponent();
            _wareHouseSource = new WareHouseViewModel();
            DataContext = _wareHouseSource;
            dgNum.ItemsSource = _ReadyNumList;
            dpDateNow.Text = DateTime.Now.ToString();
        }

        private void RefreshOrder()
        {
            _wareHouseSource.OrderList = fsql.Select<ST_OutBound>().Limit(100).OrderByDescending(x=> x.CreationDate).ToList();
        }

        private string GetMaxPalletNum(string tray)
        {
            string sql = "select Max(PalletNum) as ccc from ST_OutBound where PalletNum like '" + tray + "%' and ActiveStatus = '1'";
            var result = fsql.Select<object>().WithSql(sql).ToDataTable("ccc");
            if (result == null)
            {
                return "";
            }
            else
            {
                return result.Rows[0][0].ToString();
            }
        }

        public void CombinedDialogOpenedEventHandler(object sender, DialogOpenedEventArgs eventArgs)
        {
            CombinedCalendar.SelectedDate = ((WareHouseViewModel)DataContext).StartDate;
        }

        public void CombinedEndDialogOpenedEventHandler(object sender, DialogOpenedEventArgs eventArgs)
        {
            CombinedCalendar2.SelectedDate = ((WareHouseViewModel)DataContext).EndDate;
        }

        public void CombinedDialogClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (Equals(eventArgs.Parameter, "1") &&
                CombinedCalendar.SelectedDate is DateTime selectedDate)
            {
                var combined = selectedDate.AddSeconds(CombinedClock.Time.TimeOfDay.TotalSeconds);
                ((WareHouseViewModel)DataContext).StartDate = combined;
            }
        }

        public void CombinedEndDialogClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (Equals(eventArgs.Parameter, "1") &&
                CombinedCalendar2.SelectedDate is DateTime selectedDate)
            {
                var combined = selectedDate.AddSeconds(CombinedClock2.Time.TimeOfDay.TotalSeconds);
                ((WareHouseViewModel)DataContext).EndDate = combined;
            }
        }

        private void dgOrder_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void dgOrder_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var result = e.PropertyName;
            var p = (e.PropertyDescriptor as PropertyDescriptor)?.ComponentType.GetProperties().FirstOrDefault(x => x.Name == e.PropertyName);

            if (p != null)
            {
                var found = p.GetCustomAttribute<DisplayAttribute>();
                if (found != null) result = found.Name;
            }

            if (e.PropertyName == "ActiveStatus")
                e.Column.Visibility = Visibility.Collapsed;
            else
            {
                e.Column.Header = result;
                e.Column.IsReadOnly = true;
            }
        }

        private void dgNum_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void dgNum_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var result = e.PropertyName;
            var p = (e.PropertyDescriptor as PropertyDescriptor)?.ComponentType.GetProperties().FirstOrDefault(x => x.Name == e.PropertyName);

            if (p != null)
            {
                var found = p.GetCustomAttribute<DisplayAttribute>();
                if (found != null) result = found.Name;
            }

            if (e.PropertyName == "ActiveStatus" || e.PropertyName == "OutDate" || e.PropertyName == "OrderID" || e.PropertyName == "Customer" ||
                e.PropertyName == "Remark" || e.PropertyName == "CreationDate" || e.PropertyName == "PalletNum")
                e.Column.Visibility = Visibility.Collapsed;
            else
            {
                e.Column.Header = result;
                e.Column.IsReadOnly = true;
            }
        }

        private void tbContainerNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            string str1 = tbContainerNum.Text.Trim();
            if (str1.Length < 2 || cbTemplate.SelectedIndex != 0)
                return;
            DateTime dateTime = Convert.ToDateTime(dpDateNow.Text);
            string[] strArray = new string[7];
            strArray[0] = "P";
            strArray[1] = dateTime.Year.ToString("D4");
            int num1 = dateTime.Month;
            strArray[2] = num1.ToString("D2");
            num1 = dateTime.Day;
            strArray[3] = num1.ToString("D2");
            strArray[4] = "-";
            strArray[5] = str1;
            strArray[6] = "-";
            string pallet = string.Concat(strArray);
            string maxPalletNum = GetMaxPalletNum(pallet);
            string str2 = pallet + "01";
            if (maxPalletNum.Length > 3)
            {
                int num2 = Convert.ToInt32(maxPalletNum.Substring(maxPalletNum.Length - 2, 2)) + 1;
                str2 = pallet + num2.ToString("D2");
            }
            tbTray.Text = str2;
            _ReadyNumList = new ObservableCollection<ST_OutBound>();
        }

        private void tbContainerNum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Return || cbTemplate.SelectedIndex != 1)
                return;
            string str1 = tbContainerNum.Text.Trim();
            if (str1.IndexOf("/") <= 0 || str1.Length < 3)
                return;
            string str2 = str1;
            if (str1.Substring(0, 3) != "NO.")
                str2 = "NO. " + str1;
            tbTray.Text = str2;
        }

        private void tbInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Return)
                return;
            string str1 = tbInput.Text.Trim();
            bool flag = str1.IndexOf("5315") >= 0 && str1.Length == 12;
            if (str1.Length == 16 | flag)
            {
                
                if (dgNum.Items.Count == 32)
                {
                    MessageBox.Show("最多绑定32个序列号，请保存后更换托盘编号!");
                    tbInput.Text = "";
                    return;
                }
                string str2 = str1;
                string checkCode = !flag ? Methods.CalcCheckCode(str2) : "";
                if (!CheckPackResult(str2))
                {
                    tbInput.Text = "";
                    return;
                }

                if (!AddOneNum(str2, checkCode, tbModel.Text.Trim(), tbInvCode.Text.Trim(), tbConfigInfo.Text.Trim(), tbWarranty.Text.Trim()))
                {
                    tbInput.Text = "";
                    return;
                }              
                tbInput.Text = "";
            }
            if (str1.Trim().ToUpper() == "BIND")
            {
                if (!this.CheckInputData())
                {
                    tbInput.Text = "";
                    return;
                }
                if (!this.CheckSN())
                {
                    tbInput.Text = "";
                    return;
                }
                if (!SaveReadyNumLists())
                {
                    tbInput.Text = "";
                    return;
                }
                RefreshOrder();
                tbInput.Text = "";
            }
            int num1 = str1.Trim().ToUpper() == "CLEAR" ? 1 : 0;
        }


        private bool CheckPackResult(string inverterSN)
        {
            var packTestBySn = fsql.Select<TBS_PackTest>().Where(x=> x.InverterSN == inverterSN || x.OdmSN == inverterSN).OrderByDescending(x=> x.TestTime).First();         
            if (packTestBySn == null)
            {
                MessageBox.Show("序列号" + inverterSN + "未做包装清零测试，不能绑定!");
                return false;
            }
            if (!(packTestBySn.TestResult.ToUpper().Trim() != "PASS"))
                return true;
            MessageBox.Show("序列号" + inverterSN + "包装清零测试失败，不能绑定!");
            return false;
        }

        private bool CheckInputData()
        {
            if (tbOrderId.Text.Trim() == "")
            {
                int num = (int)MessageBox.Show("请输入订单号!");
                return false;
            }
            if (tbCustomer.Text.Trim() == "")
            {
                int num = (int)MessageBox.Show("请输入客户名称!");
                return false;
            }
            if (tbModel.Text.Trim() == "")
            {
                int num = (int)MessageBox.Show("请输入产品型号!");
                return false;
            }
            if (tbWarranty.Text.Trim() == "")
            {
                int num = (int)MessageBox.Show("请输入质保年限!");
                return false;
            }
            try 
            {
                int warranty = Convert.ToInt32(tbWarranty.Text.Trim());
            }
            catch(Exception ex) 
            {
                MessageBox.Show("质保年限只能是数字！");
                return false;
            }

            if (!(this.tbTray.Text.Trim() == ""))
                return true;
            int num1 = (int)MessageBox.Show("请输入托盘编号!");
            return false;
        }

        private bool AddOneNum(string inverterSN, string checkCode, string model, string invcode, string config, string waranty)
        {
            foreach (var a in _ReadyNumList)
            {
                if (a.InverterSN == inverterSN)
                {
                    MessageBox.Show("序列号:" + inverterSN + "已扫描!");
                    return false;
                }
            }

            try
            {
                int warranty = Convert.ToInt32(tbWarranty.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show("质保年限只能是数字！");
                return false;
            }

            _ReadyNumList.Add(new ST_OutBound
            {
                InverterSN = inverterSN,
                CheckCode = checkCode,
                Model = model,
                InvCode = invcode,
                ConfigInfo = config,
                Warranty = waranty
            });
            dgNum.ItemsSource = _ReadyNumList;
            return true;
        }

        private bool CheckSN()
        {
            foreach (var a in _ReadyNumList)
            {
                var result = fsql.Select<ST_OutBound>().Where(x => x.InverterSN == a.InverterSN && x.PalletNum != a.PalletNum &&
                x.ActiveStatus == "1").ToList().FirstOrDefault();
                if (result !=null )
                {
                    MessageBox.Show("绑定失败!序列号" + a.InverterSN + "已和其他托盘绑定!");
                    return false;
                }
            }

            return true;        
        }

        private bool SaveReadyNumLists()
        {
            try 
            {
                foreach (var a in _ReadyNumList)
                {
                    a.OutDate = Convert.ToDateTime(dpDateNow.Text);
                    a.OrderID = tbOrderId.Text.Trim();
                    a.Customer = tbCustomer.Text.Trim();
                    a.PalletNum = tbTray.Text.Trim();
                    a.ActiveStatus = "1";
                    a.Remark = tbRemark.Text.Trim();
                    a.CreationDate = DateTime.Now;
                }
                fsql.Insert<ST_OutBound>(_ReadyNumList).ExecuteAffrows();


                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败,请联系管理员!");
                Log.Error(ex.Message);
                return false;
            }
        }

        private void RelieveBind_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tbTray.Text.Trim() != "")
                {
                    if (MessageBox.Show("确定解除托盘" + tbTray.Text.Trim() + "和序列号的绑定?", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
                        return;
                    string tray = tbTray.Text.Trim();
                    if (tray.Substring(0, 1) == "P")
                    {
                        fsql.Delete<ST_OutBound>().Where(x => x.PalletNum == tray).ExecuteAffrows();
                    }
                    else
                    {
                        if (tbOrderId.Text.Trim() == "")
                        {
                            MessageBox.Show("解绑失败,请输入订单号!");
                            return;
                        }
                        fsql.Delete<ST_OutBound>().Where(x => x.PalletNum == tray && x.OrderID == tbOrderId.Text.Trim()).ExecuteAffrows();
                    }
                    MessageBox.Show("解除绑定成功!");
                }
                else 
                {
                    if (_ReadyNumList.Count <= 0)
                    {
                        MessageBox.Show("请扫描需要解绑的序列号!");
                        return;
                    }
                    if (MessageBox.Show("确定对序列号列表中的序列号进行解除绑定?", "提示", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                        return;

                    foreach (var a in _ReadyNumList)
                    {
                        fsql.Delete<ST_OutBound>().Where(x => x.InverterSN == a.InverterSN).ExecuteAffrows();
                    }
                }
                RefreshOrder();
            }
            catch(Exception ex)
            {
                MessageBox.Show("解除绑定失败!" + ex.Message);
            }
        }   
    }
}
