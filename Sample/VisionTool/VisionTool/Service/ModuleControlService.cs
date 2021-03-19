using DevExpress.Compression;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using VisionTool.Helper;
using VisionTool.Model;


namespace VisionTool.Service
{
    public class ModuleControlService
    {

        private readonly SettingConfigService settingConfigService;

        public ModuleControlService(SettingConfigService _settingConfigService)
        {
            this.settingConfigService = _settingConfigService;


            this.UpdateModuleInfo();
        }


        private ObservableCollection<ModuleConfig> _ModuleConfigCollection = null;
        public ObservableCollection<ModuleConfig> ModuleConfigCollection
        {
            get
            {
                _ModuleConfigCollection ??= new ObservableCollection<ModuleConfig>();
                return _ModuleConfigCollection;
            }
        }

        private ObservableCollection<Module> _ModuleCollection = null;
        public ObservableCollection<Module> ModuleCollection
        {
            get
            {
                _ModuleCollection ??= new ObservableCollection<Module>();
                return _ModuleCollection;
            }
        }
        public string GetLibraryFromPath()
        {
            var maindll = DialogHelper.OpenFile("library file (.dll)|*.dll");
            if (maindll.Length == 0) throw new Exception("File is not selected");

            return maindll;
        }

        public void DeleteModule(Module module)
        {
            try
            {

                if (module == null) throw new Exception("Module is not correct.");
                if (File.Exists(module.FilePath) == false) throw new Exception("Module file is not exists");

                File.Delete(module.FilePath);
                
            }
            catch(Exception e)
            {
                System.Console.WriteLine(e.Message);
                throw e;
            }
            
        }


        public ObservableCollection<DependentDLL> Get3rdLibrariesFromPath()
        {
            try
            {
                ObservableCollection<DependentDLL> thridlibrary = new ObservableCollection<DependentDLL>();
                var dependentlibrary = DialogHelper.OpenFiles("library files (.dll)|*.dll");
                
                foreach(var library in dependentlibrary)
                {
                    thridlibrary.Add(new DependentDLL()
                    {
                        FileName = Path.GetFileName(library),
                        FilePath = library
                    });
                }

                return thridlibrary;
            }
            catch(Exception e)
            {
                throw e;
            }
        }
        

        public void UpdateModuleInfo()
        {

            FileSystemHelper.DeleteFiles(this.settingConfigService.ApplicationSetting.ModuleConfigPath);
            FileSystemHelper.DeleteFiles(this.settingConfigService.ApplicationSetting.ModuleMainPath);
            FileSystemHelper.DeleteFiles(this.settingConfigService.ApplicationSetting.ModuleThirdPartyDLLPath);


            try
            {
                this.ModuleCollection.Clear();
                this.ModuleConfigCollection.Clear();
                var files = FileSystemHelper.GetFiles(this.settingConfigService.ApplicationSetting.ModulePath, "*.module");
                foreach(var file in files)
                {
                    FileSystemHelper.DeleteFiles(this.settingConfigService.TempModulePackagePath);
                    try
                    {
                        FileSystemHelper.UnZipFile(file, this.settingConfigService.TempModulePackagePath, this.settingConfigService.SecurityPassword);

                    }
                    catch(Exception e)
                    {
                        System.Console.WriteLine(e.Message);
                        continue;
                    }

                    FileSystemHelper.CopyFiles(this.settingConfigService.TempModulePackagePath, this.settingConfigService.ApplicationSetting.ModuleMainPath, "*.dll");
                    FileSystemHelper.CopyFiles(this.settingConfigService.TempModulePackagePath + "dependent", this.settingConfigService.ApplicationSetting.ModuleThirdPartyDLLPath, "*.dll");
                    FileSystemHelper.CopyFiles(this.settingConfigService.TempModulePackagePath, this.settingConfigService.ApplicationSetting.ModuleConfigPath, "*.json");

                    this.ModuleCollection.Add(new Module()
                    {
                        FilePath = file,
                        FileName = Path.GetFileName(file)
                    });

                }


                try
                {
                    var configs = Directory.GetFiles(this.settingConfigService.ApplicationSetting.ModuleConfigPath, "*.json");
                    foreach (var config in configs)
                    {
                        var jsonContent = File.ReadAllText(config);
                        var module = JsonConvert.DeserializeObject<ModuleConfig>(jsonContent);
                        this.ModuleConfigCollection.Add(module);
                    }

                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                }

                FileSystemHelper.DeleteFiles(this.settingConfigService.TempModulePackagePath);
            }
            catch(Exception e)
            {
                System.Console.WriteLine(e.Message);
            }

        }




        public void CreateModulePackage(string _moduleName, 
                                        string _moduleModifyDate, 
                                        int _moduleVersion,
                                        string _moduleComment, 
                                        string _moduleMainPath,
                                        ObservableCollection<DependentDLL> _dependentCollection)
        {
            try
            {
                if (_moduleName.Length == 0)
                {
                    throw new Exception("Module name is not correct");
                }
                if (_moduleModifyDate.Length == 0)
                {
                    throw new Exception("Module date is not correct");
                }
                if (_moduleVersion < 1)
                {
                    throw new Exception("Module version is not correct");
                }
                //if (_moduleTargetPath.Length == 0) return false;
                //if (_moduleTempPackagePath.Length == 0) return false;
                //if (_moduleMainPath.Length == 0) return false;

                //if (Directory.Exists(_moduleTargetPath) == false) return false;
                //if (Directory.Exists(_moduleTempPackagePath) == false) return false;



                var regexItem = new Regex("^[a-zA-Z0-9]*$");
                if (regexItem.IsMatch(_moduleName) == false)
                {
                    throw new Exception("Module name contains special character");
                }


                FileSystemHelper.DeleteFiles(this.settingConfigService.TempModulePackagePath);


                FileSystemHelper.CopyFile(_moduleMainPath, this.settingConfigService.TempModulePackagePath + _moduleName + ".dll");



                var dependentDLL = this.settingConfigService.TempModulePackagePath + "dependent" + Path.DirectorySeparatorChar;
                Directory.CreateDirectory(dependentDLL);
                foreach (var file in _dependentCollection)
                {
                    var targetFilePath = dependentDLL + file.FileName;
                    File.Copy(file.FilePath, targetFilePath);
                }


                ModuleConfig config = new ModuleConfig()
                {
                    ModuleComment = _moduleComment,
                    ModuleName = _moduleName,
                    ModuleVersion = _moduleVersion,
                    ModuleModifyDate = _moduleModifyDate
                };


                var targetConfigPath = this.settingConfigService.TempModulePackagePath + _moduleName + ".json";
                string jsonString = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(targetConfigPath, jsonString);


                var targetZipPath = this.settingConfigService.ApplicationSetting.ModulePath + _moduleName + ".module";

                string zipFileName = targetZipPath;
                EncryptionType encryptionType = EncryptionType.Aes256;
                using (ZipArchive archive = new ZipArchive())
                {
                    archive.Password = this.settingConfigService.SecurityPassword;
                    archive.EncryptionType = encryptionType;
                    archive.AddDirectory(this.settingConfigService.TempModulePackagePath, "/");
                    archive.Save(zipFileName);
                }

            }
            catch(Exception e)
            {
                DialogHelper.ShowToastErrorMessage("모듈 패키지", "모듈 패키징 실패 : " + e.Message);
                throw e;
            }
            
        }
    }
}
