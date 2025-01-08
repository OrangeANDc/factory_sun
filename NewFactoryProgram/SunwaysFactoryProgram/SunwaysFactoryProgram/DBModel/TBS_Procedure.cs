using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunwaysFactoryProgram.DBModel
{
    public class TBS_Procedure
    {
        public string ProcedureId { get; set; }
        public string ProcedureName { get; set; }
        public string StationId { get; set; }
        public string StationName { get; set; }
        public DateTime CreationDate { get; set; }
        public string ActiveStatus { get; set; }
    }
}
