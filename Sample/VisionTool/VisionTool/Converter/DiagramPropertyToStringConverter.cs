using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;
using VisionTool.Model;
using VisionTool.Model.DiagramProperty;

namespace VisionTool.Converter
{
    public class DiagramPropertyToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            try
            {
                var property = value as BaseDiagramProperty;
                if (property == null) return "Invalid Value";
                return property.GetType().ToString();
            }
            catch (Exception e)
            {
                return "Invalid Value";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("throw convert back");
        }



    }
}
