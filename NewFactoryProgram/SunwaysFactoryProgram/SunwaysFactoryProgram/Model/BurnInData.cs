using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunwaysFactoryProgram.Model
{
    public class BurnInDataSupply
    {
        public int DeviceInfo { get; set; } = -1;
        public int? CommunicationMode { get; set; } = -1;
        public uint Firmwareversion { get; set; }
        public uint InternalFirmwareVersion { get; set; }

        public bool DataUpdate { get; set; }

    }

    public class New_BurnInData  : BurnInDataSupply
    {
        // 设备 SN 号
        public string inverterSN { get; set; }
        // 采集器SN
        public string dataLoggerSN { get; set; }
        // 数据入库时间
        public string createTime { get; set; }
        // 创建时间
        public string createDate { get; set; }
        // 创建时间
        public string rtcDate { get; set; }
        public string rtcTime { get; set; }
        public string rssi { get; set; }
        // 数据类型(0或空：正常值 1：脏数据 2：未创建电站数据 3：老化数据)
        public string dataTypeFlg { get; set; }
        // 0：正常数据 1：续传数据
        public string resumedType { get; set; }
        public string yearMonth { get; set; }
        public string dayHour { get; set; }
        public string minuteSecond { get; set; }
        // 安规/地区代码
        public string saftyCountryCode { get; set; }
        // 逆变器工作状态
        public string workStatus { get; set; }
        // 逆变器运行状态 1
        public string workStatus1 { get; set; }
        // 逆变器运行状态 2
        public string workStatus2 { get; set; }
        // 逆变器运行状态 3
        public string workStatus3 { get; set; }
        // 故障 FLAG1
        public string faultFlag1 { get; set; }
        // 故障 FLAG2
        public string faultFlag2 { get; set; }
        // 警告 FLAG1
        public string warnFlag1 { get; set; }
        // 警告 FLAG2
        public string warnFlag2 { get; set; }
        // 故障 FLAG3
        public string faultFlag3 { get; set; }
        // 警告 FLAG3
        public string warnFlag3 { get; set; }
        // A 相电表功率
        public string pmeterPhaseA { get; set; }
        // B 相电表功率
        public string pmeterPhaseB { get; set; }
        // C 相电表功率
        public string pmeterPhaseC { get; set; }
        // 电表总功率
        public string pmeterTotal { get; set; }
        // 电表总卖电量
        public string emeterTotalSell { get; set; }
        // 电表总买电量
        public string emeterTotalBuy { get; set; }
        // 电网 A 相电压
        public string vgridPhaseA { get; set; }
        // 电网 A 相电流
        public string igridPhaseA { get; set; }
        // 电网 B 相电压
        public string vgridPhaseB { get; set; }
        // 电网 B 相电流
        public string igridPhaseB { get; set; }
        // 电网 C 相电压
        public string vgridPhaseC { get; set; }
        // 电网 C 相电流
        public string igridPhaseC { get; set; }
        // 电网频率
        public string fgrid { get; set; }
        // 瞬时有功功率
        public string pac { get; set; }
        // 当日发电量
        public string eday { get; set; }
        // 总发电量
        public string etotal { get; set; }
        // 总发电时间
        public string timeTotal { get; set; }
        // PV 输入总功率
        public string ppvInput { get; set; }
        // 功率因数
        public string pf { get; set; }
        // 逆变器效率
        public string inverterEfficiency { get; set; }
        // 温度 1
        public string temperature1 { get; set; }
        public string temperature2 { get; set; }
        public string temperature3 { get; set; }
        public string temperature4 { get; set; }
        // PV1 电压
        public string vpv1 { get; set; }
        // PV1 电流
        public string ipv1 { get; set; }
        // PV2 电压
        public string vpv2 { get; set; }
        // PV2 电流
        public string ipv2 { get; set; }
        // PV3 电压
        public string vpv3 { get; set; }
        // PV3 电流
        public string ipv3 { get; set; }
        // PV4 电压
        public string vpv4 { get; set; }
        // PV4 电流
        public string ipv4 { get; set; }
        // PV5 电压
        public string vpv5 { get; set; }
        // PV5 电流
        public string ipv5 { get; set; }

        // PV6 电压


        public string vpv6 { get; set; }

        // PV6 电流


        public string ipv6 { get; set; }

        // PV7 电压


        public string vpv7 { get; set; }

        // PV7 电流


        public string ipv7 { get; set; }

        // PV8 电压


        public string vpv8 { get; set; }

        // PV8 电流


        public string ipv8 { get; set; }

        // PV9 电压


        public string vpv9 { get; set; }

        // PV9 电流


        public string ipv9 { get; set; }

        // PV10 电压


        public string vpv10 { get; set; }

        // PV10 电流


        public string ipv10 { get; set; }


        // ARM 故障FLAG1

        public string faultArmFlag1 { get; set; }

        // ARM 故障FLAG2

        public string faultArmFlag2 { get; set; }


        public string warnArmFlag1 { get; set; }


        public string warnArmFlag2 { get; set; }


        public string inverterArmStatus1 { get; set; }


        public string inverterArmStatus2 { get; set; }

        // Backup_A_V


        public string backupAV { get; set; }

        // Backup_A_I


        public string backupAI { get; set; }

        // Backup_A_F


        public string backupAF { get; set; }

        // Backup_A_Mode

        public string backupAMode { get; set; }

        // Backup_A_P

        public string backupAP { get; set; }

        // Backup_A_S


        public string backupAS { get; set; }

        // Backup_A_Q


        public string backupAQ { get; set; }

        // Backup_B_V


        public string backupBV { get; set; }

        // Backup_B_I


        public string backupBI { get; set; }

        // Backup_B_F


        public string backupBF { get; set; }

        // Backup_B_Mode

        public string backupBMode { get; set; }

        // Backup_B_P


        public string backupBP { get; set; }

        // Backup_B_S


        public string backupBS { get; set; }

        // Backup_B_Q


        public string backupBQ { get; set; }

        // Backup_C_V


        public string backupCV { get; set; }

        // Backup_C_I


        public string backupCI { get; set; }

        // Backup_C_F


        public string backupCF { get; set; }

        // Backup_C_Mode

        public string backupCMode { get; set; }

        // Backup_C_P


        public string backupCP { get; set; }

        // Backup_C_S


        public string backupCS { get; set; }

        // Backup_C_Q


        public string backupCQ { get; set; }

        // Total_Backup_P


        public string totalBackupP { get; set; }

        // Total_Backup_S


        public string totalBackupS { get; set; }

        // Total_Backup_Q


        public string totalBackupQ { get; set; }



        public string invtAP { get; set; }



        public string invtAS { get; set; }



        public string invtAQ { get; set; }



        public string invtBP { get; set; }



        public string invtBS { get; set; }



        public string invtBQ { get; set; }



        public string invtCP { get; set; }



        public string invtCS { get; set; }



        public string invtCQ { get; set; }

        // Battery_V


        public string batteryV { get; set; }

        // Battery_I


        public string batteryI { get; set; }

        // Battery_Mode 电池型号

        public string batteryMode { get; set; }

        // Battery_P


        public string batteryP { get; set; }


        // 日卖电量


        public string esellDay { get; set; }

        // 日买电量


        public string ebuyDay { get; set; }

        // 日 Backup 负载电量


        public string ebackupDay { get; set; }

        // 日电池充电量


        public string ebatterychargeDaily { get; set; }

        // 日电池放电量


        public string ebatteryDischargeDaily { get; set; }

        // PV 日发电量


        public string epvDay { get; set; }

        // 日负载用电量


        public string eloadUseDay { get; set; }

        // 日负载上行电量


        public string eloadUpDay { get; set; }

        // 逆变器日用电量


        public string eInverterUseDay { get; set; }

        // 累计工作时间

        public string workHours { get; set; }

        // 总卖电量


        public string esellTotal { get; set; }

        // 总买电量


        public string ebuyTotal { get; set; }

        // 总 Backup 负载电量


        public string eTotalBackupLoad { get; set; }

        // 总电池充电量


        public string eTotalbatteryCharge { get; set; }

        // 总电池放电量


        public string eTotalbatteryDischarge { get; set; }

        // PV 总发电量


        public string etotalPv { get; set; }

        // 总负载用电量


        public string etotalLoaduse { get; set; }

        // 总负载上行电量


        public string eTotalLoadUplink { get; set; }

        // 逆变器总用电量


        public string etotalInverterUse { get; set; }

        // 总负载功率


        public string pload { get; set; }

        // SOC 电池剩余电量


        public string soc { get; set; }

        // SOH 电池健康强度


        public string soh { get; set; }


        public string bmsStatus { get; set; }

        // 电池温度


        public string bmsPackTemperature { get; set; }

        // Max Cell Voltage，最大电芯电压


        public string maxCellVoltage { get; set; }

        // Min Cell Voltage 最小电芯电压


        public string minCellVoltage { get; set; }

        // BMS ERROR CODE

        public string bmsErrorCode { get; set; }

        // BMS WARN CODE

        public string bmsWarnCode { get; set; }

        // 充电截止电压


        public string vChargingCutoff { get; set; }

        // 充电电流限值


        public string iChargingLimit { get; set; }

        // 放电截止电压


        public string vDischargeCutoff { get; set; }

        // 放电电流限值


        public string iDischargeLimit { get; set; }

        //	BUSVoltage


        public string busVoltage { get; set; }

        //	NBUSVoltage


        public string nbusVoltage { get; set; }

        // 	电网 AB 线电压


        public string vgridAB { get; set; }

        //	电网 BC 线电压


        public string vgridBC { get; set; }

        //	电网 CA 线电压


        public string vgridCA { get; set; }

        // 输出视在功率


        public string poutputApparent { get; set; }

        // 输出无功功率


        public string pOutputReactive { get; set; }

        // 组串 1 电流     


        public string istr1 { get; set; }
        // 组串 2 电流     


        public string istr2 { get; set; }
        // 组串 3 电流     


        public string istr3 { get; set; }
        // 组串 4 电流     


        public string istr4 { get; set; }
        // 组串 5 电流     


        public string istr5 { get; set; }
        // 组串 6 电流     


        public string istr6 { get; set; }
        // 组串 7 电流     


        public string istr7 { get; set; }
        // 组串 8 电流     


        public string istr8 { get; set; }
        // 组串 9 电流     


        public string istr9 { get; set; }
        // 组串 10 电流    


        public string istr10 { get; set; }
        // 组串 11 电流    


        public string istr11 { get; set; }
        // 组串 12 电流    


        public string istr12 { get; set; }
        // 组串 13 电流    


        public string istr13 { get; set; }
        // 组串 14 电流    


        public string istr14 { get; set; }
        // 组串 15 电流    


        public string istr15 { get; set; }
        // 组串 16 电流    


        public string istr16 { get; set; }
        // 组串 17 电流    


        public string istr17 { get; set; }
        // 组串 18 电流    


        public string istr18 { get; set; }
        // 组串 19 电流    


        public string istr19 { get; set; }
        // 组串 20 电流    


        public string istr20 { get; set; }
        // PV1 输入功率    


        public string ppv1 { get; set; }
        // PV2 输入功率    


        public string ppv2 { get; set; }
        // PV3 输入功率    


        public string ppv3 { get; set; }
        // PV4 输入功率    


        public string ppv4 { get; set; }
        // PV5 输入功率    


        public string ppv5 { get; set; }
        // PV6 输入功率    


        public string ppv6 { get; set; }
        // PV7 输入功率    


        public string ppv7 { get; set; }
        // PV8 输入功率    


        public string ppv8 { get; set; }
        // PV9 输入功率    


        public string ppv9 { get; set; }
        // PV10 输入功率   


        public string ppv10 { get; set; }
        // 电表接入状态  0：为异常  1：为正常

        public string meterStatus { get; set; }



        public string ploadPhaseA { get; set; }



        public string ploadPhaseB { get; set; }



        public string ploadPhaseC { get; set; }


        public string maxCellTemperatureID { get; set; }



        public string maxCellTemperature { get; set; }


        public string minCellTemperatureID { get; set; }



        public string minCellTemperature { get; set; }


        public string maxCelVoltageID { get; set; }


        public string minCelVoltageID { get; set; }

        // 更新时间

        public string updateDate { get; set; }

    }

    /*public class BurnInData
    {
        public BurnInData()
        {
        
            this.DeviceInfo = -1;
            this.CommunicationMode = -1;
        }

        public string InverterSN { get; set; }

        public string DataLoggerSN { get; set; }

        public string DataTypeFlg { get; set; }

        public string ResumedType { get; set; }

        public string YearMonth { get; set; }

        public string DayHour { get; set; }

        public string MinuteSecond { get; set; }

        public string RTCTime { get; set; }

        public string RSSI { get; set; }

        public string SaftyCountryCode { get; set; }

        public string WorkStatus { get; set; }

        public string WorkStatus1 { get; set; }

        public string WorkStatus2 { get; set; }

        public string WorkStatus3 { get; set; }

        public string FaultFlag1 { get; set; }

        public string FaultFlag2 { get; set; }

        public string WarnFlag1 { get; set; }

        public string WarnFlag2 { get; set; }

        public string FaultFlag3 { get; set; }

        public string WarnFlag3 { get; set; }

        public string Rsved_10124 { get; set; }

        public string Rsved_10125 { get; set; }

        public string Rsved_10126 { get; set; }

        public string Rsved_10127 { get; set; }

        public string Rsved_10128 { get; set; }

        public string Rsved_10129 { get; set; }

        public string Rsved_10130 { get; set; }

        public string Rsved_10131 { get; set; }

        public string Rsved_10132 { get; set; }

        public string Rsved_10133 { get; set; }

        public string Rsved_10134 { get; set; }

        public string Rsved_10135 { get; set; }

        public string Rsved_10136 { get; set; }

        public string Rsved_10137 { get; set; }

        public string Rsved_10138 { get; set; }

        public string Rsved_10139 { get; set; }

        public string Rsved_10140 { get; set; }

        public string Rsved_10141 { get; set; }

        public string Rsved_10142 { get; set; }

        public string Rsved_10143 { get; set; }

        public string PmeterPhaseA { get; set; }

        public string PmeterPhaseB { get; set; }

        public string PmeterPhaseC { get; set; }

        public string PmeterTotal { get; set; }

        public string EmeterTotalSell { get; set; }

        public string EmeterTotalBuy { get; set; }

        public string Vgrid_ab { get; set; }

        public string Vgrid_bc { get; set; }

        public string Vgrid_ca { get; set; }

        public string Vgrid_PhaseA { get; set; }

        public string Igrid_PhaseA { get; set; }

        public string Vgrid_PhaseB { get; set; }

        public string Igrid_PhaseB { get; set; }

        public string Vgrid_PhaseC { get; set; }

        public string Igrid_PhaseC { get; set; }

        public string Fgrid { get; set; }

        public string Pac { get; set; }

        public string Eday { get; set; }

        public string Etotal { get; set; }

        public string TimeTotal { get; set; }

        public string PoutputApparent { get; set; }

        public string POutputReactive { get; set; }

        public string PpvInput { get; set; }

        public string PF { get; set; }

        public string Inverter_efficiency { get; set; }

        public string Temperature1 { get; set; }

        public string Temperature2 { get; set; }

        public string Temperature3 { get; set; }

        public string Temperature4 { get; set; }

        public string BUSVoltage { get; set; }

        public string NBUSVoltage { get; set; }

        public string Vpv1 { get; set; }

        public string Ipv1 { get; set; }

        public string Vpv2 { get; set; }

        public string Ipv2 { get; set; }

        public string Vpv3 { get; set; }

        public string Ipv3 { get; set; }

        public string Vpv4 { get; set; }

        public string Ipv4 { get; set; }

        public string Vpv5 { get; set; }

        public string Ipv5 { get; set; }

        public string Vpv6 { get; set; }

        public string Ipv6 { get; set; }

        public string Istr1 { get; set; }

        public string Istr2 { get; set; }

        public string Istr3 { get; set; }

        public string Istr4 { get; set; }

        public string Istr5 { get; set; }

        public string Istr6 { get; set; }

        public string Istr7 { get; set; }

        public string Istr8 { get; set; }

        public string Istr9 { get; set; }

        public string Istr10 { get; set; }

        public string Istr11 { get; set; }

        public string Istr12 { get; set; }

        public string Ppv1 { get; set; }

        public string Ppv2 { get; set; }

        public string Ppv3 { get; set; }

        public string Ppv4 { get; set; }

        public string Ppv5 { get; set; }

        public string Ppv6 { get; set; }

        public string Vpv7 { get; set; }

        public string Ipv7 { get; set; }

        public string Vpv8 { get; set; }

        public string Ipv8 { get; set; }

        public string Vpv9 { get; set; }

        public string Ipv9 { get; set; }

        public string Vpv10 { get; set; }

        public string Ipv10 { get; set; }

        public string Istr13 { get; set; }

        public string Istr14 { get; set; }

        public string Istr15 { get; set; }

        public string Istr16 { get; set; }

        public string Istr17 { get; set; }

        public string Istr18 { get; set; }

        public string Istr19 { get; set; }

        public string Istr20 { get; set; }

        public string Ppv7 { get; set; }

        public string Ppv8 { get; set; }

        public string Ppv9 { get; set; }

        public string Ppv10 { get; set; }

        public string FaultArmFlag1 { get; set; }

        public string WarnArmFlag1 { get; set; }

        public string FaultArmFlag2 { get; set; }

        public string WarnArmFlag2 { get; set; }

        public string InverterArmStatus1 { get; set; }

        public string InverterArmStatus2 { get; set; }

        public string Backup_A_V { get; set; }

        public string Backup_A_I { get; set; }

        public string Backup_A_F { get; set; }

        public string Backup_A_Mode { get; set; }

        public string Backup_A_P { get; set; }

        public string Backup_A_S { get; set; }

        public string Backup_A_Q { get; set; }

        public string Backup_B_V { get; set; }

        public string Backup_B_I { get; set; }

        public string Backup_B_F { get; set; }

        public string Backup_B_Mode { get; set; }

        public string Backup_B_P { get; set; }

        public string Backup_B_S { get; set; }

        public string Backup_B_Q { get; set; }

        public string Backup_C_V { get; set; }

        public string Backup_C_I { get; set; }

        public string Backup_C_F { get; set; }

        public string Backup_C_Mode { get; set; }

        public string Backup_C_P { get; set; }

        public string Backup_C_S { get; set; }

        public string Backup_C_Q { get; set; }

        public string Total_Backup_P { get; set; }

        public string Total_Backup_S { get; set; }

        public string Total_Backup_Q { get; set; }

        public string Invt_A_P { get; set; }

        public string Invt_A_S { get; set; }

        public string Invt_A_Q { get; set; }

        public string Invt_B_P { get; set; }

        public string Invt_B_S { get; set; }

        public string Invt_B_Q { get; set; }

        public string Invt_C_P { get; set; }

        public string Invt_C_S { get; set; }

        public string Invt_C_Q { get; set; }

        public string Battery_V { get; set; }

        public string Battery_I { get; set; }

        public string Battery_Mode { get; set; }

        public string Rsved_40257 { get; set; }

        public string Battery_P { get; set; }

        public string Rsved_40260 { get; set; }

        public string Rsved_40261 { get; set; }

        public string Rsved_40262 { get; set; }

        public string Rsved_40263 { get; set; }

        public string Rsved_40264 { get; set; }

        public string Rsved_40265 { get; set; }

        public string EsellDay { get; set; }

        public string EbuyDay { get; set; }

        public string EbackupDay { get; set; }

        public string EbatterychargeDaily { get; set; }

        public string EbatteryDischargeDai { get; set; }

        public string EpvDay { get; set; }

        public string EloadUseDay { get; set; }

        public string EloadUpDay { get; set; }

        public string EInverterUseDay { get; set; }

        public string WorkHours { get; set; }

        public string EsellTotal { get; set; }

        public string EbuyTotal { get; set; }

        public string ETotalBackupLoad { get; set; }

        public string ETotalbatteryCharge { get; set; }

        public string ETotalbatteryDischar { get; set; }

        public string EtotalPv { get; set; }

        public string EtotalLoaduse { get; set; }

        public string ETotalLoadUplink { get; set; }

        public string EtotalInverterUse { get; set; }

        public string PloadPhaseA { get; set; }

        public string PloadPhaseB { get; set; }

        public string PloadPhaseC { get; set; }

        public string Pload { get; set; }

        public string SOC { get; set; }

        public string SOH { get; set; }

        public string BMSStatus { get; set; }

        public string BMSPackTemperature { get; set; }

        public string Rsved_43004 { get; set; }

        public string Rsved_43005 { get; set; }

        public string Rsved_43006 { get; set; }

        public string Rsved_43007 { get; set; }

        public string MaxCellTemperatureID { get; set; }

        public string MaxCellTemperature { get; set; }

        public string MinCellTemperatureID { get; set; }

        public string MinCellTemperature { get; set; }

        public string MaxCelVoltageID { get; set; }

        public string MaxCellVoltage { get; set; }

        public string MinCelVoltageID { get; set; }

        public string MinCellVoltage { get; set; }

        public string BMS_Error_Code { get; set; }

        public string BMS_Warn_Code { get; set; }

        public string VChargingCutoff { get; set; }

        public string IChargingLimit { get; set; }

        public string VDischargeCutoff { get; set; }

        public string IDischargeLimit { get; set; }

        public string Rsved_43024 { get; set; }

        public string Rsved_43025 { get; set; }

        public string Rsved_43026 { get; set; }

        public string Rsved_43027 { get; set; }

        public string Rsved_43028 { get; set; }

        public string Rsved_43029 { get; set; }

        public string Rsved_43030 { get; set; }

        public string Rsved_43031 { get; set; }

        public string Rsved_43032 { get; set; }

        public string Rsved_43033 { get; set; }

        public string Rsved_43034 { get; set; }

        public string Rsved_43035 { get; set; }

        public string Rsved_43036 { get; set; }

        public string Rsved_43037 { get; set; }

        public string Rsved_43038 { get; set; }

        public string Rsved_43039 { get; set; }

        public string Rsved_43040 { get; set; }

        public string Rsved_43041 { get; set; }

        public string Rsved_43042 { get; set; }

        public string Rsved_43043 { get; set; }

        public string Rsved_43044 { get; set; }

        public string Rsved_43045 { get; set; }

        public string Rsved_43046 { get; set; }

        public string Rsved_43047 { get; set; }

        public string Rsved_43048 { get; set; }

        public string Rsved_43049 { get; set; }

        public string Rsved_43050 { get; set; }

        public string Rsved_43051 { get; set; }

        public string Rsved_43052 { get; set; }

        public string Rsved_43053 { get; set; }

        public DateTime CreationDate { get; set; }

        public string UpdateDate { get; set; }

        public int DeviceInfo { get; set; }

        public int CommunicationMode { get; set; }

        public uint Firmwareversion { get; set; }

        public uint InternalFirmwareversion { get; set; }

        public bool DataUpdate { get; set; }
    }*/
}
