using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TasksAndJobLists
{
    public class WidthToParentConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            double width = (double)value;
            double adjustment = Convert.ToDouble(parameter);

            return width + adjustment;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            double width = (double)value;
            double adjustment = Convert.ToDouble(parameter);

            return width - adjustment;
        }
    }
}
