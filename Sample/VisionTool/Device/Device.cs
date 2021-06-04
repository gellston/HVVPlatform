using GalaSoft.MvvmLight.Command;
using System;
using System.Windows.Input;

namespace Device
{
    public class Device : PropertyChangedBase, ICloneable
    {

        private readonly SPIDER.Function AliveCheckFunction;
        private readonly SPIDER.Function ExitFunction;

        public Device()
        {
            try
            {
                this.AliveCheckFunction = new SPIDER.Function(this.Uid + "_alive");
                this.AliveCheckFunction.Delay(100)
                                       .Returns()
                                       .Ret<bool>("isalive")
                                       .Complete();

                this.ExitFunction = new SPIDER.Function(this.Uid + "_exit");
                this.ExitFunction.Delay(100)
                                 .Returns()
                                 .Ret<bool>("isexit")
                                 .Complete();


            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);

                this.HasError = true;
                this.ErrorText = e.Message;
            }

        }

        public virtual object Clone()
        {
            Device newCopy = new Device()
            {
                IsAlive = this.IsAlive,
                IsDeviceRunning = this.IsDeviceRunning,
                HasError = this.HasError,
                ErrorText = this.ErrorText,
                Pid = this.Pid,
                Type = this.Type,
                Name = this.Name,
            };
            
            return newCopy;
        }


        private string _ErrorText = "";
        public string ErrorText
        {
            get => _ErrorText;
            set => Set(ref _ErrorText, value);
        }


        private bool _HasError = false;
        public bool HasError
        {
            get => _HasError;
            set => Set(ref _HasError, value);
        }

        private string _Name = "";
        public string Name
        {
            get => _Name;
            set => Set(ref _Name, value);
        }

        private string _Type = "";
        public string Type
        {
            get => _Type;
            set => Set(ref _Type, value);
        }

        private string _Uid = "";
        public string Uid
        {
            get
            {
                _Uid ??= Guid.NewGuid().ToString("N");
                return _Uid;
            }
        }


        private string _Pid = "";
        public string Pid
        {
            get => _Pid;
            set => Set(ref _Pid, value);
        }

        private bool _IsDeviceRunning = false;
        public bool IsDeviceRunning
        {
            get => _IsDeviceRunning;
            set => Set(ref _IsDeviceRunning, value);
        }

        private bool _IsAlive = false;
        public bool IsAlive
        {
            get => _IsAlive;
            set => Set(ref _IsAlive, value);
        }

        public ICommand CheckAliveCommand
        {
            get => new RelayCommand(() =>
            {

                try
                {
                    this.AliveCheckFunction.Call();

                    bool isAlive = false;
                    this.AliveCheckFunction.Returns()
                                           .Get<bool>("isalive", out isAlive);

                    this.IsDeviceRunning = isAlive;
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    this.HasError = true;
                    this.ErrorText = e.Message;
                }

            });
        }


        public ICommand ExitDeviceCommand
        {
            get => new RelayCommand(() =>
            {
                string fullname = this.Pid + "_" + "exit";


                try
                {
                    this.ExitFunction.Call();
                    bool isexit = false;
                    this.ExitFunction.Returns()
                                           .Get<bool>("isexit", out isexit);
                    if(isexit == true)
                        this.IsDeviceRunning = false;
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    this.HasError = true;
                    this.ErrorText = e.Message;
                }

            });
        }
    }
}
