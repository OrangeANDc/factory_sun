using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafetyTestTool.Protocol
{
    public static class Modbus
    {
        public static bool Check(byte[] recvData)
        {
            var crc = CaculateCheckSum(recvData.Take(recvData.Length - 2).ToArray());
            if (!crc.SequenceEqual(recvData.TakeLast(2)))
                return false;
            // crc校验

            return true;
        }

        public static byte[] CaculateCheckSum(byte[] byteData)
        {
            byte[] CRC = new byte[2];

            ushort wCrc = 0xFFFF;
            for (int i = 0; i < byteData.Length; i++)
            {
                wCrc ^= Convert.ToUInt16(byteData[i]);
                for (int j = 0; j < 8; j++)
                {
                    if ((wCrc & 0x0001) == 1)
                    {
                        wCrc >>= 1;
                        wCrc ^= 0xA001;//异或多项式
                    }
                    else
                    {
                        wCrc >>= 1;
                    }
                }
            }

            CRC[1] = (byte)((wCrc & 0xFF00) >> 8);//高位在后
            CRC[0] = (byte)(wCrc & 0x00FF);       //低位在前
            return CRC;

        }

        public static byte[] GetData(byte[] recvData)
        {

            byte dataLen = recvData[2];
            byte[] data = new byte[dataLen];

            Array.Copy(recvData, 3, data, 0, dataLen);
            return data;

        }

        public static string BytetoString2(byte c)
        {
            return Convert.ToString(c, 10).PadLeft(2, '0');
        }

        public static string ByteArrayToSting(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
                return "empty";
            string result = string.Empty;
            foreach (var b in bytes)
            {
                result += (b.ToString("x2") + " ");
            }
            return result;
        }
    }
}
