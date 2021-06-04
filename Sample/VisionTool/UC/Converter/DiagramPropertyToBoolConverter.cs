using Model.DiagramProperty;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Converter
{
    public class DiagramPropertyToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            try
            {
                var property = value as BaseDiagramProperty;
                if (property == null) return false;
                var propertyTypeString = property.GetType().Name;

                switch (propertyTypeString)
                {
                    case "RectROIDiagramProperty":
                        return true;
                        break;
                    case "LineFitROIDiagramProperty":
                        return true;
                        break;
                    case "CircleFitROIDiagramProperty":
                        return true;
                        break;
                    case "CircleROIDiagramProperty":
                        return true;
                        break;

                }

                return false;
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
                return false;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("throw convert back");
        }



    }
}
