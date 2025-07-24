using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroDevice_S.Model
{
    public class DataSendModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DataSend data { get; set; }
    }

    public class DataSend
    {
        /// <summary>
        /// 
        /// </summary>
        public string sn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<int> val { get; set; }
    }

    public class DataResponseModel
    {
        public int code { get; set; }

        public List<int> data { get; set; }
    }


}
