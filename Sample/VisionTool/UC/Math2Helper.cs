using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace UClib
{
    public static class Math2DHelper
    {
        public static Vector RotateVector2d(double x0, double y0, double rad)
        {
            Vector result = new Vector();
            result.X = x0 * Math.Cos(rad) - y0 * Math.Sin(rad);
            result.Y = x0 * Math.Sin(rad) + y0 * Math.Cos(rad);
            return result;
        }

        public static double D2R(double degree)
        {
            return (degree % 360) * Math.PI / 180;
        }
    }
}
