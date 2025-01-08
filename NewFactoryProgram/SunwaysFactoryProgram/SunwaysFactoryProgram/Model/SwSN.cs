using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunwaysFactoryProgram.Model
{
    public class SwSN
    {
        public string Company_test = "sunways-test";
        public string Company_sunways = "sunways";
        public string Company_Aarusha = "Aarusha";
        public string Company_GikaSun = "GikaSun";
        public string Company_selfa = "selfa";
        public string Company_wattsonic = "wattsonic";
        public string Company_Stromherz = "Stromherz";
        public string Company_Nelumbo = "Nelumbo";
        public string Company_ReComm = "ReComm";
        public string Company_REGITIC = "REGITIC";
        public string Company_M_TECH = "M-TECH";
        public string Company_test_short = "TEST";
        public string Company_sunways_short = "SUN";
        public string Company_Aarusha_short = "ASE";
        public string Company_GikaSun_short = "GIK";
        public string Company_selfa_short = "SEL";
        public string Company_wattsonic_short = "WAT";
        public string Company_Stromherz_short = "STR";
        public string Company_Nelumbo_short = "NLB";
        public string Company_ReComm_short = "RCM";
        public string Company_REGITIC_short = "REG";
        public string Company_M_TECH_short = "TEC";
        public string Company_test_bit = "00";
        public string Company_sunways_bit = "10";
        public string Company_Aarusha_bit = "20";
        public string Company_GikaSun_bit = "30";
        public string Company_selfa_bit = "40";
        public string Company_wattsonic_bit = "50";
        public string Company_Stromherz_bit = "60";
        public string Company_Nelumbo_bit = "70";
        public string Company_ReComm_bit = "80";
        public string Company_REGITIC_bit = "Z0";
        public string Company_M_TECH_bit = "Z1";
        public string DevicType_Grid = "0";
        public string DevicType_ES = "1";
        public string DevicType_Meter = "M";
        public string DevicType_Logger = "L";
        public string[] RatePower_Bit = new string[11]
        {
      "0",
      "1",
      "2",
      "3",
      "4",
      "5",
      "6",
      "7",
      "8",
      "9",
      "A"
        };
        public string DeviceInfoDS_3_6 = "01";
        public string DeviceInfoSS_1_3 = "02";
        public string DeviceInfoDS_7_11 = "03";
        public string DeviceInfoDT_3_6 = "11";
        public string DeviceInfoDT_4_25 = "12";
        public string DeviceInfoDT_30_60 = "16";
        public string DeviceInfoDT_33_75 = "20";
        public string DeviceInfoDT_80_125 = "21";
        public string DeviceInfoHT_4_12 = "30";
        public string DeviceInfoHS_3_8 = "31";
        public const int PvCount_1 = 1;
        public const int PvCount_2 = 2;
        public const int PvCount_4 = 4;
        public const int PvCount_6 = 6;
        public const int PvCount_10 = 10;
        public const string Country_Sunways = "sunways";
        public const string Area_Inside = "国内";
        public const string Area_Outside = "国外";
        public const string Device_Empty = "空壳机";
        public const string Device_Full = "标准机";
        public const string Device_R16 = "R1.6";
        public const string Device_R18 = "R1.8";
        public const string Meter_OnePhase = "单相";
        public const string Meter_ThreePhase = "三相";
        public const string Meter_TwoPhase = "三相接2火线";
        public const string Meter_STM = "STM";
        public const string Meter_STK = "STK";
        public const string Meter_STP = "STK-PRO";
        public const string Logger_Cfg2_NULL = "NULL";
        public const string Logger_Cfg2_RS485 = "RS485";
        public const string Logger_Cfg2_PLC = "PLC";
        public const string Logger_Cfg3_NULL = "NULL";
        public const string Logger_Cfg3_WIFI = "WIFI";
        public const string Logger_Cfg3_2G = "2G";
        public const string Logger_Cfg3_4G = "4G";
        public SwSN.Product_Type ProductType;
        public List<CodeModel> listMeterConfig = new List<CodeModel>();
        public List<CodeModel> listCtProvider = new List<CodeModel>();
        public List<CodeModel> listCtConfig = new List<CodeModel>();
        public List<CodeModel> listModuleConfig = new List<CodeModel>();
        public List<OdmCompany> listCompany = new List<OdmCompany>();
        public List<SNModel2> listModelInfo = new List<SNModel2>();

        public string CompanyBit0 { get; set; }

        public string SellAreaBit1 { get; set; }

        public string DeviceTypeBit2 { get; set; }

        public string YearBit34 { get; set; }

        public string BackDeviceBit5 { get; set; }

        public string FlowCodeBit6 { get; set; }

        public string DeviceConfigBit7 { get; set; }

        public string FlowCode8910 { get; set; }

        public string DeviceInfoBit1112 { get; set; }

        public string CompanyBit13 { get; set; }

        public string RatePowerBit14 { get; set; }

        public string MonthBit15 { get; set; }

        public string ModelName { get; set; }

        public string CompanyName { get; set; }

        public string Area { get; set; }

        public string Config1 { get; set; }

        public string Config2 { get; set; }

        public string Config3 { get; set; }

        public string CtProvider { get; set; }

        public string CtConfig { get; set; }

        public string ModuleProvider { get; set; }

        public string ModuleConfig { get; set; }

        public SwSN()
        {
            this.InitListCompany();
            this.InitListModel();
        }

        public void InitListModel()
        {
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDS_3_6, this.RatePower_Bit[0], "STS-3KTL", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDS_3_6, this.RatePower_Bit[1], "STS-3.6KTL", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDS_3_6, this.RatePower_Bit[2], "STS-4.2KTL", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDS_3_6, this.RatePower_Bit[3], "STS-4.6KTL", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDS_3_6, this.RatePower_Bit[4], "STS-5KTL", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDS_3_6, this.RatePower_Bit[5], "STS-6KTL", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoSS_1_3, this.RatePower_Bit[0], "STS-700TL-S", 1);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoSS_1_3, this.RatePower_Bit[1], "STS-1KTL-S", 1);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoSS_1_3, this.RatePower_Bit[2], "STS-1.5KTL-S", 1);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoSS_1_3, this.RatePower_Bit[3], "STS-2KTL-S", 1);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoSS_1_3, this.RatePower_Bit[4], "STS-2.5KTL-S", 1);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoSS_1_3, this.RatePower_Bit[5], "STS-3KTL-S", 1);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoSS_1_3, this.RatePower_Bit[6], "STS-3.3KTL-S", 1);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDS_7_11, this.RatePower_Bit[0], "STS-7KTL", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDS_7_11, this.RatePower_Bit[1], "STS-8KTL", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDS_7_11, this.RatePower_Bit[2], "STS-9KTL", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDS_7_11, this.RatePower_Bit[3], "STS-10KTL", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDS_7_11, this.RatePower_Bit[4], "STS-11KTL", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_3_6, this.RatePower_Bit[0], "STT-3KTL-MINI", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_3_6, this.RatePower_Bit[1], "STT-4KTL-MINI", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_3_6, this.RatePower_Bit[2], "STT-5KTL-MINI", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_3_6, this.RatePower_Bit[3], "STT-6KTL-MINI", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_4_25, this.RatePower_Bit[0], "STT-6KTL", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_4_25, this.RatePower_Bit[1], "STT-8KTL", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_4_25, this.RatePower_Bit[2], "STT-10KTL", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_4_25, this.RatePower_Bit[3], "STT-12KTL", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_4_25, this.RatePower_Bit[4], "STT-15KTL", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_4_25, this.RatePower_Bit[5], "STT-17KTL", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_4_25, this.RatePower_Bit[6], "STT-20KTL", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_4_25, this.RatePower_Bit[7], "STT-25KTL", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_4_25, this.RatePower_Bit[8], "STT-4KTL", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_4_25, this.RatePower_Bit[9], "STT-5KTL", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_30_60, this.RatePower_Bit[0], "STT-29.9KTL", 4);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_30_60, this.RatePower_Bit[1], "STT-30KTL", 4);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_30_60, this.RatePower_Bit[2], "STT-33KTL", 4);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_30_60, this.RatePower_Bit[3], "STT-36KTL", 4);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_30_60, this.RatePower_Bit[4], "STT-40KTL", 4);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_30_60, this.RatePower_Bit[5], "STT-45KTL", 4);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_30_60, this.RatePower_Bit[6], "STT-50KTL-M", 4);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_30_60, this.RatePower_Bit[7], "STT-60KTL-M", 4);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_30_60, this.RatePower_Bit[8], "STT-40KTL-HV", 4);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_30_60, this.RatePower_Bit[9], "STT-50KTL-HV", 4);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_30_60, this.RatePower_Bit[10], "STT-60KTL-HV", 4);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_33_75, this.RatePower_Bit[0], "STT-50KTL", 6);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_33_75, this.RatePower_Bit[1], "STT-60KTL", 6);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_33_75, this.RatePower_Bit[2], "STT-70KTL-HV", 6);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_33_75, this.RatePower_Bit[3], "STT-75KTL-HV", 6);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_33_75, this.RatePower_Bit[4], "STT-33KTL-LV", 6);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_80_125, this.RatePower_Bit[0], "STT-80KTL", 10);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_80_125, this.RatePower_Bit[1], "STT-90KTL", 10);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_80_125, this.RatePower_Bit[2], "STT-100KTL", 10);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_80_125, this.RatePower_Bit[3], "STT-110KTL", 10);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_80_125, this.RatePower_Bit[4], "STT-80KTL-HV", 10);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_80_125, this.RatePower_Bit[5], "STT-90KTL-HV", 10);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_80_125, this.RatePower_Bit[6], "STT-100KTL-HV", 10);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_80_125, this.RatePower_Bit[7], "STT-110KTL-HV", 10);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_80_125, this.RatePower_Bit[8], "STT-125KTL-HV", 10);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_Grid, this.DeviceInfoDT_80_125, this.RatePower_Bit[9], "STT-125KTL-BHV", 10);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_ES, this.DeviceInfoHT_4_12, this.RatePower_Bit[0], "STH-4KTL-HT", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_ES, this.DeviceInfoHT_4_12, this.RatePower_Bit[1], "STH-5KTL-HT", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_ES, this.DeviceInfoHT_4_12, this.RatePower_Bit[2], "STH-6KTL-HT", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_ES, this.DeviceInfoHT_4_12, this.RatePower_Bit[3], "STH-8KTL-HT", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_ES, this.DeviceInfoHT_4_12, this.RatePower_Bit[4], "STH-10KTL-HT", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_ES, this.DeviceInfoHT_4_12, this.RatePower_Bit[5], "STH-12KTL-HT", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_ES, this.DeviceInfoHS_3_8, this.RatePower_Bit[0], "STH-3KTL-HS", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_ES, this.DeviceInfoHS_3_8, this.RatePower_Bit[1], "STH-3.6KTL-HS", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_ES, this.DeviceInfoHS_3_8, this.RatePower_Bit[2], "STH-4.2KTL-HS", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_ES, this.DeviceInfoHS_3_8, this.RatePower_Bit[3], "STH-4.6KTL-HS", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_ES, this.DeviceInfoHS_3_8, this.RatePower_Bit[4], "STH-5KTL-HS", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_ES, this.DeviceInfoHS_3_8, this.RatePower_Bit[5], "STH-6KTL-HS", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_ES, this.DeviceInfoHS_3_8, this.RatePower_Bit[6], "STH-7KTL-HS", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_ES, this.DeviceInfoHS_3_8, this.RatePower_Bit[7], "STH-8KTL-HS", 2);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_ES, this.DeviceInfoHS_3_8, this.RatePower_Bit[8], "STH-3KTL-HSS", 1);
            this.AddListModel(this.Company_sunways_bit, this.DevicType_ES, this.DeviceInfoHS_3_8, this.RatePower_Bit[9], "STH-3.6KTL-HSS", 1);
        }

        public void AddListModel(
          string company,
          string deviceType,
          string deviceInfo,
          string ratePower,
          string model,
          int pvCount)
        {
            this.listModelInfo.Add(new SNModel2()
            {
                CompanyBit0 = company.Substring(0, 1),
                CompanyBit13 = company.Substring(1, 1),
                DeviceTypeBit2 = deviceType,
                DeviceInfoBit1112 = deviceInfo,
                RatePowerBit14 = ratePower,
                Model = model,
                PvCount = pvCount
            });
        }

        public void InitListCompany()
        {
            this.AddListCompany("sunways", "SUN", "10");
            this.AddListCompany("Aarusha", "ASE", "20");
            this.AddListCompany("GikaSun", "GIK", "30");
            this.AddListCompany("selfa", "SEL", "40");
            this.AddListCompany("wattsonic", "WAT", "50");
            this.AddListCompany("Stromherz", "STR", "60");
            this.AddListCompany("Nelumbo", "NLB", "70");
            this.AddListCompany("ReComm", "RCM", "80");
            this.AddListCompany("REGITIC", "REG", "81");
            this.AddListCompany("SALICRU", "SSA", "90");
            this.AddListCompany("M-TECH", "TEC", "Z1");
            this.AddListCompany("sunways-test", "TEST", "00");
        }

        public void AddListCompany(string companyName, string shortName, string code) => this.listCompany.Add(new OdmCompany()
        {
            CompanyName = companyName,
            ShortName = shortName,
            CompanyCode = code
        });

        public long GetPowerBySN(string sn)
        {
            string modelBySn = this.GetModelBySN(sn);
            if (modelBySn == "")
                return 0;
            string[] strArray = modelBySn.Split('-');
            if (strArray.Length < 2)
                return 0;
            int length = strArray[1].IndexOf("KTL");
            return length <= 0 ? 0L : (long)(Convert.ToDouble(strArray[1].Substring(0, length)) * 1000.0);
        }

        public string GetModelBySN(string sn)
        {
            if (sn.Length < 16)
                return "";
            string str1 = sn.Substring(11, 2);
            string str2 = sn.Substring(14, 1);
            for (int index = 0; index < this.listModelInfo.Count; ++index)
            {
                if (this.listModelInfo[index].DeviceInfoBit1112 == str1 && this.listModelInfo[index].RatePowerBit14 == str2)
                    return this.listModelInfo[index].Model;
            }
            return "";
        }

        public int GetPvCount(string deviceInfo)
        {
            if (deviceInfo.Length != 4)
                return 0;
            string str1 = deviceInfo.Substring(0, 2);
            string str2 = deviceInfo.Substring(3, 1);
            for (int index = 0; index < this.listModelInfo.Count; ++index)
            {
                if (this.listModelInfo[index].DeviceInfoBit1112 == str1 && this.listModelInfo[index].RatePowerBit14 == str2)
                    return this.listModelInfo[index].PvCount;
            }
            return 0;
        }

        public enum Product_Type
        {
            en_Inverter,
            en_CT,
            en_Module,
            en_Meter,
            en_Logger,
        }
    }

    public class OdmCompany
    {
        public string CompanyName { get; set; }

        public string ShortName { get; set; }

        public string CompanyCode { get; set; }
    }

    public class SNModel2
    {
        public string CompanyBit0 { get; set; }

        public string SellAreaBit1 { get; set; }

        public string DeviceTypeBit2 { get; set; }

        public string YearBit34 { get; set; }

        public string BackDeviceBit5 { get; set; }

        public string FlowCodeBit6 { get; set; }

        public string DeviceConfigBit7 { get; set; }

        public string FlowCode8910 { get; set; }

        public string DeviceInfoBit1112 { get; set; }

        public string CompanyBit13 { get; set; }

        public string RatePowerBit14 { get; set; }

        public string MonthBit15 { get; set; }

        public string Model { get; set; }

        public int PvCount { get; set; }
    }

    public class CodeModel
    {
        public string DataType { get; set; }

        public string DataValue { get; set; }

        public string DataCode { get; set; }

        public string InvCode { get; set; }

        public string Provider { get; set; }
    }
}
