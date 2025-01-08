using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using SunwaysFactoryProgram.DBModel;
using SunwaysFactoryProgram.StaticSource;
using SupportProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SunwaysFactoryProgram.ViewModels
{
    public class WareHouseViewModel : BindableBase
    {
        private IFreeSql fsql = DBHelper.SqlEntity;
        public WareHouseViewModel()
        {
            ResetCfgCommand = new DelegateCommand(ResetCfg);
            QueryCommand = new DelegateCommand(Query);
            ExportCommand = new DelegateCommand(Export);
            OrderList = fsql.Select<ST_OutBound>().Limit(100).OrderByDescending(x => x.CreationDate).ToList();
            MdColor.SetThemeColor("#FF2196F3");
        }

        private string _inverterNum;
        public string InverterNum
        {
            get => _inverterNum;
            set => SetProperty(ref _inverterNum, value);
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
            set => SetProperty(ref _startDate, value);
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get => _endDate;
            set => SetProperty(ref _endDate, value);
        }

        private List<ST_OutBound> _orderList;
        public List<ST_OutBound> OrderList
        {
            get => _orderList;
            set => SetProperty(ref _orderList, value);
        }

        public DelegateCommand ResetCfgCommand { get; set; }
        public DelegateCommand QueryCommand { get; set; }
        public DelegateCommand ExportCommand { get; set; }

        private void ResetCfg()
        {
            InverterNum = "";
            StartDate = DateTime.MinValue;
            EndDate = DateTime.MinValue;
        }

        private void Query()
        {
            try
            {
                if (DateTime.Compare(StartDate, EndDate) > 0)
                {
                    MessageBox.Show("开始时间必须早于结束时间!");
                    return;
                }

                if (DateTime.Compare(StartDate, EndDate) == 0)
                {
                    if (string.IsNullOrEmpty(InverterNum))
                    {
                        return;
                    }
                    else
                    {
                        OrderList = fsql.Select<ST_OutBound>().Where(x => x.InverterSN == InverterNum 
                        && x.ActiveStatus == "1").OrderByDescending(x => x.CreationDate).ToList();
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(InverterNum))
                    {
                        OrderList = fsql.Select<ST_OutBound>().Where(x => x.CreationDate.Between(StartDate, EndDate) 
                        && x.ActiveStatus == "1").OrderByDescending(x => x.CreationDate).ToList();

                       /* List<OutBoundDto> outBoundDtos = new List<OutBoundDto>();
                        foreach (var a in OrderList)
                        {
                            outBoundDtos.Add(new OutBoundDto(a) );
                        }*/

                        //HttpApi.HttpPostAddOutBound(OrderList);
                    }
                    else
                    {
                        OrderList = fsql.Select<ST_OutBound>().Where(x => x.CreationDate.Between(StartDate, EndDate) 
                        && x.InverterSN == InverterNum && x.ActiveStatus == "1").OrderByDescending(x => x.CreationDate).ToList();
                    }
                }
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
                Log.Error(ex.Message);
                return;
            }
        }

        private void Export()
        {
            if (_orderList == null || _orderList.Count <= 0)
            {
                MessageBox.Show("列表中没有数据可以导出!");
                return;
            }

            string filepath = "";

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "Excel文件保存路径";  //提示的文字
            dialog.Filter = "Excel文件(*.xlsx)|*.xlsx";       //筛选文件
            dialog.DefaultExt = "xlsx";
            dialog.RestoreDirectory = true;
            if (dialog.ShowDialog() == true)
            {
                filepath = dialog.FileName;
                var result = ExcelHelper.ExportFileInfo(_orderList, filepath);
                if (result.Exception == null)
                {
                    MessageBox.Show("导出成功!");
                    return;
                }
                else
                {
                    MessageBox.Show("导出失败: " + result.Exception.Message);
                    return;
                }
            }
            else
                return;
        }
    }
}
