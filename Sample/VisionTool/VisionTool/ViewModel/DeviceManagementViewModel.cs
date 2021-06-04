using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using VisionTool.Message;
using VisionTool.Service;

namespace VisionTool.ViewModel
{
    class DeviceManagementViewModel : ViewModelBase
    {
        private readonly DeviceControlService deviceControlService;
        private readonly SettingConfigService settingConfigService;

        public DeviceManagementViewModel(DeviceControlService _deviceControlService,
                                         SettingConfigService _settingConfigService)
        {

            this.deviceControlService = _deviceControlService;
            this.settingConfigService = _settingConfigService;


            this.DeviceCollection = this.deviceControlService.DeviceCollection;
            this.DeviceConfigCollection = this.deviceControlService.DeviceConfigCollection;
            this.DependentDLLCollection = this.deviceControlService.DependentDLLCollection;
            this.DeviceTypeCollection = this.settingConfigService.ApplicationSetting.DeviceTypeCollection;


            this.deviceControlService.SetCallbackCurrentDeviceComment(data => this.DeviceComment = data);
            this.deviceControlService.SetCallbackCurrentDeviceMainPath(data => this.DeviceMainPath = data);
            this.deviceControlService.SetCallbackCurrentDeviceModifyDate(data => this.DeviceModifyDate = data);
            this.deviceControlService.SetCallbackCurrentDeviceName(data => this.DeviceName = data);
            this.deviceControlService.SetCallbackCurrentDeviceVersion(data => this.DeviceVersion = data);

            


        }

        private void FileAssociationCallback(AssociationModeMessage message)
        {
            if (message.AssociationMode == "Device")
            {

                try
                {
                    this.deviceControlService.ImportDevice(message.FilePath);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }


            }
        }


        public ICommand ImportDeviceCommand
        {
            get => new RelayCommand(() =>
            {
                this.deviceControlService.ImportDevice();

            });
        }

        public ICommand ModifyUpdateDeviceCommand
        {
            get => new RelayCommand(() =>
            {
                if (this.SelectedDeviceConfig == null) return;

                try
                {

                    this.deviceControlService.LoadDeviceInfoFromConfig(this.SelectedDeviceConfig);

                    this.RaisePropertyChanged("DependentDLLCollection");
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            });
        }

        public ICommand ModifyDeviceCommand
        {
            get => new RelayCommand(() =>
            {
                try
                {
                    this.DeviceModifyDate = DateTime.Now.ToString("yyyy-MM-HH hh:mm:ss");

                    try
                    {
                        if (this.deviceControlService.CheckDeviceExists(this.DeviceName) == false) return;
                        this.deviceControlService.CreateDevicePackage(this.DeviceName,
                                                                      this.DeviceModifyDate,
                                                                      this.DeviceVersion + 1,
                                                                      this.DeviceComment,
                                                                      this.DeviceMainPath,
                                                                      this.SelectedDeviceType,
                                                                      true);




                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            });
        }

        public ICommand DeleteDeviceCommand
        {
            get => new RelayCommand(() =>
            {
                try
                {
                    this.deviceControlService.DeleteDevice(this.SelectedDevice);
                    this.deviceControlService.UpdateDeviceInfo();

                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }


            });
        }

        public ICommand DeleteDependentDLLCommand
        {
            get => new RelayCommand<DependentDLL>((dll) =>
            {
                this.deviceControlService.DeleteDeleteDependentDLL(dll);

            });
        }

        public ICommand ReLoadDeviceCommand
        {
            get => new RelayCommand(() =>
            {
              
                this.deviceControlService.UpdateDeviceInfo();
            });
        }

        public ICommand ClearDeviceCommand
        {
            get => new RelayCommand(() =>
            {
                this.deviceControlService.ClearDevice();
            });
        }




        public ICommand AddMainDeviceCommand
        {
            get => new RelayCommand(() =>
            {
                try
                {
                    this.deviceControlService.LoadDeviceFromPath();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }

            });
        }


        public ICommand AddDependentDLLCommand
        {
            get => new RelayCommand(() =>
            {
                try
                {
                    this.deviceControlService.Load3rdLibrariesFromPath();


                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }

            });
        }

        public ICommand PackageDeviceCommand
        {
            get => new RelayCommand(() =>
            {
                this.DeviceModifyDate = DateTime.Now.ToString("yyyy-MM-HH hh:mm:ss");

                try
                {
                    this.deviceControlService.CreateDevicePackage(this.DeviceName,
                                                                  this.DeviceModifyDate,
                                                                  this.DeviceVersion,
                                                                  this.DeviceComment,
                                                                  this.DeviceMainPath,
                                                                  this.SelectedDeviceType,
                                                                  false);

                    this.deviceControlService.UpdateDeviceInfo();

                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }

            });
        }



        private string _DeviceName = "";
        public string DeviceName
        {
            get => _DeviceName;
            set => Set(ref _DeviceName, value);
        }


        private string _DeviceModifyDate = "";
        public string DeviceModifyDate
        {
            get => _DeviceModifyDate;
            set => Set(ref _DeviceModifyDate, value);
        }


        private string _DeviceMainPath = "";
        public string DeviceMainPath
        {
            get => _DeviceMainPath;
            set => Set(ref _DeviceMainPath, value);
        }


        private int _DeviceVersion = 1;
        public int DeviceVersion
        {
            get => _DeviceVersion;
            set => Set(ref _DeviceVersion, value);
        }

        private string _DeviceComment = "";
        public string DeviceComment
        {
            get => _DeviceComment;
            set => Set(ref _DeviceComment, value);
        }


        private string _SelectedDeviceType = null;
        public string SelectedDeviceType
        {
            get => _SelectedDeviceType;
            set => Set(ref _SelectedDeviceType, value);
        }



        



        private ObservableCollection<DependentDLL> _DependentDLLCollection = null;
        public ObservableCollection<DependentDLL> DependentDLLCollection
        {
            get => _DependentDLLCollection;
            set => Set(ref _DependentDLLCollection, value);
        }


        private ObservableCollection<DeviceConfig> _DeviceConfigCollection = null;
        public ObservableCollection<DeviceConfig> DeviceConfigCollection
        {
            get => _DeviceConfigCollection;
            set => Set(ref _DeviceConfigCollection, value);
        }


        private ObservableCollection<Model.Device> _DeviceCollection = null;
        public ObservableCollection<Model.Device> DeviceCollection
        {
            get => _DeviceCollection;
            set => Set(ref _DeviceCollection, value);
        }

        private ObservableCollection<string> _DeviceTypeCollection = null;
        public ObservableCollection<string> DeviceTypeCollection
        {
            get => _DeviceTypeCollection;
            set => Set(ref _DeviceTypeCollection, value);
        }

        private Model.Device _SelectedDevice = null;
        public Model.Device SelectedDevice
        {
            get => _SelectedDevice;
            set => Set(ref _SelectedDevice, value);
        }

        private DeviceConfig _SelectedDeviceConfig = null;
        public DeviceConfig SelectedDeviceConfig
        {
            get => _SelectedDeviceConfig;
            set => Set(ref _SelectedDeviceConfig, value);
        }
        


        

    }
}
