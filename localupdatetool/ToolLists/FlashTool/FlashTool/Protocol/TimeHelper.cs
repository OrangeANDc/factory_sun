using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FlashTool.Protocol
{
    public static class TimeHelper
    {
        public static void TimeCheck()
        {
            string filePath = "starttime.txt"; // 文件路径用于存储启动时间
            DateTime startTime;

            // 检查文件是否存在，并读取启动时间
            if (File.Exists(filePath))
            {
                startTime = DateTime.Parse(File.ReadAllText(filePath));
            }
            else
            {
                startTime = DateTime.Now; // 如果文件不存在，记录当前时间为启动时间
                File.WriteAllText(filePath, startTime.ToString()); // 写入文件
            }

           

            // 检查是否已运行超过24小时，并更新文件中的时间（如果需要）
            if ((DateTime.Now - startTime) > TimeSpan.FromDays(1))
            {
                MessageBox.Show("程序运行时间已超过24小时，即将退出。");
                Environment.Exit(0); // 退出程序
            }
          
        }
    }
}
