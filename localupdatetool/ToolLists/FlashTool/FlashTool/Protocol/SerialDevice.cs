using FlashTool.Protocol;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ConfigTool.Protocol
{
    class SerialDevice : ISerial
    {
        private object _dataLock = new object();
        private SerialPort _serialPort = new SerialPort();
        private byte[] _receiveData;
        
        public bool isOpen { get; set; }

        public SerialDevice()
        {
            _serialPort = new SerialPort();
            //_serialPort.DataReceived += new SerialDataReceivedEventHandler(ReceiveData);
        }

        public bool Open(string portName, string baudRate)
        {
            try
            {
                if (_serialPort.IsOpen)
                {
                    return true;
                }

                _serialPort.PortName = portName;
                _serialPort.BaudRate = Convert.ToInt32(baudRate);
                _serialPort.Parity = Parity.None;
                _serialPort.DataBits = 8;
                _serialPort.StopBits = StopBits.One;
                _serialPort.ReceivedBytesThreshold = 1;
                _serialPort.Open();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool GetStatus() { return _serialPort.IsOpen; }

        public bool Close()
        {
            try
            {
                if (!_serialPort.IsOpen)
                {
                    return true;
                }
                _serialPort.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool GetFunc(string type, ref string result)
        {
            try
            {
                if (!_serialPort.IsOpen)
                {
                    MessageBox.Show("串口未打开");
                    return false;
                }

                byte[] data = null;
                byte[] recvData = new byte[] { };
                switch (type)
                {
                    case "获取序列号":
                        data = new byte[] { 0xfe, 0x03, 0x27, 0x10, 0x00, 0x08 };
                        if (!SendAndReceiveFunc03(data, ref recvData))
                            return false;

                        result = Encoding.ASCII.GetString(Modbus.GetData(recvData));
                        break;
                   
                    case "获取软件版本号":
                        data = new byte[] { 0xfe, 0x03, 0x27, 0x1b, 0x00, 0x04 };
                        if (!SendAndReceiveFunc03(data, ref recvData))
                            return false;

                        var version = Modbus.GetData(recvData);
                        result = "V1." + Modbus.BytetoString2(version[0]) + "." + Modbus.BytetoString2(version[1]) + "." + Modbus.BytetoString2(version[2]) + "." + Modbus.BytetoString2(version[3]) +
                            "  (Internal:V" + Modbus.BytetoString2(version[4]) + "." + Modbus.BytetoString2(version[5]) + "." + Modbus.BytetoString2(version[6]) + "." + Modbus.BytetoString2(version[7]) + ")";
                        break;                  
                    default:
                        return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return false;
            }
        }

        private bool SendAndReceiveFunc03(byte[] sendData, ref byte[] recvData)
        {
            try
            {
                //CRC 校验拼接
                byte[] temp = new byte[sendData.Length + 2];
                Array.Copy(sendData, temp, sendData.Length);
                Array.Copy(Modbus.CaculateCheckSum(sendData), 0, temp, temp.Length - 2, 2);
                Log.Debug("send : " + Modbus.ByteArrayToSting(temp));
                
                 _serialPort.Write(temp, 0, temp.Count());

                Thread.Sleep(1000);
                recvData = new byte[_serialPort.BytesToRead];
                _serialPort.Read(recvData, 0, recvData.Length);

                //  03功能码时计算出期望数据长度
                int expectLen = ((sendData[4] << 8) + sendData[5]) * 2 + 5;
                if (recvData.Length != expectLen)
                {
                    Log.Debug("数据接收长度错误");
                    return false;
                }
                return true;

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return false;
            }
        }

        #region Datalog升级     
        public bool Flash_Datalog(byte[] data)
        {
            try
            {
                byte[] _commonHead = new byte[] { 0xAA, 0x55, 0x80, 0x7F, 0x02,0x29,0x00 };
                if (data.Length < 256)
                {
                    _commonHead[6] = (byte)(data.Length);
                }
                             
                _commonHead = _commonHead.Concat(data).ToArray();

                if (!SendAndReceiveFlash_Datalog(_commonHead))
                    return false;

                return true;
            }
            catch(Exception ex)
            {
                Log.Error(ex.ToString());
                return false;
            }
        }

        private bool SendAndReceiveFlash_Datalog(byte[] sendData)
        {
            try
            {
                //CRC 校验拼接
                byte[] temp = new byte[sendData.Length + 2];
                Array.Copy(sendData, temp, sendData.Length);
                Array.Copy(Modbus.CaculateCheckSum(sendData), 0, temp, temp.Length - 2, 2);
                Log.Debug("send : " + Modbus.ByteArrayToSting(temp));

                _receiveData = null;
                _serialPort.Write(temp, 0, temp.Count());
                int expectLen = 10;

                Thread.Sleep(4000);
                byte[] bytes = new byte[_serialPort.BytesToRead];
                _serialPort.Read(bytes, 0, bytes.Length);
                Log.Debug("recv : " + Modbus.ByteArrayToSting(bytes));
                if (bytes.Length != expectLen)
                    return false;

                if (!Modbus.Check(bytes))
                {
                    Log.Debug("CRC校验失败");
                    return false;
                }

                _receiveData = bytes;
                return true;

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return false;
            }
        }

        public bool FlashHead_Datalog(byte[] header)
        {
            try
            {
                byte[] _commonHead = new byte[] { 0xAA, 0x55, 0x80, 0x7F, 0x02 };
                ushort len = (ushort)((ushort)header.Length + 0x2800);
                var tail = BitConverter.GetBytes(len).Reverse().ToArray();
                _commonHead = _commonHead.Concat(tail).ToArray();
                _commonHead = _commonHead.Concat(header).ToArray();

                if (!SendAndReceiveFlashHead_Datalog(_commonHead))
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return false;
            }
        }

        private bool SendAndReceiveFlashHead_Datalog(byte[] sendData)
        {
            try
            {
                //CRC 校验拼接
                byte[] temp = new byte[sendData.Length + 2];
                Array.Copy(sendData, temp, sendData.Length);
                Array.Copy(Modbus.CaculateCheckSum(sendData), 0, temp, temp.Length - 2, 2);
                Log.Debug("send : " + Modbus.ByteArrayToSting(temp));
                _receiveData = null;
                _serialPort.Write(temp, 0, temp.Count());
                int expectLen = 10;

                Thread.Sleep(4000);
                byte[] bytes = new byte[_serialPort.BytesToRead];
                _serialPort.Read(bytes, 0, bytes.Length);
                Log.Debug("recv : " + Modbus.ByteArrayToSting(bytes));
                if (bytes.Length != expectLen)
                    return false;

                if (!Modbus.Check(bytes))
                {
                    Log.Debug("CRC校验失败");
                    return false;
                }

                _receiveData = bytes;
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return false;
            }
        }
        #endregion

        #region arm 刷写
        public bool Flash_Arm(byte[] data, byte[] count)
        {
            try
            {
                byte[] _commonHead = new byte[] { 0xfd, 0xfe, 0x42};
                ushort len = (ushort)data.Length;
                var tail = BitConverter.GetBytes(len).Reverse().ToArray();
                _commonHead = _commonHead.Concat(count).ToArray();
                _commonHead = _commonHead.Concat(tail).ToArray();
                _commonHead = _commonHead.Concat(data).ToArray();

                if (!SendAndReceiveFlash_Arm(_commonHead))
                    return false;

                if (!Enumerable.SequenceEqual(_receiveData.Take(5), _commonHead.Take(5)))
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return false;
            }
        }

        private bool SendAndReceiveFlash_Arm(byte[] sendData)
        {
            try
            {
                //CRC 校验拼接
                byte[] temp = new byte[sendData.Length + 2];
                Array.Copy(sendData, temp, sendData.Length);
                Array.Copy(Modbus.CaculateCheckSum(sendData), 0, temp, temp.Length - 2, 2);
                Log.Debug("send : " + Modbus.ByteArrayToSting(temp));
                _receiveData = null;
                _serialPort.Write(temp, 0, temp.Count());
                int expectLen = 8;

                //读取返回数据
                while (_serialPort.BytesToRead == 0)
                {
                    Thread.Sleep(10);
                }
                Thread.Sleep(50); //50毫秒内数据接收完毕，可根据实际情况调整
                byte[] bytes = new byte[_serialPort.BytesToRead];
                _serialPort.Read(bytes, 0, bytes.Length);
                Log.Debug("recv : " + Modbus.ByteArrayToSting(bytes));
                if (bytes.Length != expectLen)
                    return false;
                if (!Modbus.Check(bytes))
                {
                    Log.Debug("CRC校验失败");
                    return false;
                }

                _receiveData = bytes;
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return false;
            }
        }

        public bool FlashHead_Arm(byte[] header)
        {
            try
            {
                byte[] _commonHead = new byte[] { 0xfd, 0xfe, 0x41, 0x00, 0x00 };
                ushort len = (ushort)header.Length;
                var tail = BitConverter.GetBytes(len).Reverse().ToArray();
                _commonHead = _commonHead.Concat(tail).ToArray();
                _commonHead = _commonHead.Concat(header).ToArray();

                if (!SendAndReceiveFlashHead_Arm(_commonHead))
                    return false;

                if (!Enumerable.SequenceEqual(_receiveData.Take(6), _commonHead.Take(6)))
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return false;
            }
        }

        private bool SendAndReceiveFlashHead_Arm(byte[] sendData)
        {
            try
            {
                //CRC 校验拼接
                byte[] temp = new byte[sendData.Length + 2];
                Array.Copy(sendData, temp, sendData.Length);
                Array.Copy(Modbus.CaculateCheckSum(sendData), 0, temp, temp.Length - 2, 2);
                Log.Debug("send : " + Modbus.ByteArrayToSting(temp));
                _receiveData = null;
                _serialPort.Write(temp, 0, temp.Count());
                int expectLen = 8;

                Thread.Sleep(3000);
                byte[] bytes = new byte[_serialPort.BytesToRead];
                _serialPort.Read(bytes, 0, bytes.Length);
                Log.Debug("recv : " + Modbus.ByteArrayToSting(bytes));
                if (bytes.Length != expectLen)
                    return false;
                if (!Modbus.Check(bytes))
                {
                    Log.Debug("CRC校验失败");
                    return false;
                }

                _receiveData = bytes;
                return true;

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return false;
            }
        }

        #endregion

        ~SerialDevice()
        {
            _serialPort?.Close();
            _serialPort?.Dispose();
        }
    }
}
