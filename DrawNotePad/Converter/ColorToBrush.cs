using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace DrawNotePad.Converter
{
    public class ColorToBrush : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = value.ToString();
            Color white = Colors.White;
            if (s == "red")
            {
                white = Colors.Red;
            }
            else if (s == ("blue"))
            {
                white = Colors.Blue;
            }
            else if (s == ("green"))
            {
                white = Colors.Green;
            }

            return new SolidColorBrush(white);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}