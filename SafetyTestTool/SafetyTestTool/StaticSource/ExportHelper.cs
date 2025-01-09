using Microsoft.Win32;
using SafetyTestTool.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SafetyTestTool.StaticSource
{
    public static class ExportHelper
    {
        public static bool ExportPdf(string filePath,bool isChinese)
        {
            try 
            {
                PDFOperation pdfOperation = new PDFOperation();
                pdfOperation.Open((Stream)new FileStream(filePath, FileMode.Create));
                string path = "C:\\Windows\\Fonts\\Arial.TTF";
                if (isChinese)
                    path = "C:\\Windows\\Fonts\\SIMHEI.TTF";
                pdfOperation.SetFontName(path);
                pdfOperation.SetBaseFont(path);
                string title = "";
                return true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.Error(ex.Message);
                return false;
            }

           
        }
    }
}
