using LanguageConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafetyTestTool.StaticSource
{
    public delegate void InfoDelegate();
    public class InfoConfig
    {
        public event InfoDelegate OnChangeInfo;

        public void ChangeInfo()
        {
            if (OnChangeInfo != null)
            {
                OnChangeInfo();
            }
        }
    }

    public class InfoService
    {
        public static InfoConfig Service { get; set; } = new InfoConfig();
    }
}
