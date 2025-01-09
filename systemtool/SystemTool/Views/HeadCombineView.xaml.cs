using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SystemTool.Views
{
    /// <summary>
    /// HeadCombineView.xaml 的交互逻辑
    /// </summary>
    public partial class HeadCombineView : UserControl
    {
        private string type = string.Empty;
        private string EncryptType = string.Empty;
        private byte[] crcCheck = new byte[30];

        private string SaveFileName;

        public HeadCombineView()
        {
            InitializeComponent();
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;      //该值确定是否可以选择多个文件
            dialog.Title = "请选择需要升级的文件";     //弹窗的标题
            dialog.InitialDirectory = OpenFilePath.Text;       //默认打开的文件夹的位置
            dialog.Filter = "Bin文件(*.bin)|*.bin";       //筛选文件

            if (dialog.ShowDialog() == true)
            {
                OpenFilePath.Text = dialog.FileName;
                SaveFileName = dialog.SafeFileName;
                SaveFilePath.Text = string.Empty;
            }
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "请选择生成文件及目录";  //提示的文字
            dialog.Filter = "Bin文件(*.bin)|*.bin";       //筛选文件
            dialog.InitialDirectory = OpenFilePath.Text;

            if (dialog.ShowDialog() == true)
            {
                SaveFilePath.Text = dialog.FileName;
            }
        }

        private void GenerateFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(OpenFilePath.Text) || string.IsNullOrEmpty(SaveFilePath.Text))
                {
                    MessageBox.Show("选择文件路径或生成文件路径为空,无法生成!");
                    return;
                }

                byte[] binChar = new byte[] { }; //读取bin文件数组
                int fileLen;

                //读取bin文件
                using (var fileStream = new FileStream(OpenFilePath.Text, FileMode.Open, FileAccess.Read))
                {
                    BinaryReader br = new BinaryReader(fileStream, Encoding.UTF8);
                    fileLen = (int)fileStream.Length;
                    binChar = br.ReadBytes(fileLen);
                }

                // 根据文件名区分 类型
                string[] nameArray = OpenFilePath.Text.Split('\\');
                if (nameArray.Last().ToUpper().StartsWith("FA0"))
                    type = "ARM";
                else if (nameArray.Last().ToUpper().StartsWith("FA1"))
                    type = "DATALOG";
                else if (nameArray.Last().ToUpper().StartsWith("FA2"))
                    type = "METER";
                else if (nameArray.Last().ToUpper().StartsWith("FA3"))
                    type = "AFCI";
                else if (nameArray.Last().ToUpper().StartsWith("FDM") || nameArray.Last().ToUpper().StartsWith("FDS"))
                {
                    if (nameArray.Last().ToUpper().Substring(0, 5) == "FDM-M" || nameArray.Last().ToUpper().Substring(0, 5) == "FDS-S")
                        type = "DSP-M";
                    else
                        type = "DSP";
                }
                else
                {
                    MessageBox.Show("无法识别类型");
                    return;
                }


                switch (type)
                {
                    case "ARM":
                        //判断ARM文件是否已经加密
                        if (binChar[0] == 0x53 && binChar[1] == 0x54 && binChar[2] == 0x4D ) 
                        {
                            Array.Copy(binChar, 0, crcCheck, 0, crcCheck.Length);
                            crcCheck[3] = 0x33;
                            crcCheck[4] = 0x32;
                            EncryptType = "已加密";
                            
                        }
                        else  // short
                        {
                            EncryptType = "未加密";
                            crcCheck = new byte[30] { 0x53, 0x54, 0x4D, 0x33,0x32, 0xFF, 0xFF, 0xFF,
                                                0xFF, 0xFF, 0xFF, 0xFF,0xFF, 0xFF, 0xFF, 0xFF,
                                                0xFF, 0xFF, 0xFF, 0xFF,0xFF, 0xFF, 0xFF, 0xFF,
                                                0xFF, 0xFF, 0xFF, 0xFF,0xFF, 0xFF};
                            CrcDataCombine(binChar, fileLen);
                        }
                        break;
                        
                    case "DSP":
                        Array.Copy(binChar, 0, crcCheck, 0, crcCheck.Length);
                        //额外填充DSP头
                        byte[] dspDefault = new byte[12] {0x36, 0x00, 0x31,0x00,0x46,0x00, 0x30,0x00,0x32,0x00,0x31,0x00};
                        Array.Copy(dspDefault, 0, crcCheck, 18, 12);
                        break;

                    case "DSP-M":
                        //判断DSP-M文件是否已经加密
                        if (binChar[0] == 0x54 && binChar[1] == 0x4d && binChar[2] == 0x53)
                        {
                            Array.Copy(binChar, 0, crcCheck, 0, crcCheck.Length);
                            EncryptType = "已加密";
                        }
                        else
                        {
                            EncryptType = "未加密";
                            crcCheck = new byte[30] {  0x54, 0x4D, 0x53, 0xFF,0xFF, 0xFF, 0xFF, 0xFF,
                                                0xFF, 0xFF, 0xFF, 0xFF,0xFF, 0xFF, 0xFF, 0xFF,
                                                0xFF, 0xFF, 0xFF, 0xFF,0xFF, 0xFF, 0xFF, 0xFF,
                                                0xFF, 0xFF, 0xFF, 0xFF,0xFF, 0xFF};
                            CrcDataCombine(binChar, fileLen);
                        }
                        break;
                    

                    case "DATALOG":
                        //判断DATALOG文件是否已经加密
                        if (binChar[0] == 0x53 && binChar[1] == 0x54 && binChar[2] == 0x4C)
                        {
                            Array.Copy(binChar, 0, crcCheck, 0, crcCheck.Length);
                            EncryptType = "已加密";
                        }
                        else
                        {
                            EncryptType = "未加密";
                            crcCheck = new byte[30] { 0x53, 0x54, 0x4C, 0xFF,0xFF, 0xFF, 0xFF, 0xFF,
                                                0xFF, 0xFF, 0xFF, 0xFF,0xFF, 0xFF, 0xFF, 0xFF,
                                                0xFF, 0xFF, 0xFF, 0xFF,0xFF, 0xFF, 0xFF, 0xFF,
                                                0xFF, 0xFF, 0xFF, 0xFF,0xFF, 0xFF};
                            CrcDataCombine(binChar, fileLen);
                        }
                        break;
                    case "METER":
                        //判断METER文件是否已经加密
                        if (binChar[0] == 0x53 && binChar[1] == 0x54 && binChar[2] == 0x4B)
                        {
                            Array.Copy(binChar, 0, crcCheck, 0, crcCheck.Length);
                            EncryptType = "已加密";
                        }
                        else
                        {
                            EncryptType = "未加密";
                            crcCheck = new byte[30] { 0x53, 0x54, 0x4B, 0xFF,0xFF, 0xFF, 0xFF, 0xFF,
                                                0xFF, 0xFF, 0xFF, 0xFF,0xFF, 0xFF, 0xFF, 0xFF,
                                                0xFF, 0xFF, 0xFF, 0xFF,0xFF, 0xFF, 0xFF, 0xFF,
                                                0xFF, 0xFF, 0xFF, 0xFF,0xFF, 0xFF};
                            CrcDataCombine(binChar, fileLen);
                        }
                        break;
                    case "AFCI":
                        //判断AFCI文件是否已经加密
                        if (binChar[0] == 0x41 && binChar[1] == 0x46 && binChar[2] == 0x43 && binChar[2] == 0x49)
                        {
                            Array.Copy(binChar, 0, crcCheck, 0, crcCheck.Length);
                            EncryptType = "已加密";
                        }
                        else
                        {
                            EncryptType = "未加密";
                            crcCheck = new byte[30] { 0x41, 0x46, 0x43, 0x49,0xFF, 0xFF, 0xFF, 0xFF,
                                                0xFF, 0xFF, 0xFF, 0xFF,0xFF, 0xFF, 0xFF, 0xFF,
                                                0xFF, 0xFF, 0xFF, 0xFF,0xFF, 0xFF, 0xFF, 0xFF,
                                                0xFF, 0xFF, 0xFF, 0xFF,0xFF, 0xFF};
                            CrcDataCombine(binChar, fileLen);
                        }
                        break;

                    default:
                        MessageBox.Show("无法判断文件类型!");
                        break;
                }

                //计算CRC值
                byte[] crcNumber = new byte[2];
                crcNumber = ToModbus(crcCheck, 30);
                //计算校验位31
                byte first = (Byte)((int)crcNumber[0] + (int)crcNumber[1]);
                //计算校验位32
                int secondInt = crcNumber[0] * 16 * 16 + crcNumber[1];
                byte sencond = (byte)(secondInt / 10000 + secondInt % 10000 / 1000 + secondInt % 1000 / 100 + secondInt % 100 / 10 + secondInt % 10);


                //写入新的bin文件
                string fileName = SaveFilePath.Text;
                using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(fileName, FileMode.Create)))
                {
                    byte[] total = new byte[] { };
                    switch (type)
                    {
                        case "ARM":
                            if (EncryptType == "已加密")
                            {
                                binChar[3] = 0x33;
                                binChar[4] = 0x32;
                                binChar[30] = first;
                                binChar[31] = sencond;
                                binaryWriter.Write(binChar);
                            }
                            else
                            {
                                total = new byte[fileLen + 32];
                                Array.Copy(crcCheck, 0, total, 0, 30);
                                total[30] = first;
                                total[31] = sencond;
                                Array.Copy(binChar, 0, total, 32, fileLen);
                                binaryWriter.Write(total);
                            }
                            break;
                       
                        case "DSP":
                            total = new byte[fileLen];
                            Array.Copy(crcCheck, 0, total, 0, 30);
                            total[30] = first;
                            total[31] = sencond;
                            Array.Copy(binChar, 32, total, 32, fileLen - 32);
                            binaryWriter.Write(total);
                            break;

                        case "DSP-M":
                        case "DATALOG":
                        case "AFCI":
                        case "METER":
                            if (EncryptType == "已加密")
                            {
                                binChar[30] = first;
                                binChar[31] = sencond;
                                binaryWriter.Write(binChar);
                            }
                            else
                            {

                                total = new byte[fileLen + 32];
                                Array.Copy(crcCheck, 0, total, 0, 30);
                                total[30] = first;
                                total[31] = sencond;
                                Array.Copy(binChar, 0, total, 32, fileLen);
                                binaryWriter.Write(total);
                            }
                            break;
                    }
                }

                String okMessage = "生成成功！校验位为[31]:" + first.ToString("X2") + ",[32]:" + sencond.ToString("X2");
                MessageBox.Show(okMessage, "生成成功");
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private byte[] ToModbus(byte[] byteData, int byteLength)
        {
            byte[] CRC = new byte[2];

            UInt16 wCrc = 0xFFFF;
            for (int i = 0; i < byteLength; i++)
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

            CRC[0] = (byte)((wCrc & 0xFF00) >> 8);//高位在后
            CRC[1] = (byte)(wCrc & 0x00FF);       //低位在前
            return CRC;

        }

        private void CrcDataCombine(byte[] binChar, int fileLen)
        {
            byte[] crcShort = ToModbus(binChar, fileLen);
            byte[] length = BitConverter.GetBytes(fileLen);
            Array.Copy(length, 0, crcCheck, 16, 4);
            crcCheck[20] = crcShort[1];
            crcCheck[21] = crcShort[0];
        }
    }
}
