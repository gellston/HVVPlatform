using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace WPFHVVPlatform.Converter
{



    public class HVObjectBitmapImageConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            try
            {
                if (value == null) return null;
                var hvObject = value as HV.V1.Object;
                if (hvObject.Type.Contains("image") != true) return null;
                var hvImage = new HV.V1.Image(hvObject);

                PixelFormat format = PixelFormat.Format8bppIndexed;
                System.Windows.Media.PixelFormat bitmapImageFormat = System.Windows.Media.PixelFormats.Gray8;


                switch (hvImage.PixelType())
                {
                    case HV.V1.ImageDataType.u8Image:
                        format = PixelFormat.Format8bppIndexed;
                        bitmapImageFormat = System.Windows.Media.PixelFormats.Gray8;
                        break;
                    case HV.V1.ImageDataType.u16Image:
                        format = PixelFormat.Format16bppGrayScale;
                        bitmapImageFormat = System.Windows.Media.PixelFormats.Gray16;
                        break;
                    case HV.V1.ImageDataType.u32Image:
                        format = PixelFormat.Format32bppRgb;
                        bitmapImageFormat = System.Windows.Media.PixelFormats.Gray32Float;
                        break;
                    default:
                        return null;
                }

                WriteableBitmap bmp = new WriteableBitmap(hvImage.Width(), hvImage.Height(), 96, 96, bitmapImageFormat, null);

                bmp.Lock();
                try
                {
                    System.Windows.Int32Rect rect;
                    rect.X = 0;
                    rect.Y = 0;
                    rect.Width = hvImage.Width();
                    rect.Height = hvImage.Height();

                    int totalSize = hvImage.Stride() * hvImage.Height();
                    IntPtr backbuffer = bmp.BackBuffer;

                    //CopyMemory(bmp.BackBuffer, hvImage.Ptr(),hvImage.Size());
                    bmp.WritePixels(rect, hvImage.Ptr(), hvImage.Size(), hvImage.Stride());
                 
                }
                catch(Exception e)
                {

                }
                finally
                {
                    bmp.Unlock();
                }

                return bmp;

            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }



    }
}
