using SafetyTestTool.StaticSource;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace SafetyTestTool.Converter
{
    public class ObjectConvert : IValueConverter
    {
        private string realWord = "";
        private char replaceChar = '*';

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter != null)
            {
                string temp = (string)parameter;
                if (!string.IsNullOrEmpty(temp))
                {
                    replaceChar = temp.First();
                }
            }
            if (value is not null)
                realWord = (string)value;

            string replaceWord = "";
            for (int index = 0; index < realWord.Length; index++)
            {
                replaceWord += replaceChar;
            }
            return replaceWord;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string backValue = "";
            if (value != null)
            {
                string strValue = (string)value;
                for (int index = 0; index < strValue.Length; ++index)
                {
                    if (strValue.ElementAt(index) == replaceChar)
                    {
                        backValue += realWord.ElementAt(index);
                    }
                    else
                    {
                        backValue += strValue.ElementAt(index);
                    }
                }
            }
            return backValue;
        }

    }

    
    [ValueConversion(typeof(ushort), typeof(Brush))]
    public class HeadBrushConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)     
        {
            if (value == null )
            {         
                return new SolidColorBrush(Colors.Orange);                       
            }

            return new SolidColorBrush(Colors.Transparent); 


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value.ToString();
            return value;
        }
    }


    [ValueConversion(typeof(ushort), typeof(Visibility))]
    public class ButtonVisableConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Collapsed;
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value.ToString();
            return value;
        }
    }

    [ValueConversion(typeof(ushort), typeof(Visibility))]
    public class ShowComboBoxConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(bool)value)
            {
                return Visibility.Collapsed;
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value.ToString();
            return value;
        }
    }

    [ValueConversion(typeof(ushort), typeof(Visibility))]
    public class ShowTextBoxConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return Visibility.Collapsed;
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value.ToString();
            return value;
        }
    }

    public class SwitchValueConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == null)
                {
                    return -1;
                }
                else if (value.ToString() == "1")
                {
                    return 1;
                }
                else if (value.ToString() == "0")
                {
                    return 0;
                }
                else
                    return -1;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if ((int)value == 0)
                {
                    return "0";
                }
                else if ((int)value == 1)
                {
                    return "1";                       
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return null;
            }

        }
    }

    public class TextChangeConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "";
            }
            else
            {
                return value.ToString();
            }                      
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "";
            }
            else
            {
                return value.ToString();
            }

        }
    }
}
