using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemTool.StaticSource;

namespace SystemTool.Protocol
{
    public class SerialDevice : SerialBase
    {
        public string _deviceSN = "";
        public string _version = "";
        public string _internalVersion = "";
        public ushort _safetyCode;
        public bool ReadFirmwareVersion()
        {
            string msg = string.Empty;
            if (!GetFunc(GetCommandType.获取软件版本号, ref msg))
            {
                Log.Error("读取逆变器软件版本失败!");
                return false;
            }
            else
            {
                _version = msg.Split(",")[0];
                _internalVersion = msg.Split(",")[1];
                return true;
            }
        }

        public bool ReadSN()
        {
            string result = "";
            //储能机是10000
            byte[] address = new byte[] { 0x27, 0x10 };

            if (!GetFunc(GetCommandType.获取序列号, ref result))
            {
                Log.Error("读取SN失败");
                return false;
            }
            _deviceSN = result;
            return true;
        }

        public bool ReadSafetyCode()
        {
            //储能机是10104
            byte[] address = new byte[] { 0x27, 0x78 };
            byte[] safety = new byte[] { };
            if (!ReadData(address, 1, ref safety))
            {
                Log.Error("读取安规失败");
                return false;
            }

            _safetyCode = (ushort)((safety[0] << 8) + safety[1]);
            return true;
        }


    }
}
