using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Thesis.Data.Model
{
    [ValueConversion(typeof(double), typeof(string))]
    class NumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
       System.Globalization.CultureInfo culture)
        {
            // Возвращаем строку в формате 123.456.789 руб.
            return ((int)value).ToString("# ### ### ### ###", culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            int result;
            if (int.TryParse(value.ToString(), System.Globalization.NumberStyles.Any,
                         culture, out result))
            {
                return result;
            }
            else if (int.TryParse(value.ToString().Replace("",""), System.Globalization.NumberStyles.Any,
                         culture, out result))
            {
                return result;
            }
            return value;
        }
    }
}
