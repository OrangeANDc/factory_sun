using SunwaysFactoryProgram.DBModel;
using SunwaysFactoryProgram.Messenager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SunwaysFactoryProgram.StaticSource
{
    public static class Variable
    {
        public static string _burnRoom = "SUN-ROOM-001";
        public static DelegateService BurninService { get; set; } = new DelegateService();

        // 左侧treeview  依赖注入 header绑定
        public static Dictionary<string, string> ViewMap = new Dictionary<string, string>()
        {
            {"包装测试" ,"PackView" },
            {"低压_包装测试" ,"Low_PackView" },
            {"功能测试" ,"FunctionView" },
            {"入库扫码" ,"WareHouseView" },
            {"订单管理" ,"OrderManagerView" },
        };

        public static Dictionary<string, ushort> _safetyDic = new Dictionary<string, ushort>
            {
                { "Sunways Test"            ,1      },
                { "自定义一"                 ,4      },
                { "中国能标"                 ,5      },
                { "SPAIN PO12.3"             ,6      },
                { "Yemen(也门)/Korea(韩国)"  ,7     },
                { "Tunisia(突尼斯)"          ,8     },
                {  "50HzDefault"            ,10     },
                {  "60HzDefault"            ,11     },
                {  "VDE4105(德国"           ,12     },
                {  "AS4777.2(AU)"           ,13     },
                {  "AS4777.2(NZ)"           ,14     },
                {  "SPAIN NTS-A"            ,15     },
                {  "EN50549"                ,16     },
                {  "Vietnam(越南)"           ,17     },
                {  "IEC61727_50Hz"          ,18     },
                {  "IEC61727_60Hz"          ,19     },
                {  "Brazil(巴西)"                 ,20     },
                {  "India(印度)"                  ,21     },
                {  "Philippines(菲律宾)"            ,22     },
                {  "Sri Lanka(斯里兰卡)"              ,23     },
                {  "Italy(意大利)"                  ,24     },
                {  "EN50549(CZ)(捷克)"            ,25     },
                {  "EN50549(TR)(土耳其)"            ,26     },
                {  "EN50549(IE)(爱尔兰)"            ,27     },
                {  "EN50549(SE)(瑞典)"            ,28     },
                {  "EN50549(PL)"                 ,29     },
                {  "EN50549(HR)(克罗地亚)"            ,30     },
                {  "Belgium(比利时)"                ,31     },
                {  "FRA Mainland(法国大陆)"           ,32     },
                {  "France(50Hz)(法国岛屿50Hz)"           ,33     },
                {  "France(60Hz)(法国岛屿60Hz)"           ,34     },
                {  "VDE0126"                ,35     },
                {  "Italy(MV)"              ,36     },
                {  "SouthAfrica(南非)"            ,37     },
                {  "60Hz(LV)"               ,38     },
                {  "IEEE1547"               ,39     },
                {  "G98"                    ,40     },
                {  "G99"                    ,41     },
                {  "Austria"                ,42     },
                {  "50Hz(LV)"               ,43     },
                {  "MEA"                    ,44    },
                {  "PEA"                    ,45         },
                {  "AS4777.2(B)"            ,46         },
                {  "SPAIN NTS-B"            ,47      },
                {  "AS4777.2(C)"            ,48       },
                {  "ES:NTS631"              ,49    }
            };

        public static string Version = "软件版本:V1.2.3";

        public static bool isFirstStart = true;

        public static TBS_FuncTest _funcTestResult = new TBS_FuncTest();
        public static TBS_PackTest _packTestResult = new TBS_PackTest();

        public static List<string> _funcLogList = new List<string>();
        public static List<string> _packLogList = new List<string>();
    }

    public static class Methods
    {                                
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, System.Text.StringBuilder retVal, int size, string filePath);
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string sectionName, string key, string value, string filePath);

        /// <summary>
        /// 根据Key读取Value
        /// </summary>
        /// <param name="sectionName">section名称</param>
        /// <param name="key">key的名称</param>
        /// <param name="filePath">文件路径</param>
        public static string INIRead(string sectionName, string key, string filePath)
        {
            // 每次从ini中读取多少字节
            System.Text.StringBuilder temp = new System.Text.StringBuilder(255);

            // section=配置节点名称，key=键名，temp=上面，path=路径
            GetPrivateProfileString(sectionName, key, "", temp, 255, filePath);
            return temp.ToString();
        }

        /// <summary>
        /// 保存内容到ini文件
        /// <para>若存在相同的key，就覆盖，否则就增加</para>
        /// </summary>
        /// <param name="sectionName">section名称</param>
        /// <param name="key">key的名称</param>
        /// <param name="value">存储的值</param>
        /// <param name="filePath">文件路径</param>
        public static bool INIWrite(string sectionName, string key, string value, string filePath)
        {
            int rs = (int)WritePrivateProfileString(sectionName, key, value, filePath);
            return rs > 0;
        }

        public static ObservableCollection<T> ConvertList<T>(this List<T> source)
        {
            ObservableCollection<T> to = new ObservableCollection<T>(source);
            //或者source.ForEach(p => to.Add(p));
            return to;
        }

        public static EnCompany GetEnCompany(string sn)
        {
            if (sn == null || sn.Length < 16)
                return EnCompany.NULL;
            switch (sn.Substring(0, 1) + sn.Substring(13, 1))
            {
                case "00":
                    return EnCompany.TestSN;
                case "10":
                    return EnCompany.Sunways;
                case "20":
                    return EnCompany.Aarusha;
                case "30":
                    return EnCompany.GikaSun;
                case "40":
                    return EnCompany.Selfa;
                case "50":
                    return EnCompany.Wattsonic;
                case "60":
                    return EnCompany.Stromherz;
                case "70":
                    return EnCompany.Nelumbo_Energy;
                case "80":
                    return EnCompany.ReComm;
                case "81":
                    return EnCompany.Regitic;
                case "82":
                    return EnCompany.Decheng;
                case "83":
                    return EnCompany.KunLan;
                case "84":
                    return EnCompany.SheenPlus;
                case "85":
                    return EnCompany.ETEK;
                case "90":
                    return EnCompany.Salicru;
                case "Z0":
                    return EnCompany.AkkuSys;
                case "Z1":
                    return EnCompany.M_Tec;
                default:
                    return EnCompany.NULL;
            }
        }

        public static string CalcCheckCode(string strSN)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(strSN);
            for (int index = 0; index < bytes.Length; ++index)
                bytes[index] = (byte)((uint)bytes[index] - 6U);
            byte[] modbus = ToModbus(bytes, 16);
            ushort num = (ushort)(((uint)modbus[1] << 8) + (uint)modbus[0]);
            byte[] numArray = new byte[6]
            {
        (byte) ((uint) num / 10000U),
        (byte) ((int) num / 1000 % 10),
        (byte) ((int) num / 100 % 10),
        (byte) ((int) num / 10 % 10),
        (byte) ((uint) num % 10U),
        (byte) 0
            };
            numArray[5] = (byte)(((int)numArray[0] + (int)numArray[1] + (int)numArray[2] + (int)numArray[3] + (int)numArray[4]) % 10);
            return numArray[0].ToString() + numArray[1].ToString() + numArray[2].ToString() + numArray[3].ToString() + numArray[4].ToString() + numArray[5].ToString();
        }

        private static byte[] ToModbus(byte[] byteData, int byteLength)
        {
            byte[] modbus = new byte[2];
            ushort num = ushort.MaxValue;
            for (int index1 = 0; index1 < byteLength; ++index1)
            {
                num ^= Convert.ToUInt16(byteData[index1]);
                for (int index2 = 0; index2 < 8; ++index2)
                {
                    if (((int)num & 1) == 1)
                        num = (ushort)((uint)(ushort)((uint)num >> 1) ^ 40961U);
                    else
                        num >>= 1;
                }
            }
            modbus[1] = (byte)(((int)num & 65280) >> 8);
            modbus[0] = (byte)((uint)num & (uint)byte.MaxValue);
            return modbus;
        }
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

    
    public enum EnCompany
    {
        TestSN,
        Sunways,
        Aarusha,
        GikaSun,
        Selfa,
        Wattsonic,
        Stromherz,
        Nelumbo_Energy,
        ReComm,
        Regitic,
        Decheng,
        KunLan,
        SheenPlus,
        ETEK,
        Salicru,
        AkkuSys,
        M_Tec,
        NULL,
    }
}
