using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Device
{
    public class GigECamera : Device
    {

        private readonly SPIDER.Function OpenCamera;
        private readonly SPIDER.Function CloseCamera;
        private readonly SPIDER.Function AcquisitionStart;
        private readonly SPIDER.Function AcquisitionStop;



        public GigECamera()
        {
            try
            {
                this.OpenCamera = new SPIDER.Function(this.Uid + "_Open");
                this.OpenCamera.Args()
                               .Arg<string>("ip", 256)
                               .Returns()
                               .Ret<bool>("return")
                               .Complete();

                this.CloseCamera = new SPIDER.Function(this.Uid + "_Close");
                this.CloseCamera.Returns()
                                .Ret<bool>("return")
                                .Complete();



                this.AcquisitionStart = new SPIDER.Function(this.Uid + "_AcquisitionStart");
                this.AcquisitionStart.Returns()
                                     .Ret<bool>("return")
                                     .Complete();


                this.AcquisitionStop = new SPIDER.Function(this.Uid + "_AcquisitionStop");
                this.AcquisitionStop.Returns()
                                    .Ret<bool>("return")
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
                IsDeviceRunning = this.IsDeviceRunning,
                HasError = this.HasError,
                ErrorText = this.ErrorText,
                Pid = this.Pid,
                Type = this.Type,
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


        public ICommand OpenCameraCommand
        {
            get => new RelayCommand(() =>
            {
                try
                {
                    this.OpenCamera.Args()
                                   .Push<string>("ip", this.IP);

                    this.OpenCamera.Call();

                    bool isOpen = false;
                    this.OpenCamera.Returns()
                                   .Get<bool>("return", out isOpen);

                    System.Diagnostics.Debug.WriteLine("received data = " + isOpen);

                }catch(Exception e)
                {
                    this.HasError = true;
                    this.ErrorText = e.Message;

                }
            });
        }

        public ICommand CloseCameraCommand
        {
            get => new RelayCommand(() =>
            {
                try
                {

                    this.CloseCamera.Call();

                    bool isClosed = false;
                    this.CloseCamera.Returns()
                                   .Get<bool>("return", out isClosed);

                    System.Diagnostics.Debug.WriteLine("received data = " + isClosed);
                }
                catch(Exception e)
                {
                    this.HasError = true;
                    this.ErrorText = e.Message;
                }
            });
        }


        public ICommand AcquisitionStartCommand
        {
            get => new RelayCommand(() =>
            {
                try
                {

                    this.AcquisitionStart.Call();

                    bool isAcquisitionStart = false;
                    this.AcquisitionStart.Returns()
                                   .Get<bool>("return", out isAcquisitionStart);

                    System.Diagnostics.Debug.WriteLine("received data = " + isAcquisitionStart);
                }
                catch (Exception e)
                {
                    this.HasError = true;
                    this.ErrorText = e.Message;
                }
            });
        }


        public ICommand AcquisitionStopCommand
        {
            get => new RelayCommand(() =>
            {
                try
                {

                    this.AcquisitionStop.Call();

                    bool isAcquisitionStop = false;
                    this.AcquisitionStop.Returns()
                                        .Get<bool>("return", out isAcquisitionStop);

                    System.Diagnostics.Debug.WriteLine("received data = " + isAcquisitionStop);
                }
                catch (Exception e)
                {
                    this.HasError = true;
                    this.ErrorText = e.Message;
                }
            });
        }


    }
}
