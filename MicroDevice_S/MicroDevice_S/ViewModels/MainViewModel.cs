using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InTheHand.Bluetooth;
using MicroDevice_S.DelegataService;
using MicroDevice_S.Model;
using MicroDevice_S.StaticMethod;
using Newtonsoft.Json;

namespace MicroDevice_S.ViewModels
{
    internal class MainViewModel : ObservableObject
    {
        private GattCharacteristic? _characteristic = null;
        private byte[]? _revMsg = null;
        private object _dataLocker = new object();
        private BluetoothDevice? _conDevice = null;

        public MainViewModel()
        {
            IsFuncEnable = false;
            ClearInfoCommand = new RelayCommand(ClearInfo);
            OpenCommand = new RelayCommand<string>(OpenBLEDevice);
            CloseCommand = new RelayCommand(CloseBLEDevice);
            ReadSNCommand = new RelayCommand(ReadSN);
            WriteSNCommand = new RelayCommand<string>(WriteSN);
            SetDomainCommand = new RelayCommand(SetDomain);
            SetOldCommand = new RelayCommand(SetOld);
            ReadOldCommand = new RelayCommand(ReadOld);
            ReadSoftVersionCommand = new RelayCommand(ReadSoftVersion);
            WriteSafetyCommand = new RelayCommand<string>(WriteSafety);
            ReadSafetyCommand = new RelayCommand(ReadSafety);

            DelegateInfo.Service = new ChangeLogService();
            DelegateInfo.Service.OnChangeLog += SetRecordLog;
        }

        private void SetRecordLog(string log)
        {
            if (RecordLog == null)
                RecordLog = "";
            if (RecordLog.Length > 2000)
                RecordLog.Remove(0, 1000);
            RecordLog += (log + Environment.NewLine);
        }

        private bool _isFuncEnable;
        public bool IsFuncEnable
        {
            get { return _isFuncEnable; }
            set => SetProperty(ref _isFuncEnable, value);
        }

        private string? _connectText;
        public string? ConnectText
        {
            get { return _connectText; }
            set => SetProperty(ref _connectText, value);
        }

        private string? _recordLog;
        public string? RecordLog
        {
            get { return _recordLog; }
            set => SetProperty(ref _recordLog, value);
        }

        private void ClearInfo()
        {
            RecordLog = "";
        }

        public IRelayCommand ClearInfoCommand { get; }
        public IRelayCommand<string> OpenCommand { get; }
        public IRelayCommand CloseCommand { get; }
        public IRelayCommand ReadSNCommand { get; }
        public IRelayCommand ReadSoftVersionCommand { get; }
        public IRelayCommand<string> WriteSNCommand { get; }
        public IRelayCommand SetDomainCommand { get; }
        public IRelayCommand SetOldCommand { get; }
        public IRelayCommand ReadOldCommand { get; }
        public IRelayCommand<string> WriteSafetyCommand { get; }
        public IRelayCommand ReadSafetyCommand { get; }



        private void OpenBLEDevice(string? Name)
        {
            string? MicroSn = Name?.Trim();
            if (MicroSn?.Length != 16)
            {
                DelegateInfo.Service.ChangeLog("SN长度错误！");
                return;
            }
            //DelegateInfo.Service.ChangeLog("蓝牙连接中，请等待.........");
            Task task = Task.Run(async () => {
                try
                {

                    var devices = await Bluetooth.ScanForDevicesAsync();


                    if (devices != null)
                    {
                        foreach (var device in devices)
                        {
                            if (device.Name.Equals(MicroSn))
                            {
                                var gatt = device.Gatt;
                                if (gatt != null)
                                {
                                    if (!device.Gatt.IsConnected)
                                    {
                                        await device.Gatt.ConnectAsync();
                                    }
                                    _conDevice = device;
                                    // 0x00ff 为可读可写可监听服务，只需获取该服务即可
                                    var service = await gatt.GetPrimaryServiceAsync((BluetoothUuid)0x00ff);

                                    if (service != null)
                                    {
                                        // 该服务只有一个特征值
                                        var characteristics = await service.GetCharacteristicsAsync();
                                        if (characteristics != null)
                                        {
                                            _characteristic = characteristics.FirstOrDefault();
                                            if (_characteristic != null)
                                            {
                                                //异步接收信息
                                                _characteristic.CharacteristicValueChanged += Characteristic_CharacteristicValueChanged;
                                                await _characteristic.StartNotificationsAsync();

                                                //设备密钥匹配信息，见微逆协议表
                                                KeySend keySend = new KeySend()
                                                {
                                                    code = 1,
                                                    data = new KeySendData { key = "Sws" + MicroSn.Substring(8, 8) + "@2024" }
                                                };

                                                byte[] keyBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(keySend));
                                                Log.Debug("Send: " + JsonConvert.SerializeObject(keySend));
                                                await _characteristic.WriteValueWithResponseAsync(keyBytes);

                                                Thread.Sleep(3000);
                                                if (_revMsg != null)
                                                {
                                                    KeyReturn keyReturn = JsonConvert.DeserializeObject<KeyReturn>(Encoding.ASCII.GetString(_revMsg));
                                                    if (keyReturn.code == 200)
                                                    {
                                                        DelegateInfo.Service.ChangeLog($"{MicroSn}已连接");
                                                        ConnectText = MicroSn + "已连接";
                                                        IsFuncEnable = true;
                                                        _revMsg = null;
                                                        return;
                                                    }
                                                    else
                                                    {
                                                        _revMsg = null;
                                                        DelegateInfo.Service.ChangeLog("密钥匹配失败！");
                                                        return;
                                                    }
                                                }
                                                else
                                                {
                                                    DelegateInfo.Service.ChangeLog("密钥匹配超时！");
                                                    return;
                                                }
                                            }
                                        }

                                    }
                                }

                                DelegateInfo.Service.ChangeLog("连接失败,请确认设备和蓝牙状态!");
                                return;
                            }
                        }

                        DelegateInfo.Service.ChangeLog("未查询到该蓝牙设备，请确认设备和蓝牙状态！");
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

            });

            task.Wait();

        }

        private void CloseBLEDevice()
        {
            if (_conDevice != null)
            {
                if (!_conDevice.Gatt.IsConnected)
                {
                    DelegateInfo.Service.ChangeLog("已关闭");
                }
                else
                {
                    _conDevice.Gatt.Disconnect();
                }

                if (_characteristic != null)
                {
                    _characteristic.CharacteristicValueChanged -= Characteristic_CharacteristicValueChanged;
                    _characteristic = null;
                }

                ConnectText = "";
                IsFuncEnable = false;
                return;
            }
        }

        private void ReadSN()
        {
            if (_conDevice == null || !_conDevice.Gatt.IsConnected)
            {
                DelegateInfo.Service.ChangeLog("蓝牙设备异常或蓝牙已断开！");
                return;
            }

            DataSendModel dataSend = new DataSendModel()
            {
                code = 18,
                data = new DataSend
                {
                    sn = _conDevice.Gatt.Device.Name,
                    val = new List<int> { }
                }
            };

            // 30026
            byte[] sendDatas = new byte[] { 0x01, 0x03, 0x75, 0x4a, 0x00, 0x08 };

            //CRC 校验拼接
            byte[] temp = new byte[sendDatas.Length + 2];
            Array.Copy(sendDatas, temp, sendDatas.Length);
            Array.Copy(Modbus.CaculateCheckSum(sendDatas), 0, temp, temp.Length - 2, 2);

            foreach (var dt in temp)
            {
                dataSend.data.val.Add((int)dt);
            }

            _revMsg = null;
            JsonConvert.SerializeObject(dataSend);
            Log.Debug("Send: " + JsonConvert.SerializeObject(dataSend));
            _characteristic?.WriteValueWithResponseAsync(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dataSend)));

            int tickCount = Environment.TickCount;
            while (_revMsg == null)
            {
                Thread.Sleep(200);
                if (Environment.TickCount - tickCount > 5000)
                {
                    DelegateInfo.Service.ChangeLog("蓝牙数据接收超时");
                    return;

                }
            }

            DataResponseModel response = JsonConvert.DeserializeObject<DataResponseModel>(Encoding.ASCII.GetString(_revMsg));

            if (response.code == 200)
            {
                byte[] revDatas = new byte[response.data.Count];
                for (int i = 0; i < revDatas.Count(); i++)
                {
                    revDatas[i] = (byte)response.data[i];
                }

                string readSn = Encoding.ASCII.GetString(Modbus.GetData(revDatas));
                DelegateInfo.Service.ChangeLog($"读取SN为-{readSn}成功！");
                return;
            }
            else
            {
                DelegateInfo.Service.ChangeLog($"读取失败！");
                return;
            }

        }


        private void WriteSN(string Sn)
        {
            if (_conDevice == null || !_conDevice.Gatt.IsConnected)
            {
                DelegateInfo.Service.ChangeLog("蓝牙设备异常或蓝牙已断开！");
                return;
            }
            if (Sn.Length != 16)
            {
                DelegateInfo.Service.ChangeLog("SN长度错误！");
                return;
            }

            if (Sn.Substring(2, 1) != "2")
            {
                DelegateInfo.Service.ChangeLog("SN第2位不符合规则！");
                return;
            }

            DataSendModel dataSend = new DataSendModel()
            {
                code = 18,
                data = new DataSend
                {
                    sn = _conDevice.Gatt.Device.Name,
                    val = new List<int> { }
                }
            };

            // 40026
            byte[] address = new byte[] { 0x9c, 0x5a };
            //byte[] address = new byte[] { 0x75, 0x4a };
            var bytes = Encoding.UTF8.GetBytes(Sn).ToArray();
            byte[] sendData;
            byte len = (byte)bytes.Length;
            ushort count = (ushort)(len / 2);
            sendData = new byte[] { 0x01, 0x10 };
            sendData = sendData.Concat(address).ToArray();
            sendData = sendData.Concat(BitConverter.GetBytes(count).Reverse().ToArray()).ToArray();
            sendData = sendData.Concat(new byte[] { len }).ToArray();
            sendData = sendData.Concat(bytes).ToArray();

            //CRC 校验拼接
            byte[] temp = new byte[sendData.Length + 2];
            Array.Copy(sendData, temp, sendData.Length);
            Array.Copy(Modbus.CaculateCheckSum(sendData), 0, temp, temp.Length - 2, 2);

            foreach (var dt in temp)
            {
                dataSend.data.val.Add((int)dt);
            }

            _revMsg = null;
            JsonConvert.SerializeObject(dataSend);
            _characteristic?.WriteValueWithResponseAsync(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dataSend)));

            int tickCount = Environment.TickCount;
            while (_revMsg == null)
            {
                Thread.Sleep(200);
                if (Environment.TickCount - tickCount > 5000)
                {
                    DelegateInfo.Service.ChangeLog("蓝牙数据接收超时");
                    return;

                }
            }

            DataResponseModel response = JsonConvert.DeserializeObject<DataResponseModel>(Encoding.ASCII.GetString(_revMsg));

            if (response.code == 200)
            {
                DelegateInfo.Service.ChangeLog($"{Sn}写入成功！");
                return;
            }
            else
            {
                DelegateInfo.Service.ChangeLog($"{Sn}写入失败！");
                return;
            }

        }


        private void ReadSoftVersion()
        {
            if (_conDevice == null || !_conDevice.Gatt.IsConnected)
            {
                DelegateInfo.Service.ChangeLog("蓝牙设备异常或蓝牙已断开！");
                return;
            }

            DataSendModel dataSend = new DataSendModel()
            {
                code = 18,
                data = new DataSend
                {
                    sn = _conDevice.Gatt.Device.Name,
                    val = new List<int> { }
                }
            };

            // 30039
            byte[] sendDatas = new byte[] { 0x01, 0x03, 0x75, 0x57, 0x00, 0x02 };

            //CRC 校验拼接
            byte[] temp = new byte[sendDatas.Length + 2];
            Array.Copy(sendDatas, temp, sendDatas.Length);
            Array.Copy(Modbus.CaculateCheckSum(sendDatas), 0, temp, temp.Length - 2, 2);

            foreach (var dt in temp)
            {
                dataSend.data.val.Add((int)dt);
            }

            _revMsg = null;
            JsonConvert.SerializeObject(dataSend);
            Log.Debug("Send: " + JsonConvert.SerializeObject(dataSend));
            _characteristic?.WriteValueWithResponseAsync(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dataSend)));

            int tickCount = Environment.TickCount;
            while (_revMsg == null)
            {
                Thread.Sleep(200);
                if (Environment.TickCount - tickCount > 5000)
                {
                    DelegateInfo.Service.ChangeLog("蓝牙数据接收超时");
                    return;

                }
            }

            DataResponseModel response = JsonConvert.DeserializeObject<DataResponseModel>(Encoding.ASCII.GetString(_revMsg));

            if (response.code == 200)
            {
                byte[] revDatas = new byte[response.data.Count];
                for (int i = 0; i < revDatas.Count(); i++)
                {
                    revDatas[i] = (byte)response.data[i];
                }

                byte[] result = Modbus.GetData(revDatas);

                string version = string.Format("V-{0:00}.{1:00}.{2:00}.{3:00}", result[0], result[1], result[2], result[3]);

                DelegateInfo.Service.ChangeLog($"软件版本为-{version}！");
                return;
            }
            else
            {
                DelegateInfo.Service.ChangeLog($"读取失败！");
                return;
            }


        }

        private void SetOld()
        {
            if (_conDevice == null || !_conDevice.Gatt.IsConnected)
            {
                DelegateInfo.Service.ChangeLog("蓝牙设备异常或蓝牙已断开！");
                return;
            }

            DataSendModel dataSend = new DataSendModel()
            {
                code = 18,
                data = new DataSend
                {
                    sn = _conDevice.Gatt.Device.Name,
                    val = new List<int> { }
                }
            };

            // 40099
            byte[] address = new byte[] { 0x9c, 0xa3 };
            var bytes = BitConverter.GetBytes((ushort)1).Reverse().ToArray();
            byte[] sendData;
            byte len = (byte)bytes.Length;
            ushort count = (ushort)(len / 2);
            sendData = new byte[] { 0x01, 0x10 };
            sendData = sendData.Concat(address).ToArray();
            sendData = sendData.Concat(BitConverter.GetBytes(count).Reverse().ToArray()).ToArray();
            sendData = sendData.Concat(new byte[] { len }).ToArray();
            sendData = sendData.Concat(bytes).ToArray();

            //CRC 校验拼接
            byte[] temp = new byte[sendData.Length + 2];
            Array.Copy(sendData, temp, sendData.Length);
            Array.Copy(Modbus.CaculateCheckSum(sendData), 0, temp, temp.Length - 2, 2);

            foreach (var dt in temp)
            {
                dataSend.data.val.Add((int)dt);
            }

            _revMsg = null;
            JsonConvert.SerializeObject(dataSend);
            _characteristic?.WriteValueWithResponseAsync(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dataSend)));

            int tickCount = Environment.TickCount;
            while (_revMsg == null)
            {
                Thread.Sleep(200);
                if (Environment.TickCount - tickCount > 5000)
                {
                    DelegateInfo.Service.ChangeLog("蓝牙数据接收超时");
                    return;

                }
            }

            DataResponseModel response = JsonConvert.DeserializeObject<DataResponseModel>(Encoding.ASCII.GetString(_revMsg));

            if (response.code == 200)
            {
                DelegateInfo.Service.ChangeLog($"写入成功！");
                return;
            }
            else
            {
                DelegateInfo.Service.ChangeLog($"写入失败！");
                return;
            }
        }

        private void ReadOld()
        {
            if (_conDevice == null || !_conDevice.Gatt.IsConnected)
            {
                DelegateInfo.Service.ChangeLog("蓝牙设备异常或蓝牙已断开！");
                return;
            }

            DataSendModel dataSend = new DataSendModel()
            {
                code = 18,
                data = new DataSend
                {
                    sn = _conDevice.Gatt.Device.Name,
                    val = new List<int> { }
                }
            };

            // 40099
            byte[] sendDatas = new byte[] { 0x01, 0x03, 0x9c, 0xa3, 0x00, 0x01 };

            //CRC 校验拼接
            byte[] temp = new byte[sendDatas.Length + 2];
            Array.Copy(sendDatas, temp, sendDatas.Length);
            Array.Copy(Modbus.CaculateCheckSum(sendDatas), 0, temp, temp.Length - 2, 2);

            foreach (var dt in temp)
            {
                dataSend.data.val.Add((int)dt);
            }

            _revMsg = null;
            JsonConvert.SerializeObject(dataSend);
            Log.Debug("Send: " + JsonConvert.SerializeObject(dataSend));
            _characteristic?.WriteValueWithResponseAsync(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dataSend)));

            int tickCount = Environment.TickCount;
            while (_revMsg == null)
            {
                Thread.Sleep(200);
                if (Environment.TickCount - tickCount > 5000)
                {
                    DelegateInfo.Service.ChangeLog("蓝牙数据接收超时");
                    return;

                }
            }

            DataResponseModel response = JsonConvert.DeserializeObject<DataResponseModel>(Encoding.ASCII.GetString(_revMsg));

            if (response.code == 200)
            {
                byte[] revDatas = new byte[response.data.Count];
                for (int i = 0; i < revDatas.Count(); i++)
                {
                    revDatas[i] = (byte)response.data[i];
                }

                byte[] result = Modbus.GetData(revDatas);
                ushort oldCode = ((ushort)((result[0] << 8) + result[1]));
                DelegateInfo.Service.ChangeLog($"读取老化模式为-{oldCode}成功！");
                return;
            }
            else
            {
                DelegateInfo.Service.ChangeLog($"读取失败！");
                return;
            }
        }

        private void SetDomain()
        {
            if (_conDevice == null || !_conDevice.Gatt.IsConnected)
            {
                DelegateInfo.Service.ChangeLog("蓝牙设备异常或蓝牙已断开！");
                return;
            }

            string cmd = "{\"code\":22, \"md\":\"mqtt\", \"host\":\"test-mq.sunways-portal.com\", \"port\":8883, \"SSL/TLS\":1}";
            //string cmd = "{\"code\":22, \"md\":\"mqtt\", \"host\":\"iot-broker.sunways-portal.com\", \"port\":8883, \"SSL/TLS\":1}";

            _revMsg = null;
            _characteristic?.WriteValueWithResponseAsync(Encoding.UTF8.GetBytes(cmd));

            int tickCount = Environment.TickCount;
            while (_revMsg == null)
            {
                Thread.Sleep(200);
                if (Environment.TickCount - tickCount > 5000)
                {
                    DelegateInfo.Service.ChangeLog("蓝牙数据接收超时");
                    return;

                }
            }

            //string cmdR = "{\n\t\"code\":\t200,\n\t\"md\":\t\"mqtt\",\n\t\"host\":\t\"iot-broker.sunways-portal.com\",\n\t\"port\":\t8883,\n\t\"SSL/TLS\":\t1\n}";
            string cmdR = "{\n\t\"code\":\t200,\n\t\"md\":\t\"mqtt\",\n\t\"host\":\t\"test-mq.sunways-portal.com\",\n\t\"port\":\t8883,\n\t\"SSL/TLS\":\t1\n}";

            if (Encoding.ASCII.GetString(_revMsg) == cmdR)
            {
                DelegateInfo.Service.ChangeLog("设置域名端口成功");
                return;
            }
            else
            {
                DelegateInfo.Service.ChangeLog("设置域名端口失败");
                return;
            }
        }

        private void WriteSafety(string value)
        {
            if (_conDevice == null || !_conDevice.Gatt.IsConnected)
            {
                DelegateInfo.Service.ChangeLog("蓝牙设备异常或蓝牙已断开!");
                return;
            }

            if (string.IsNullOrEmpty(value)) 
            {
                DelegateInfo.Service.ChangeLog("请选择安规!");
                return;
            }

            DataSendModel dataSend = new DataSendModel()
            {
                code = 18,
                data = new DataSend
                {
                    sn = _conDevice.Gatt.Device.Name,
                    val = new List<int> { }
                }
            };

            ushort address = (ushort)30045;
            byte[] data = BitConverter.GetBytes(Variable._safetyDic[value]).Reverse().ToArray();

            byte[] sendData;
            byte len = (byte)data.Length;
            ushort count = (ushort)(len / 2);

            sendData = new byte[] { 0x01, 0x10 };
            sendData = sendData.Concat(BitConverter.GetBytes(address).Reverse().ToArray()).ToArray();
            sendData = sendData.Concat(BitConverter.GetBytes(count).Reverse().ToArray()).ToArray();
            sendData = sendData.Concat(new byte[] { len }).ToArray();
            sendData = sendData.Concat(data).ToArray();

            //CRC 校验拼接
            byte[] temp = new byte[sendData.Length + 2];
            Array.Copy(sendData, temp, sendData.Length);
            Array.Copy(Modbus.CaculateCheckSum(sendData), 0, temp, temp.Length - 2, 2);

            foreach (var dt in temp)
            {
                dataSend.data.val.Add((int)dt);
            }


            JsonConvert.SerializeObject(dataSend);
            Log.Debug("Send: " + JsonConvert.SerializeObject(dataSend));
            _revMsg = null;
            _characteristic?.WriteValueWithResponseAsync(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dataSend)));

            int tickCount = Environment.TickCount;
            while (_revMsg == null)
            {
                Thread.Sleep(200);
                if (Environment.TickCount - tickCount > 5000)
                {
                    DelegateInfo.Service.ChangeLog("蓝牙数据接收超时");
                    return;

                }
            }

            DataResponseModel response = JsonConvert.DeserializeObject<DataResponseModel>(Encoding.ASCII.GetString(_revMsg));

            if (response.code == 200)
            {
                DelegateInfo.Service.ChangeLog($"安规写入成功!");
                return;
            }
            else
            {
                DelegateInfo.Service.ChangeLog($"安规写入失败!");
                return;
            }

        }

        private void ReadSafety()
        {
            if (_conDevice == null || !_conDevice.Gatt.IsConnected)
            {
                DelegateInfo.Service.ChangeLog("蓝牙设备异常或蓝牙已断开！");
                return;
            }

            DataSendModel dataSend = new DataSendModel()
            {
                code = 18,
                data = new DataSend
                {
                    sn = _conDevice.Gatt.Device.Name,
                    val = new List<int> { }
                }
            };

            // 30045
            byte[] sendDatas = new byte[] { 0x01, 0x03, 0x75, 0x5d, 0x00, 0x01 };

            //CRC 校验拼接
            byte[] temp = new byte[sendDatas.Length + 2];
            Array.Copy(sendDatas, temp, sendDatas.Length);
            Array.Copy(Modbus.CaculateCheckSum(sendDatas), 0, temp, temp.Length - 2, 2);

            foreach (var dt in temp)
            {
                dataSend.data.val.Add((int)dt);
            }

            _revMsg = null;
            JsonConvert.SerializeObject(dataSend);
            Log.Debug("Send: " + JsonConvert.SerializeObject(dataSend));
            _characteristic?.WriteValueWithResponseAsync(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dataSend)));

            int tickCount = Environment.TickCount;
            while (_revMsg == null)
            {
                Thread.Sleep(200);
                if (Environment.TickCount - tickCount > 5000)
                {
                    DelegateInfo.Service.ChangeLog("蓝牙数据接收超时");
                    return;

                }
            }

            DataResponseModel response = JsonConvert.DeserializeObject<DataResponseModel>(Encoding.ASCII.GetString(_revMsg));

            if (response.code == 200)
            {
                byte[] revDatas = new byte[response.data.Count];
                for (int i = 0; i < revDatas.Count(); i++)
                {
                    revDatas[i] = (byte)response.data[i];
                }

                byte[] result = Modbus.GetData(revDatas);
                ushort safety = ((ushort)((result[0] << 8) + result[1]));
                string value = "";
                foreach (var a in Variable._safetyDic)
                {
                    if (a.Value == safety)
                    {
                        value = a.Key;
                        break;
                    }
                }
                 
                DelegateInfo.Service.ChangeLog($"读取安规为-{value}成功！");
                return;
            }
            else
            {
                DelegateInfo.Service.ChangeLog($"读取失败！");
                return;
            }
        }


        private  void Characteristic_CharacteristicValueChanged(object sender, GattCharacteristicValueChangedEventArgs e)
        {

            lock (_dataLocker)
            {
                _revMsg = e.Value;
            }
  
            var msg = e.Value;
            Log.Debug("Recv: " + Encoding.ASCII.GetString(msg));
        }

        
    }
}
