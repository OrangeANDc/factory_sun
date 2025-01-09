using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using SafetyTestTool.StaticSource;

namespace SafetyTestTool.Protocol
{
    public class SerialBase
    {
        private object _dataLock = new object();
        private SerialPort _serialPort;
        private byte[] _receiveData;
        public bool isOpen { get; set; }

        public SerialBase()
        {
            _serialPort = new SerialPort();
            _serialPort.DataReceived += new SerialDataReceivedEventHandler(ReceiveData);
        }

        public bool Open(string port)
        {
            try
            {
                if (_serialPort.IsOpen)
                {
                    return true;
                }

                _serialPort.PortName = port;
                _serialPort.BaudRate = 9600;
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
                Log.Error(ex.Message);
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
                Log.Error(ex.Message);
                return false;
            }
        }

        private void ReceiveData(object sender, SerialDataReceivedEventArgs s)
        {
            Thread.Sleep(400);
            byte[] bytes = new byte[_serialPort.BytesToRead];
            _serialPort.Read(bytes, 0, bytes.Length);
            lock (_dataLock)
            {
                _receiveData = bytes;
            }
        }


        public bool GetFunc(byte[] address, ushort length, ref byte[] result)
        {
            try
            {
                if (!_serialPort.IsOpen)
                {
                    MessageBox.Show("串口未打开");
                    return false;
                }

                byte[] data = { 0xfe, 0x03 };
                data = data.Concat(address).ToArray();
                data = data.Concat(BitConverter.GetBytes(length).Reverse().ToArray()).ToArray();

                byte[] recvData = new byte[] { };

                if (!SendAndReceiveFunc03(data, ref recvData))
                    return false;

                result = Modbus.GetData(recvData);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return false;
            }
        }

        public bool GetFunc(GetCommandType type, ref byte[] result)
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

                    case GetCommandType.获取RTC:
                        data = new byte[] { 0xfe, 0x03, 0x27, 0x74, 0x00, 0x03 };
                        if (!SendAndReceiveFunc03(data, ref recvData))
                            return false;

                        result = Modbus.GetData(recvData);

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

        public bool GetFunc(GetCommandType type, ref string result)
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
                    case GetCommandType.获取序列号:
                        data = new byte[] { 0xfe, 0x03, 0x27, 0x10, 0x00, 0x08 };
                        if (!SendAndReceiveFunc03(data, ref recvData))
                            return false;

                        result = Encoding.ASCII.GetString(Modbus.GetData(recvData));
                        break;
                    case GetCommandType.获取RTC:
                        data = new byte[] { 0xfe, 0x03, 0x27, 0x74, 0x00, 0x03 };
                        if (!SendAndReceiveFunc03(data, ref recvData))
                            return false;

                        var tem = Modbus.GetData(recvData);
                        result = "20" + Modbus.BytetoString2(tem[0]) + "-" + Convert.ToString(tem[1], 10) + "-" + Convert.ToString(tem[2], 10) + " " +
                                Modbus.BytetoString2(tem[3]) + ":" + Modbus.BytetoString2(tem[4]) + ":" + Modbus.BytetoString2(tem[5]);
                        break;
                    case GetCommandType.获取软件版本号:
                        data = new byte[] { 0xfe, 0x03, 0x27, 0x1b, 0x00, 0x04 };
                        if (!SendAndReceiveFunc03(data, ref recvData))
                            return false;

                        var version = Modbus.GetData(recvData);
                        result = string.Format("{0:00}.{1:00}.{2:00}.{3:00}", recvData[0], recvData[1], recvData[2], recvData[3]) +
                                 "," + string.Format("{0:00}.{1:00}.{2:00}.{3:00}", recvData[4], recvData[5], recvData[6], recvData[7]);


                        break;
                    case GetCommandType.获取DSP错误信息:
                        data = new byte[] { 0xfe, 0x03, 0x27, 0x80, 0x00, 0x08 };
                        if (!SendAndReceiveFunc03(data, ref recvData))
                            return false;

                        var DspEMsg = Modbus.GetData(recvData);

                        result = $"F1:0x{((DspEMsg[0] << 8) + DspEMsg[1]).ToString("x2")}," +
                            $"F2:0x{((DspEMsg[2] << 8) + DspEMsg[3]).ToString("x2")}," +
                            $"W1:0x{((DspEMsg[4] << 8) + DspEMsg[5]).ToString("x2")}," +
                            $"W2:0x{((DspEMsg[6] << 8) + DspEMsg[7]).ToString("x2")}";
                        break;
                    case GetCommandType.获取ARM错误信息:
                        data = new byte[] { 0xfe, 0x03, 0x27, 0x88, 0x00, 0x04 };
                        if (!SendAndReceiveFunc03(data, ref recvData))
                            return false;

                        var ArmEMsg = Modbus.GetData(recvData);
                        result = $"F3:0x{((ArmEMsg[0] << 8) + ArmEMsg[1]).ToString("x2")}," +
                            $"W3:0x{((ArmEMsg[2] << 8) + ArmEMsg[3]).ToString("x2")}";
                        break;
                    case GetCommandType.获取DSP运行状态:
                        data = new byte[] { 0xfe, 0x03, 0x27, 0x7A, 0x00, 0x06 };
                        if (!SendAndReceiveFunc03(data, ref recvData))
                            return false;

                        var DspRMesg = Modbus.GetData(recvData);
                        result = $"s1:0x{((DspRMesg[0] << 8) + DspRMesg[1]).ToString("x2")}," +
                            $"s2:0x{((DspRMesg[2] << 8) + DspRMesg[3]).ToString("x2")}," +
                            $"s3:0x{((DspRMesg[4] << 8) + DspRMesg[5]).ToString("x2")}";
                        break;

                    case GetCommandType.获取DRED状态:
                        data = new byte[] { 0xfe, 0x03, 0x2A, 0xF8, 0x00, 0x02 };
                        if (!SendAndReceiveFunc03(data, ref recvData))
                            return false;

                        var Dred = Modbus.GetData(recvData);
                        result = ((Dred[0] << 24) + (Dred[1] << 16) + (Dred[2] << 8) + Dred[3]).ToString() + "W";
                        break;
                    case GetCommandType.获取CheckCode:
                        data = new byte[] { 0xfe, 0x03, 0x27, 0x3e, 0x00, 0x03 };
                        if (!SendAndReceiveFunc03(data, ref recvData))
                            return false;

                        result = Encoding.ASCII.GetString(Modbus.GetData(recvData));
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

        public bool WriteSingleData(byte[] address, byte[] data)
        {
            try
            {
                if (!_serialPort.IsOpen)
                {
                    MessageBox.Show("串口未打开");
                    return false;
                }

                byte[] sendData;
                sendData = new byte[] { 0xfe, 0x06 };
                sendData = sendData.Concat(address).ToArray();
                sendData = sendData.Concat(data).ToArray();
                if (!SendAndReceiveFunc06(sendData))
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return false;
            }
        }

        public bool WriteMulData(byte[] address, byte[] data)
        {
            try
            {
                if (!_serialPort.IsOpen)
                {
                    MessageBox.Show("串口未打开");
                    return false;
                }

                byte[] sendData;
                byte len = (byte)data.Length;
                ushort count = (ushort)(len / 2);

                sendData = new byte[] { 0xfe, 0x10 };
                sendData = sendData.Concat(address).ToArray();
                sendData = sendData.Concat(BitConverter.GetBytes(count).Reverse().ToArray()).ToArray();
                sendData = sendData.Concat(new byte[] { len }).ToArray();

                sendData = sendData.Concat(data).ToArray();
                if (!SendAndReceiveFunc10(sendData))
                    return false;
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

                _receiveData = null;
                _serialPort.Write(temp, 0, temp.Count());
                //  03功能码时计算出期望数据长度
                int expectLen = ((sendData[4] << 8) + sendData[5]) * 2 + 5;

                int tickCountLast = Environment.TickCount;
                while (_receiveData == null || _receiveData.Length != expectLen)
                {
                    Thread.Sleep(100);
                    if (Environment.TickCount - tickCountLast > 2000)
                    {
                        Log.Debug("recv : " + Modbus.ByteArrayToSting(_receiveData));
                        Log.Debug("数据接收长度错误/超时");
                        return false;

                    }
                }

                //_receiveData = new byte[] { 0xfe, 0x03, 0xe6, 0x07, 0x3b, 0x0a, 0x32, 0x12, 0x90, 0x14, 0x1c, 0x07, 0x30, 0x00, 0x4b, 0x09, 0xf9, 0x00, 0x05, 0x02, 0x3f, 0x00, 0x19, 0x0a, 0x55, 0x00, 0x05, 0x12, 0x8e, 0x00, 0x05, 0x14, 0x1e, 0x00, 0x05, 0x12, 0x8e, 0x00, 0x05, 0x14, 0x1e, 0x00, 0x05, 0x00, 0x01, 0x00, 0x01, 0x00, 0xc8, 0x00, 0x32, 0x00, 0x01, 0x08, 0x44, 0x01, 0xb4, 0x08, 0xa0, 0x00, 0x00, 0x09, 0x6f, 0x00, 0x00, 0x09, 0xb4, 0xfe, 0x4c, 0x00, 0x00, 0x00, 0x02, 0x00, 0xc8, 0x01, 0xf4, 0x03, 0xe8, 0x03, 0xe8, 0x03, 0xe8, 0x03, 0x84, 0x09, 0x6f, 0x08, 0xfc, 0x00, 0xc8, 0x00, 0x01, 0x00, 0x3c, 0x01, 0x2c, 0x07, 0xa3, 0x09, 0xcb, 0x12, 0x8e, 0x13, 0x92, 0x07, 0xa3, 0x09, 0xcb, 0x12, 0x8e, 0x13, 0x92, 0x00, 0x00, 0x00, 0x00, 0x09, 0xf9, 0x03, 0xe8, 0x0a, 0x10, 0x00, 0x00, 0x00, 0x01, 0x00, 0x03, 0x13, 0x9c, 0x03, 0xe8, 0x14, 0x1e, 0x02, 0x58, 0x13, 0x92, 0x00, 0x01, 0x00, 0x01, 0x00, 0x50, 0x00, 0x00, 0x03, 0xe8, 0x12, 0x4b, 0x12, 0x4b, 0x03, 0x0c, 0x06, 0x19, 0x0a, 0x29, 0x0f, 0x3e, 0x12, 0x4b, 0x01, 0xf4, 0x07, 0x6c, 0x0e, 0x10, 0x18, 0x38, 0x18, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x16, 0x5b, 0x16, 0x5b, 0x17, 0x2c, 0x18, 0x30, 0x19, 0x34, 0xdc, 0x90, 0x27, 0x38, 0x00, 0xf0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x42, 0x00, 0x00, 0x09, 0xe2, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xc3, 0x3a };

                Log.Debug("recv : " + Modbus.ByteArrayToSting(_receiveData));
                if (!Modbus.Check(_receiveData))
                {
                    Log.Debug("CRC校验失败");
                    return false;
                }

                recvData = _receiveData;
                return true;

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return false;
            }
        }

        private bool SendAndReceiveFunc06(byte[] sendData)
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
                //  06功能码时计算出期望数据长度
                int expectLen = temp.Length;

                int tickCountLast = Environment.TickCount;
                while (_receiveData == null || _receiveData.Length != expectLen)
                {
                    Thread.Sleep(100);
                    if (Environment.TickCount - tickCountLast > 2000)
                    {
                        Log.Debug("recv : " + Modbus.ByteArrayToSting(_receiveData));
                        Log.Debug("数据接收长度错误/超时");
                        return false;

                    }
                }

                Log.Debug("recv : " + Modbus.ByteArrayToSting(_receiveData));
                if (!Enumerable.SequenceEqual(_receiveData, temp))
                {
                    Log.Debug("数据校验错误");
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
    
        private bool SendAndReceiveFunc10(byte[] sendData)
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
                //  10功能码时计算出期望数据长度 
                int expectLen = 8;

                int tickCountLast = Environment.TickCount;
                while (_receiveData == null || _receiveData.Length != expectLen)
                {
                    Thread.Sleep(100);
                    if (Environment.TickCount - tickCountLast > 2000)
                    {
                        Log.Debug("recv : " + Modbus.ByteArrayToSting(_receiveData));
                        Log.Debug("数据接收长度错误/超时");
                        return false;

                    }
                }

                Log.Debug("recv : " + Modbus.ByteArrayToSting(_receiveData));
                if (!Modbus.Check(_receiveData))
                {
                    Log.Debug("CRC校验失败");
                    return false;
                }

                if (!Enumerable.SequenceEqual(_receiveData.Take(6).ToArray(), sendData.Take(6).ToArray()))
                {
                    Log.Debug("数据校验错误");
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

        ~SerialBase()
        {
            _serialPort?.Close();
            _serialPort?.Dispose();
        }
    }
}
