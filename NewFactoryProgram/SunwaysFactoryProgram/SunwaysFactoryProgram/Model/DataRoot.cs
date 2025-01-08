using SunwaysFactoryProgram.Views.BurnInViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunwaysFactoryProgram.Model
{
   /* public class DataRoot
    {
        public string code { get; set; }

        public string msg { get; set; }

        public List<BurnInData> data { get; set; }

        public string time { get; set; }
    }*/

    public class DeviceData
    {
        public List<New_BurnInData> data { get; set; }
        public string sn { get;set; }
    }

    public class New_DataRoot
    {
        public string code { get; set; }

        public string msg { get; set; }

        public DeviceData data { get; set; }

        public string time { get; set; }
    }
}
