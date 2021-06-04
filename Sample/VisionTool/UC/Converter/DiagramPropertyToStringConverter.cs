using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;
using Model;
using Model.DiagramProperty;

namespace Converter
{
    public class DiagramPropertyToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            try
            {
                var property = value as BaseDiagramProperty;
                if (property == null) return "Invalid Value";
                var propertyTypeString = property.GetType().Name;
                return propertyTypeString;
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
                return "Invalid Value";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("throw convert back");
        }



    }
}
