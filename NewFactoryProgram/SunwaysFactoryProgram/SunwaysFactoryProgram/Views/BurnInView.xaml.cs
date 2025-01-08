using Collections.Pooled;
using ImTools;
using Magicodes.ExporterAndImporter.Core.Models;
using Microsoft.Win32;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using SunwaysFactoryProgram.DBModel;
using SunwaysFactoryProgram.Model;
using SunwaysFactoryProgram.StaticSource;
using SunwaysFactoryProgram.Views.BurnInViews;
using SupportProject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Printing;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SunwaysFactoryProgram.Views
{
    /// <summary>
    /// BurnInView.xaml 的交互逻辑
    /// </summary>
    public partial class BurnInView : Window
    {
        private IFreeSql fsql = DBHelper.SqlEntity;
        private ObservableCollection<LogInfo> _logInfos = new ObservableCollection<LogInfo>();
        private string INIPath123 = System.AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\BurnParaSet.ini";
        private int _pvBurnInTime = 5;
        private int _batBurnInTime = 3;
        public BurnInView(string user, string procedure, string station)
        {
            InitializeComponent();
            Variable.BurninService.OnChangeBurninView += RefreshChildView;
            dgInf.ItemsSource = _logInfos;
            tbVersion.Text = Variable.Version;
            tbUser.Text = " 当前用户:" + user;
            tbProduce.Text = procedure;
            switch (station)
            {
                case "老化房-01":
                    Variable._burnRoom = "SUN-ROOM-001";
                    break;
                case "老化房-02":
                    Variable._burnRoom = "SUN-ROOM-002";
                    break;
                case "老化房-03":
                    Variable._burnRoom = "SUN-ROOM-003";
                    break;
            }
            tbStationName.Text = "测试站点:" + Variable._burnRoom;



            _pvBurnInTime = Convert.ToInt32(Methods.INIRead("老化参数", "PV老化时间", INIPath123));
            _batBurnInTime = Convert.ToInt32(Methods.INIRead("老化参数", "电池老化时间", INIPath123));

            Load();
            Running();
        }

        private void Load()
        {

            var data = Methods.ConvertList(fsql.Select<ST_BurnInPosition>().Where(x => x.DataStatus == "1" && x.BurnInRoom == Variable._burnRoom).
               OrderBy(x => x.BurnInCar).OrderBy(x => x.InverterSN).ToList());
            if (data != null || data.Count > 0)
            {
                var cars = data.Select(t => t.BurnInCar).Distinct().ToList();

                foreach (var car in cars)
                {
                    var view = new BurnModelView(data.Where(t => t.BurnInCar == car).ToList());
                    wpView.Children.Add(view);
                }
            }
        }

        private void RefreshChildView()
        {
            var data = Methods.ConvertList(fsql.Select<ST_BurnInPosition>().Where(x => x.DataStatus == "1" && x.BurnInRoom == Variable._burnRoom).
               OrderBy(x => x.BurnInCar).OrderBy(x => x.InverterSN).ToList());
            if (data != null || data.Count > 0)
            {
                var cars = data.Select(t => t.BurnInCar).Distinct().ToList();
                List<string> views = new List<string>();
                foreach (var child in wpView.Children)
                {
                    views.Add((child as BurnModelView).Id);


                    /* var view = new BurnModelView(data.Where(t => t.BurnInCar == car).ToList());
                     wpView.Children.Add(view);
                     _burnViews.Add(view);*/
                }

                var intersect = cars.Intersect(views).ToList();
                var addLists = cars.Except(intersect).ToList();
                var removeLists = views.Except(intersect).ToList();

                for (int i = 0; i < wpView.Children.Count; i++)
                {
                    if (removeLists.Contains((wpView.Children[i] as BurnModelView).Id))
                    {
                        wpView.Children.Remove(wpView.Children[i]);
                    }
                }

                foreach (var addsn in addLists)
                {
                    var view = new BurnModelView(data.Where(t => t.BurnInCar == addsn).ToList());
                    wpView.Children.Add(view);
                }
            }
        }

        private void AddInfo(string inf, DateTime dateTime)
        {
            if (_logInfos.Count > 100)
                _logInfos.RemoveAt(0);

            _logInfos.Add(new LogInfo { Inf = inf, Time = dateTime });
        }


        private object _lockObject1 = new object();
        private object _lockObject2 = new object();
        private void Running()
        {
            try
            {
                //Thread.Sleep(1000);
                Task.Run(() => { GetData(); });
                Task.Run(() => { DataAccessMethod(); });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.Error(ex.Message);
            }
        }

        //监控api获取老化数据
        private void GetData()
        {
            while (true)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                App.Current.Dispatcher.Invoke((Delegate)(() =>
                {
                    lock (_lockObject1)
                    {
                        if (wpView.Children.Count > 0)
                        {
                            AddInfo("数据查询...", DateTime.Now);

                            for (int j = 0; j < wpView.Children.Count; j++)
                            {
                                var viewB = wpView.Children[j] as BurnModelView;
                                if (viewB != null)
                                {
                                    for (int i = 0; i < viewB._displayDatas.Count; i++)
                                    {
                                        string sn = viewB._displayDatas[i].SN;
                                        try
                                        {
                                            New_DataRoot? dataRoot = JsonConvert.DeserializeObject<New_DataRoot>(HttpApi.HttpPostBurnData(sn));
                                            if (dataRoot is not null)
                                            {
                                                /*if (dataRoot.data is not null && dataRoot.data.Count > 0)
                                                {
                                                    // api 老化数据,只有一条
                                                    var dataRoot0 = dataRoot.data[0];

                                                    if (Is03DataEmpty(dataRoot0, viewB))
                                                    {
                                                        ConfigData configData = JsonConvert.DeserializeObject<ConfigDataRoot>(HttpApi.HttpGetConfigData(sn)).data;
                                                        if (configData is not null)
                                                        {
                                                            dataRoot0.DeviceInfo = (int)configData.deviceinfo;

                                                            dataRoot0.CommunicationMode = (int)configData.communicationmode;
                                                            dataRoot0.Firmwareversion = (uint)configData.firmwareversion;
                                                            dataRoot0.InternalFirmwareversion = (uint)configData.internalfirmwareversion;
                                                        }
                                                    }

                                                    AddSnToBurnInDatas(dataRoot0, viewB);

                                                }*/
                                                if (dataRoot.data is not null && dataRoot.data.data.Count > 0)
                                                {
                                                    // api 老化数据,只有一条
                                                    var dataRoot0 = dataRoot.data.data[0];

                                                    if (Is03DataEmpty(dataRoot0, viewB))
                                                    {
                                                        New_ConfigDataRoot configData1 = JsonConvert.DeserializeObject<New_ConfigDataRoot>(HttpApi.HttpGetConfigData(sn));
                                                        New_ConfigData configData = configData1.data;
                                                        if (configData is not null)
                                                        {
                                                            dataRoot0.DeviceInfo = (int)configData.deviceInfo;

                                                            dataRoot0.CommunicationMode = configData.communicationMode == null ? null : (int)configData.communicationMode;
                                                            dataRoot0.Firmwareversion = (uint)configData.firmwareVersion;
                                                            dataRoot0.InternalFirmwareVersion = (uint)configData.internalFirmwareVersion;
                                                        }
                                                    }

                                                    AddSnToBurnInDatas(dataRoot0, viewB);

                                                }
                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            Log.Error($"{sn}: {ex.Message}");
                                            continue;
                                        }
                                    }

                                }
                            }
                        }
                    }
                }));

                stopwatch.Stop();
                long num = stopwatch.ElapsedMilliseconds / 1000L;
                if (num >= 58L)
                    return;
                Thread.Sleep((int)(58L - num) * 1000);

            }
        }

        private bool Is03DataEmpty(New_BurnInData data, BurnModelView view)
        {
            bool flag = false;
            for (int index = 0; index < view._burnDatas.Count; ++index)
            {
                if (view._burnDatas[index].inverterSN == data.inverterSN)
                {
                    if (view._burnDatas[index].DeviceInfo < 0 || view._burnDatas[index].CommunicationMode < 0)
                        return true;
                    flag = true;
                    break;
                }
            }
            return !flag;
        }

        private void AddSnToBurnInDatas(New_BurnInData data, BurnModelView view)
        {
            for (int i = 0; i < view._burnDatas.Count; i++)
            {
                var tmp = view._burnDatas[i];
                if (tmp.inverterSN == data.inverterSN)
                {
                    if (Math.Abs(Convert.ToDateTime(data.createDate).Subtract(Convert.ToDateTime(tmp.createDate)).TotalSeconds) > 5.0)
                    {
                        if (data.DeviceInfo < 0 || data.CommunicationMode < 0)
                        {
                            data.DeviceInfo = tmp.DeviceInfo;
                            data.CommunicationMode = tmp.CommunicationMode;
                            data.InternalFirmwareVersion = tmp.InternalFirmwareVersion;
                            data.Firmwareversion = tmp.Firmwareversion;
                        }
                        view._burnDatas[i] = data;
                        view._burnDatas[i].DataUpdate = true;
                    }
                    return;
                }
            }
            data.DataUpdate = true;
            view._burnDatas.Add(data);
        }


        private void DataAccessMethod()
        {
            while (true)
            {
                Thread.Sleep(5000);
                App.Current.Dispatcher.Invoke((Delegate)(() =>
                {
                    lock (_lockObject2)
                    {
                        if (wpView.Children.Count > 0)
                        {
                            for (int i = 0; i < wpView.Children.Count; i++)
                            {
                                var viewB = wpView.Children[i] as BurnModelView;

                                for (int j = 0; j < viewB._burnDatas.Count; j++)
                                {
                                    New_BurnInData data = viewB._burnDatas[j];
                                    string firmware = "";
                                    string internalFirmware = "";
                                    int num1 = 0;
                                    int num2 = 0;
                                    int num3 = -1;
                                    int num4 = 0;
                                    string str1 = "";
                                    string str2 = "";
                                    bool bBatBurnIn = false;
                                    bool flag = this.IsESInverter(data.inverterSN);
                                    List<BurnIn_ErrorMsg> errorMsgList = GetErrorMsgList(data, ref bBatBurnIn);
                                    if (flag)
                                    {
                                        BurnIn_Result burnInResult = fsql.Select<BurnIn_Result>().Where(x => x.InverterSN == data.inverterSN).Limit(1).First();
                                        if (burnInResult != null)
                                        {
                                            num1 = burnInResult.PvTime;
                                            num2 = burnInResult.BatTime;
                                            str1 = burnInResult.PvResult;
                                            str2 = burnInResult.BatResult;
                                        }
                                        else
                                        {
                                            num1 = -1;
                                            num2 = -1;
                                            str1 = "";
                                            str2 = "";
                                        }
                                    }
                                    if (data.timeTotal != null || data.timeTotal != "")
                                        num3 = Convert.ToInt32(data.timeTotal);
                                    if (data.Firmwareversion > 0U)
                                        firmware = string.Format("{0:D2}.{1:D2}.{2:D2}.{3:D2}", (object)(byte)(data.Firmwareversion >> 24), (object)(byte)(data.Firmwareversion >> 16), (object)(byte)(data.Firmwareversion >> 8), (object)(byte)data.Firmwareversion);
                                    if (data.InternalFirmwareVersion > 0U)
                                        internalFirmware = string.Format("{0:D2}.{1:D2}.{2:D2}.{3:D2}", (object)(byte)(data.InternalFirmwareVersion >> 24), (object)(byte)(data.InternalFirmwareVersion >> 16), (object)(byte)(data.InternalFirmwareVersion >> 8), (object)(byte)data.InternalFirmwareVersion);
                                    int num5;
                                    int num6;

                                    if (flag)
                                    {
                                        if (bBatBurnIn)
                                        {
                                            num5 = num1 < 0 ? num3 : num3 - num1;
                                            num2 = num5;
                                            num6 = this._batBurnInTime;
                                        }
                                        else
                                        {
                                            num5 = num2 < 0 ? num3 : num3 - num2;
                                            num1 = num5;
                                            num6 = this._pvBurnInTime;
                                        }
                                    }
                                    else
                                    {
                                        num5 = num3;
                                        num1 = num5;
                                        num6 = this._pvBurnInTime;
                                    }


                                    if (!data.DataUpdate)
                                    {
                                        if (CalcDivTime(DateTime.Now, Convert.ToDateTime(data.createDate)) > 300.0)
                                            viewB.SetBurnInStatus(data.inverterSN, num5, "离线", firmware, internalFirmware);
                                    }
                                    else
                                    {
                                        data.DataUpdate = false;
                                        if (errorMsgList != null && errorMsgList.Count > 0)
                                        {
                                            num4 = 1;
                                            for (int index2 = 0; index2 < errorMsgList.Count; ++index2)
                                            {
                                                if (errorMsgList[index2].ErrorLevel == 2)
                                                    num4 = 2;
                                                AddInfo(errorMsgList[index2].InverterSN + ":" + errorMsgList[index2].ErrorMsg, Convert.ToDateTime(errorMsgList[index2].CreationDate));
                                            }

                                        }
                                        if (fsql.Select<BurnIn_ErrorMsg>().Where(x => x.InverterSN == data.inverterSN && x.CreationDate == Convert.ToDateTime(data.createDate)).ToList().Count() <= 0)
                                        {
                                            fsql.Insert<BurnIn_ErrorMsg>(errorMsgList).ExecuteAffrows();
                                        }

                                        int commonErrorCount = 0;
                                        int serialErrorCount = 0;
                                        GetErrorInfoCount(data.inverterSN, num5, ref commonErrorCount, ref serialErrorCount);
                                        if (serialErrorCount > 0 || commonErrorCount > 20)
                                        {
                                            if (flag & bBatBurnIn)
                                                str2 = "FAIL";
                                            else
                                                str1 = "FAIL";
                                            viewB.SetBurnInStatus(data.inverterSN, num5, "FAIL", firmware, internalFirmware);
                                            if (fsql.Select<BurnIn_Result>().Where(x => x.InverterSN == data.inverterSN).ToList().Count > 0)
                                            {
                                                fsql.Update<BurnIn_Result>(new BurnIn_Result
                                                {
                                                    InverterSN = data.inverterSN,
                                                    PvTime = num1,
                                                    BatTime = num2,
                                                    PvResult = str1,
                                                    BatResult = str2,
                                                    TestTime = Convert.ToDateTime(data.createDate)
                                                }).ExecuteAffrows();
                                            }
                                            else
                                            {
                                                fsql.Insert<BurnIn_Result>(new BurnIn_Result
                                                {
                                                    InverterSN = data.inverterSN,
                                                    PvTime = num1,
                                                    BatTime = num2,
                                                    PvResult = str1,
                                                    BatResult = str2,
                                                    TestTime = Convert.ToDateTime(data.createDate)
                                                }).ExecuteAffrows();
                                            }

                                        }
                                        else
                                        {
                                            string status = "FAIL";
                                            int num7 = data.CommunicationMode == 3 || data.CommunicationMode == 5 ? num6 * 60 / 2 : num6 * 60;
                                            if (num5 >= num6)
                                            {
                                                Stopwatch stopwatch = new Stopwatch();
                                                stopwatch.Start();
                                                string allData2 = HttpApi.HttpPostAllData(data.inverterSN, Convert.ToDateTime(data.createDate), num5);
                                                stopwatch.Stop();
                                                long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                                                New_DataRoot? dataRoot = JsonConvert.DeserializeObject<New_DataRoot>(allData2);
                                                if (dataRoot != null)
                                                {
                                                    int count = dataRoot.data.data.Count;
                                                    double num8 = (double)(num7 - count) * 1.0 / (double)num7;
                                                    if (num8 > 0.2)
                                                    {
                                                        status = "FAIL";
                                                        string str3 = string.Format(":丢包率过高!({0:N2}%)", (object)(num8 * 100.0));
                                                        AddInfo(data.inverterSN + str3, Convert.ToDateTime(data.createDate));
                                                    }
                                                    else
                                                        status = "PASS";
                                                    viewB.SetBurnInStatus(data.inverterSN, num5, status, firmware, internalFirmware);
                                                }
                                                else
                                                    continue;
                                            }
                                            else
                                            {
                                                switch (num4)
                                                {
                                                    case 0:
                                                        status = "正常";
                                                        break;
                                                    case 1:
                                                        status = "异常";
                                                        break;
                                                    case 2:
                                                        status = "FAIL";
                                                        break;
                                                }
                                                viewB.SetBurnInStatus(data.inverterSN, num5, status, firmware, internalFirmware);
                                            }
                                            if (flag & bBatBurnIn)
                                                str2 = status;
                                            else
                                                str1 = status;

                                            if (fsql.Select<BurnIn_Result>().Where(x => x.InverterSN == data.inverterSN).ToList().FirstOrDefault() != null)
                                            {
                                                fsql.Update<BurnIn_Result>(new BurnIn_Result
                                                {
                                                    InverterSN = data.inverterSN,
                                                    PvTime = num1,
                                                    BatTime = num2,
                                                    PvResult = str1,
                                                    BatResult = str2,
                                                    TestTime = Convert.ToDateTime(data.createDate),
                                                }).ExecuteAffrows();
                                            }
                                            else
                                            {
                                                fsql.Insert<BurnIn_Result>(new BurnIn_Result
                                                {
                                                    InverterSN = data.inverterSN,
                                                    PvTime = num1,
                                                    BatTime = num2,
                                                    PvResult = str1,
                                                    BatResult = str2,
                                                    TestTime = Convert.ToDateTime(data.createDate),
                                                }).ExecuteAffrows();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }));
            }
        }

        private double CalcDivTime(DateTime time1, DateTime time2)
        {
            return Math.Abs(time2.Subtract(time1).TotalSeconds);
        }
        private bool IsESInverter(string sn) => sn.Substring(2, 1) == "1";
        private double GetDivValue(double dValue1, double dValue2) => dValue2 > 1E-05 || dValue2 < -1E-05 ? Math.Abs(dValue1 / dValue2) : 0.0;

        private void GetErrorInfoCount(string sn, int burnTime, ref int commonErrorCount, ref int serialErrorCount)
        {
            try
            {
                DateTime dateTime = DateTime.Now;
                dateTime = dateTime.AddHours(burnTime * -1);
                var msg1s = fsql.Select<BurnIn_ErrorMsg>().Where(x => x.InverterSN == sn &&
                            x.CreationDate.CompareTo(dateTime) > 0).Distinct().ToList(x => x.CreationDate);
                if (msg1s != null)
                    commonErrorCount = msg1s.Count;

                var msg2s = fsql.Select<BurnIn_ErrorMsg>().Where(x => x.InverterSN == sn && x.ErrorLevel == 2 &&
                            x.CreationDate.CompareTo(dateTime) > 0).ToList();

                if (msg2s != null)
                    serialErrorCount = msg2s.Count;
            }
            catch
            (Exception ex)
            {
                Log.Error(ex.Message);
            }


        }

        private List<BurnIn_ErrorMsg>? GetErrorMsgList(New_BurnInData obj, ref bool bBatBurnIn)
        {
            bBatBurnIn = false;
            string inverterSn = obj.inverterSN;
            if (inverterSn == null || inverterSn.Length != 16)
                return null;
            double[] numArray1 = new double[10];
            double[] numArray2 = new double[10];
            numArray1[0] = Convert.ToDouble(obj.ipv1);
            numArray2[0] = Convert.ToDouble(obj.vpv1);
            numArray1[1] = Convert.ToDouble(obj.ipv2);
            numArray2[1] = Convert.ToDouble(obj.vpv2);
            numArray1[2] = Convert.ToDouble(obj.ipv3);
            numArray2[2] = Convert.ToDouble(obj.vpv3);
            numArray1[3] = Convert.ToDouble(obj.ipv4);
            numArray2[3] = Convert.ToDouble(obj.vpv4);
            numArray1[4] = Convert.ToDouble(obj.ipv5);
            numArray2[4] = Convert.ToDouble(obj.vpv5);
            numArray1[5] = Convert.ToDouble(obj.ipv6);
            numArray2[5] = Convert.ToDouble(obj.vpv6);
            numArray1[6] = Convert.ToDouble(obj.ipv7);
            numArray2[6] = Convert.ToDouble(obj.vpv7);
            numArray1[7] = Convert.ToDouble(obj.ipv8);
            numArray2[7] = Convert.ToDouble(obj.vpv8);
            numArray1[8] = Convert.ToDouble(obj.ipv9);
            numArray2[8] = Convert.ToDouble(obj.vpv9);
            numArray1[9] = Convert.ToDouble(obj.ipv10);
            numArray2[9] = Convert.ToDouble(obj.vpv10);
            double dValue1_1 = Convert.ToDouble(obj.pac) * 1000.0;
            uint uint32_1 = Convert.ToUInt32(obj.faultFlag1);
            uint uint32_2 = Convert.ToUInt32(obj.faultFlag2);
            int uint32_3 = (int)Convert.ToUInt32(obj.faultFlag3);
            double[] numArray3 = new double[4]
            {
                Convert.ToDouble(obj.temperature1),
                Convert.ToDouble(obj.temperature2),
                Convert.ToDouble(obj.temperature3),
                Convert.ToDouble(obj.temperature4)
            };
            double num1 = Convert.ToDouble(obj.batteryI);
            double num2 = Convert.ToDouble(obj.batteryV);
            double dValue1_2 = Convert.ToDouble(obj.batteryP) * 1000.0;
            bool flag1 = false;
            bool flag2 = false;
            if (num2 > 10.0)
            {
                flag2 = true;
                bBatBurnIn = true;
            }
            if (numArray2[0] > 100.0)
                flag1 = true;
            List<BurnIn_ErrorMsg> errorMsgList = new List<BurnIn_ErrorMsg>();
            SwSN swSn = new SwSN();
            if (flag1)
            {
                double dValue2 = 0.0;
                int pvCount = swSn.GetPvCount(inverterSn.Substring(11, 4));
                for (int index = 0; index < pvCount; ++index)
                    dValue2 += numArray1[index] * numArray2[index];
                double divValue = GetDivValue(dValue1_1, dValue2);
                if (divValue < 0.8 || divValue > 1.15)
                {
                    string strMsg = string.Format("Pac/Pdc异常({0:N2})", (object)divValue);
                    errorMsgList.Add(GetBurnInErrorMsg(obj, strMsg, 1));
                }
                double powerBySn = (double)swSn.GetPowerBySN(inverterSn);
                if (dValue1_1 < powerBySn * 0.7)
                {
                    string strMsg = "Pac<Pn*0.7";
                    errorMsgList.Add(GetBurnInErrorMsg(obj, strMsg, 1));
                }
                for (int index = 0; index < pvCount; ++index)
                {
                    if (numArray1[index] < 5.0)
                    {
                        string strMsg = string.Format("Ipv{0}<5A", (object)(index + 1));
                        errorMsgList.Add(GetBurnInErrorMsg(obj, strMsg, 1));
                        break;
                    }
                }
            }
            else if (flag2)
            {
                double dValue2 = num2 * num1;
                double divValue = GetDivValue(dValue1_2, dValue2);
                if (divValue < 0.8 || divValue > 1.15)
                {
                    string strMsg = string.Format("Pbat/(Vat*Ibat)异常({0:N2})", (object)divValue);
                    errorMsgList.Add(GetBurnInErrorMsg(obj, strMsg, 1));
                }
                double powerBySn = (double)swSn.GetPowerBySN(inverterSn);
                if (dValue1_2 < powerBySn * 0.7)
                {
                    string strMsg = "Pbat<Pn*0.7";
                    errorMsgList.Add(GetBurnInErrorMsg(obj, strMsg, 1));
                }
                if (num1 < 5.0)
                {
                    string strMsg = "Ibat<5A";
                    errorMsgList.Add(GetBurnInErrorMsg(obj, strMsg, 1));
                }
            }
            for (int index = 0; index < 4; ++index)
            {
                if (numArray3[index] > 100.0)
                {
                    string strMsg = "Temperature>100";
                    errorMsgList.Add(GetBurnInErrorMsg(obj, strMsg, 1));
                }
            }
            if (((int)uint32_1 & 8) != 0)
            {
                string strMsg = "故障代码FaultFlag1(直流分量超限)";
                errorMsgList.Add(GetBurnInErrorMsg(obj, strMsg, 2));
            }
            if (((int)uint32_1 & 128) != 0)
            {
                string strMsg = "故障代码FaultFlag1(母线电压超限)";
                errorMsgList.Add(GetBurnInErrorMsg(obj, strMsg, 2));
            }
            if (((int)uint32_1 & 256) != 0)
            {
                string strMsg = "故障代码FaultFlag1(设备温度超限)";
                errorMsgList.Add(GetBurnInErrorMsg(obj, strMsg, 2));
            }
            if (uint32_2 > 0U)
            {
                string strMsg = "故障代码FaultFlag2异常";
                errorMsgList.Add(GetBurnInErrorMsg(obj, strMsg, 2));
            }
            return errorMsgList;
        }

        private BurnIn_ErrorMsg GetBurnInErrorMsg(New_BurnInData obj, string strMsg, int errorLevel) => new BurnIn_ErrorMsg()
        {
            InverterSN = obj.inverterSN,
            ErrorMsg = strMsg,
            ErrorLevel = errorLevel,
            CreationDate = Convert.ToDateTime(obj.createDate)
        };






        private void DeviceBind_Click(object sender, RoutedEventArgs e)
        {
            DeviceBindView deviceBindView = new DeviceBindView();
            deviceBindView.ShowDialog();
        }

        private void ParaSet_Click(object sender, RoutedEventArgs e)
        {
            ParaSetView paraSetView = new ParaSetView();
            paraSetView.ShowDialog();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BurnInDataView burnInDataView = new BurnInDataView();
            burnInDataView.ShowDialog();
        }

        private object _lockObject3 = new object();
        private void Export_Click(object sender, RoutedEventArgs e)
        {
            ExportTypeView exportTypeView = new ExportTypeView();
            if (exportTypeView.ShowDialog() != true)
                return;

            List<string> conditions = exportTypeView.exportTypes;
            string contents = "";

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = " 数据导出";
            saveFileDialog.DefaultExt = "txt";
            saveFileDialog.FileName = "SN";
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files|*.*";
            if (saveFileDialog.ShowDialog() != true)
                return;

            lock (_lockObject3)
            {
                try
                {
                    for (int i = 0; i < wpView.Children.Count; i++)
                    {
                        var view = wpView.Children[i] as BurnModelView;
                        if (view != null)
                        {
                            contents += (view.Id + "\r\n");
                            foreach (var data in view._displayDatas)
                            {
                                if (conditions.Contains(data.Status))
                                {
                                    contents += (data.SN + " " + data.Status + "\r\n");
                                }
                            }
                            contents += Environment.NewLine;
                        }
                    }

                    File.WriteAllText(saveFileDialog.FileName, contents);
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

    public class LogInfo
    {
        public DateTime Time { get; set; } = DateTime.Now;
        public string Inf { get; set; }
    }

}
