using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunwaysFactoryProgram.DBModel
{
    public class TBS_PackTest
    {
        [Display(Name = "测试时间")]
        public DateTime TestTime { get; set; }

        [Display(Name = "序列号")]
        public string InverterSN { get; set; }

        [Display(Name = "版本")]
        public string Version { get; set; }

        [Display(Name = "内部版本")]
        public string InternalVersion { get; set; }

        [Display(Name = "安规代码")]
        public int SafetyCode { get; set; }

        [Display(Name = "安规名称")]
        public string SafetyName { get; set; }

        [Display(Name = "语言")]
        public string Language { get; set; }

        [Display(Name = "E2限载值")]
        public int E2Limit { get; set; }

        [Display(Name = "清零状态")]
        public string ClearOK { get; set; }

        [Display(Name = "测试结果")]
        public string TestResult { get; set; }

        [Display(Name = "错误信息")]
        public string ErrorMsg { get; set; }
      
        public string Tester { get; set; }

        public string StationId { get; set; }

        public string OdmSN { get; set; }

        public void InitData()
        {
            this.InverterSN = "";
            this.Version = "";
            this.InternalVersion = "";
            this.SafetyCode = -1;
            this.SafetyName = "";
            this.Language = "";
            this.E2Limit = -1;
            this.ClearOK = "FAIL";
            this.TestResult = "FAIL";
            this.TestTime = DateTime.MinValue;
            this.OdmSN = "";
            this.Tester = "sunways";
            this.StationId = "sunways";
        }
    }  
}
