using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;

namespace VisionTool.Helper
{
    public class ImageHelper
    {

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
    }
}
