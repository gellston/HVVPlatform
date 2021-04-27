using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;
using Model;

namespace Converter
{
    public class FunctionDiagramTypeCheckConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var check = value as Function;
            if (check == null) return false;
            else return true;
    
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("throw convert back");
        }



    }
}
