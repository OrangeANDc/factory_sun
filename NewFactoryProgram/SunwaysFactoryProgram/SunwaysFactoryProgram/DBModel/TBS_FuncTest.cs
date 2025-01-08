using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunwaysFactoryProgram.DBModel
{
    public class TBS_FuncTest
    {
        public string InverterSN { get; set; }

        public string Version { get; set; }

        public string InternalVersion { get; set; }

        public string VolCalibrate { get; set; }

        public string OnGridTest { get; set; }

        public string DredTest { get; set; }

        public string ExportLimitTest { get; set; }

        public int BurnInSafetyCode { get; set; }

        public string ErrorMsg { get; set; }

        public string TestResult { get; set; }

        public string OdmSN { get; set; }

        public string Tester { get; set; }

        public string StationId { get; set; }

        public DateTime TestTime { get; set; }

        public void InitData()
        {
            this.InverterSN = "";
            this.Version = "";
            this.InternalVersion = "";
            this.VolCalibrate = "";
            this.OnGridTest = "";
            this.DredTest = "";
            this.ExportLimitTest = "";
            this.BurnInSafetyCode = -1;
            this.ErrorMsg = "";
            this.TestResult = "";
            this.OdmSN = "";
            this.Tester = "sunways";
            this.StationId = "sunways";
            this.TestTime = DateTime.MinValue;
        }

    }
}
