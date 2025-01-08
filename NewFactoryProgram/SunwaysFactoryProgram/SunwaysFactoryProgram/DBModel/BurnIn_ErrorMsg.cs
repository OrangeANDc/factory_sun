using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SunwaysFactoryProgram.DBModel
{
    public class BurnIn_ErrorMsg
    {
        public string InverterSN { get; set; }
        [Display(Name = "错误信息")]
        public string ErrorMsg { get; set; }
        [Display(Name = "错误等级")]
        public int ErrorLevel { get; set; }
        [Display(Name = "创建时间")]
        public DateTime CreationDate { get; set; }
    }
}
