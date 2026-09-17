using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Encrypter
{
    public class FontSizeToPaddingConverter : IValueConverter
    {
        public double Factor { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double fontSize)
            {
                return new Thickness(fontSize * Factor);
            }
            return new Thickness(4);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
