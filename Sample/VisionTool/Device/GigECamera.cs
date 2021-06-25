using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

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
        ManualResetEventSlim liveTrigger = new ManualResetEventSlim(false);
        private readonly Thread liveTask;


        private bool IsLoop = true;


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
                this.GrabProc.Delay(1000)
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



                this.liveTask = new Thread(() =>
                {
                    var startTime = DateTime.Now;
                    int currentFps = 0;

                    while (this.IsLoop == true)
                    {
                        this.liveTrigger.Wait();
                        if (this.IsLiveStart == false || this.IsLoop == false) break;

                        if (this.Grab() == false)
                        {
                            this.ErrorText = "Grab failed";
                            this.HasError = true;
                            Logger.Logger.Write(Logger.TYPE.DEVICE, this.ErrorText);
                            continue;
                        }


                        var endTime = DateTime.Now;
                        var measureTaktTime = (endTime - startTime).TotalMilliseconds;
                        currentFps++;

                        if(measureTaktTime > 1000)
                        {
                            startTime = DateTime.Now;
                            this.FPS = currentFps;
                            currentFps = 0;
                        }

                        this.DisplayImage();
                        //Thread.Sleep(16);
                    }

                    try
                    {
                        this.SharedImageBuffer?.Dispose();
                        this.GrabProc?.Dispose();
                        this.ImageBuffer = null;
                        
                    }
                    catch(Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        Logger.Logger.Write(Logger.TYPE.DEVICE, e.Message);
                    }
                    
                });
                this.liveTask.Priority = ThreadPriority.Normal;
                this.liveTask.Start();
                
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                this.HasError = true;
                this.ErrorText = e.Message;
                Logger.Logger.Write(Logger.TYPE.DEVICE, e.Message);
            }


        }
        private void DisplayImage()
        {
            try
            {
                if (this.IsLoop == false) return;
                //int size = (int)(this.ImageWidth * this.ImageHeight);
                unsafe
                {
                    fixed (byte* p = this.ImageByteBuffer)
                    {
                        IntPtr ptr = (IntPtr)p;
                        if (this.IsLoop == false) return;
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            try
                            {
                                this.ImageBuffer?.Lock();
                                this.ImageBuffer?.WritePixels(new System.Windows.Int32Rect(0, 0, this.ImageWidth, this.ImageHeight), ptr, this.ImageSize, this.ImageStride);
                                //this.SharedImageBuffer?.Receive(this.ImageBuffer.BackBuffer, size);
                                this.ImageBuffer?.AddDirtyRect(new Int32Rect(0, 0, this.ImageWidth, this.ImageHeight));
                                this.ImageBuffer?.Unlock();
                                this.OnPropertyChanged(nameof(this.ImageBuffer));
                            }
                            catch(Exception e)
                            {
                                System.Diagnostics.Debug.WriteLine(e.Message);
                                Logger.Logger.Write(Logger.TYPE.DEVICE, e.Message);
                            }

                        }));
                    }
                }


            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                Logger.Logger.Write(Logger.TYPE.DEVICE, e.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            this.IsLiveStart = true;
            this.IsLoop = false;
            this.liveTrigger.Set();
            //if(this.liveTask?.IsAlive == true)
            //    this.liveTask?.Join();
           
            if (disposing)
            {

                this.OpenProc?.Dispose();
                this.CloseProc?.Dispose();
                this.AcquisitionStartProc?.Dispose();
                this.AcquisitionStopProc?.Dispose();
                this.GetImageInfoProc?.Dispose();
                this.ConfigureSoftwareTriggerProc?.Dispose();
          
             
                
                
            }
            base.Dispose(disposing);
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


        public override bool DefaultSetup()
        {

            try
            {
                this.CheckAlive();
                if (this.Open(this.IP) == false) return false;
                if (this.ConfigureSoftwareTrigger() == false) return false;
                if (this.GetImageInfo() == false) return false;
                if (this.AcquisitionStart() == false) return false;


            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                this.HasError = true;
                this.ErrorText = "Default setup failed";
                Logger.Logger.Write(Logger.TYPE.DEVICE, this.ErrorText);
            }



            return true;
        }



        private string _IP = "";
        public string IP
        {
            get => _IP;
            set => Set(ref _IP, value);
        }

        private int _FPS = 0;
        public int FPS
        {
            get => _FPS;
            set => Set(ref _FPS, value);
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

        private int _ImageChannel = 0;
        public int ImageChannel
        {
            get => _ImageChannel;
            set => Set(ref _ImageChannel, value);
        }

        private int _ImageSize = 0;
        public int ImageSize
        {
            get => _ImageSize;
            set => Set(ref _ImageSize, value);
        }

        private int _ImageStride = 0;
        public int ImageStride
        {
            get => _ImageStride;
            set => Set(ref _ImageStride, value);
        }

        private WriteableBitmap _ImageBuffer = null;
        public WriteableBitmap ImageBuffer
        {
            get => _ImageBuffer;
            set => Set(ref _ImageBuffer, value);
        }


 
        public byte[] ImageByteBuffer { get; set; }



        private bool _IsLiveStart = false;
        public bool IsLiveStart
        {
            get => _IsLiveStart;
            set => Set(ref _IsLiveStart, value);
        }

        public bool LiveStart()
        {
            if (this.IsLiveStart == true) return false;
            this.IsLiveStart = true;
            this.liveTrigger.Set();
            

            return true;
        }

        public bool LiveStop()
        {

          
            this.liveTrigger.Reset();
            this.IsLiveStart = false;



            return false;
        }


        public bool Open(string ip)
        {
            this.CheckAlive();
            if (this.IsLiveStart == true) return false;
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
                    if(isOpen == false)
                    {
                        this.HasError = true;
                        this.ErrorText = "Open failed";
                    }
                    return isOpen;

                }
                catch (Exception e)
                {
                    this.HasError = true;
                    this.ErrorText = e.Message;
                    Logger.Logger.Write(Logger.TYPE.DEVICE, this.ErrorText);
                    return false;
                }
            }
        }


        public bool Close()
        {
            this.CheckAlive();
            if (this.IsLiveStart == true) return false;
            lock (lockObject)
            {
                try
                {


                    this.CloseProc.Call();

                    bool isClosed = false;
                    this.CloseProc.Returns()
                                   .Get<bool>("return", out isClosed);

                    System.Diagnostics.Debug.WriteLine("received data = " + isClosed);
                    if(isClosed == false)
                    {
                        this.HasError = true;
                        this.ErrorText = "Close failed";
                    }
                    return isClosed;
                }
                catch (Exception e)
                {
                    this.HasError = true;
                    this.ErrorText = e.Message;
                    Logger.Logger.Write(Logger.TYPE.DEVICE, this.ErrorText);
                    return false;
                }
            }
        }

        public bool AcquisitionStart()
        {
            this.CheckAlive();
            if (this.IsLiveStart == true) return false;
            lock (lockObject)
            {
                try
                {

                    this.AcquisitionStartProc.Call();

                    bool isAcquisitionStart = false;
                    this.AcquisitionStartProc.Returns()
                                   .Get<bool>("return", out isAcquisitionStart);

                    System.Diagnostics.Debug.WriteLine("received data = " + isAcquisitionStart);
                    if(isAcquisitionStart == false)
                    {
                        this.HasError = true;
                        this.ErrorText = "Acquisition start failed";
                        Logger.Logger.Write(Logger.TYPE.DEVICE, this.ErrorText);
                    }
                    return isAcquisitionStart;
                }
                catch (Exception e)
                {
                    this.HasError = true;
                    this.ErrorText = e.Message;
                    Logger.Logger.Write(Logger.TYPE.DEVICE, this.ErrorText);
                    return false;
                }
            }
        }


        public bool AcquisitionStop()
        {
            this.CheckAlive();
            if (this.IsLiveStart == true) return false;
            lock (lockObject)
            {
                try
                {

                    this.AcquisitionStopProc.Call();

                    bool isAcquisitionStop = false;
                    this.AcquisitionStopProc.Returns()
                                        .Get<bool>("return", out isAcquisitionStop);

                    System.Diagnostics.Debug.WriteLine("received data = " + isAcquisitionStop);
                    if(isAcquisitionStop == false)
                    {
                        this.HasError = true;
                        this.ErrorText = "Acquisition stop failed";
                        Logger.Logger.Write(Logger.TYPE.DEVICE, this.ErrorText);
                    }
                    return isAcquisitionStop;
                }
                catch (Exception e)
                {
                    this.HasError = true;
                    this.ErrorText = e.Message;
                    Logger.Logger.Write(Logger.TYPE.DEVICE, this.ErrorText);
                    return false;
                }

            }
        }

        public bool ConfigureSoftwareTrigger()
        {
            this.CheckAlive();
            if (this.IsLiveStart == true) return false;
            lock (lockObject)
            {
                try
                {

                    this.ConfigureSoftwareTriggerProc.Call();

                    bool isConfiguredSoftwareTrigger = false;
                    this.ConfigureSoftwareTriggerProc.Returns()
                                        .Get<bool>("return", out isConfiguredSoftwareTrigger);

                    System.Diagnostics.Debug.WriteLine("received data = " + isConfiguredSoftwareTrigger);
                    if(isConfiguredSoftwareTrigger == false)
                    {
                        this.HasError = true;
                        this.ErrorText = "Configure software trigger failed";
                        Logger.Logger.Write(Logger.TYPE.DEVICE, this.ErrorText);
                    }
                    return isConfiguredSoftwareTrigger;
                }
                catch (Exception e)
                {
                    this.HasError = true;
                    this.ErrorText = e.Message;
                    Logger.Logger.Write(Logger.TYPE.DEVICE, this.ErrorText);
                    return false;
                }

            }
        }

        public bool Grab()
        {
            lock (lockObject)
            {

                try
                {
                    
                    if (this.SharedImageBuffer == null) return false;
                    if (this.ImageWidth <= 0 || this.ImageHeight <= 0) return false;


                    this.HasError = false;

                    this.GrabProc.Call();

                    bool checkGrab = false;



                    this.GrabProc.Returns()
                                 .Get<bool>("return", out checkGrab);
                    System.Diagnostics.Debug.WriteLine("received data = " + checkGrab);
                    if (checkGrab == false)
                    {
                        this.HasError = true;
                        this.ErrorText = "Grab failed";
                        Logger.Logger.Write(Logger.TYPE.DEVICE, this.ErrorText);
                    }


                    
                    unsafe
                    {
                        fixed (byte* p = this.ImageByteBuffer)
                        {
                            IntPtr ptr = (IntPtr)p;
                            this.SharedImageBuffer?.Receive(ptr,(uint)this.ImageSize);
                        }
                    }


                    return checkGrab;
                }
                catch (Exception e)
                {
                    this.HasError = true;
                    this.ErrorText = e.Message;
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    Logger.Logger.Write(Logger.TYPE.DEVICE, this.ErrorText);
                    return false;
                }
            }
        }


        public bool GetImageInfo()
        {
            this.CheckAlive();
            if (this.IsLiveStart == true) return false;
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

                    if (width <= 0) return false;
                    if (height <= 0) return false;

                    this.ImageWidth = (int)width;
                    this.ImageHeight = (int)height;
                    this.ImageChannel = (int)channel;
                        


                    if (getImageInfoCheck == true)
                    {
                        this.ImageSize = (int)(this.ImageWidth * this.ImageHeight * this.ImageChannel);
                        this.ImageStride = (int)(this.ImageWidth * this.ImageChannel);
                        this.SharedImageBuffer ??= new SPIDER.Variable<byte>(this.Uid + "_ImageBuffer", (uint)this.ImageSize, 1000, SPIDER.SPIDER_MODE.CREATE, SPIDER.SPIDER_ACCESS.READ_WRITE);
                        if (this.IsLoop == false) return false;
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            if (this.ImageBuffer == null || this.ImageBuffer.Width != this.ImageWidth || this.ImageBuffer.Height != this.ImageBuffer.Height)
                            {
                                WriteableBitmap writeBitmap = null;
                                switch (this.ImageChannel)
                                {
                                    case 1:
                                        writeBitmap = new WriteableBitmap(this.ImageWidth, this.ImageHeight, 96, 96, PixelFormats.Gray8, null);
                                        break;
                                    case 3:
                                        writeBitmap = new WriteableBitmap(this.ImageWidth, this.ImageHeight, 96, 96, PixelFormats.Bgr24, null);
                                        break;
                                }
                                this.ImageBuffer = writeBitmap;
                                this.ImageByteBuffer = new byte[this.ImageSize];
                            }

                        },DispatcherPriority.ApplicationIdle);

                    }

                    if(getImageInfoCheck == false)
                    {
                        this.HasError = true;
                        this.ErrorText = "Get image info failed";
                        Logger.Logger.Write(Logger.TYPE.DEVICE, this.ErrorText);
                    }

                    System.Diagnostics.Debug.WriteLine("received data = " + getImageInfoCheck);
                    return getImageInfoCheck;

                }
                catch(Exception e)
                {
                    this.HasError = true;
                    this.ErrorText = e.Message;
                    Logger.Logger.Write(Logger.TYPE.DEVICE, this.ErrorText);
                    return false;
                }
            }
        }


        [Newtonsoft.Json.JsonIgnore]
        public ICommand LiveStartCommand
        {
            get => new RelayCommand(() =>
            {
                this.LiveStart();
            });
        }


        [Newtonsoft.Json.JsonIgnore]
        public ICommand LiveStopCommand
        {
            get => new RelayCommand(() =>
            {

                this.LiveStop();

            });
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
                this.DisplayImage();
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

        [Newtonsoft.Json.JsonIgnore]
        public ICommand test
        {
            get => new RelayCommand(() =>
            {
                this.ConfigureSoftwareTrigger();
            });
        }

    }
}
