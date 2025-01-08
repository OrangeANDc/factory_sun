using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magicodes.ExporterAndImporter.Core;

namespace SunwaysFactoryProgram.DBModel
{
    public class ST_OutBound
    {
        [Display(Name = "出库日期")]
        public DateTime OutDate { get; set; }
        [Display(Name = "订单号")]
        public string OrderID { get; set; }
        [Display(Name = "客户名称")]
        public string Customer { get; set; }
        [Display(Name = "序列号")]
        public string InverterSN { get; set; }
        [Display(Name = "验证码")]
        public string CheckCode { get; set; }
        [Display(Name = "质保年限")]
        public string Warranty { get; set; }
        [Display(Name = "托盘编号")]
        public string PalletNum { get; set; }
        [Display(Name = "型号")]
        public string Model { get; set; }
        [Display(Name = "料号")]
        public string InvCode { get; set; }
        [Display(Name = "产品配置")]
        public string ConfigInfo { get; set; }
        [Display(Name = "备注")]
        public string Remark { get; set; }
        [Display(Name = "生成时间")]
        public DateTime CreationDate { get; set; }
        [ExporterHeaderAttribute(IsIgnore = true)]
        public string ActiveStatus { get; set; }
    }
}
