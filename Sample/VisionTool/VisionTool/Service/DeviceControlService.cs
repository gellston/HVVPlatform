using DevExpress.Compression;
using DevExpress.Xpf.CodeView;
using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using VisionTool.Helper;

namespace VisionTool.Service
{
    public class DeviceControlService
    {

        private readonly SettingConfigService settingConfigService;


        private Action<string> currentDeviceComment;
        private Action<string> currentDeviceMainPath;
        private Action<int> currentDeviceVersion;
        private Action<string> currentDeviceModifyDate;
        private Action<string> currentDeviceName;


        public DeviceControlService(SettingConfigService _settingConfigService,
                                    ProcessManagerService _processManagerService)
        {
            this.settingConfigService = _settingConfigService;


            this.UpdateDeviceInfo();
        }

        public void SetCallbackCurrentDeviceVersion(Action<int> check)
        {
            this.currentDeviceVersion += check;
        }

        public void SetCallbackCurrentDeviceName(Action<string> check)
        {
            this.currentDeviceName += check;
        }

        public void SetCallbackCurrentDeviceModifyDate(Action<string> check)
        {
            this.currentDeviceModifyDate += check;
        }

        public void SetCallbackCurrentDeviceMainPath(Action<string> check)
        {
            this.currentDeviceMainPath += check;
        }
        public void SetCallbackCurrentDeviceComment(Action<string> check)
        {
            this.currentDeviceComment += check;
        }



        private ObservableCollection<DeviceConfig> _DeviceConfigCollection = new ObservableCollection<DeviceConfig>();
        public ObservableCollection<DeviceConfig> DeviceConfigCollection
        {
            get
            {
                return _DeviceConfigCollection;
            }
        }

        private ObservableCollection<DependentDLL> _DependentDLLCollection = new ObservableCollection<DependentDLL>();
        public ObservableCollection<DependentDLL> DependentDLLCollection
        {
            get
            {
                return _DependentDLLCollection;
            }

        }



        private ObservableCollection<Model.Device> _DeviceCollection = new ObservableCollection<Model.Device>();
        public ObservableCollection<Model.Device> DeviceCollection
        {
            get
            {
                return _DeviceCollection;
            }
        }


        private ObservableCollection<Device.Device> _DeviceTypeCollection = new ObservableCollection<Device.Device>();
        public ObservableCollection<Device.Device> DeviceTypeCollection
        {
            get
            {
                return _DeviceTypeCollection;
            }
        }


        

        public void LoadDeviceInfoFromConfig(DeviceConfig config)
        {


            try
            {
                if (config == null) return;

                FileSystemHelper.DeleteFiles(this.settingConfigService.TempDeviceModPackagePath);


                var diagramPath = this.settingConfigService.ApplicationSetting.DevicePath + config.DeviceName + ".device";
                FileSystemHelper.UnZipFile(diagramPath,
                                           this.settingConfigService.TempDeviceModPackagePath,
                                           this.settingConfigService.SecurityPassword);



                this.currentDeviceModifyDate.Invoke(config.DeviceModifyDate);
                this.currentDeviceComment.Invoke(config.DeviceComment);
                this.currentDeviceMainPath.Invoke(this.settingConfigService.TempDeviceModPackagePath + Path.DirectorySeparatorChar + config.DeviceName + ".exe");
                this.currentDeviceName.Invoke(config.DeviceName);
                this.currentDeviceVersion.Invoke(config.DeviceVersion);

                var files = Directory.GetFiles(this.settingConfigService.TempDeviceModPackagePath + Path.DirectorySeparatorChar, "*.dll").ToList();
                var dependentDLLList = new List<DependentDLL>();
                foreach (var file in files)
                {
                    var dependentLibrary = new DependentDLL()
                    {
                        FilePath = file,
                        FileName = Path.GetFileName(file)
                    };
                    dependentDLLList.Add(dependentLibrary);
                }

                this.DependentDLLCollection.Clear();
                this.DependentDLLCollection.AddRange(dependentDLLList);

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }



        }


        public void ImportDevice()
        {
            try
            {
                var path = DialogHelper.OpenFile("device file (.device)|*.device");
                var fileName = Path.GetFileName(path);

                var targetPath = settingConfigService.ApplicationSetting.DevicePath + fileName;

                if (File.Exists(targetPath) == true)
                {
                    if (DialogHelper.ShowConfirmMessage("디바이스가 존재합니다. 덮어쓰시겠습니까?") == false)
                    {
                        return;
                    }


                }
                File.Copy(path, targetPath, true);
                this.UpdateDeviceInfo();

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        public void ImportDevice(string path)
        {
            try
            {
                var fileName = Path.GetFileName(path);

                var targetPath = settingConfigService.ApplicationSetting.DevicePath + fileName;

                if (File.Exists(targetPath) == true)
                {
                    if (DialogHelper.ShowConfirmMessage("디바이스가 존재합니다. 덮어쓰시겠습니까?") == false)
                    {
                        return;
                    }


                }
                File.Copy(path, targetPath, true);
                this.UpdateDeviceInfo();

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        public void DeleteDeleteDependentDLL(DependentDLL dll)
        {
            if (dll == null) return;
            this.DependentDLLCollection.Remove(dll);
        }


        public void LoadDeviceFromPath()
        {

            try
            {
                var exeFile = DialogHelper.OpenFile("device execution file (.exe)|*.exe");
                if (exeFile.Length == 0) throw new Exception("File is not selected");

                this.currentDeviceMainPath.Invoke(exeFile);

            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public void ClearDevice()
        {
            this.currentDeviceComment.Invoke("");
            this.currentDeviceMainPath.Invoke("");
            this.currentDeviceModifyDate.Invoke("");
            this.currentDeviceName.Invoke("");
            this.currentDeviceVersion.Invoke(1);



            this.DependentDLLCollection.Clear();
        }

        public void DeleteDevice(Model.Device device)
        {
            try
            {

                if (device == null) throw new Exception("Device is not correct.");
                if (File.Exists(device.FilePath) == false) throw new Exception("Device file is not exists");

                File.Delete(device.FilePath);

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                throw e;
            }

        }


        public void Load3rdLibrariesFromPath()
        {
            try
            {
                this.DependentDLLCollection.Clear();
                ObservableCollection<DependentDLL> thirdlibrary = new ObservableCollection<DependentDLL>();
                var dependentlibrary = DialogHelper.OpenFiles("library files (.dll)|*.dll");

                foreach (var library in dependentlibrary)
                {
                    thirdlibrary.Add(new DependentDLL()
                    {
                        FileName = Path.GetFileName(library),
                        FilePath = library
                    });
                }

                this.DependentDLLCollection.AddRange(thirdlibrary);
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        

        public void UpdateDeviceInfo()
        {




            FileSystemHelper.DeleteFiles(this.settingConfigService.ApplicationSetting.DeviceConfigPath);
            FileSystemHelper.DeleteFiles(this.settingConfigService.ApplicationSetting.DeviceMainPath);


            foreach (var deviceName in this.settingConfigService.ApplicationSetting.DeviceTypeCollection)
            {
                try
                {

                    var assembly = Assembly.GetExecutingAssembly();

                    Type propertyType = Type.GetType("Device." + deviceName + ", Device");
                    var property = Activator.CreateInstance(propertyType);
                    this.DeviceTypeCollection.Add((Device.Device)property);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }


            try
            {
                this.DeviceCollection.Clear();
                this.DeviceConfigCollection.Clear();
                var files = FileSystemHelper.GetFiles(this.settingConfigService.ApplicationSetting.DevicePath, "*.device");
                foreach (var file in files)
                {
                    FileSystemHelper.DeleteFiles(this.settingConfigService.TempDevicePackagePath);
                    try
                    {
                        FileSystemHelper.UnZipFile(file, this.settingConfigService.TempDevicePackagePath, this.settingConfigService.SecurityPassword);

                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        continue;
                    }
                    var targetDirectory = this.settingConfigService.ApplicationSetting.DeviceMainPath + Path.GetFileNameWithoutExtension(file) + Path.DirectorySeparatorChar;
                    Directory.CreateDirectory(targetDirectory);
                    FileSystemHelper.CopyFiles(this.settingConfigService.TempDevicePackagePath, targetDirectory, "*.exe");
                    FileSystemHelper.CopyFiles(this.settingConfigService.TempDevicePackagePath, targetDirectory, "*.dll");
                    FileSystemHelper.CopyFiles(this.settingConfigService.TempDevicePackagePath, this.settingConfigService.ApplicationSetting.DeviceConfigPath, "*.json");

                    this.DeviceCollection.Add(new Model.Device()
                    {
                        FilePath = file,
                        FileName = Path.GetFileName(file)
                    });

                }


                try
                {
                    var configs = Directory.GetFiles(this.settingConfigService.ApplicationSetting.DeviceConfigPath, "*.json");
                    foreach (var config in configs)
                    {
                        var jsonContent = File.ReadAllText(config);
                        var device = JsonConvert.DeserializeObject<DeviceConfig>(jsonContent);
                        this.DeviceConfigCollection.Add(device);
                    }

                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }

                FileSystemHelper.DeleteFiles(this.settingConfigService.TempDevicePackagePath);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

        }


        public bool CheckDeviceExists(string _deviceName)
        {
            if (this.DeviceConfigCollection.ToList().Exists(x => x.DeviceName == _deviceName) == true)
                return true;
            else return false;
        }



        public void CreateDevicePackage(string _deviceName,
                                        string _deviceModifyDate,
                                        int _deviceVersion,
                                        string _deviceComment,
                                        string _deviceMainPath,
                                        string _deviceType,
                                        bool _isOverwrite)
        {
            try
            {
                if (_deviceName.Length == 0)
                {
                    throw new Exception("Device name is not correct");
                }
                if (_deviceModifyDate.Length == 0)
                {
                    throw new Exception("Device date is not correct");
                }
                if (_deviceVersion < 1)
                {
                    throw new Exception("Device version is not correct");
                }


                if(_deviceType == null)
                {
                    throw new Exception("Device type is not correct");
                }

                if(_deviceType.Length == 0)
                {
                    throw new Exception("Device type is not correct");
                }

                if (this.DeviceConfigCollection.ToList().Exists(x => x.DeviceName == _deviceName) && _isOverwrite == false)
                    throw new Exception("Device name is already exists");

                //if (_moduleTargetPath.Length == 0) return false;
                //if (_moduleTempPackagePath.Length == 0) return false;
                //if (_moduleMainPath.Length == 0) return false;

                //if (Directory.Exists(_moduleTargetPath) == false) return false;
                //if (Directory.Exists(_moduleTempPackagePath) == false) return false;



                var regexItem = new Regex("^[a-zA-Z0-9]*$");
                if (regexItem.IsMatch(_deviceName) == false)
                {
                    throw new Exception("Device name contains special character");
                }


                FileSystemHelper.DeleteFiles(this.settingConfigService.TempDevicePackagePath);


                FileSystemHelper.CopyFile(_deviceMainPath, this.settingConfigService.TempDevicePackagePath + _deviceName + ".exe");



                var dependentDLL = this.settingConfigService.TempDevicePackagePath + Path.DirectorySeparatorChar;
                foreach (var file in this.DependentDLLCollection)
                {
                    var targetFilePath = dependentDLL + file.FileName;
                    File.Copy(file.FilePath, targetFilePath);
                }


                DeviceConfig config = new DeviceConfig()
                {
                    DeviceComment = _deviceComment,
                    DeviceName = _deviceName,
                    DeviceVersion = _deviceVersion,
                    DeviceModifyDate = _deviceModifyDate,
                    DeviceType = _deviceType
                };


                var targetConfigPath = this.settingConfigService.TempDevicePackagePath + _deviceName + ".json";
                string jsonString = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(targetConfigPath, jsonString);


                var targetZipPath = this.settingConfigService.ApplicationSetting.DevicePath + _deviceName + ".device";

                string zipFileName = targetZipPath;
                EncryptionType encryptionType = EncryptionType.Aes256;
                using (ZipArchive archive = new ZipArchive())
                {
                    archive.Password = this.settingConfigService.SecurityPassword;
                    archive.EncryptionType = encryptionType;
                    archive.AddDirectory(this.settingConfigService.TempDevicePackagePath, "/");
                    archive.Save(zipFileName);
                }


                this.UpdateDeviceInfo();

            }
            catch (Exception e)
            {
                DialogHelper.ShowToastErrorMessage("디바이스 패키지", "디바이스 패키징 실패 : " + e.Message);
                throw e;
            }

        }
    }
}
