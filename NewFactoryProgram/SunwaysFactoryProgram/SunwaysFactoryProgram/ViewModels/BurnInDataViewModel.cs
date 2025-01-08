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
    public class BurnInDataViewModel : BindableBase
    {
        private IFreeSql fsql = DBHelper.SqlEntity;
        public BurnInDataViewModel()
        {
            ResetCfgCommand = new DelegateCommand(ResetCfg);
            QueryCommand = new DelegateCommand<object>(Query);
            ExportCommand = new DelegateCommand<object>(Export);
            StartDate = DateTime.Now.Date;
            EndDate = DateTime.Now.AddDays(1).Date;
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

        private List<BurnIn_ErrorMsg> _errList;
        public List<BurnIn_ErrorMsg> ErrList
        {
            get => _errList;
            set => SetProperty(ref _errList, value);
        }

        private List<BurnIn_Result> _resultList;
        public List<BurnIn_Result> ResultList
        {
            get => _resultList;
            set => SetProperty(ref _resultList, value);
        }

        public DelegateCommand ResetCfgCommand { get; set; }
        public DelegateCommand<object> QueryCommand { get; set; }
        public DelegateCommand<object> ExportCommand { get; set; }

        private void ResetCfg()
        {
            InverterNum = "";
            StartDate = DateTime.MinValue;
            EndDate = DateTime.MinValue;
        }

        private void Query(object index)
        {
            try
            {
                int selectIndex = Convert.ToInt32(index);

                if (DateTime.Compare(StartDate, EndDate) >= 0)
                {
                    MessageBox.Show("开始时间必须早于结束时间!");
                    return;
                }             
                else
                {
                    switch (selectIndex)
                    {
                        case 0:
                            if (string.IsNullOrEmpty(InverterNum))
                            {
                                ResultList = fsql.Select<BurnIn_Result>().Where(x=> x.TestTime.Between(StartDate, EndDate)).
                                OrderByDescending(x => x.TestTime).ToList();
                            }
                            else
                            {
                                ResultList = fsql.Select<BurnIn_Result>().Where(x => x.InverterSN == InverterNum && x.TestTime.Between(StartDate, EndDate)).
                                OrderByDescending(x => x.TestTime).ToList();
                            }
                             break;
                        case 1:
                            if (string.IsNullOrEmpty(InverterNum))
                            {
                                ErrList = fsql.Select<BurnIn_ErrorMsg>().Where(x=> x.CreationDate.Between(StartDate, EndDate)).
                                OrderByDescending(x => x.CreationDate).ToList();
                            }
                            else
                            {
                                ErrList = fsql.Select<BurnIn_ErrorMsg>().Where(x => x.InverterSN == InverterNum && x.CreationDate.Between(StartDate, EndDate)).
                                OrderByDescending(x => x.CreationDate).ToList();
                            }
                            break;
                        case 2:
                            break;
                        default:
                            break;
                    }
                }          
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.Error(ex.Message);
                return;
            }
        }

        private void Export(object index)
        {
            int selectIndex = Convert.ToInt32(index);

            if (selectIndex == 0)
            {
                if (ResultList == null || ResultList.Count == 0)
                {
                    MessageBox.Show("列表中没有数据可以导出");
                    return;
                }

            }
            else if (selectIndex == 1)
            {
                if (ErrList == null || ErrList.Count == 0)
                {
                    MessageBox.Show("列表中没有数据可以导出");
                    return;
                }
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
                dynamic result = new object();
                switch (selectIndex)
                {
                    case 0:
                        result = ExcelHelper.ExportFileInfo(ResultList, filepath);
                        break;
                    case 1:
                        result = ExcelHelper.ExportFileInfo(ErrList, filepath);
                        break;
                    default:
                        break;
                }

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
