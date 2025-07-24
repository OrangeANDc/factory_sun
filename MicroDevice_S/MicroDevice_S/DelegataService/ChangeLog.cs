using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroDevice_S.DelegataService
{ 
    public delegate void ChangeLogDelegate(string log);
    public class ChangeLogService
    {
        public event ChangeLogDelegate OnChangeLog;

        public void ChangeLog(string log)
        {
            if (OnChangeLog != null) 
                OnChangeLog(log);
        }
    }

    public  class DelegateInfo
    {
       public static ChangeLogService Service { get; set; } = new ChangeLogService();
    }
}
