using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Device
{
    public class GigECamera : Device
    {

        private readonly SPIDER.Function OpenProc;
        private readonly SPIDER.Function CloseProc;
        private readonly SPIDER.Function AcquisitionStartProc;
        private readonly SPIDER.Function AcquisitionStopProc; 
        private readonly SPIDER.Function GrabProc;
        private readonly SPIDER.Function GetImageInfoProc;
        private readonly SPIDER.Function ConfigureSoftwareTriggerProc;
        private  SPIDER.Variable<byte> SharedImageBuffer;

        private object lockObject = new object();

        public GigECamera()
        {
            try
            {
                this.OpenProc = new SPIDER.Function(this.Uid + "_Open");
                this.OpenProc.Delay(5000).Args()
                               .Arg<string>("ip", 256)
                               .Returns()
                               .Ret<bool>("return")
                               .Complete();

                this.CloseProc = new SPIDER.Function(this.Uid + "_Close");
                this.CloseProc.Returns()
                                .Ret<bool>("return")
                                .Complete();



                this.AcquisitionStartProc = new SPIDER.Function(this.Uid + "_AcquisitionStart");
                this.AcquisitionStartProc.Returns()
                                     .Ret<bool>("return")
                                     .Complete();


                this.AcquisitionStopProc = new SPIDER.Function(this.Uid + "_AcquisitionStop");
                this.AcquisitionStopProc.Returns()
                                    .Ret<bool>("return")
                                    .Complete();

                this.GrabProc = new SPIDER.Function(this.Uid + "_Grab");
                this.GrabProc.Delay(5000)
                          .Returns()
                         .Ret<bool>("return")
                         .Complete();


                this.ConfigureSoftwareTriggerProc = new SPIDER.Function(this.Uid + "_ConfigureSoftwareTrigger");
                this.ConfigureSoftwareTriggerProc.Returns()
                                                 .Ret<bool>("return")
                                                 .Complete();


                this.GetImageInfoProc = new SPIDER.Function(this.Uid + "_GetImageInfo");
                this.GetImageInfoProc.Delay(1000)
                                     .Returns()
                                     .Ret<bool>("return")
                                     .Ret<uint>("width")
                                     .Ret<uint>("height")
                                     .Ret<uint>("channel")
                                     .Complete();




                
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                this.HasError = true;
                this.ErrorText = e.Message;
            }


        }


        public override object Clone()
        {


            GigECamera newCopy = new GigECamera()
            {
                IsAlive = this.IsAlive,
                HasError = this.HasError,
                ErrorText = this.ErrorText,
                Pid = this.Pid,
                DeviceName = this.DeviceName,
                Name = this.Name,
                IP = this.IP,

            };

            return newCopy;
        }



        private string _IP = "";
        public string IP
        {
            get => _IP;
            set => Set(ref _IP, value);
        }


        private int _ImageWidth = 0;
        public int ImageWidth
        {
            get => _ImageWidth;
            set => Set(ref _ImageWidth, value);
        }


        private int _ImageHeight = 0;
        public int ImageHeight
        {
            get => _ImageHeight;
            set => Set(ref _ImageHeight, value);
        }

        private WriteableBitmap _ImageBuffer = null;
        public WriteableBitmap ImageBuffer
        {
            get => _ImageBuffer;
            set => Set(ref _ImageBuffer, value);
        }



        bool Open(string ip)
        {
            lock (lockObject)
            {
                try
                {
                    this.OpenProc.Args()
                                   .Push<string>("ip", this.IP);

                    this.OpenProc.Call(); 

                    bool isOpen = false;
                    this.OpenProc.Returns()
                                   .Get<bool>("return", out isOpen);
                    System.Diagnostics.Debug.WriteLine("received data = " + isOpen);
                    return isOpen;

                }
                catch (Exception e)
                {
                    this.HasError = true;
                    this.ErrorText = e.Message;
                    return false;
                }
            }
        }


        bool Close()
        {
            lock (lockObject)
            {
                try
                {


                    this.CloseProc.Call();

                    bool isClosed = false;
                    this.CloseProc.Returns()
                                   .Get<bool>("return", out isClosed);

                    System.Diagnostics.Debug.WriteLine("received data = " + isClosed);
                    return isClosed;
                }
                catch (Exception e)
                {
                    this.HasError = true;
                    this.ErrorText = e.Message;
                    return false;
                }
            }
        }

        bool AcquisitionStart()
        {
            lock (lockObject)
            {
                try
                {

                    this.AcquisitionStartProc.Call();

                    bool isAcquisitionStart = false;
                    this.AcquisitionStartProc.Returns()
                                   .Get<bool>("return", out isAcquisitionStart);

                    System.Diagnostics.Debug.WriteLine("received data = " + isAcquisitionStart);
                    return isAcquisitionStart;
                }
                catch (Exception e)
                {
                    this.HasError = true;
                    this.ErrorText = e.Message;
                    return false;
                }
            }
        }


        bool AcquisitionStop()
        {
            lock (lockObject)
            {
                try
                {

                    this.AcquisitionStopProc.Call();

                    bool isAcquisitionStop = false;
                    this.AcquisitionStopProc.Returns()
                                        .Get<bool>("return", out isAcquisitionStop);

                    System.Diagnostics.Debug.WriteLine("received data = " + isAcquisitionStop);
                    return isAcquisitionStop;
                }
                catch (Exception e)
                {
                    this.HasError = true;
                    this.ErrorText = e.Message;
                    return false;
                }

            }
        }

        bool ConfigureSoftwareTrigger()
        {
            lock (lockObject)
            {
                try
                {

                    this.ConfigureSoftwareTriggerProc.Call();

                    bool isConfiguredSoftwareTrigger = false;
                    this.ConfigureSoftwareTriggerProc.Returns()
                                        .Get<bool>("return", out isConfiguredSoftwareTrigger);

                    System.Diagnostics.Debug.WriteLine("received data = " + isConfiguredSoftwareTrigger);
                    return isConfiguredSoftwareTrigger;
                }
                catch (Exception e)
                {
                    this.HasError = true;
                    this.ErrorText = e.Message;
                    return false;
                }

            }
        }

        bool Grab()
        {
            lock (lockObject)
            {

                try
                {

                    if (this.SharedImageBuffer == null) return false;
                    if (this.ImageWidth <= 0 || this.ImageHeight <= 0) return false;



                    this.GrabProc.Call();

                    bool checkGrab = false;



                    this.GrabProc.Returns()
                                 .Get<bool>("return", out checkGrab);
                    System.Diagnostics.Debug.WriteLine("received data = " + checkGrab);
                    if (checkGrab == false) return false;




                    if (this.ImageBuffer == null || this.ImageBuffer.Width != this.ImageWidth || this.ImageBuffer.Height != this.ImageBuffer.Height)
                    {
                        this.ImageBuffer = new WriteableBitmap(this.ImageWidth, this.ImageHeight, 96, 96, PixelFormats.Gray8, null);
                    }



                    uint size = (uint)(this.ImageWidth * this.ImageHeight);
                    this.ImageBuffer.Lock();
                    this.SharedImageBuffer.Receive(this.ImageBuffer.BackBuffer, size);
                    this.ImageBuffer.Unlock();


                    //if (TrackingImagePresenter == null || TrackingImagePresenter.Width != width || TrackingImagePresenter.Height != height)
                    //{
                    //    TrackingImagePresenter = new WriteableBitmap(width, height, 96, 96, PixelFormats.Gray8, null);
                    //}
                    //TrackingImagePresenter.WritePixels(new System.Windows.Int32Rect(0, 0, width, height), hvImage.Ptr(), size, stride);



                    return checkGrab;
                }
                catch (Exception e)
                {
                    this.HasError = true;
                    this.ErrorText = e.Message;
                    return false;
                }
            }
        }


        bool GetImageInfo()
        {
            lock (lockObject)
            {
                try
                {
                    uint width = 0;
                    uint height = 0;
                    uint channel = 0;

                    this.GetImageInfoProc.Call();

                    bool getImageInfoCheck = false;
                    this.GetImageInfoProc.Returns()
                                        .Get<bool>("return", out getImageInfoCheck)
                                        .Get<uint>("width", out width)
                                        .Get<uint>("height", out height)
                                        .Get<uint>("channel", out channel);

                    this.ImageWidth = (int)width;
                    this.ImageHeight = (int)height;

                    if (getImageInfoCheck == true)
                    {
                        this.SharedImageBuffer ??= new SPIDER.Variable<byte>(this.Uid + "_ImageBuffer", SPIDER.SPIDER_MODE.CREATE, SPIDER.SPIDER_ACCESS.READ_WRITE);
                    }

                    System.Diagnostics.Debug.WriteLine("received data = " + getImageInfoCheck);
                    return getImageInfoCheck;

                }
                catch(Exception e)
                {
                    this.HasError = true;
                    this.ErrorText = e.Message;
                    return false;
                }
            }
        }


        [Newtonsoft.Json.JsonIgnore]
        public ICommand OpenCommand
        {
            get => new RelayCommand(() =>
            {

                this.Open(this.IP);

            });
        }


        [Newtonsoft.Json.JsonIgnore]
        public ICommand CloseCommand
        {
            get => new RelayCommand(() =>
            {
                this.Close();
                
            });
        }

        [Newtonsoft.Json.JsonIgnore]
        public ICommand AcquisitionStartCommand
        {
            get => new RelayCommand(() =>
            {
                this.AcquisitionStart();
            });
        }

        [Newtonsoft.Json.JsonIgnore]
        public ICommand AcquisitionStopCommand
        {
            get => new RelayCommand(() =>
            {
                this.AcquisitionStop();

            });
        }

        [Newtonsoft.Json.JsonIgnore]
        public ICommand GetImageInfoCommand
        {
            get => new RelayCommand(() =>
            {
                this.GetImageInfo();
            });
        }



        [Newtonsoft.Json.JsonIgnore]
        public ICommand GrabCommand
        {
            get => new RelayCommand(() =>
            {
                this.Grab();
            });
        }


        [Newtonsoft.Json.JsonIgnore]
        public ICommand ConfigureSoftwareTriggerCommand
        {
            get => new RelayCommand(() =>
            {
                this.ConfigureSoftwareTrigger();
            });
        }

    }
}
