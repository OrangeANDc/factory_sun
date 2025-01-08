using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunwaysFactoryProgram.Model
{
    public class New_ConfigDataRoot
    {
        public string code { get; set; }

        public string msg { get; set; }

        public New_ConfigData data { get; set; }

        public string time { get; set; }
    }

    public class New_ConfigData
    {
        /// <summary>
        /// 
        /// </summary>
        public string firstUnderVoltageTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int resumeFunctionSwitch { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int communicationVersion { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string bmsChargeImax { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string firstOverFrequencyTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string quType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string simccid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int internalFirmwareVersion { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string batteryProtocolSet { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lowThroughVoltage5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string yearMonth { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string allowParallelSwitch { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lowThroughVoltage1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lowThroughVoltage2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lowThroughVoltage3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string minuteSecond { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lowThroughVoltage4 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string overFrequencyLoadReductionStartFrequencyForFp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string frequencyUpperThresholdForFault { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string cycleEnableFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string protectionTimeForHighThroughVoltage2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string totalPowerSet { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string protectionTimeForHighThroughVoltage3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string secondOverFrequencyValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string updateDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string protectionTimeForHighThroughVoltage1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rsved42016 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rsved42015 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rsved42014 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string highThroughType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rsved42013 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rsved42018 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rsved42017 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string chargingModeSelection3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string chargingModeSelection2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string pfTypeCurve { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string offGridSocProtection { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string chargingModeSelection1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string highThroughEnable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string chargingModeSelection6 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string chargingModeSelection5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string chargingModeSelection4 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string startTime2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string inQuPowerThreshold { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string dataLoggerSn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string startTime1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string startTime6 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string startTime5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string startTime4 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string startTime3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int safetyCountry { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string slaveControlSn4 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string slaveControlSn3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string slaveControlSn2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string pfPowerC { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string slaveControlSn1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string slaveControlSn8 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string pfPowerA { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string slaveControlSn7 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string pfPowerB { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string slaveControlSn6 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string slaveControlSn5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string languageSetting { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string slaveControlSn9 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rsved42009 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rsved42008 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rsved42007 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string inverterOnOffE2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string antiRefluxChoose { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rsved42012 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rsved42011 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rsved42010 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string secondDomain { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string overFrequencyLoadReductionTypeForFp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lowThroughReactivePowerGenerationEnable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string powerSlowEnable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string batteryModelSet { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int inverterReconnectionTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string hybirdWorkMode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string peakLoadShiftSwitch { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string firstUnderFrequencyTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string voltage3ForPuCurve { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string pfCurvePowerC { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string pfCurvePowerB { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string pfCurvePowerA { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string protectionTimeForLowThroughVoltage1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string slaveControlSn10 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string protectionTimeForLowThroughVoltage4 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string slaveControlSn11 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string protectionTimeForLowThroughVoltage5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string slaveControlSn12 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string protectionTimeForLowThroughVoltage2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string slaveControlSn13 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string protectionTimeForLowThroughVoltage3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string slaveControlSn14 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string isLandSwitch { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string slaveControlSn15 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int deviceInfo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? communicationMode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string slaveControlSn16 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string checkCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string voltageUpperThresholdForNoFault { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string firstUnderVoltageValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lowThroughEnable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string totalTimeSet { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string voltage4ForPuCurve { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string secondUnderVoltageTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string power3ForPuCurve { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string gridMaximumCapacitySet { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string batteryTypeCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string underFrequencyRecoveryLimit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string power4ForPuCurve { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string secondOverVoltageValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string slowLoadRate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string endTime1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string quVoltage1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string endTime3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string endTime2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string endTime5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string endTime4 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string batteryProtocol { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string endTime6 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string overVoltageLoadReductionEnable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string powerLimitation1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string powerLimitation2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int startingVoltageOfInverter { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string powerLimitation5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string quVoltage4 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rtcTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string powerLimitation6 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string powerLimitation3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string quVoltage2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string powerLimitation4 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string quVoltage3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int dataUploadTimeSet { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string recoveryPowerFrequencyThresholdForFpOverFrequencyLoadReduction { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string gridConnectedSocProtection { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double pfSettings { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lowThroughExitVoltage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string powerSlowRate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string powerRecoverySlowLoadRateForFpOverFrequencyLoadReduction { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string chargeDischargeSelection6 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string chargeDischargeSelection2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string overFrequencyRecoveryLimit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string chargeDischargeSelection3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string chargeDischargeSelection4 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string chargeDischargeSelection5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string chargeDischargeSelection1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string overVoltageRecoveryLimit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string frequencyUpperThresholdForNoFault { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string offGridVoltageSet { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string pfFunctionSwitch { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string slowLoadEnable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string activePowerLimitE2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string highThroughVoltage1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string highThroughVoltage2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string frequencyLowerThresholdForNoFault { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string tenMinOverVoltageThreshold { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string waitingTimeForFault { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string batteryStrings { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rsved28104 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rsved28103 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string highThroughVoltage3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rsved28106 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string secondOverFrequencyTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rsved28105 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rsved28108 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string highThroughExitVoltage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rsved28107 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string quFunctionSwitch { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string gridConnectedUnbalancedSwitch { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string offGridOverload { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string meterSn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string highThroughReactivePowerGenerationEnable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string pfCurveInVoltage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string waitingTimeForNoFault { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string pfCurveOutVoltage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string softwareVersion { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string voltageLowerThresholdForFault { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rsved28111 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rsved28110 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rsved28113 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rsved28112 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int isoLimits { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rsved28115 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rsved28114 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rsved28117 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rsved28116 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int antiCounterCurrentStartStop { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rsved28119 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rsved28118 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rsved28109 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string secondUnderFrequencyValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string quReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string bmsDischargeImax { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string frequencyLowerThresholdForFault { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string overVoltageLoadReductionType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string hardwareVersion { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lowThroughType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rsved28120 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string secondUnderFrequencyTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rsved28122 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rsved28121 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rsved28124 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rsved28123 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rsved28126 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rsved28125 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string overFrequencyLoadReductionEndFrequencyPowerForFp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rsved28127 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string outQuPowerThreshold { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string powerRecoveryWaitingTimeForFpOverFrequencyLoadReduction { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string secondOverVoltageTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string voltageLowerThresholdForNoFault { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string underVoltageRecoveryLimit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string quReactive3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string quReactive4 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string quReactive1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ctRatioSetting { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string quReactive2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string powerRecoverySlowLoadEnableForFpOverFrequencyLoadReduction { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int firmwareVersion { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int outputModeSetting { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string gridConnectedDischargeDepth { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string imeimac { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string secondUnderVoltageValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string relaxSwitch { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string creationDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string firstUnderFrequencyValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string dischargeModeSelection5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string highThroughEntryVoltage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string dischargeModeSelection6 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string dischargeModeSelection3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string dischargeModeSelection4 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string dischargeModeSelection1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string dischargeModeSelection2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string moduleFirmwareVersion { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string daysSelection6 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string daysSelection5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string firstOverFrequencyValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string firstOverVoltageTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string firstOverVoltageValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int outputType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string tenMinOverVoltageEnable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string overFrequencyLoadReductionStartFrequencyPowerForFp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string emergencyStopFunctionSwitch { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reconnectionLimitEnable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string pfCurveOutPower { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string dayHour { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string overFrequencyLoadReductionEnableForFp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string voltageUpperThresholdForFault { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string offGridFrequencySet { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string masterControlSn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double antiReverseCurrentPowerSetting { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lowThroughEntryVoltage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string offGridDischargeDepth { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string overFrequencyLoadReductionEndFrequencyForFp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string daysSelection4 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string daysSelection3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string daysSelection2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string inverterSn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string daysSelection1 { get; set; }
    }

   
}
