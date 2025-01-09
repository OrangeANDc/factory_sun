using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafetyTestTool.Model
{
    public class ParaInfModel
    {
        public string DataName { get; set; } = "";
        public string DataValue { get; set; } = "";
        public string DataUnit { get; set; } = "";
        public string CommandInf { get; set; } = "";

        public double? Gain { get; set; }

        public ushort? DataAddress { get; set; } = null;

        public bool? IsSwitch { get; set; } = false;

        public bool IsUnCheck { get; set;} = false;

        public bool IsFix { get; set;} = false;

        public string FixValue { get; set; }

        public string OldValue { get; set; }
    }
}
