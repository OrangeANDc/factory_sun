using SafetyTestTool.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafetyTestTool.StaticSource
{
    public static class LanguageList
    {
        public static List<string> ChineseList = new List<string>
            {
                // 第1组
                "电网和系统保护" ,
                "一级欠压保护限值",
                "一级欠压保护时间",
                "一级过压保护限值",
                "一级过压保护时间",
                "二级欠压保护限值",
                "二级欠压保护时间",
                "二级过压保护限值",
                "二级过压保护时间",
                "一级欠频保护限值",
                "一级欠频保护时间",
                "一级过频保护限值",
                "一级过频保护时间",
                "二级欠频保护限值",
                "二级欠频保护时间",
                "二级过频保护限值",
                "二级过频保护时间", 

                // 第2组
                "无功功率/电压特性Q(U)",
                "QU曲线功能开关",
                                                                                                                                             "进入QU曲线功率阈值",
                "退出QU曲线功率阈值",
                 "QU时间常数",
                "最小功率因素限值",
                "QU曲线1点电压",
                "QU曲线1点无功百分比",
                "QU曲线2点电压",
                "QU曲线2点无功百分比",
                "QU曲线3点电压",
                "QU曲线3点无功百分比",
                "QU曲线4点电压",
                "QU曲线4点无功百分比",

                "定无功参数",
                "定无功限值",

                "定有功参数" ,
                "有功功率调度设置",

                "定功率因素参数",
                "定功率因素限值",


                // 第3组
                "功率因数/功率特性PF(P)",
                "PF曲线功能开关",
                "PF曲线功率点A", 
                "PF曲线功率点B",
                "PF曲线功率点C",
                "功率A点PF",
                "功率B点PF",
                "功率C点PF",
                "PF曲线进入电压阈值",
                "PF曲线退出电压阈值",
                "PF曲线退出功率阈值",

               
         
                // 第4组 
                "并网限制条件",
                "无故障并网等待时间",
                "电网故障重连等待时间",
                "欠压恢复限值",
                "过压恢复限值",
                "欠频恢复限值",
                "过频恢复限值",
                ////新加+++
                "功率缓变速率",    
   
         
                // 第5组 
                "过压时功率降低P(U)",
                "过压降载功能开关",
                "P(U)时间常数",
                "PU曲线电压点3号电压",
                "PU曲线3号电压对应的功率",
                "PU曲线电压点4号电压",
                "PU曲线4号电压对应的功率",
                         

                // 第6组            
                "过频时降低功率",
                "过频降载功能开关",
                "过频降载开始频率",
                "过频降载终点频率",       
                "FP过频降载恢复功率频率阈值",
                "FP过频降载恢复功率等待时间",
                "FP过频降载功率恢复缓慢加载开关",
                "FP过频降载功率恢复缓慢加载速率",
                "降载系数",
     
              
                // 第7组 
                "LVRT",
                "低穿功能开关",
                "低穿进入电压", 
                "低穿电压点1",      
                "低穿电压点2",
                "低穿电压点3",
                "低穿电压点4",
                "低穿电压点5",
                "低穿电压点1保护时间",
                "低穿电压点2保护时间",
                "低穿电压点3保护时间",
                "低穿电压点4保护时间",
                "低穿电压点5保护时间", 
          
                // 第8组 
                "HVRT",
                "高穿功能开关",
                "高穿进入电压",

                // 第12组 
                "其他",
                "10分钟过压开关",
                "10分钟过压阈值",

            };

        public static List<string> EnglishList = new List<string>
            {
                // 第1组
                "Grid and System Protection" ,
                "Level-1 Undervoltage Protection Threshold",
                "Level-1 Undervoltage Protection Duration",
                "Level-1 Overvoltage Protection Threshold",
                "Level-1 Overvoltage Protection Duration",
                "Level-2 Undervoltage Protection Threshold",
                "Level-2 Undervoltage Protection Duration",
                "Level-2 Overvoltage Protection Threshold",
                "Level-2 Overvoltage Protection Duration",

                "Level-1 Underfrequency Protection Threshold",
                "Level-1 Underfrequency Protection Duration",
                "Level-1 Overfrequency Protection Threshold",
                "Level-1 Overfrequency Protection Duration",
                "Level-2 Underfrequency Protection Threshold",
                "Level-2 Underfrequency Protection Duration",
                "Level-2 Overfrequency Protection Threshold",
                "Level-2 Overfrequency Protection Duration",

                // 第2组
                "Reactive Power/Voltage Feature",
                "QU Curve Switch",
                "QU Lock-In Power",
                "QU Lock-Out Power",
                "Time constant for Q(U)",
                "Minimum cos(phi)",
                "QU Point1 Voltage",
                "QU Curve Point1 Reactive Power Percent",
                "QU Point2 Voltage",
                "QU Curve Point2 Reactive Power Percent",
                "QU Point3 Voltage",
                "QU Curve Point3 Reactive Power Percent",
                "QU Point4 Voltage",
                "QU Curve Point4 Reactive Power Percent",

                "Fix Q Parameter",
                "Q/Smax", 
                
                // 第5组
                ////新加+++2
                "Fix P Parameter",
                "P fix",
                
                // 第6组 
                "Fixed cos phi Parameter",
                "Cos phi fix Value",  
                

                // 第3组
                "Power Factor/Power Feature",
                "PF Curve Switch",
                "PF Curve PointA Power",
                "PF Curve PointB Power",
                "PF Curve PointC Power",
                "PF Curve PointA PF",
                "PF Curve PointB PF",
                "PF Curve PointC PF",
                "PF Curve Lock-In Voltage",
                "PF Curve Lock-Out Voltage",
                "PF Curve Lock-Out Power",

                        
                // 第4组 
                "Grid Connected Restrictions",
                "Start Delay Time",
                "Grid Connected Recovery Time from Grid Faults",
                "Undervoltage Recovery Limit",
                "Overvoltage Recovery Limit",
                "Underfrequency Recovery Limit",
                "Overfrequency Recovery Limit", 
                ////新加+++
                "Active Power increase Gradient",   
         
                // 第5组 
                "Overvoltage Derating",
                "Overvoltage Derating Switch",
                "P(U) Time constant",
                "PU Curve Start Voltage",
                "Power of PU Curve Start Voltage",
                "PU Curve End Voltage",
                "Power of PU Curve End Voltage",
                         

                // 第6组            
                "Overfrequency Derating",
                "Overfrequency Derating Switch",
                "Start Threshold of Overfrequency Derating", 
                "Stop Threshold of Overfrequency Derating",     
                "FPFrequency Threshold of Recovery Power from FP Overfrequency Derating",
                "Allow Time of Recovery Power from FP Overfrequency Derating",
                "Slow Loading Switch of Recovery Power from FP Overfrequency Derating",
                "Slow Loading Rate of Recovery Power from FP Overfrequency Derating",
                "Droop for P(f)",
        
              
                // 第7组 
                "LVRT",
                "LVRT Switch",
                "Zero current lock-in voltage",      

                "LVRT Point 1",
                "LVRT Point 2",
                "LVRT Point 3",
                "LVRT Point 4",
                "LVRT Point 5",
                "LVRT Point 1 protect time",
                "LVRT Point 2 protect time",
                "LVRT Point 3 protect time",
                "LVRT Point 4 protect time",
                "LVRT Point 5 protect time",
  
          
                // 第8组 
                "HVRT",
                "HVRT Switch",
                "HVRT Lock-in Voltage",

                // 第12组 
                "Others",
                "10-min Overvoltage Protection Switch",
                "10-min Overvoltage Protection Threshold",

            };

        public static List<string> DeutschList = new List<string>
            {
                // 第1组
                "Netz und Anlagenschutz" ,
                "Primärer Unterspannungsschutz Schwellwert",
                "Primäre Dauer Unterspannungsschutz (1Periode=20msek)",
                "Primärer Überspannungsschutz Schwellwert",
                "Primäre Dauer Überspannungsschutz",
                "Sekundärer Unterspannungsschutz Schwellwert",
                "Sekundärer Dauer Unterspannungsschutz",
                "Sekundärer Überspannungsschutz Schwellwert",
                "Sekundärer Dauer Überspannungsschutz",
                "Primärer Unterfrequenz-Schwellwert",
                "Primäre Dauer Unterfrequenzschutz",
                "Primärer Überfrequenzschutz Schwellwert",
                "Primärer Dauer Überfrequenzschutz",
                "Sekundärer Unterfrequenzschutz Schwellwert",
                "Sekundäre  Dauer Unterfrequenzschutz",
                "Sekundärer  Überfrequenzschutz Schwellwert",
                "Sekundäre Dauer Überfrequenzschutz", 

                // 第2组
                "Blindleistungsbereitstellung - Q(U)",
                "Blindleistungs-/Spannungskennlinie Ein/Aus",
                /*"Q(U)Einschalt-Leistung[% Pn]",
                "Q(U)Ausschalt-Leistung[% Pn]",*/
                "Q(U)Spannung U1 (übererregt)",
                "Q(U1)/Smax[%](übererregt)",
                "Q(U)Spannung U2",
                "Q(U2)/Smax [%]",
                "Q(U)Spannung U3",
                "Q(U3)/Smax [%]",
                "Q(U)Spannung U4(untererregt)",
                "Q(U4)/Smax[%](untererregt)",
                "QU Zeitkonstante",
                "cos phi minimum Einstellung",   

                // 第3组
                "Verschiebungsfaktor-/Wirkleistungskennlinie cos Phi (P)",
                "cos Phi(P) - Kennlinien Ein/Aus",
                "StützpunktA [P/Pmax] (cos Phi 1)",
                "StützpunktB [P/Pmax] (cos Phi 1)",
                "StützpunktC [P/Pmax] (cos Phi 1)",
                "Stützpunkt A (cos Phi 1)",
                "Stützpunkt B (cos Phi 1)",
                "Stützpunkt C (cos Phi 1)",
                //"Startspannung cos Phi Kennlinie",
                //"Stopspannung cos Phi Kennlinie",
                "Stopleistung cos Phi Kennline",

                // 第4组
                "Feste Blindleistung Q fix",
                "Q fix",
                "Q/Smax",
                
                // 第5组
                ////新加+++2
                "P-Parameter-Einstellung",
                "P fix",
                
                // 第6组 
                "Fester Verschiebungsfaktor cos phi fix",
                "Cos phi Ein/Aus", 
                "Cos phi", 
         
                // 第7组 
                "Netzanschlussbestimmungen",
                "Startverzögerungszeit",
                "Startverzögerung bei häufigen Netzfehlern",
                "Min. Zuschaltspannung nach Netzwiederkehr",
                "Max. Zuschaltspannung nach Netzwiederkehr",
                "Min. Zuschaltfrequenz nach Netzfehler",
                "Max. Zuschaltfrequenz nach Netzfehler",
                "Langsamladefunktion EIN/AUS",   
                ////新加+++
                "Entkupplungsschutzes wird ein Gradient",      
         
                // 第8组 
                "Leistungsreduzierung bei Überspannung P(U)",
                "Leistungsreduktion bei Überpannung P(U) Ein/Aus",
                "Startpunkt der Kennlinie bei Spannung",
                "Leistung am Startpunkt der Kennlinie",
                "Endpunkt der Kennlinie bei Spannung",
                "Leistung am Endpunkt der Kennlinie",
                "P(U) Zeitkonstante",           

                // 第9组            
                "Wirkleistungsreduktion bei Überfrequenz (LFSM-O)",
                "Wirkleistungsreduktion bei Überfrequenz Ein/Aus",
                "Start-Frequenz Wirkleistungsreduktion", 
                ////新加+++ 1
                "Stop-Frequenz wirkleistungsreduktion",
                "Statik S2",
                "Frequenzbereich für die Rückkehr zum Normalbetrieb [Hz]",
                "Wartezeit für die Rückkehr zum Normalbetrieb [s]",
                "Schalter für langsame Belastung der Rückspeiseleistung von FP Überfrequenz-Derating",
                "Langsame Belastungsrate der Rückspeiseleistung von FP Überfrequenz-Derating",
               /* "Unterfrequenzbelastung Ein/Aus",
                "Unterfrequenzbelastung Startfrequenz",
                "Gradient",
                "Unterfrequenz-Laderate",     */          
              
                // 第10组 
                "Durchfahren von Unterspannung (LVRT)",
                "LVRT EIN / AUS",
                "Eingeschränkte dynamische Netzstützung",   
                ////新加+++ 10
                "Niedrige Durchschlagsspannung 1",
                "Schutzzeit mit niedrigem Durchschlagsspannungspunkt 1",
                "Niedrige Durchschlagsspannung 2",
                "Schutzzeit mit niedrigem Durchschlagsspannungspunkt 2",
                "Niedrige Durchschlagsspannung 3",
                "Schutzzeit mit niedrigem Durchschlagsspannungspunkt 3",
                "Niedrige Durchschlagsspannung 4",
                "Schutzzeit mit niedrigem Durchschlagsspannungspunkt 4",
                "Niedrige Durchschlagsspannung 5",
                "Schutzzeit mit niedrigem Durchschlagsspannungspunkt 5",
          
                // 第11组 
                "Durchfahren von Überspannung (HVRT)",
                "HVRT EIN / AUS",
                "HVRT Abschaltspannung",
         
                // 第12组 
                "Anderes",
                "Mindestzeitraum 10 Minuten bei Ueberspannung EIN / AUS",
                "10-min-Ueberspannungsschutz-Schwellwert",
                
            };
    }
}
