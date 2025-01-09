using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SystemTool.StaticSource
{
    public static class FileHelper
    {
        public static string? ReadJsonFile(string filePath)
        {
            try
            {
                string json = string.Empty;
                using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Write))
                {
                    using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                    {
                        json = sr.ReadToEnd().ToString();
                    }
                }
                return json;
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
                Log.Error(ex.Message);
                return null;
            }
        }

        public static bool WriteJsonFile(string path, string json)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Write))
                {
                    fs.SetLength(0);
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                    {
                        sw.Write(json);
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.Error(ex.Message);
                return false;   
            }

        }
    }
}
