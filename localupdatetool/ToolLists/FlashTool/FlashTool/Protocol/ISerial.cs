using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigTool.Protocol
{
    interface ISerial
    {
        bool Open(string portName, string baudRate);
        bool Close();

        bool GetFunc(string type, ref string para);

        bool Flash_Datalog(byte[] data);
        bool FlashHead_Datalog(byte[] header);

        bool Flash_Arm(byte[] data, byte[] count);
        bool FlashHead_Arm(byte[] header);
        bool GetStatus();


    }
}
