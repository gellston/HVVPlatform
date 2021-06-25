using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Model
{
    public class ImageResultObject : ResultObject
    {

        public ImageResultObject()
        {

        }

     
        public override HV.V1.Object Data
        {
            get
            {
                return base.Data;
            }
            set
            {
                try
                {
                    if (base.Data == null)
                        base.Data = value;

                    HV.V1.Object _object = value;

                    if (_object.GetType().Name.Contains("Image"))
                    {
                        HV.V1.Image target = _object as HV.V1.Image;
                        HV.V1.Image source = this.Data as HV.V1.Image;

                        // Image Conversion
                        var width = target.Width;
                        var height = target.Height;
                        var stride = target.Stride;
                        var size = target.Size;


                        PixelFormat format = PixelFormats.Gray8;
                        //System.Windows.Media.PixelFormat bitmapImageFormat = System.Windows.Media.PixelFormats.Gray8;


                        switch (target.PixelType)
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
                                return;
                        }

                        if (this.BitmapImage == null || source.Size != target.Size || source.PixelType != target.PixelType)
                        {
                            BitmapSource bmp = WriteableBitmap.Create(width, height, 96, 96, format, null, target.Ptr(), target.Size, target.Stride);

                            WriteableBitmap writeBmp = new WriteableBitmap(bmp);
                            this.BitmapImage = writeBmp;
                        }
                        else
                        {
                            this.BitmapImage?.Lock();
                            this.BitmapImage?.WritePixels(new System.Windows.Int32Rect(0, 0, target.Width, target.Height), target.Ptr(), target.Size, target.Stride);
                            this.BitmapImage?.AddDirtyRect(new Int32Rect(0, 0, target.Width, target.Height));
                            this.BitmapImage?.Unlock();
                        }
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }

        }


        private WriteableBitmap _BitmapImage = null;
        public WriteableBitmap BitmapImage
        {
            get => _BitmapImage;
            set => Set(ref _BitmapImage, value);
        }
    }
}
