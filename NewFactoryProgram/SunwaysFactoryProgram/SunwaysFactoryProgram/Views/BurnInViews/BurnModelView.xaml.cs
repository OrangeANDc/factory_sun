using Microsoft.Win32;
using Prism.Events;
using Prism.Services.Dialogs;
using SunwaysFactoryProgram.DBModel;
using SunwaysFactoryProgram.Model;
using SunwaysFactoryProgram.StaticSource;
using SunwaysFactoryProgram.ViewModels;
using SupportProject;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static MaterialDesignThemes.Wpf.Theme;
using static SunwaysFactoryProgram.Views.BurnInViews.BurnModelView;

namespace SunwaysFactoryProgram.Views.BurnInViews
{
    /// <summary>
    /// BurnModelView.xaml 的交互逻辑
    /// </summary>
    public partial class BurnModelView : UserControl
    {
        private IFreeSql fsql = DBHelper.SqlEntity;
        public List<Data> _displayDatas = new List<Data>();
        public List<New_BurnInData> _burnDatas = new List<New_BurnInData>();
        public string Id;
        public BurnModelView(List<ST_BurnInPosition> data)
        {
            InitializeComponent();
            Variable.BurninService.OnChangeBurninData += RefreshView;
            Id = tbTitle.Text = data.First().BurnInCar;
            LoadStatus(data);

        }

        ~BurnModelView() {
            Variable.BurninService.OnChangeBurninData -= RefreshView;
        }
             
        private void LoadStatus(List<ST_BurnInPosition> data)
        {
            _displayDatas = new List<Data>();
            //_burnDatas = new List<BurnInData>();
            data.ForEach(x => _displayDatas.Add(new Data ( x.InverterSN )));
            //data.ForEach(x => _burnDatas.Add(new BurnInData { InverterSN = x.InverterSN }));

            dgBurnData.ItemsSource = null;
            dgBurnData.ItemsSource = _displayDatas;
        }

        private void RefreshView()
        {
            List<Data> newDatas = new List<Data>();
            var datas = fsql.Select<ST_BurnInPosition>().Where(x => x.BurnInRoom == Variable._burnRoom && x.BurnInCar == Id && x.DataStatus == "1").ToList(x => x.InverterSN);
            foreach (var displayData in _displayDatas)
            {
                if (datas.Contains(displayData.SN))
                {
                    newDatas.Add(displayData);
                    datas.Remove(displayData.SN);
                }
              /*  else
                {
                    var removeitem = _burnDatas.Where(x => x.InverterSN == displayData.SN).First();
                    _burnDatas.Remove(removeitem);
                }*/
 
            }

            foreach (var data in datas)
            {
                newDatas.Add(new Data(data));
                //_burnDatas.Add(new BurnInData {InverterSN = data});
            }

            
            _displayDatas = newDatas;
            /*_burnDatas = new List<BurnInData>();
            _displayDatas.ForEach(x => _burnDatas.Add(new BurnInData { InverterSN = x.SN }));*/
            dgBurnData.ItemsSource = null;
            dgBurnData.ItemsSource = _displayDatas;
        }

        public void SetBurnInStatus(string sn, int burninTime, string status, string firmware, string internalFirmwar)
        {
            foreach (var a in _displayDatas)
            {
                
                if (a.SN == sn)
                {
                    a.Status = status;
                    if (status == "离线")
                    {
                        a.DisplayMsg = sn + "  " + burninTime.ToString() + "H  " + status;
                    }
                    else
                    {
                        a.DisplayMsg = sn + "  " + burninTime.ToString() + "H  " + status + "  F:" + firmware + "-IF:" + internalFirmwar;
                    }
                    break;
                }

            }
            dgBurnData.ItemsSource = null;
            dgBurnData.ItemsSource = _displayDatas;

        }

        public class Data
        {
            public Data(string sn)
            {
                SN = sn;
                DisplayMsg = sn;
            }
            public string SN { get; set; }
            public string DisplayMsg { get; set; }
            public string Status { get; set; }
        }

        private object _datalocker = new object();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "序列号导出";
                saveFileDialog.DefaultExt = "txt";
                saveFileDialog.FileName = tbTitle.Text;
                saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files|*.*";
                if (saveFileDialog.ShowDialog() != true)
                    return;


                lock (_datalocker)
                {
                    string contents = "";
                    List<Data> DataCopy = _displayDatas;
                    DataCopy.ForEach(data => { contents += (data.SN + "\r\n"); });
                    File.WriteAllText(saveFileDialog.FileName, contents);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.Error(ex.Message);
                return;
            }

            
        }
    }
}
