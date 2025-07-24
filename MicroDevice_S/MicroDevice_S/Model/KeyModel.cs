using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroDevice_S.Model
{
    public class KeySend
    {
        public int code { get; set; }
        public KeySendData data { get; set; }
    }

    public class KeySendData
    {
        public string key { get; set; }
    }

    public class KeyReturn
    {
        public int code { get; set; }
        public string data { get; set; }
    }
}
