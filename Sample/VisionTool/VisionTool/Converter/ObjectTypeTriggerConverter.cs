using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace VisionTool.Converter
{
    public class ObjectTypeTriggerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return true;
            try
            {
                HV.V1.Object temp = value as HV.V1.Object;

                switch (temp.Type)
                {
                    case "image":
                        return false;

                    default:
                        return true;
                }

            }
            catch
            {
                return true;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }



    }
}
