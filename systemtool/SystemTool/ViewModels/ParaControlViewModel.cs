using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.Win32;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Common;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using SystemTool.DelegateService;
using SystemTool.Model;
using SystemTool.Protocol;
using SystemTool.StaticSource;

namespace SystemTool.ViewModels
{
    public partial class ParaControlViewModel : BindableBase
    {
        private SerialDevice _serialDevice;

        public CustomTab? SelectedTab { get; set; }
        public ObservableCollection<CustomTab> CustomTabs { get; set; }
        public ParaControlViewModel(IContainerProvider containerProvider)
        {
            _serialDevice = containerProvider.Resolve<SerialDevice>();
            DelegateCommand<string> closeCommand = new DelegateCommand<string>(Close);          
            ExportCommand = new DelegateCommand(Export);
            ImportCommand = new DelegateCommand(Import);
            AddCommand = new DelegateCommand(Add);
            AddConfigCommand = new DelegateCommand(AddConfig);
            DeleteCommand = new DelegateCommand(Delete);
            ReadAllCommand = new DelegateCommand(ReadAll);
            ExportSafetyCommand = new DelegateCommand(ExportSafety);
            CustomTabs = new ObservableCollection<CustomTab>();
           /* ShowConfigCommand = new DelegateCommand<bool?>(ShowConfig);
            HideConfigCommand = new DelegateCommand(HideConfig);*/
        }

        public DelegateCommand ExportCommand { get; set; }
        public DelegateCommand AddCommand { get; set; }
        public DelegateCommand AddConfigCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        public DelegateCommand ImportCommand { get; set; }
        public DelegateCommand ReadAllCommand { get; set; }
        public DelegateCommand ExportSafetyCommand { get; set; }


        private OneItem GetOneItem(string name, string value, string unit) => new OneItem()
        {
            Name = name,
            Value = value,
            Unit = unit
        };

        private void ExportSafety()
        {
            if (SelectedTab == null)
            {
                MessageBox.Show("当前没有配置表!");
                return;
            }

            if (_serialDevice.ReadFirmwareVersion() && _serialDevice.ReadSN() && _serialDevice.ReadSafetyCode())
            {
                try
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.DefaultExt = "pdf";
                    saveFileDialog.Filter = "Pdf(*.pdf)|*.pdf";
                    saveFileDialog.RestoreDirectory = true;
                    saveFileDialog.Title = "File Save Path";
                    saveFileDialog.FileName = "Parameter";
                    if (saveFileDialog.ShowDialog() != true)
                        return;

                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    PDFOperation pdfOperation = new PDFOperation();
                    pdfOperation.Open((Stream)new FileStream(saveFileDialog.FileName, FileMode.Create));
                    string path = "C:\\Windows\\Fonts\\Arial.TTF";

                    pdfOperation.SetFontName(path);
                    pdfOperation.SetBaseFont(path);

                    string title = "";
                    List<OneItem> list = new List<OneItem>();
                    List<PdfPTable> pdfPtableList = new List<PdfPTable>();
                    //大纲标题
                    pdfOperation.AddParagraph("Safety Parameters List", 18f);
                    pdfOperation.Add((IElement)new iTextSharp.text.Paragraph(" "));

                    //小标题及内容
                    //机种信息
                    string flag1 = _serialDevice._deviceSN.Substring(11, 2);
                    string flag2 = _serialDevice._deviceSN.Substring(13, 2);
                    string deviceType = Variable._machineType[flag1][flag2];

                    list.Add(GetOneItem("Time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ""));
                    list.Add(GetOneItem("Device Name", deviceType.Trim(), ""));
                    list.Add(GetOneItem("Serial Number", _serialDevice._deviceSN.Trim(), ""));
                    list.Add(GetOneItem("SW Version", _serialDevice._version.Trim(), ""));
                    list.Add(GetOneItem("HW Version", _serialDevice._internalVersion.Trim(), ""));
                    list.Add(GetOneItem("Safety Code", Variable._safetyDic[_serialDevice._safetyCode].Trim(), ""));
                    PdfPTable table2 = pdfOperation.CreateTable2("System Information", list);
                    pdfOperation.Add((IElement)table2);
                    pdfOperation.Add((IElement)new iTextSharp.text.Paragraph(" "));


                    //
                    list.Clear();
                    foreach (var data in SelectedTab.CustomContent)
                    {
                        if (data != null)
                        {
                            list.Add(GetOneItem(data.DataName, 
                                data.DataValue == null ? "" : data.DataValue, 
                                data.DataUnit == null ? "" : data.DataUnit));
                        }
                    }

                    PdfPTable pdfPTable = pdfOperation.CreateTable("Safety Parameters", list);
                    pdfOperation.Add((IElement)pdfPTable);
                    pdfOperation.Add((IElement)new iTextSharp.text.Paragraph(" "));

                    pdfOperation.Close();
                    MessageBox.Show("Export PDF Succeed!");

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
            else
            {
                MessageBox.Show("读取逆变器基础参数失败！");
                return;
            }
        }


        private void Export()
        {
            if (SelectedTab is { } selectedTab && selectedTab.CustomContent != null)
            {             
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Title = "文件保存路径";
                dialog.Filter = "数据结构文件(*.Json)|*.Json";
                dialog.DefaultExt = "Json";
                dialog.RestoreDirectory = true;
                if (dialog.ShowDialog() == true)
                {

                    List<ParaInfModel> contents = new List<ParaInfModel>();
                    foreach (var data in selectedTab.CustomContent)
                    {
                        ParaInfModel paraInfModel = new ParaInfModel();
                        paraInfModel.DataAddress = data.DataAddress;
                        paraInfModel.Remark = data.Remark;
                        paraInfModel.DataName = data.DataName;
                        paraInfModel.DataGain = data.DataGain;
                        paraInfModel.DataUnit = data.DataUnit;
                        paraInfModel.DataLength = data.DataLength;
                        paraInfModel.IsSigned = data.IsSigned;

                        contents.Add(paraInfModel);
                    }

                    string json = JsonConvert.SerializeObject(contents, Formatting.Indented);

                    if (FileHelper.WriteJsonFile(dialog.FileName, json))
                    {
                        MessageBox.Show("导出成功");
                        return;
                    }
                    else
                    {
                        MessageBox.Show("导出失败");
                        return;
                    }
                }
                else
                    return;
            }
        }

        private void Import()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = Environment.CurrentDirectory + @"\Resources";
            dialog.Title = "选择文件";
            dialog.Filter = "数据结构文件(*.Json)|*.Json";
            dialog.DefaultExt = "Json";
            dialog.RestoreDirectory = true;
            if (dialog.ShowDialog() == true)
            {
                if (CustomTabs.Where(x => x.CustomHeader == dialog.SafeFileName.Split('.').First()).Any())
                {
                    MessageBox.Show("无法导入文件名相同的结构！");
                    return;
                }
                string? json = FileHelper.ReadJsonFile(dialog.FileName);
                if (json != null)
                {
                    List<ParaInfModel>? paraInfModels =  JsonConvert.DeserializeObject<List<ParaInfModel>>(json);
                    DelegateCommand<string> closeCommand = new DelegateCommand<string>(Close);
                    CustomTab customTab = new CustomTab(closeCommand);
                    customTab.CustomContent = new ObservableCollection<ParaModel>();
                    customTab.CustomHeader = dialog.SafeFileName.Split('.').First();
               
                    foreach (var data in paraInfModels)
                    {
                        ParaModel paraModel = new ParaModel() 
                        {
                            DataAddress = data.DataAddress,
                            DataName = data.DataName,
                            Remark = data.Remark,
                            DataLength = data.DataLength,
                            DataGain = data.DataGain,
                            DataUnit = data.DataUnit,
                            IsSigned = data.IsSigned,
                        };
                        customTab.CustomContent.Add(paraModel);
                    }
                    CustomTabs.Add(customTab);

                    DelegateInfo._refreshTabView.Refresh();
                }
            }
            return;
        }

        private void Add()
        {
            if (SelectedTab == null)
            {
                MessageBox.Show("当前没有配置表!");
                return;
            }
            int cc = SelectedTab.SelectIndex;

            SelectedTab.CustomContent.Insert(SelectedTab.SelectIndex + 1, new ParaModel());
            DelegateInfo._refreshTabView.Refresh();
        }

        private void AddConfig()
        {
            DelegateCommand<string> closeCommand = new DelegateCommand<string>(Close);
            CustomTab customTab = new CustomTab(closeCommand);
            customTab.CustomContent = new ObservableCollection<ParaModel>();
            customTab.CustomContent.Add(new ParaModel());
            customTab.CustomHeader = DateTime.Now.ToString("yyyyMMddHHmmss");

            if (CustomTabs.Where(x => x.CustomHeader == customTab.CustomHeader).Any())
            {
                MessageBox.Show("无法导入文件名相同的结构！");
                return;
            }
            CustomTabs.Add(customTab);
        }

        private void Delete()
        {
            if (SelectedTab == null)
            {
                MessageBox.Show("当前没有配置表!");
                return;
            }
            if (SelectedTab.SelectIndex < 0)
            {
                MessageBox.Show("没有选中行，无法删除!");
                return;
            }
            SelectedTab.CustomContent.RemoveAt(SelectedTab.SelectIndex);
            DelegateInfo._refreshTabView.Refresh();
        }

        private void ReadAll()
        {
            try
            {
                if (SelectedTab == null)
                {
                    MessageBox.Show("当前没有配置表!");
                    return;
                }

                if (SelectedTab.CustomContent == null || SelectedTab.CustomContent.Count() == 0)
                {
                    MessageBox.Show("配置项为空，无法读取");
                    return;
                }

                List<ushort> datalists = new List<ushort>();
                foreach (var data in SelectedTab.CustomContent)
                {
                    datalists.Add(data.DataAddress);
                }


                ushort startAddr = datalists.Min();
                ushort endAddr = datalists.Max();
                ushort endlen = SelectedTab.CustomContent.Last().DataLength;



                if (!_serialDevice.GetStatus())
                {
                    MessageBox.Show("串口未连接！");
                    return;
                }

                ushort length = (ushort)(endAddr - startAddr + endlen);
                byte[] addr = BitConverter.GetBytes((ushort)startAddr).Reverse().ToArray();
                byte[] result = new byte[] { };
                if (_serialDevice.ReadData(addr, length, ref result))
                {
                    for (int i = 0; i < SelectedTab.CustomContent.Count(); i++)
                    {
                        int index = SelectedTab.CustomContent[i].DataAddress - startAddr;

                        dynamic value;

                        switch (SelectedTab.CustomContent[i].DataLength)
                        {
                            case 1:
                                value = (result[index * 2] << 8) + result[index * 2 + 1];
                                if (SelectedTab.CustomContent[i].IsSigned)
                                    value = (short)value;
                                else
                                    value = (ushort)value;
                                break;
                            case 2:
                                //value = (0x18 << 24) + (0x23 << 8) + 0x00 ;
                                value = (result[index * 2] << 24) + (result[index * 2 + 1] << 16) + (result[index * 2 + 2] << 8) + result[index * 2 + 3];
                                if (SelectedTab.CustomContent[i].IsSigned)
                                    value = (int)value;
                                else
                                    value = (uint)value;
                                break;
                            case 3:
                                value = (result[index * 2] << 40) + (result[index * 2 + 1] << 32) + (result[index * 2 + 2] << 24) +
                                        (result[index * 2 + 3] << 16) + (result[index * 2 + 4] << 8) + result[index * 2 + 5];
                                if (SelectedTab.CustomContent[i].IsSigned)
                                    value = (long)value;
                                else
                                    value = (ulong)value;
                                break;
                            default:
                                MessageBox.Show("不支持长度3以上的数据,请联系管理员!");
                                return;
                        }

                        switch (Convert.ToDouble(SelectedTab.CustomContent[i].DataGain))
                        {
                            case 0.05:
                                SelectedTab.CustomContent[i].DataValue = (value * 20).ToString();
                                break;
                            case 1:
                                SelectedTab.CustomContent[i].DataValue = value.ToString();
                                break;
                            case 10:
                                SelectedTab.CustomContent[i].DataValue = (value * 0.1).ToString("0.0");
                                break;
                            case 100:
                                SelectedTab.CustomContent[i].DataValue = (value * 0.01).ToString("0.00");
                                break;
                            case 1000:
                                SelectedTab.CustomContent[i].DataValue = (value * 0.001).ToString("0.000");
                                break;
                            default:
                                MessageBox.Show("不支持该增益,请联系管理员!");
                                return;
                        }

                        SelectedTab.CustomContent[i].CommandInf = DateTime.Now.ToString("hh:mm:ss ") + "Read data succeed.";

                         
                    }

                    DelegateInfo._refreshTabView.Refresh();
                    return;


                }
                else
                {
                    MessageBox.Show("读取失败！");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;

            }
           

        }



        public void Close(string text)
        {

            for (int i = 0; i < CustomTabs.Count; i++)
            {
                if (CustomTabs[i].CustomHeader == text)
                {
                    CustomTabs.Remove(CustomTabs[i]);
                    break;
                }    

            }
        }
    }

    public class CustomTab : BindableBase
    {
        public CustomTab(DelegateCommand<string> closeCommand)
        {
            CloseCommand = closeCommand;
            IsHide = true;
        }
        public DelegateCommand<string> CloseCommand { get; }
        public DelegateCommand AddCommand { get; }


        private bool? _isHide;
        public bool? IsHide
        {
            get => _isHide;
            set => SetProperty(ref _isHide, value);
        }

        private string? _customHeader;
        public string? CustomHeader
        {
            get => _customHeader;
            set => SetProperty(ref _customHeader, value);
        }

        private ObservableCollection<ParaModel>? _customContent;
        public ObservableCollection<ParaModel>? CustomContent
        {
            get => _customContent;
            set => SetProperty(ref _customContent, value);
        }

        private int _selectIndex;
        public int SelectIndex
        {
            get => _selectIndex;
            set => SetProperty(ref _selectIndex, value);
        }

    }
}
