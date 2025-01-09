using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemTool.StaticSource
{
    // 静态变量类
    public static class Variable
    {
        public static readonly Dictionary<string, string> _viewMaps = new Dictionary<string, string>()
        {
                { "软件升级", "FlashView" },
                { "bin文件加密", "HeadCombineView" },
               
                { "基础信息", "BaseControlView" },
                { "基础设置", "AdvanceSettingView" },
                { "数据调试", "ParaControlView" },
                { "数据监控", "DataMonitorView" }
            };


        public static Dictionary<ushort, string> _safetyDic = new Dictionary<ushort, string>()
        {
            {1,      "Sunways Test"               },
            {4,      "自定义一"                    },
            {5,      "中国能标"                    },
            {6,      "自定义二"                    },
            {7,      "Yemen(也门)/Korea(韩国)"     },
            {8,      "Tunisia(突尼斯)"             },
            {10,     "50HzDefault"               },
            {11,     "60HzDefault"               },
            {12,      "VDE4105(德国)"              },
            {13,      "AS4777.2(AU)"              },
            {14,      "AS4777.2(NZ)"              },
            {15,      "ES:TED749(西班牙)"          },
            {16,      "EN50549"                   },
            {17,      "Vietnam(越南)"             },
            {18,      "IEC61727_50Hz"             },
            {19,      "IEC61727_60Hz"             },
            {20,      "Brazil(巴西)"              },
            {21,      "India(印度)"               },
            {22,      "Philippines(菲律宾)"        },
            {23,      "Sri Lanka(斯里兰卡)"        },
            {24,      "Italy(意大利)"              },
            {25,      "EN50549(CZ)(捷克)"          },
            {26,      "EN50549(TR)(土耳其)"        },
            {27,      "EN50549(IE)(爱尔兰)"        },
            {28,      "EN50549(SE)(瑞典)"          },
            {29,      "Poland(波兰)"               },
            {30,      "EN50549(HR)(克罗地亚)"      },
            {31,      "Belgium(比利时)"            },
            {32,      "FRA Mainland(法国大陆)"     },
            {33,      "France(50Hz)(法国岛屿50Hz)" },
            {34,      "France(60Hz)(法国岛屿60Hz)" },
            {35,      "VDE0126"                   },
            {36,      "Italy(MV)"                 },
            {37,      "SouthAfrica(南非)"         },
            {38,      "60Hz(LV)"                  },
            {39,      "IEEE1547"                  },
            {40,      "G98"                       },
            {41,      "G99"                       },
            {42,      "Austria"                   },
            {43,      "50Hz(LV)"                  },
            {44,      "MEA"                       },
            {45,      "PEA"                       },
            {46,      "AS4777.2(B)"               },
            {47,      "ES:UNE217002"              },
            {48,      "AS4777.2(C)"               },
            {49,      "ES:NTS631"                 }
        };

        public static Dictionary<ushort, string> _lanDic = new Dictionary<ushort, string>()
        {
            { 0,   "中文"    },
            { 1,   "英语"      },
            { 2,   "西班牙语"    },
            { 3,   "葡萄牙语"  },
            { 4,   "波兰语"    },
            { 5,   "意大利语"  },
            { 6,   "德语"    },
            { 7,   "乌克兰语"  },
        };


        public static Dictionary<string, Dictionary<string, string>> _machineType = new Dictionary<string, Dictionary<string, string>>()
        {
            {
                "01", new Dictionary<string, string>()
                {
                    {"00", "STS-3KTL" },
                    {"01", "STS-3.6KTL" },
                    {"02", "STS-4.2KTL" },
                    {"03", "STS-4.6KTL" },
                    {"04", "STS-5KTL" },
                    {"05", "STS-6KTL" },
                }
            },

            {
                "02", new Dictionary<string, string>()
                {
                    {"00", "STS-700TL-S" },
                    {"01", "STS-1KTL-S" },
                    {"02", "STS-1.5KTL-S" },
                    {"03", "STS-2KTL-S" },
                    {"04", "STS-2.5KTL-S" },
                    {"05", "STS-3KTL-S" },
                    {"06", "STS-3.3KTL-S" },
                    {"07", "STS-1KTL-S-P" },
                    {"08", "STS-1.5KTL-S-P" },
                    {"09", "STS-2KTL-S-P" },
                    {"10", "STS-2.5KTL-S-P" },
                    {"11", "STS-3KTL-S-P" },
                    {"12", "STS-3.3KTL-S-P" },
                    {"13", "STS-3.3KTL-S-SE" },
                }
            },

             {
                "03", new Dictionary<string, string>()
                {
                    {"00", "STS-7KTL" },
                    {"01", "STS-8KTL" },
                    {"02", "STS-8KTL" },
                    {"03", "STS-10KTL" },
                    {"04", "STS-11KTL" },          
                }
            },

            {
                "04", new Dictionary<string, string>()
                {
                    {"00", "STS-3KTL-SE-S" },
                    {"01", "STS-3.3KTL-SE-S" },
                    {"02", "STS-3.6KTL-SE-S" },
                    {"03", "STS-4KTL-SE-S" },
                    {"04", "STS-4.6KTL-SE-S" },
                    {"05", "STS-5KTL-SE-S" },
                    {"06", "STS-6KTL-SE-S" },
                    {"07", "STS-4.2KTL-SE-S" },
                }    
            },

            {
                "05", new Dictionary<string, string>()
                {
                    {"00", "STS-3KTL-SE" },
                    {"01", "STS-3.6KTL-SE" },
                    {"02", "STS-4KTL-SE" },
                    {"03", "STS-4.6KTL-SE" },
                    {"04", "STS-5KTL-SE" },
                    {"05", "STS-6KTL-SE" },
                }  
                
            }, 



            {
                "11", new Dictionary<string, string>()
                {
                    {"00", "STT-3KTL-M" },
                    {"01", "STT-4KTL-M" },
                    {"02", "STT-5KTL-M" },
                    {"03", "STT-6KTL-M" },

                }
            },

            {
                "12", new Dictionary<string, string>()
                {
                    {"00", "STT-6KTL" },
                    {"01", "STT-8KTL" },
                    {"02", "STT-10KTL" },
                    {"03", "STT-12KTL" },
                    {"04", "STT-15KTL" },
                    {"05", "STT-17KTL" },
                    {"06", "STT-20KTL" },
                    {"07", "STT-25KTL" },
                    {"08", "STT-4KTL" },
                    {"09", "STT-5KTL" },
                    {"10", "STT-30KTL" },
                }    
            },

            {
                "13", new Dictionary<string, string>()
                {
                    {"00", "STT-15KTL-SE" },
                    {"01", "STT-20KTL-SE" },
                    {"02", "STT-25KTL-SE" },
                    {"03", "STT-30KTL-SE" },
                }
            },

            {
                "16", new Dictionary<string, string>()
                {
                    {"00", "STT-29.9KTL" },
                    {"01", "STT-30KTL" },
                    {"02", "STT-33KTL" },
                    {"03", "STT-36KTL" },
                    {"04", "STT-40KTL" },
                    {"05", "STT-45KTL" },
                    {"06", "STT-50KTL-M" },
                    {"07", "STT-60KTL-M" },
                    {"08", "STT-40KTL-HV" },
                    {"09", "STT-50KTL-HV" },
                    {"10", "STT-60KTL-HV" },
                    {"11", "STT-15KTL-LV" },
                    {"12", "STT-20KTL-LV" },
                    {"13", "STT-25KTL-LV" },
                }
            },

             {
                "17", new Dictionary<string, string>()
                {
                    {"00", "STT-29.9KTL-P" },
                    {"01", "STT-30KTL-P" },
                    {"02", "STT-33KTL-P" },
                    {"03", "STT-36KTL-P" },
                    {"04", "STT-40KTL-P" },
                    {"05", "STT-45KTL-P" },
                    {"06", "STT-50KTL-M-P" },
                    {"07", "STT-60KTL-M-P" },
                }
            },

             {
                "20", new Dictionary<string, string>()
                {
                    {"00", "STT-50KTL" },
                    {"01", "STT-60KTL" },
                    {"02", "STT-70KTL-HV" },
                    {"03", "STT-75KTL-HV" },
                    {"04", "STT-33KTL-HV" },
                }
             },

             {
                "21", new Dictionary<string, string>()
                {
                    {"00", "STT-80KTL" },
                    {"01", "STT-90KTL" },
                    {"02", "STT-100KTL" },
                    {"03", "STT-110KTL" },
                    {"04", "STT-80KTL-HV" },
                    {"05", "STT-90KTL-HV" },
                    {"06", "STT-100KTL-HV" },
                    {"07", "STT-110KTL-HV" },
                    {"08", "STT-125KTL-HV" },
                    {"09", "STT-125KTL-BHV" },
                    {"10", "STT-136KTL-BHV" },
                    {"11", "STT-80KTL-P" },
                    {"12", "STT-100KTL-P" },
                    {"13", "STT-110KTL-P" },
                    {"14", "STT-125KTL-P" },
                }
             },

             {
                "22", new Dictionary<string, string>()
                {
                    {"00", "STT-100KTL-SE" },
                    {"01", "STT-110KTL-SE" },
                    {"02", "STT-125KTL-SE" },
                    {"03", "STT-136KTL-SE" },
                    {"04", "STT-100KTL-SE-HV" },
                    {"05", "STT-110KTL-SE-HV" },
                    {"06", "STT-125KTL-SE-HV" },
                    {"07", "STT-136KTL-SE-HV" },

                }
             },

             {
                "30", new Dictionary<string, string>()
                {
                    {"00", "STH-4KTL-HT" },
                    {"01", "STH-5KTL-HT" },
                    {"02", "STH-6KTL-HT" },
                    {"03", "STH-8KTL-HT" },
                    {"04", "STH-10KTL-HT" },
                    {"05", "STH-12KTL-HT" },
                    {"06", "STH-4KTL-HT-P" },
                    {"07", "STH-5KTL-HT-P" },
                    {"08", "STH-6KTL-HT-P" },
                    {"09", "STH-8KTL-HT-P" },
                    {"10", "STH-10KTL-HT-P" },
                    {"11", "STH-12KTL-HT-P" },
                }
             },

             {
                "31", new Dictionary<string, string>()
                {
                    {"00", "STH-3KTL-HS" },
                    {"01", "STH-3.6KTL-HS" },
                    {"02", "STH-4.2KTL-HS" },
                    {"03", "STH-4.6KTL-HS" },
                    {"04", "STH-5KTL-HS" },
                    {"05", "STH-6KTL-HS" },
                    {"06", "STH-7KTL-HS" },
                    {"07", "STH-8KTL-HS" },
                    {"08", "STH-3KTL-HSS" },
                    {"09", "STH-3.6KTL-HSS" },
                }
             },

             {
                "32", new Dictionary<string, string>()
                {
                    {"00", "STH-15KTL-HT" },
                    {"01", "STH-17KTL-HT" },
                    {"02", "STH-20KTL-HT" },
                    {"03", "STH-25KTL-HT" },
                    {"04", "STH-29.9KTL-HT" },
                    {"05", "STH-30KTL-HT" },
                    {"06", "STH-33KTL-HT" },
                }
             },

             {
                "40", new Dictionary<string, string>()
                {
                    {"00", "STH-3KTL-LSS" },
                    {"01", "STH-3.6KTL-LS" },
                    {"02", "STH-4.2KTL-LS" },
                    {"03", "STH-4.6KTL-LS" },
                    {"04", "STH-5KTL-LS" },
                    {"05", "STH-6KTL-LS" },
                    {"06", "STH-7KTL-LS" },
                    {"07", "STH-8KTL-LS" },
                    {"08", "STH-10KTL-LS" },

                }
             },

             {
                "41", new Dictionary<string, string>()
                {
                    {"00", "STH-5KTL-LT" },
                    {"01", "STH-6KTL-LT" },
                    {"02", "STH-8KTL-LT" },
                    {"03", "STH-10KTL-LT" },
                    {"04", "STH-12KTL-LT" },
                    {"05", "STH-15KTL-LT" },
                }
             },

             {
                "50", new Dictionary<string, string>()
                {
                    {"00", "STM-600W" },
                    {"01", "STM-800W" },
                }
             },

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
