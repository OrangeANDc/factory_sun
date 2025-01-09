using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafetyTestTool.StaticSource
{
    public static class Variable
    {
        public static int InputCount = 0;
        public static string ExportHeader1 = "安规参数表";
        public static string ExportHeader2 = "系统信息";

        public static Dictionary<ushort, string> DicSafetyCounty = new Dictionary<ushort, string>
        {
             {1, "SunwaysTest"},
            {2, "Rsved0"},
            {3, "Rsved1"},
            {4, "CustomizedL1"},
            {5, "NB/T 32004"},
            {6, "CustomizedL2"},
            {7, "Lithuania"},

            {8, "Pakistan"},
            {9, "K ELECTRIC"},
            {10, "50HzDefault"},
            {11, "60HzDefault"},
            {12, "VDE4105"},
            {13, "AS4777.2(AU)"},
            {14, "AS4777.2(NZ)"},
            {15, "ES:RD1699"},
            {16, "EN50549"},
            {17, "Vietnam"},
            {18, "IEC_50Hz"},
            {19, "IEC_60Hz"},
            {20, "Brazil"},
            {21, "India"},
            {22, "Philippines"},
            {23, "Sri Lanka"},
            {24, "Italy"},
            {25, "EN50549(CZ)"},
            {26, "EN50549(TR)"},
            {27, "EN50549(IE)"},
            {28, "EN50549(SE)"},
            {29, "EN50549(PL)"},
            {30, "EN50549(HR)"},
            {31, "Belgium"},
            {32, "FRA Mainland"},
            {33, "France(50Hz)"},
            {34, "France(60Hz)"},
            {35, "VDE0126"},
            {36, "Italy(MV)"},
            {37, "SouthAfrica"},
            {38, "60Hz(LV)"},
            {39, "IEEE1547"},
            {40, "G98"},
            {41, "G99"},
            {42, "Austria"},
            {43, "50Hz(LV)"},
            {44, "MEA"},
            {45, "PEA"},
            {46, "AS4777.2(B)"},
            {47, "ES:UNE217002"},
            {48, "AS4777.2(C)"},
            {49, "ES:TED749"},
        };

    }
    public enum GetCommandType
    {
        获取序列号,
        获取RTC,
        获取软件版本号,
        获取DSP错误信息,
        获取ARM错误信息,
        获取DSP运行状态,
        获取DRED状态,
        获取CheckCode,
    }
}
