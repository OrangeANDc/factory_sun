using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunwaysFactoryProgram.DBModel
{
    public class ST_BurnInPosition
    {
        [Display(Name = "老化房")]
        public string BurnInRoom { get; set; }
        [Display(Name = "老化车")]
        public string BurnInCar { get; set; }
        [Display(Name = "序列号")]
        public string InverterSN { get; set; }
        [Display(Name = "绑定时间")]
        public DateTime CreateTime { get; set; }
        public string DataStatus { get; set; }
    }
}
