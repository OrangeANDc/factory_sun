using iTextSharp.text.pdf;
using iTextSharp.text;
using LanguageConfig;
using Microsoft.Win32;
using SafetyTestTool.Model;
using SafetyTestTool.Protocol;
using SafetyTestTool.StaticSource;
using SafetyTestTool.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Text;

namespace SafetyTestTool.View
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : Window
    {
        private SerialDevice _device;
        private MainViewModel _mainViewModel;
        public MainView()
        {
            InitializeComponent();
            RefreshDataGridInfo.refreshDataGridService = new RefreshDataGridService();
            RefreshDataGridInfo.refreshDataGridService.OnRefreshDataGrid += RefreshData;
            _device = new SerialDevice();
            cbPorts.ItemsSource = SerialPort.GetPortNames().ToList();
            this.DataContext = _mainViewModel = new MainViewModel(_device);
            MdColor.SetThemeColor("#FF2196F3");
            SetBtnStatus(true);
        }

        private void RefreshData()
        {
            try
            {
                dgInf.Items.Refresh();
            }
            catch
            { }

        }
        private void SetBtnStatus(bool status)
        {
            cbPorts.IsEnabled = status;
            btnRefresh.IsEnabled = status;
            btnOpen.IsEnabled = status;
            btnClose.IsEnabled = !status;
            btnReadSystemInfo.IsEnabled = !status;
            btnReadData.IsEnabled = !status;
            btnReset.IsEnabled = !status;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lan = (cbLan.SelectedItem as ComboBoxItem)?.Content?.ToString();

            if (lan != null)
            {
                if (lan == "中文")
                {
                    Variable.ExportHeader1 = "安规参数表";
                    Variable.ExportHeader2 = "系统信息";

                    if (_mainViewModel.ParaInf.Count == LanguageList.ChineseList.Count)
                    {
                        for (int i = 0; i < _mainViewModel.ParaInf.Count; i++)
                        {
                            _mainViewModel.ParaInf[i].DataName = LanguageList.ChineseList[i];
                        }
                    }
                    ChangeInfo.Service.ChangeLan("Chinese");
                    RefreshDataGridInfo.refreshDataGridService.RefreshDataGrid();
                }
                if (lan == "English")
                {
                    Variable.ExportHeader1 = "Safety Parameters List";
                    Variable.ExportHeader2 = "System Information";
                    if (_mainViewModel.ParaInf.Count == LanguageList.EnglishList.Count)
                    {
                        for (int i = 0; i < _mainViewModel.ParaInf.Count; i++)
                        {
                            _mainViewModel.ParaInf[i].DataName = LanguageList.EnglishList[i];
                        }
                    }
                    ChangeInfo.Service.ChangeLan("English");
                    RefreshDataGridInfo.refreshDataGridService.RefreshDataGrid();
                }
                if (lan == "Deutsch")
                {
                    Variable.ExportHeader1 = "Parameter Liste";
                    Variable.ExportHeader2 = "Systeminformationen";
                    if (_mainViewModel.ParaInf.Count == LanguageList.DeutschList.Count)
                    {
                        for (int i = 0; i < _mainViewModel.ParaInf.Count; i++)
                        {
                            _mainViewModel.ParaInf[i].DataName = LanguageList.DeutschList[i];
                        }
                    }
                    ChangeInfo.Service.ChangeLan("Deutsch");
                    RefreshDataGridInfo.refreshDataGridService.RefreshDataGrid();
                }
            }
        }

        private void dgInf_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            cbPorts.ItemsSource = SerialPort.GetPortNames().ToList();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (!_device.GetStatus())
            {
                return;
            }

            if (_device.Close())
            {
                MessageBox.Show("Close COM Succeed!");
                SetBtnStatus(true);
                return;
            }
            else
            {
                MessageBox.Show("Open COM Failed!");
                return;
            }
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            if (_device.GetStatus())
            {
                return;
            }

            if (_device.Open(cbPorts.Text))
            {
                MessageBox.Show("Open COM Succeed!");
                SetBtnStatus(false);
                return;
            }
            else
            {
                MessageBox.Show("Open COM Failed!");
                return;
            }
        }

        private void btnReadSystemInfo_Click(object sender, RoutedEventArgs e)
        {
            if (!_device.GetStatus())
            {
                MessageBox.Show("Please Open the COM!");
                return;
            }

            try
            {



                if (_device.ReadFirmwareVersion() && _device.ReadSN() && _device.ReadSafetyCode())
                {
                    string model = "";
                    string power = "";
                    GetModelAndPower(_device._deviceSN, ref model, ref power);
                    tbSerialNum.Text = _device._deviceSN;
                    tbDeviceName.Text = model; 
                    tbRatePower.Text = power;
                    tbSwVersion.Text = "V1.6.0.0"/*"V1." + _device._internalVersion.Substring(0, 2)*/;
                    tbHwVersion.Text = "V1.00";
                    tbSafety.Text = Variable.DicSafetyCounty[_device._safetyCode];
                    tbVersion.Text = _device._version;
                    tbInternalVersion.Text = _device._internalVersion;
                    tbTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    if (_device._safetyCode == 42)
                        tbGridStandard.Text = "TOR Erzeuger Type A";
                    else
                        tbGridStandard.Text = "NA";
                    return;
                }
                else
                {
                    MessageBox.Show("Read data fail!");
                    tbSerialNum.Text = "";
                    tbDeviceName.Text = "";
                    tbSwVersion.Text = "";
                    tbHwVersion.Text = "";
                    tbSafety.Text = "";
                    tbVersion.Text = "";
                    tbInternalVersion.Text = "";
                    tbTime.Text = tbInternalVersion.Text = "";
                    tbGridStandard.Text = "";

                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.Error(ex.Message);
                return;
            }
        }

        private void GetModelAndPower(string sn, ref string model, ref string power)
        {
            if (sn.Length < 16)
            {
                model = "";
                power = "";
            }
            else
            {
                 switch (sn.Substring(11, 4))
                 {
                     case "3000":
                         model = "WTS-4KW-3P";
                         power = "4KW";
                         break;
                     case "3001":
                         model = "WTS-5KW-3P";
                         power = "5KW";
                         break;
                     case "3002":
                         model = "WTS-6KW-3P";
                         power = "6KW";
                         break;
                     case "3003":
                         model = "WTS-8KW-3P";
                         power = "8KW";
                         break;
                     case "3004":
                         model = "WTS-10KW-3P";
                         power = "10KW";
                         break;
                     case "3005":
                         model = "WTS-12KW-3P";
                         power = "12KW";
                         break;
                 }
                model = "STH-12KW-3P";
                //power = "12KW";
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            SafetyView safetyView = new SafetyView();
            if (safetyView.ShowDialog() != true)
                return;

            ushort data = 0;
            bool isFind = false;
            foreach (var a in Variable.DicSafetyCounty)
            {
                if (a.Value == safetyView.SafetyName)
                {
                    data = a.Key;
                    isFind = true;
                    break;
                }

            }

            if (!isFind)
            {
                Log.Error("没有匹配到安规字典");
                return;
            }

            // 25000
            byte[] address = new byte[] { 0x61, 0xA8 };
            if (_device.WriteSingleData(address, BitConverter.GetBytes(data).Reverse().ToArray()))
            {
                MessageBox.Show("Reset Safety Succeed..");
            }
            else
                MessageBox.Show("Reset Safety failed..");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(cbControl.Text))
            {
                MessageBox.Show("Please choose the control mode!");
                return;
            }

            if (!_device.GetStatus())
            {
                MessageBox.Show("Please Open the COM!");
                return;
            }

            ushort value = (cbControl.Text == "STOP" ? (ushort)(0x0404) : (ushort)(0x0400));
            // 25008
            byte[] address = new byte[] { 0x61, 0xB0 };
            if (_device.WriteSingleData(address, BitConverter.GetBytes(value).Reverse().ToArray()))
            {
                MessageBox.Show("Change control mode Succeed..");
            }
            else
            {
                MessageBox.Show("Change control mode Failed..");
            }
        }

        private void BtnRead_Click(object sender, RoutedEventArgs e)
        {
            ParaInfModel para = (ParaInfModel)((Button)e.Source).DataContext;

            if (!_device.GetStatus())
            {
                MessageBox.Show("Please Open the COM!");
                return;
            }

            if (para.IsFix)
            {
                para.CommandInf = DateTime.Now.ToString("hh:mm:ss ") + "Read data succeed.";
                para.DataValue = para.FixValue;
                RefreshDataGridInfo.refreshDataGridService.RefreshDataGrid();
                return;
            }
 

            byte[] address = BitConverter.GetBytes((ushort)para.DataAddress).Reverse().ToArray();
            byte[] result = new byte[] { };
            if (_device.GetFunc(address, 1, ref result))
            {
                dynamic value;
                //有符号判断
                if (para.IsUnCheck)
                    value = unchecked((short)((result[0] << 8) + result[1]));
                else
                    value = (ushort)((result[0] << 8) + result[1]);
                //ushort value = (ushort)((result[0] << 8) + result[1]);
                para.CommandInf = DateTime.Now.ToString("hh:mm:ss ") + "Read data succeed.";

                switch (para.Gain)
                {
                    case 0.5:
                        para.DataValue = (value * 2).ToString();
                        break;
                    case 1:
                        para.DataValue = value.ToString();
                        break;
                    case 10:
                        para.DataValue = (value * 0.1).ToString("0.0");
                        break;
                    case 16:
                        para.DataValue = (value * 1.0 / 16).ToString("0.0");
                        break;
                    case 100:
                        para.DataValue = (value * 0.01).ToString("0.00");
                        break;
                    case 1000:
                        para.DataValue = (value * 0.001).ToString("0.000");
                        break;
                    case 5200:
                        para.DataValue = (value * 1.0 / 5200).ToString("0.000");
                        break;
                    case 10000:
                        para.DataValue = (value * 0.0001).ToString("0.0000");
                        break;
                    default:
                        para.DataValue = value.ToString();
                        break;
                }
            }
            else
            {
                para.DataValue = "";
                para.CommandInf = DateTime.Now.ToString("hh:mm:ss ") + "Read data failed.";
            }
            RefreshDataGridInfo.refreshDataGridService.RefreshDataGrid();
        }

        private void BtnWrite_Click(object sender, RoutedEventArgs e)
        {

            if (Variable.InputCount <= 0)
            {
                PassWordView passWordView = new PassWordView();
                if (passWordView.ShowDialog() != true)
                    return;


                if (passWordView.PassWord.ToUpper() != "SUN")
                {
                    MessageBox.Show("Wrong PassWord!");
                    return;
                }

                Variable.InputCount ++;
            }
            if (!_device.GetStatus())
            {
                MessageBox.Show("Please Open the COM!");
                return;
            }


            ParaInfModel para = (ParaInfModel)((Button)e.Source).DataContext;
            if (string.IsNullOrEmpty(para.DataValue))
            {
                MessageBox.Show("Please Input the Data Value!");
                return;
            }
            try
            {
                if (para.DataAddress == 28049)
                {
                    if (Convert.ToDouble(para.DataValue) < -1.000 || Convert.ToDouble(para.DataValue) > 1.000)
                    {
                        MessageBox.Show("This Value's Ranage is [-1.000~1.000]");
                        return;
                    }
                }

                byte[] address = BitConverter.GetBytes((ushort)para.DataAddress).Reverse().ToArray();
                if (para.IsUnCheck)
                {
                    short signedValue = (short)(Convert.ToDouble(para.DataValue) * para.Gain);
                    if (_device.WriteSingleData(address, BitConverter.GetBytes((short)signedValue).Reverse().ToArray()))
                    {
                        para.CommandInf = DateTime.Now.ToString("hh:mm:ss ") + "Write data succeed.";
                    }
                    else
                    {
                        para.CommandInf = DateTime.Now.ToString("hh:mm:ss ") + "Write data failed.";
                    }
                }
                else
                {
                    ushort value;
                    value = (ushort)(Convert.ToDouble(para.DataValue) * para.Gain);

                    if (_device.WriteSingleData(address, BitConverter.GetBytes(value).Reverse().ToArray()))
                    {
                        //para.OldValue = para.DataValue;
                        para.CommandInf = DateTime.Now.ToString("hh:mm:ss ") + "Write data succeed.";
                    }
                    else
                    {
                        para.CommandInf = DateTime.Now.ToString("hh:mm:ss ") + "Write data failed.";
                    }
                }

                if (para.DataAddress >= 28000)
                {
                    _device.WriteSingleData(new byte[] { 0x3, 0xf5 }, BitConverter.GetBytes((ushort)1).Reverse().ToArray());
                    _device.WriteSingleData(new byte[] { 0x3, 0xf6 }, BitConverter.GetBytes((ushort)1).Reverse().ToArray());
                }

                RefreshDataGridInfo.refreshDataGridService.RefreshDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

        }        

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InfoService.Service.ChangeInfo();

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
                if (cbLan.Text == "中文")
                    path = "C:\\Windows\\Fonts\\SIMHEI.TTF";
                pdfOperation.SetFontName(path);
                pdfOperation.SetBaseFont(path);

                string title = "";
                List<OneItem> list = new List<OneItem>();
                List<PdfPTable> pdfPtableList = new List<PdfPTable>();
                pdfOperation.AddParagraph(Variable.ExportHeader1, 18f);
                pdfOperation.Add((IElement)new iTextSharp.text.Paragraph(" "));
                list.Add(this.GetOneItem(tbkTime.Text, tbTime.Text.Trim(), ""));
                list.Add(this.GetOneItem(tbkDeviceName.Text, tbDeviceName.Text.Trim(), ""));
                list.Add(this.GetOneItem(tbkSerialNum.Text, tbSerialNum.Text.Trim(), ""));
                list.Add(this.GetOneItem(tbkSwVersion.Text, tbSwVersion.Text.Trim(), ""));
                list.Add(this.GetOneItem(tbkVersion.Text, tbVersion.Text.Trim(), ""));
                list.Add(this.GetOneItem(tbkInternalVersion.Text, tbInternalVersion.Text.Trim(), ""));
                list.Add(this.GetOneItem(tbkHwVersion.Text, tbHwVersion.Text.Trim(), ""));
                list.Add(this.GetOneItem(tbkRatePower.Text, tbRatePower.Text.Trim(), ""));
                list.Add(this.GetOneItem(tbkSafety.Text, tbSafety.Text.Trim(), ""));
                list.Add(this.GetOneItem(tbkGridStandard.Text, tbGridStandard.Text.Trim(), ""));

                PdfPTable table2 = pdfOperation.CreateTable2(Variable.ExportHeader2, list);
                pdfOperation.Add((IElement)table2);
                pdfOperation.Add((IElement)new iTextSharp.text.Paragraph(" "));

                foreach (var inf in _mainViewModel.ParaInf)
                {
                    if (inf.DataAddress == null)
                    {
                        if (list.Count > 0 && title.Length > 0)
                        {
                            PdfPTable table = pdfOperation.CreateTable(title, list);
                            pdfPtableList.Add(table);
                        }
                        title = inf.DataName;
                        list.Clear();
                    }
                    else
                    {
                        list.Add(this.GetOneItem(inf.DataName, inf.DataValue, inf.DataUnit));
                    }
                }
                if (list.Count > 0 && title.Length > 0)
                {
                    PdfPTable table = pdfOperation.CreateTable(title, list);
                    pdfPtableList.Add(table);
                }
                for (int index = 0; index < pdfPtableList.Count; ++index)
                {
                    pdfOperation.Add((IElement)pdfPtableList[index]);
                    pdfOperation.Add((IElement)new Paragraph(" "));
                }
                pdfOperation.Close();
                MessageBox.Show("Export PDF Succeed!");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Export PDF Failed: " + ex.Message);
                Log.Error(ex.Message);
                return;
            }


        }

        private OneItem GetOneItem(string name, string value, string unit) => new OneItem()
        {
            Name = name,
            Value = value,
            Unit = unit
        };


    } 
    
}
