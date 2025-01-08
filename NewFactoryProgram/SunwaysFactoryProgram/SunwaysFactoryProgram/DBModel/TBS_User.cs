using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunwaysFactoryProgram.DBModel
{
    public class TBS_User
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public int Authority { get; set; }
    }
}
