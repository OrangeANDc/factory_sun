using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Core.Models;
using Magicodes.ExporterAndImporter.Excel;
using Microsoft.Win32;
using SupportProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunwaysFactoryProgram.StaticSource
{
    public static class ExcelHelper
    {
        public static Task ExportFileInfo<T>(List<T> datas,string filePath) where T : class, new()
        {
            try
            {
                IExporter exporter = new ExcelExporter();

                var result = exporter.Export<T>(filePath, datas);
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return Task.FromException(ex);
            }
            
        }
    }
}
