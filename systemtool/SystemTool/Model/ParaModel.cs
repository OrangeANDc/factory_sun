using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace SystemTool.Model
{
    public class ParaModel : ParaInfModel
    {
        public string DataValue { get; set; }
        public string CommandInf { get; set; }
    }

    public class ParaInfModel
    {
        public string DataName { get; set; }
        public string Remark { get; set; }
        public ushort DataAddress { get; set; }
        public ushort DataLength { get; set; } = 1;
        public bool IsSigned { get; set; }
        public string DataUnit { get; set; }
        public string DataGain { get; set; } = "1";
    }


    public class DataModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;
        private string _dataValue;
        public string DataValue
        {
            get => _dataValue;
            set
            {
                _dataValue = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Value"));
                }
            }
        }
        public string DataName { get; set; }
        public string DataUnit { get; set; }

        public string Remark { get; set; }
        public ushort DataAddress { get; set; }
        public ushort DataLength { get; set; } = 1;
        public bool IsSigned { get; set; }
        public string DataGain { get; set; } = "1";
    }
}
