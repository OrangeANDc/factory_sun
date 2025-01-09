using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LanguageConfig;
using Microsoft.Win32;
using SafetyTestTool.Model;
using SafetyTestTool.Protocol;
using SafetyTestTool.StaticSource;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Provider;
using System.Windows.Documents;
using System.Windows.Markup;

namespace SafetyTestTool.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        private SerialDevice _device;
        private ObservableCollection<ParaInfModel> _paraLists = new ObservableCollection<ParaInfModel>
            {
                // 第一组
                new ParaInfModel() { DataName = "电网和系统保护" },
                new ParaInfModel() { DataName = "一级欠压保护限值", DataAddress = 29004, DataUnit = "V", Gain = 10},
                new ParaInfModel() { DataName = "一级欠压保护时间", DataAddress = 29005, DataUnit = "Prd", Gain = 1},
                new ParaInfModel() { DataName = "一级过压保护限值", DataAddress = 29006, DataUnit = "V", Gain = 10},
                new ParaInfModel() { DataName = "一级过压保护时间", DataAddress = 29007, DataUnit = "Prd", Gain = 1},
                                                                                 
                new ParaInfModel() { DataName = "二级欠压保护限值", DataAddress = 29008, DataUnit = "V", Gain = 10},
                new ParaInfModel() { DataName = "二级欠压保护时间", DataAddress = 29009, DataUnit = "Prd", Gain = 1},
                new ParaInfModel() { DataName = "二级过压保护限值", DataAddress = 29010, DataUnit = "V", Gain = 10},
                new ParaInfModel() { DataName = "二级过压保护时间", DataAddress = 29011, DataUnit = "Prd", Gain = 1},
                                                                                 
                new ParaInfModel() { DataName = "一级欠频保护限值", DataAddress = 29012, DataUnit = "Hz", Gain = 100},
                new ParaInfModel() { DataName = "一级欠频保护时间", DataAddress = 29013, DataUnit = "Prd", Gain = 1},
                new ParaInfModel() { DataName = "一级过频保护限值", DataAddress = 29014, DataUnit = "Hz", Gain = 100},
                new ParaInfModel() { DataName = "一级过频保护时间", DataAddress = 29015, DataUnit = "Prd", Gain = 1},
                                                                                 
                new ParaInfModel() { DataName = "二级欠频保护限值", DataAddress = 29016, DataUnit = "Hz", Gain = 100},
                new ParaInfModel() { DataName = "二级欠频保护时间", DataAddress = 29017, DataUnit = "Prd", Gain = 1},
                new ParaInfModel() { DataName = "二级过频保护限值", DataAddress = 29018, DataUnit = "Hz", Gain = 100},
                new ParaInfModel() { DataName = "二级过频保护时间", DataAddress = 29019, DataUnit = "Prd", Gain = 1},

                // 第二组
                new ParaInfModel() { DataName = "无功功率/电压特性Q(U)" },
                new ParaInfModel() { DataName = "QU曲线功能开关",     DataAddress = 29061, Gain = 1, IsSwitch = true},
                new ParaInfModel() { DataName = "进入QU曲线功率阈值",  DataAddress = 29063, DataUnit = "%", Gain = 10},
                new ParaInfModel() { DataName = "退出QU曲线功率阈值",  DataAddress = 29064, DataUnit = "%", Gain = 10},
                new ParaInfModel() { DataName = "QU时间常数",        DataAddress =  29065,    DataUnit = "s", Gain = 1},
                new ParaInfModel() { DataName = "最小功率因数限值",  DataAddress =   29067,   DataUnit = "", Gain = 100},

                new ParaInfModel() { DataName = "QU曲线1点电压",      DataAddress = 29069,   DataUnit = "V", Gain = 10},
                new ParaInfModel() { DataName = "QU曲线1点无功百分比", DataAddress = 29070,   DataUnit = "%", Gain = 10},
                new ParaInfModel() { DataName = "QU曲线2点电压",     DataAddress =  29071,   DataUnit = "V", Gain = 10},
                new ParaInfModel() { DataName = "QU曲线2点无功百分比", DataAddress = 29072,   DataUnit = "%", Gain = 10},
                new ParaInfModel() { DataName = "QU曲线3点电压",     DataAddress =  29073,   DataUnit = "V", Gain = 10},
                new ParaInfModel() { DataName = "QU曲线3点无功百分比", DataAddress = 29074,  DataUnit = "%", Gain = 10},
                new ParaInfModel() { DataName = "QU曲线4点电压",     DataAddress =  29075,   DataUnit = "V", Gain = 10},
                new ParaInfModel() { DataName = "QU曲线4点无功百分比", DataAddress = 29076,   DataUnit = "%", Gain = 10, IsUnCheck = true},


                new ParaInfModel() { DataName = "定无功参数" },
                new ParaInfModel() { DataName = "定无功开关",  DataAddress = 25115, DataUnit = "", Gain = 1, IsSwitch = true},
                new ParaInfModel() { DataName = "定无功限值",  DataAddress = 25118, DataUnit = "%Pn", Gain = 10, IsUnCheck = true },

                 // 第四组
                 ////新加+++
                new ParaInfModel() { DataName = "定有功参数" },
                new ParaInfModel() { DataName = "有功功率调度设置",  DataAddress = 25114, DataUnit = "%Pn", Gain = 10},
                

                // 第五组
                new ParaInfModel() { DataName = "定功率因素参数" },
                new ParaInfModel() { DataName = "定功率因素限值",    DataAddress = 25120, DataUnit = "", Gain = 1000, IsUnCheck = true},


                // 第三组
                new ParaInfModel() { DataName = "功率因数/功率特性PF(P)" },
                new ParaInfModel() { DataName = "PF曲线功能开关",    DataAddress = 29091, DataUnit = "", Gain = 1, IsSwitch = true},
                new ParaInfModel() { DataName = "PF曲线功率点A",     DataAddress = 29093, DataUnit = "%Pn", Gain = 10},
                new ParaInfModel() { DataName = "PF曲线功率点B",     DataAddress = 29094, DataUnit = "%Pn", Gain = 10},
                new ParaInfModel() { DataName = "PF曲线功率点C",     DataAddress = 29095, DataUnit = "%Pn", Gain = 10},
                new ParaInfModel() { DataName = "功率A点PF",         DataAddress = 29096, DataUnit = "", Gain = 1000},
                new ParaInfModel() { DataName = "功率B点PF",         DataAddress = 29097, DataUnit = "", Gain = 1000},
                new ParaInfModel() { DataName = "功率C点PF",         DataAddress = 29098, DataUnit = "", Gain = 1000},
                new ParaInfModel() { DataName = "PF曲线进入电压阈值", DataAddress = 29099, DataUnit = "%Un", Gain = 10},
                new ParaInfModel() { DataName = "PF曲线退出电压阈值", DataAddress = 29100, DataUnit = "%Un", Gain = 10},
                new ParaInfModel() { DataName = "PF曲线退出功率阈值", DataAddress = 29101, DataUnit = "%Pn", Gain = 10},

               
               
                // 第4组
                new ParaInfModel() { DataName = "并网限制条件" },
                new ParaInfModel() { DataName = "无故障并网等待时间",   DataAddress = 29051, DataUnit = "s", Gain = 1},
                new ParaInfModel() { DataName = "电网故障重连等待时间", DataAddress = 29052, DataUnit = "s", Gain = 1},
                new ParaInfModel() { DataName = "欠压恢复限值",        DataAddress = 29022, DataUnit = "V", Gain = 10},
                new ParaInfModel() { DataName = "过压恢复限值",        DataAddress = 29023, DataUnit = "V", Gain = 10},
                new ParaInfModel() { DataName = "欠频恢复限值",        DataAddress = 29028, DataUnit = "Hz", Gain = 100},
                new ParaInfModel() { DataName = "过频恢复限值",        DataAddress = 29029, DataUnit = "Hz", Gain = 100},
                new ParaInfModel() { DataName = "功率缓变速率",     DataAddress = 29054, DataUnit = "%Pn/s", Gain = 100},
                
                // 第5组
                new ParaInfModel() { DataName = "过压时功率降低P(U)" },
                new ParaInfModel() { DataName = "PU曲线功能开关",         DataAddress = 29111, DataUnit = "", Gain = 1, IsSwitch = true},
                new ParaInfModel() { DataName = "P(U)时间常数",            DataAddress = 29113, DataUnit = "s", Gain = 1,},
                new ParaInfModel() { DataName = "PU曲线电压点3号电压",      DataAddress = 29114, DataUnit = "V", Gain = 10},
                new ParaInfModel() { DataName = "PU曲线3号电压对应的功率",  DataAddress = 29115, DataUnit = "%Pn", Gain = 10},
                new ParaInfModel() { DataName = "PU曲线电压点4号电压",      DataAddress = 29116, DataUnit = "V", Gain = 10},
                new ParaInfModel() { DataName = "PU曲线4号电压对应的功率",  DataAddress = 29117, DataUnit = "%Pn", Gain = 10},
                

                // 第6组
                new ParaInfModel() { DataName = "过频时降低功率" },

                new ParaInfModel() { DataName = "过频降载功能开关",           DataAddress = 29131, DataUnit = "", Gain = 1, IsSwitch = true},
                new ParaInfModel() { DataName = "过频降载开始频率",           DataAddress = 29133, DataUnit = "Hz", Gain = 100},
                new ParaInfModel() { DataName = "过频降载终点频率",           DataAddress = 29135, DataUnit = "Hz", Gain = 100},            
                new ParaInfModel() { DataName = "过频降载恢复功率频率阈值",    DataAddress = 29137, DataUnit = "Hz", Gain = 100},
                new ParaInfModel() { DataName = "过频降载恢复功率等待时间",    DataAddress = 29138, DataUnit = "s", Gain = 1},
                new ParaInfModel() { DataName = "过频降载功率恢复缓慢加载开关", DataAddress = 29139, DataUnit = "", Gain = 1, IsSwitch = true},
                new ParaInfModel() { DataName = "过频降载功率恢复缓慢加载速率", DataAddress = 29140, DataUnit = "%Pn/min", Gain = 10},
                new ParaInfModel() { DataName = "降载系数",                    DataAddress = 29141, DataUnit = "%", Gain = 10},


                // 第7组
                new ParaInfModel() { DataName = "LVRT" },
                new ParaInfModel() { DataName = "低穿功能开关",        DataAddress = 29151, DataUnit = "", Gain = 1, IsSwitch = true},
                new ParaInfModel() { DataName = "低穿进入电压",    DataAddress = 29154, DataUnit = "Un", Gain = 1000               },
    

                new ParaInfModel() { DataName = "低穿电压点1",         DataAddress = 29156, DataUnit = "Un", Gain = 1000               },           
                new ParaInfModel() { DataName = "低穿电压点2",         DataAddress = 29157, DataUnit = "Un", Gain = 1000             },             
                new ParaInfModel() { DataName = "低穿电压点3",         DataAddress = 29158, DataUnit = "Un", Gain = 1000              },          
                new ParaInfModel() { DataName = "低穿电压点4",         DataAddress = 29159, DataUnit = "Un", Gain = 1000              },          
                new ParaInfModel() { DataName = "低穿电压点5",         DataAddress = 29160, DataUnit = "Un", Gain = 1000               },
                new ParaInfModel() { DataName = "低穿电压点1保护时间",  DataAddress = 29161, DataUnit = "ms", Gain = 1                },
                new ParaInfModel() { DataName = "低穿电压点2保护时间",  DataAddress = 29162, DataUnit = "ms", Gain = 1                },
                new ParaInfModel() { DataName = "低穿电压点3保护时间",  DataAddress = 29163, DataUnit = "ms", Gain = 1                },
                new ParaInfModel() { DataName = "低穿电压点4保护时间",  DataAddress = 29164, DataUnit = "ms", Gain = 1                  },
                new ParaInfModel() { DataName = "低穿电压点5保护时间",  DataAddress = 29165, DataUnit = "ms", Gain = 1                 },

                // 第8组
                new ParaInfModel() { DataName = "HVRT" },
                new ParaInfModel() { DataName = "高穿功能开关",  DataAddress = 29171, DataUnit = "", Gain = 1, IsSwitch = true},
                new ParaInfModel() { DataName = "高穿进入电压",  DataAddress = 29174, DataUnit = "Un", Gain = 1000},

                // 第11组
                new ParaInfModel() { DataName = "其他" },
                new ParaInfModel() { DataName = "10分钟过压开关",     DataAddress = 29020, DataUnit = "", Gain = 1, IsSwitch = true},
                new ParaInfModel() { DataName = "10分钟过压阈值",     DataAddress = 29021, DataUnit = "V", Gain = 10},


            };
        public MainViewModel(SerialDevice serialDevice)
        {
            _device = serialDevice;
            ReadDataCommand = new RelayCommand(ReadData);
            ParaInf = _paraLists;

            InfoService.Service = new  InfoConfig();
            InfoService.Service.OnChangeInfo += ReadData;
        }

        private ObservableCollection<ParaInfModel> _paraInf;
        public ObservableCollection<ParaInfModel> ParaInf
        {
            get => _paraInf;
            set => SetProperty(ref _paraInf, value);
        }

        public RelayCommand ReadDataCommand { get; set; }


        private void ReadData()
        {
            if (!_device.GetStatus())
            {
                MessageBox.Show("Please Open the COM!");
                return;
            }

            try
            {
                ParaInf = _paraLists;
                byte[] address = BitConverter.GetBytes((ushort)29000).Reverse().ToArray();
                byte[] result = new byte[] { };
                // 29000 ~ 29189
                if (_device.GetFunc(address, 190, ref result))
                {
                    for (int i = 0; i < ParaInf.Count; i++)
                    {
                        if (ParaInf[i].DataAddress != null && ParaInf[i].DataAddress >= 29000)
                        {
                            ParaInf[i].CommandInf = DateTime.Now.ToString("hh:mm:ss ") + "Read data succeed.";
                            if (ParaInf[i].IsFix)
                            {
                                ParaInf[i].DataValue = ParaInf[i].FixValue;
                                continue;

                            }

                            int startIndex = (int)(ParaInf[i].DataAddress - 29000) * 2;
                            dynamic value;
                            //有符号判断
                            if ((bool)ParaInf[i].IsUnCheck)
                                value = unchecked((short)((result[startIndex] << 8) + result[startIndex + 1]));
                            else
                                value = (ushort)((result[startIndex] << 8) + result[startIndex + 1]);
                            
                            switch (ParaInf[i].Gain)
                            {
                                case 0.5:
                                    ParaInf[i].DataValue = (value * 2).ToString();
                                    break;
                                case 1:
                                    ParaInf[i].DataValue = value.ToString();
                                    break;
                                case 10:
                                    ParaInf[i].DataValue = (value * 0.1).ToString("0.0");
                                    break;
                                case 16:
                                    ParaInf[i].DataValue = (value * 1.0 / 16).ToString("0.0");
                                    break;
                                case 100:
                                    ParaInf[i].DataValue = (value * 0.01).ToString("0.00");
                                    break;
                                case 1000:
                                    ParaInf[i].DataValue = (value * 0.001).ToString("0.000");
                                    break;
                                case 5200:
                                    ParaInf[i].DataValue = (value * 1.0 / 5200).ToString("0.000");
                                    break;
                                case 10000:
                                    ParaInf[i].DataValue = (value * 0.0001).ToString("0.0000");
                                    break;
                                default:
                                    ParaInf[i].DataValue = value.ToString();
                                    break;
                            }

                        }
                    }
                }
                else
                {
                    foreach (var list in ParaInf)
                    {
                        if (list.DataAddress != null && list.DataAddress >= 29000)
                        {
                            list.CommandInf = DateTime.Now.ToString("hh:mm:ss ") + "Read data failed.";
                        }
                    }
                }

                address = BitConverter.GetBytes((ushort)25114).Reverse().ToArray();
                if (_device.GetFunc(address, 10, ref result))
                {
                    for (int i = 0; i < ParaInf.Count; i++)
                    {
                        if (ParaInf[i].DataAddress != null && ParaInf[i].DataAddress >= 25000  && ParaInf[i].DataAddress < 26000)
                        {
                            ParaInf[i].CommandInf = DateTime.Now.ToString("hh:mm:ss ") + "Read data succeed.";
                            if (ParaInf[i].IsFix)
                            {
                                ParaInf[i].DataValue = ParaInf[i].FixValue;
                                continue;

                            }

                            int startIndex = (int)(ParaInf[i].DataAddress - 25114) * 2;
                            dynamic value;
                            //有符号判断
                            if ((bool)ParaInf[i].IsUnCheck)
                                value = unchecked((short)((result[startIndex] << 8) + result[startIndex + 1]));
                            else
                                value = (ushort)((result[startIndex] << 8) + result[startIndex + 1]);

                            switch (ParaInf[i].Gain)
                            {
                                case 0.5:
                                    ParaInf[i].DataValue = (value * 2).ToString();
                                    break;
                                case 1:
                                    ParaInf[i].DataValue = value.ToString();
                                    break;
                                case 10:
                                    ParaInf[i].DataValue = (value * 0.1).ToString("0.0");
                                    break;
                                case 16:
                                    ParaInf[i].DataValue = (value * 1.0 / 16).ToString("0.0");
                                    break;
                                case 100:
                                    ParaInf[i].DataValue = (value * 0.01).ToString("0.00");
                                    break;
                                case 1000:
                                    ParaInf[i].DataValue = (value * 0.001).ToString("0.000");
                                    break;
                                case 5200:
                                    ParaInf[i].DataValue = (value * 1.0 / 5200).ToString("0.000");
                                    break;
                                case 10000:
                                    ParaInf[i].DataValue = (value * 0.0001).ToString("0.0000");
                                    break;
                                default:
                                    ParaInf[i].DataValue = value.ToString();
                                    break;
                            }

                        }
                    }
                }
                else
                {
                    foreach (var list in ParaInf)
                    {
                        if (list.DataAddress != null && list.DataAddress >= 25114 && list.DataAddress < 26000)
                        {
                            list.CommandInf = DateTime.Now.ToString("hh:mm:ss ") + "Read data failed.";
                        }
                    }
                }

                address = BitConverter.GetBytes((ushort)28071).Reverse().ToArray();
                if (_device.GetFunc(address, 2, ref result))
                {
                    for (int i = 0; i < ParaInf.Count; i++)
                    {
                        if (ParaInf[i].DataAddress != null && ParaInf[i].DataAddress >= 28000 && ParaInf[i].DataAddress < 28080)
                        {
                            ParaInf[i].CommandInf = DateTime.Now.ToString("hh:mm:ss ") + "Read data succeed.";
                            if (ParaInf[i].IsFix)
                            {
                                ParaInf[i].DataValue = ParaInf[i].FixValue;
                                continue;

                            }

                            int startIndex = (int)(ParaInf[i].DataAddress - 28071) * 2;
                            dynamic value;
                            //有符号判断
                            if ((bool)ParaInf[i].IsUnCheck)
                                value = unchecked((short)((result[startIndex] << 8) + result[startIndex + 1]));
                            else
                                value = (ushort)((result[startIndex] << 8) + result[startIndex + 1]);

                            switch (ParaInf[i].Gain)
                            {
                                case 0.5:
                                    ParaInf[i].DataValue = (value * 2).ToString();
                                    break;
                                case 1:
                                    ParaInf[i].DataValue = value.ToString();
                                    break;
                                case 10:
                                    ParaInf[i].DataValue = (value * 0.1).ToString("0.0");
                                    break;
                                case 16:
                                    ParaInf[i].DataValue = (value * 1.0 / 16).ToString("0.0");
                                    break;
                                case 100:
                                    ParaInf[i].DataValue = (value * 0.01).ToString("0.00");
                                    break;
                                case 1000:
                                    ParaInf[i].DataValue = (value * 0.001).ToString("0.000");
                                    break;
                                case 5200:
                                    ParaInf[i].DataValue = (value * 1.0 / 5200).ToString("0.000");
                                    break;
                                case 10000:
                                    ParaInf[i].DataValue = (value * 0.0001).ToString("0.0000");
                                    break;
                                default:
                                    ParaInf[i].DataValue = value.ToString();
                                    break;
                            }

                        }
                    }
                }
                else
                {
                    foreach (var list in ParaInf)
                    {
                        if (list.DataAddress != null && list.DataAddress >= 28000 && list.DataAddress < 28080)
                        {
                            list.CommandInf = DateTime.Now.ToString("hh:mm:ss ") + "Read data failed.";
                        }
                    }
                }



                RefreshDataGridInfo.refreshDataGridService.RefreshDataGrid();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return;
            }
        }
     
    }
}
