using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;
using Model;

namespace Converter
{
    public class DiagramOjbectToFunctionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            try
            {
                var function = value as Function;
                return function;
            }
            catch (Exception e)
            {

                System.Diagnostics.Trace.WriteLine(e.Message);
                return null;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("throw convert back");
        }



    }
}