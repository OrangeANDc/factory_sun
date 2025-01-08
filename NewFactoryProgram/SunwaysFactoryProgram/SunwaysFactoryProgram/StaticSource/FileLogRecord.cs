using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SunwaysFactoryProgram.StaticSource
{
    public static class FileLogRecord
    {
        public static string CreateDictionary(string strPath)
        {
            if (Directory.Exists(strPath))
                return strPath;
            try
            {
                Directory.CreateDirectory(strPath);
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message);
            }
            return strPath;
        }

        public static string GetFileName(string dicPath, string meterSN, string type, string result)
        {
            string dictionary = CreateDictionary(dicPath);
            string str = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            return dictionary + "\\" + meterSN + "_" + str + "_" + type + "_" + result + ".log";
        }

        public static string GetFileNameDay(string dicPath, string type)
        {
            string dictionary = CreateDictionary(dicPath);
            string str = DateTime.Now.ToString("yyyy-MM-dd");
            return dictionary + "\\" + type + "_" + str + ".log";
        }
    }
}
