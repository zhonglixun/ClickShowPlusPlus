using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ClickShow.Utility
{
    /// <summary>
    /// "#AARRGGBB" 字符串转画刷（色块按钮绑定用，单向）。
    /// </summary>
    public class StringToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value is string s && !string.IsNullOrEmpty(s))
                {
                    var brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(s));
                    brush.Freeze();
                    return brush;
                }
            }
            catch
            {
            }
            return Brushes.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
