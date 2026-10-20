using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ClickShow.Utility
{
    /// <summary>
    /// "#AARRGGBB" 字符串与 Color 的双向转换（取色器绑定用）。
    /// </summary>
    public class StringToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value is string s && !string.IsNullOrEmpty(s))
                {
                    return (Color)ColorConverter.ConvertFromString(s);
                }
            }
            catch
            {
            }
            return Colors.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Color color ? color.ToArgb() : "#FF808080";
        }
    }
}
