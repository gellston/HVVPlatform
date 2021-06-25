using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class SerialLight : Device
    {
        private readonly SPIDER.Function OpenProc;
        private readonly SPIDER.Function CloseProc;
        private readonly SPIDER.Function OnProc;
        private readonly SPIDER.Function GetLightValueProc;
        private object lockObject = new object();

        public SerialLight()
        {
            try
            {
                this.OpenProc = new SPIDER.Function(this.Uid + "_Open");
                this.OpenProc.Delay(5000).Args()
                               .Arg<int>("comport")
                               .Arg<int>("baud")
                               .Returns()
                               .Ret<bool>("return")
                               .Complete();

                this.CloseProc = new SPIDER.Function(this.Uid + "_Close");
                this.CloseProc.Returns()
                                .Ret<bool>("return")
                                .Complete();

                this.OnProc = new SPIDER.Function(this.Uid + "_On");
                this.OnProc.Returns()
                           .Ret<bool>("return")
                           .Args()
                           .Arg<int>("channel")
                           .Arg<int>("value")
                           .Complete();


                this.GetLightValueProc = new SPIDER.Function(this.Uid + "_GetLightValue");
                this.GetLightValueProc.Returns()
                                      .Ret<bool>("return")
                                      .Ret<int>("lightValue")
                                      .Args()
                                      .Arg<int>("channel")
                                      .Complete();


                for(int index=1; index< 32; index++)
                {
                    this.ChannelCollection.Add(index);
                }


                for (int index =1; index< 13; index++)
                {
                    this.PortCollection.Add(index);
                }

                this.BaudCollection.Add(9600);
                this.BaudCollection.Add(14400);
                this.BaudCollection.Add(19200);
                this.BaudCollection.Add(38400);
                this.BaudCollection.Add(57600);
                this.BaudCollection.Add(115200);
                this.BaudCollection.Add(128000);


            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                Logger.Logger.Write(Logger.TYPE.DEVICE, this.ErrorText);
            }
        }

        public override bool DefaultSetup()
        {

            try
            {
                this.CheckAlive();
                if (this.Open(this.Comport, this.Baurate) == false) 
                    return false;
                


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

        public override object Clone()
        {


            SerialLight newCopy = new SerialLight()
            {
                IsAlive = this.IsAlive,
                HasError = this.HasError,
                ErrorText = this.ErrorText,
                Pid = this.Pid,
                DeviceName = this.DeviceName,
                Name = this.Name,
                Baurate = this.Baurate,
                Channel = this.Channel,
                Comport = this.Comport,
                Value = this.Value

            };

            return newCopy;
        }


        bool Open(int comport, int baurate)
        {
            this.CheckAlive();
            lock (lockObject)
            {
                try
                {

                    this.OpenProc.Args()
                                 .Push<int>("comport", comport)
                                 .Push<int>("baud", baurate);

                    this.OpenProc.Call();

                    bool retValue = false;

                    this.OpenProc.Returns()
                                 .Get<bool>("return", out retValue);
                    return retValue;
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

        bool Close()
        {
            this.CheckAlive();
            lock (lockObject)
            {
                try
                {

     
                    this.CloseProc.Call();

                    bool retValue = false;

                    this.CloseProc.Returns()
                                 .Get<bool>("return", out retValue);
                    return retValue;
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

        bool On(int channel, int value)
        {
            this.CheckAlive();
            lock (lockObject)
            {
                try
                {
                    this.OnProc.Args()
                                .Push<int>("channel", channel)
                                .Push<int>("value", value);



                    this.OnProc.Call();

                    bool retValue = false;

                    this.OnProc.Returns()
                                 .Get<bool>("return", out retValue);
                    return retValue;
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

        int GetLightValue(int channel)
        {
            this.CheckAlive();
            lock (lockObject)
            {
                try
                {
                    int lightValue = 0;
                    this.GetLightValueProc.Args()
                                .Push<int>("channel", channel);



                    this.GetLightValueProc.Call();

                    bool retValue = false;

                    this.GetLightValueProc.Returns()
                                 .Get<int>("lightValue", out lightValue)
                                 .Get<bool>("return", out retValue);


                    return lightValue;
                }
                catch (Exception e)
                {
                    this.HasError = true;
                    this.ErrorText = e.Message;
                    Logger.Logger.Write(Logger.TYPE.DEVICE, this.ErrorText);
                    return -1;
                }
            }

        }


        protected override void Dispose(bool disposing)
        {

            if (disposing)
            {

                this.OpenProc?.Dispose();
                this.CloseProc?.Dispose();
                this.OnProc?.Dispose();
                this.GetLightValueProc?.Dispose();
                




            }
            base.Dispose(disposing);
        }


        private int _Channel = 0;
        public int Channel
        {
            get => _Channel;
            set => Set(ref _Channel, value);
        }

        private int _Comport = 0;
        public int Comport
        {
            get => _Comport;
            set => Set(ref _Comport, value);
        }

        private int _Baurate = 0;
        public int Baurate
        {
            get => _Baurate;
            set => Set(ref _Baurate, value);
        }


        private int _Value = 0;
        public int Value
        {
            get => _Value;
            set => Set(ref _Value, value);
        }


        private ObservableCollection<int> _ChannelCollection = null;
        [Newtonsoft.Json.JsonIgnore]
        public ObservableCollection<int> ChannelCollection
        {
            get
            {
                _ChannelCollection ??= new ObservableCollection<int>();
                return _ChannelCollection;
            }
        }


        private ObservableCollection<int> _PortCollection = null;
        [Newtonsoft.Json.JsonIgnore]
        public ObservableCollection<int> PortCollection
        {
            get
            {
                _PortCollection ??= new ObservableCollection<int>();
                return _PortCollection;
            }
        }
        private ObservableCollection<int> _BaudCollection = null;
        [Newtonsoft.Json.JsonIgnore]
        public ObservableCollection<int> BaudCollection
        {
            get
            {
                _BaudCollection ??= new ObservableCollection<int>();
                return _BaudCollection;
            }
        }



        [Newtonsoft.Json.JsonIgnore]
        public ICommand OpenCommand
        {
            get => new RelayCommand(() =>
            {
                bool returnValue = this.Open(this.Comport, this.Baurate);
            });
        }



        [Newtonsoft.Json.JsonIgnore]
        public ICommand CloseCommand
        {
            get => new RelayCommand(() =>
            {
                bool returnValue = this.Close();
            });
        }


        [Newtonsoft.Json.JsonIgnore]
        public ICommand OnCommand
        {
            get => new RelayCommand(() =>
            {
                bool returnValue = this.On(this.Channel, this.Value);
            });
        }


        [Newtonsoft.Json.JsonIgnore]
        public ICommand GetLightValueCommand
        {
            get => new RelayCommand(() =>
            {
                this.Value = this.GetLightValue(this.Channel);
            });
        }



    }
}
