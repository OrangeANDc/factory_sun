using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace SunwaysFactoryProgram.Converter
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


    public class InverseBooleanConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
             System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException("The target must be a boolean");

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }

    [ValueConversion(typeof(int), typeof(Brush))]
    public class IntToBrushConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string reValue = (string)value;
                if (reValue.Contains("离线"))
                {
                    return new SolidColorBrush(Colors.Black);
                }
                else if (reValue.Contains("正常"))
                {
                    return new SolidColorBrush(Colors.CornflowerBlue);
                }
                else if (reValue.Contains("异常"))
                {
                    return new SolidColorBrush(Colors.Orange);
                }
                else if (reValue.Contains("PASS"))
                {
                    return new SolidColorBrush(Colors.Green);
                }
                else if (reValue.Contains("FAIL"))
                {
                    return new SolidColorBrush(Colors.Red);
                }

            }
           
            return new SolidColorBrush(Colors.Black);
          

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value.ToString();
            return value;
        }


    }
}
