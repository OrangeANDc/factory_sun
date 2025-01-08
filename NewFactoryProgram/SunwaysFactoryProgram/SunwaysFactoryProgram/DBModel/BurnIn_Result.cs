using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magicodes.ExporterAndImporter.Core;

namespace SunwaysFactoryProgram.DBModel
{
    public class BurnIn_Result
    {
        public BurnIn_Result()
        {
            PvTime = -1;
            BatTime = -1;
            PvResult = "";
            BatResult = "";
            TestTime = DateTime.Now;
            DataStatus = "1";
        }

        public string InverterSN { get; set; }
        [Display(Name = "Pv时间")]
        public int PvTime { get; set; }
        [Display(Name = "Bat时间")]
        public int BatTime { get; set; }
        [Display(Name = "Pv结果")]
        public string PvResult { get; set; }
        [Display(Name = "Bat结果")]
        public string BatResult { get; set; }
        [Display(Name = "测试时间")]
        public DateTime TestTime { get; set; }
        [ExporterHeader (IsIgnore = true)]
        public string DataStatus { get; set; }
    }
}
