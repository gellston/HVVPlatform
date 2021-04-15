using DevExpress.Compression;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using VisionTool.Helper;
using Model;
using DevExpress.Xpf.CodeView;
using System.Linq;
using System.Collections.Generic;
using System.Windows;

namespace VisionTool.Service
{
    public class ModuleControlService
    {

        private readonly SettingConfigService settingConfigService;
        private Action<string> currentModuleComment;
        private Action<string> currentModuleMainPath;
        private Action<int> currentModuleVersion;
        private Action<string> currentModuleModifyDate;
        private Action<string> currentModuleName;
    

        public ModuleControlService(SettingConfigService _settingConfigService)
        {
            this.settingConfigService = _settingConfigService;


            this.UpdateModuleInfo();
        }

        public void SetCallbackCurrentModuleVersion(Action<int> check)
        {
            this.currentModuleVersion += check;
        }

        public void SetCallbackCurrentModuleName(Action<string> check)
        {
            this.currentModuleName += check;
        }

        public void SetCallbackCurrentModuleModifyDate(Action<string> check)
        {
            this.currentModuleModifyDate += check;
        }

        public void SetCallbackCurrentModuleMainPath(Action<string> check)
        {
            this.currentModuleMainPath += check;
        }
        public void SetCallbackCurrentModuleComment(Action<string> check)
        {
            this.currentModuleComment += check;
        }



        private ObservableCollection<ModuleConfig> _ModuleConfigCollection = new ObservableCollection<ModuleConfig>();
        public ObservableCollection<ModuleConfig> ModuleConfigCollection
        {
            get
            {
                return _ModuleConfigCollection;
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



        private ObservableCollection<Module> _ModuleCollection = new ObservableCollection<Module>();
        public ObservableCollection<Module> ModuleCollection
        {
            get
            {
                return _ModuleCollection;
            }
        }

        public void LoadModuleInfoFromConfig(ModuleConfig config)
        {


            try
            {
                if (config == null) return;
                
                FileSystemHelper.DeleteFiles(this.settingConfigService.TempModuleModPackagePath);
               

                var diagramPath = this.settingConfigService.ApplicationSetting.ModulePath + config.ModuleName + ".module";
                FileSystemHelper.UnZipFile(diagramPath,
                                           this.settingConfigService.TempModuleModPackagePath,
                                           this.settingConfigService.SecurityPassword);

                

                this.currentModuleModifyDate.Invoke(config.ModuleModifyDate);
                this.currentModuleComment.Invoke(config.ModuleComment);
                this.currentModuleMainPath.Invoke(this.settingConfigService.TempModuleModPackagePath + config.ModuleName + ".dll");
                this.currentModuleName.Invoke(config.ModuleName);
                this.currentModuleVersion.Invoke(config.ModuleVersion);

                var files = Directory.GetFiles(this.settingConfigService.TempModuleModPackagePath + "dependent" + Path.DirectorySeparatorChar).ToList();
                var dependentDLLList = new List<DependentDLL>();
                foreach(var file in files)
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
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
           
            

        }


        public void ImportModule()
        {
            try
            {
                var path = DialogHelper.OpenFile("module file (.module)|*.module");
                var fileName = Path.GetFileName(path);

                var targetPath = settingConfigService.ApplicationSetting.ModulePath + fileName;

                if (File.Exists(targetPath) == true)
                {
                    if (DialogHelper.ShowConfirmMessage("모듈이 존재합니다. 덮어쓰시겠습니까?") == false)
                    {
                        return;
                    }

                    
                }
                File.Copy(path, targetPath, true);
                this.UpdateModuleInfo();

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        public void ImportModule(string path)
        {
            try
            {
                var fileName = Path.GetFileName(path);

                var targetPath = settingConfigService.ApplicationSetting.ModulePath + fileName;

                if (File.Exists(targetPath) == true)
                {
                    if (DialogHelper.ShowConfirmMessage("모듈이 존재합니다. 덮어쓰시겠습니까?") == false)
                    {
                        return;
                    }

                    
                }
                File.Copy(path, targetPath, true);
                this.UpdateModuleInfo();

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


        public void LoadLibraryFromPath()
        {

            try
            {
                var maindll = DialogHelper.OpenFile("library file (.dll)|*.dll");
                if (maindll.Length == 0) throw new Exception("File is not selected");

                this.currentModuleMainPath.Invoke(maindll);

            }
            catch(Exception e)
            {
                throw e;
            }

        }

        public void ClearMoudle()
        {
            this.currentModuleComment.Invoke("");
            this.currentModuleMainPath.Invoke("");
            this.currentModuleModifyDate.Invoke("");
            this.currentModuleName.Invoke("");
            this.currentModuleVersion.Invoke(1);
           


            this.DependentDLLCollection.Clear();
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
                
                foreach(var library in dependentlibrary)
                {
                    thirdlibrary.Add(new DependentDLL()
                    {
                        FileName = Path.GetFileName(library),
                        FilePath = library
                    });
                }

                this.DependentDLLCollection.AddRange(thirdlibrary);
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
                        System.Diagnostics.Debug.WriteLine(e.Message);
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
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }

                FileSystemHelper.DeleteFiles(this.settingConfigService.TempModulePackagePath);
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

        }


        public bool CheckModuleExists(string _moduleName)
        {
            if (this.ModuleConfigCollection.ToList().Exists(x => x.ModuleName == _moduleName) == true)
                return true;
            else return false;
        }

        public void CreateModulePackage(string _moduleName, 
                                        string _moduleModifyDate, 
                                        int _moduleVersion,
                                        string _moduleComment, 
                                        string _moduleMainPath,
                                        bool _isOverwrite)
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

                if (this.ModuleConfigCollection.ToList().Exists(x => x.ModuleName == _moduleName) && _isOverwrite == false)
                    throw new Exception("Module name is already exists");

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
                foreach (var file in this.DependentDLLCollection)
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


                this.UpdateModuleInfo();

            }
            catch(Exception e)
            {
                DialogHelper.ShowToastErrorMessage("모듈 패키지", "모듈 패키징 실패 : " + e.Message);
                throw e;
            }
            
        }
    }
}
