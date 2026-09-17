using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Encrypter
{
    public class TextBoxConverter : IValueConverter
    {
        public double Divisor { get; set; }
        public double MinFontSize { get; set; }
        public double MaxFontSize { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Diagnostics.Debug.WriteLine($"CONVERT: value={value}");

            if (value is double width && width > 0)
            {
                var size = width / Divisor;
                if (size < MinFontSize) size = MinFontSize;
                if (size > MaxFontSize) size = MaxFontSize;
                return size;
            }
            return MinFontSize;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
