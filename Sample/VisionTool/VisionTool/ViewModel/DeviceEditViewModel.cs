using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using VisionTool.Service;

namespace VisionTool.ViewModel
{
    public class DeviceEditViewModel : ViewModelBase
    {

        private readonly DeviceControlService deviceControlService;
        private readonly ProcessManagerService processManagerService;

        public DeviceEditViewModel(DeviceControlService _deviceControlService,
                                   ProcessManagerService _processManagerService)
        {

            this.deviceControlService = _deviceControlService;
            this.processManagerService = _processManagerService;


            this.DeviceConfigCollection = this.deviceControlService.DeviceConfigCollection;
            this.DeviceCollection = this.processManagerService.DeviceCollection;

            
           
        }



        private ObservableCollection<Model.DeviceConfig> _DeviceConfigCollection = null;
        public ObservableCollection<Model.DeviceConfig> DeviceConfigCollection
        {
            get => _DeviceConfigCollection;
            set => Set(ref _DeviceConfigCollection, value);
        }


        private Model.DeviceConfig _SelectedDeviceConfig = null;
        public Model.DeviceConfig SelectedDeviceConfig
        {
            get => _SelectedDeviceConfig;
            set => Set(ref _SelectedDeviceConfig, value);
        }


        private object _CurrentDeviceSettingView = null;
        public object CurrentDeviceSettingView
        {
            get => _CurrentDeviceSettingView;
            set => Set(ref _CurrentDeviceSettingView,value);
        }

        public ICommand AddDeviceCommand
        {
            get => new RelayCommand(() =>
            {
                try
                {
                    this.processManagerService.AddDevice(this.SelectedDeviceConfig); 

                }
                catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);

                }
            });
        }


        public ICommand ProcessAllExecutionCommand
        {
            get => new RelayCommand(() =>
            {

                try
                {
                    this.processManagerService.ShutDownAllChildProcess();
                    this.processManagerService.RunAllChildProcess();
                    
                }catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }

            });
        }

        public ICommand ProcessExecutionCommand
        {
            get => new RelayCommand(() =>
            {
                try
                {
                    this.processManagerService.RunChildProcess(this.SelectedDevice);

                }catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            });
        }


        public ICommand DeleteDeviceCommand
        {
            get => new RelayCommand<object>((_object) =>
            {
                try
                {
                    this.processManagerService.DeleteDevice(this.SelectedDevice);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }

            });
        }


        public ICommand ImportDeviceSettingCommand
        {
            get => new RelayCommand(() =>
            {

                try
                {
                    this.processManagerService.ImportDevice();

                }catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);

                }

            });
        }

        public ICommand ExportDeviceSettingCommand
        {
            get => new RelayCommand(() =>
            {

                try
                {

                    this.processManagerService.ExportDevice();

                }
                catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);

                }
            });
        }


        public ICommand ClearDeviceCommand
        {
            get => new RelayCommand(() =>
            {
                this.processManagerService.ClearDevice();
            });
        }


        public ICommand SaveDeviceCommand
        {
            get => new RelayCommand(() =>
            {
                this.processManagerService.SaveDevice();
            });
        }

        public ICommand LoadDeviceCommand
        {
            get => new RelayCommand(() =>
            {
                this.processManagerService.LoadDevice();
            });
        }


        private ObservableCollection<Device.Device> _DeviceCollection = null;
        public ObservableCollection<Device.Device> DeviceCollection
        {
            get => _DeviceCollection;
            set => Set(ref _DeviceCollection, value);
        }

        private Device.Device _SelectedDevice = null;
        public Device.Device SelectedDevice
        {
            get => _SelectedDevice;
            set => Set(ref _SelectedDevice, value);
        }


        private double _ItemsWidth = 480;
        public double ItemsWidth
        {
            get => _ItemsWidth;
            set => Set(ref _ItemsWidth, value);
        }


        private double _ItemsHeight = 600;
        public double ItemsHeight
        {
            get => _ItemsHeight;
            set => Set(ref _ItemsHeight, value);
        }







    }
}
