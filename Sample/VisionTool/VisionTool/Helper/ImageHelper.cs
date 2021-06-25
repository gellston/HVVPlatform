using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Media.Imaging;

namespace VisionTool.Helper
{
    public class ImageHelper
    {

        [DllImport("msvcrt.dll", SetLastError = false)]
        public static extern IntPtr memcpy(IntPtr dest, IntPtr src, int count);

        public static WriteableBitmap LoadImageFromPath(string path, int width, int height)
        {

            try
            {
                Mat image = new Mat(path);
                image = image.Resize(new Size(width, height));
                var writeableBitmap = OpenCvSharp.WpfExtensions.WriteableBitmapConverter.ToWriteableBitmap(image);
                return writeableBitmap;

            }catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                throw e;
            }

        }

        public static WriteableBitmap LoadImageFromPath(string path)
        {

            try
            {
                using (Mat image = new Mat(path))
                {
                    var writeableBitmap = OpenCvSharp.WpfExtensions.WriteableBitmapConverter.ToWriteableBitmap(image);
                    return writeableBitmap;
                }


            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                throw e;
            }

        }
        
        public static Mat LoadMatFromPath(string path, bool isGray)
        {
            try
            {
                ImreadModes mode = isGray == true ? ImreadModes.Grayscale : ImreadModes.Color;
                Mat image = new Mat(path, mode);
                return image;
                


            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public static HV.V1.Image ConvertFromByte(string name, byte[] _byte, int width, int height, int channel, bool isGray)
        {
            try
            {
                HV.V1.ImageDataType dataType = HV.V1.ImageDataType.u8c1_image;

                switch (channel)
                {
                    case 1:
                        dataType = HV.V1.ImageDataType.u8c1_image;
                        break;
                    case 3:
                        dataType = HV.V1.ImageDataType.u8c1_image;
                        break;
                }

                HV.V1.Image image = new HV.V1.Image(name, (uint)width, (uint)height, (uint)dataType, 1);

                if(isGray == true && image.PixelType == HV.V1.ImageDataType.u8c3_image)
                {
                    using (var source = new Mat(image.Height, image.Width, MatType.CV_8UC3))
                    {
                        memcpy(source.Data, image.Ptr(), image.Size);

                        var target = source.CvtColor(ColorConversionCodes.BGR2GRAY);

                        HV.V1.Image targetHvImage = new HV.V1.Image(name, (uint)width, (uint)height, (uint)HV.V1.ImageDataType.u8c1_image, 1);
                        memcpy(targetHvImage.Ptr(), target.Data, targetHvImage.Size);
                        target.Dispose();
                        return targetHvImage;
                    };
                }
                unsafe
                {
                    fixed (byte* p = _byte)
                    {
                        IntPtr ptr = (IntPtr)p;
                        memcpy(image.Ptr(), ptr, image.Size);
                    }
                }
               

                return image;
            }
            catch(Exception e)
            {
                Logger.Logger.Write(Logger.TYPE.UI, e.Message);
                return null;
            }
        }


        public static HV.V1.Image LoadHVImageFromPath(string path, bool isGray)
        {
            try
            {
                var mat = Helper.ImageHelper.LoadMatFromPath(path, isGray);

                var type = mat.Type();
                HV.V1.ImageDataType imageDataType = HV.V1.ImageDataType.u8c1_image;

                if (type == OpenCvSharp.MatType.CV_8UC1)
                {
                    imageDataType = HV.V1.ImageDataType.u8c1_image;
                }
                else if (type == OpenCvSharp.MatType.CV_16UC1)
                {
                    imageDataType = HV.V1.ImageDataType.u16c1_image;
                }
                else if (type == OpenCvSharp.MatType.CV_32FC1)
                {
                    imageDataType = HV.V1.ImageDataType.u32c1_image;
                }
                else if (type == OpenCvSharp.MatType.CV_8UC3)
                {
                    imageDataType = HV.V1.ImageDataType.u8c3_image;
                }
                else
                {
                    throw new Exception("Image format is not supported");
                }

                HV.V1.Image image = new HV.V1.Image("SourceImage", (uint)mat.Width, (uint)mat.Height, (uint)imageDataType, 1);
                int size = mat.Width * mat.Height * mat.Channels();
                if(image.Ptr() == null)
                {
                    throw new Exception("Image format is not supported");
                }

                Helper.ImageHelper.memcpy(image.Ptr(), mat.Data, size);
                return image;

            }
            catch(Exception e)
            {
                throw e;
            }

        }

        public static WriteableBitmap LoadImageFromPath(string path, bool isGray)
        {

            try
            {
                ImreadModes mode = isGray == true ? ImreadModes.Grayscale : ImreadModes.Color;
                using (Mat image = new Mat(path, mode))
                {
                    var writeableBitmap = OpenCvSharp.WpfExtensions.WriteableBitmapConverter.ToWriteableBitmap(image);
                    return writeableBitmap;
                }


            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                throw e;
            }

        }
    }
}
