using System;
using System.Collections.Generic;
using System.Drawing;
//using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Converter
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

                var width = hvImage.Width;
                var height = hvImage.Height;
                var stride = hvImage.Stride;
                var size = hvImage.Size;


                PixelFormat format = PixelFormats.Gray8;
                //System.Windows.Media.PixelFormat bitmapImageFormat = System.Windows.Media.PixelFormats.Gray8;


                switch (hvImage.PixelType)
                {
                    case HV.V1.ImageDataType.u8c1_image:
                        format = PixelFormats.Gray8;
                        //bitmapImageFormat = System.Windows.Media.PixelFormats.Gray8;
                        break;
                    case HV.V1.ImageDataType.u16c1_image:
                        format = PixelFormats.Gray16;
                        //bitmapImageFormat = System.Windows.Media.PixelFormats.Gray16;
                        break;
                    case HV.V1.ImageDataType.u32c1_image:
                        format = PixelFormats.Gray32Float;
                        // bitmapImageFormat = System.Windows.Media.PixelFormats.Gray32Float;
                        break;

                    //3Channnel
                    case HV.V1.ImageDataType.u8c3_image:
                        format = PixelFormats.Bgr24;
                        // bitmapImageFormat = System.Windows.Media.PixelFormats.Rgb24;
                        break;
                    case HV.V1.ImageDataType.u16c3_image:
                        format = PixelFormats.Rgb48;
                        //bitmapImageFormat = System.Windows.Media.PixelFormats.Rgb48;
                        break;
                    case HV.V1.ImageDataType.u32c3_image:
                        format = PixelFormats.Rgba128Float;
                        //bitmapImageFormat = System.Windows.Media.PixelFormats.Rgb128Float;
                        break;
                    default:
                        return null;
                }
                
                BitmapSource bmp = WriteableBitmap.Create(width, height, 96, 96, format, null, hvImage.Ptr(),hvImage.Size, hvImage.Stride);
               
                WriteableBitmap writeBmp = new WriteableBitmap(bmp);
                
                //bmp.Lock();
                //try
                //{
                //    System.Windows.Int32Rect rect;
                //    rect.X = 0;
                //    rect.Y = 0;
                //    rect.Width = width;
                //    rect.Height = height;

                //    int totalSize = stride * height;
                //    IntPtr backbuffer = bmp.BackBuffer;

                //    bmp.WritePixels(rect, hvImage.Ptr(), size, stride);

                //}
                //catch (Exception e)
                //{

                //    System.Diagnostics.Debug.WriteLine(e.Message);
                //}
                //finally
                //{
                //    bmp.Unlock();
                //}

                return writeBmp;

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                Logger.Logger.Write(Logger.TYPE.UI, e.Message);
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }



    }
}
