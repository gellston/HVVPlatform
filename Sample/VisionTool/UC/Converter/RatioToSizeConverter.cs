using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Converter
{
    public class RatioToSizeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            double ratio = (double)values[0];
            double radius = (double)values[1];


            double finalRadius = ratio * radius;

            if (finalRadius < 0)
                finalRadius = 0;


            return finalRadius * 2;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
