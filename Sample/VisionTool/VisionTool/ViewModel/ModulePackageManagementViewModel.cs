using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using VisionTool.Model;
using VisionTool.Service;

namespace VisionTool.ViewModel
{
    public class ModulePackageManagementViewModel : ViewModelBase
    {

        private readonly AppConfigService appConfigService;
        private readonly ModulePackageService modulePackageService;
        private readonly MessageDialogService messageDialogService;
        private readonly FileDialogService fileDialogService;
       

        public ModulePackageManagementViewModel(AppConfigService _appConfigService,
                                                ModulePackageService _modulePackageService,
                                                MessageDialogService _messageDialogService,
                                                FileDialogService _fileDialogService)
        {
            this.appConfigService = _appConfigService;
            this.modulePackageService = _modulePackageService;
            this.messageDialogService = _messageDialogService;
            this.fileDialogService = _fileDialogService;


            MessengerInstance.Register<NotificationMessage>(this, NotifyMessage);
        }

        public void NotifyMessage(NotificationMessage message)
        {
            if (message.Notification == "UpdateModule")
            {

                
                this.ModuleCollection.Clear();
                this.ModuleConfigCollection.Clear();

                this.modulePackageService.DeleteAllFiles(this.appConfigService.ApplicationSetting.ModuleConfigPath);
                this.modulePackageService.DeleteAllFiles(this.appConfigService.ApplicationSetting.ModuleMainPath);
                this.modulePackageService.DeleteAllFiles(this.appConfigService.ApplicationSetting.ModuleThirdPartyDLLPath);
                
                
                var files = Directory.GetFiles(this.appConfigService.ApplicationSetting.ModulePath, "*.module");
                foreach(var file in files)
                {
                    this.modulePackageService.DeleteAllFiles(this.appConfigService.TempModulePackagePath);
                    if (this.modulePackageService.UnzipModule(file, 
                                                              appConfigService.TempModulePackagePath, 
                                                              appConfigService.SecurityPassword) == false) continue;


                    var mainDlls = Directory.GetFiles(appConfigService.TempModulePackagePath, "*.dll");
                    var configFiles = Directory.GetFiles(appConfigService.TempModulePackagePath, "*.json");
                    var depedentDLLs = Directory.GetFiles(appConfigService.TempModulePackagePath + "dependent" + Path.DirectorySeparatorChar , "*.dll");

                    foreach(var maindllFile in mainDlls)
                    {
                        File.Copy(maindllFile, appConfigService.ApplicationSetting.ModuleMainPath + Path.GetFileName(maindllFile), true);
                    }

                    foreach (var dependenDLL in depedentDLLs)
                    {
                        File.Copy(dependenDLL, appConfigService.ApplicationSetting.ModuleThirdPartyDLLPath + Path.GetFileName(dependenDLL), true);
                    }

                    foreach (var configFile in configFiles)
                    {
                        File.Copy(configFile, appConfigService.ApplicationSetting.ModuleConfigPath + Path.GetFileName(configFile), true);
                    }

                    this.ModuleCollection.Add(new Module()
                    {
                        FilePath = file,
                        FileName = Path.GetFileName(file)
                    });
                }


                ModuleConfigCollection = modulePackageService.LoadAllModuleConfig(this.appConfigService.ApplicationSetting.ModuleConfigPath);
                
            }
        }

        public ICommand ImportModuleCommand
        {
            get => new RelayCommand(() =>
            {

            });
        }

        public ICommand ModifyModuleCommand
        {
            get => new RelayCommand(() =>
            {

            });
        }

        public ICommand DeleteModuleCommand
        {
            get => new RelayCommand(() =>
            {
                if (this.SelectedModule == null)
                {
                    messageDialogService.ShowToastErrorMessage("모듈 패키지", "모듈이 선택되지 않았습니다.");
                    return;
                }
                    
                if(File.Exists(this.SelectedModule.FilePath) == false)
                {
                    messageDialogService.ShowToastErrorMessage("모듈 패키지", "모듈이 선택되지 않았습니다.");
                    return;
                }


                File.Delete(this.SelectedModule.FilePath);

                this.ModuleCollection.Remove(this.SelectedModule);
                this.SelectedModule = null;


                MessengerInstance.Send<NotificationMessage>(new NotificationMessage(this, "ClearNativeModules"));
                MessengerInstance.Send<NotificationMessage>(new NotificationMessage(this, "UpdateModule"));

            });
        }

        public ICommand ReLoadModuleCommand
        {
            get => new RelayCommand(() =>
            {

                MessengerInstance.Send<NotificationMessage>(new NotificationMessage(this, "ClearNativeModules"));
                MessengerInstance.Send<NotificationMessage>(new NotificationMessage(this, "UpdateModule"));
            });
        }

        public ICommand ClearModuleCommand
        {
            get => new RelayCommand(() =>
            {

                this.ModuleComment = "";
                this.ModuleMainPath = "";
                this.ModuleVersion = 1;
                this.ModuleModifyDate = "";
                this.ModuleName = "";
                this.DependentDLLCollection.Clear();

            });
        }


        public ICommand AddMainDLLCommand
        {
            get => new RelayCommand(() =>
            {
                var maindll = this.fileDialogService.OpenFile("Script File (.dll)|*.dll");
                if (maindll.Length == 0) return;


                this.ModuleMainPath = maindll;

            });
        }

        public ICommand AddDependentDLLCommand
        {
            get => new RelayCommand(() =>
            {
                var dependentdll_list = this.fileDialogService.OpenFiles("Script File (.dll)|*.dll");
                if (dependentdll_list == null) return;
                if (dependentdll_list.Length == 0) return;


                foreach(var dependentdll in dependentdll_list)
                {
                    this.DependentDLLCollection.Add(new DependentDLL()
                    {
                        FileName = Path.GetFileName(dependentdll),
                        FilePath = dependentdll
                    });

                }


            });
        }

        public ICommand PackageModuleCommand
        {
            get => new RelayCommand(() =>
            {

                this.ModuleModifyDate = DateTime.Now.ToString("yyyy-MM-HH hh:mm:ss");
                bool check = this.modulePackageService.CreateModulePackage(this.ModuleName,
                                                                           this.ModuleModifyDate,
                                                                           this.ModuleMainPath,
                                                                           this.ModuleVersion,
                                                                           this.ModuleComment,
                                                                           this.DependentDLLCollection,
                                                                           this.appConfigService.ApplicationSetting.ModulePath,
                                                                           this.appConfigService.TempModulePackagePath,
                                                                           this.appConfigService.SecurityPassword);


                if (check == true)
                {
                    messageDialogService.ShowToastSuccessMessage("모듈 패키지", "모듈 패키징 완료");
         
                }
                else
                {
                    messageDialogService.ShowToastErrorMessage("모듈 패키지", "모듈 패키징 실패");
                    
                }

            });
        }

        private string _ModuleName = "";
        public string ModuleName
        {
            get => _ModuleName;
            set => Set<string>(nameof(ModuleName), ref _ModuleName, value);
        }

        private string _ModuleModifyDate = "";
        public string ModuleModifyDate
        {
            get => _ModuleModifyDate;
            set => Set<string>(nameof(ModuleModifyDate), ref _ModuleModifyDate, value);
        }

        private string _ModuleMainPath = "";
        public string ModuleMainPath
        {
            get => _ModuleMainPath;
            set => Set<string>(nameof(ModuleMainPath), ref _ModuleMainPath, value);
        }

        private int _ModuleVersion= 1;
        public int ModuleVersion { 
            get => _ModuleVersion;
            set => Set<int>(nameof(ModuleVersion), ref _ModuleVersion, value);
        }

        private string _ModuleComment = "";
        public string ModuleComment
        {
            get => _ModuleComment;
            set => Set<string>(nameof(ModuleComment), ref _ModuleComment, value);
        }

        private ObservableCollection<DependentDLL> _DependentDLLCollection = null;
        public ObservableCollection<DependentDLL> DependentDLLCollection
        {
            get
            {
                if (_DependentDLLCollection == null)
                    _DependentDLLCollection = new ObservableCollection<DependentDLL>();
                return _DependentDLLCollection;
            }
            
        }

        private ObservableCollection<ModuleConfig> _ModuleConfigCollection = null;
        public ObservableCollection<ModuleConfig> ModuleConfigCollection
        {
            get
            {
                if (_ModuleConfigCollection == null)
                    _ModuleConfigCollection = new ObservableCollection<ModuleConfig>();
                return _ModuleConfigCollection;
            }
            set => Set(ref _ModuleConfigCollection, value);
        }

        private ObservableCollection<Module> _ModuleCollection = null;
        public ObservableCollection<Module> ModuleCollection
        {
            get
            {
                if (_ModuleCollection == null)
                    _ModuleCollection = new ObservableCollection<Module>();
                return _ModuleCollection;
            }
        }


        private Module _SelectedModule = null;
        public Module SelectedModule
        {
            get => _SelectedModule;
            set => Set(ref _SelectedModule, value);
        }

        private ModuleConfig _SelectedModuleConfig = null;
        public ModuleConfig SelectedModuleConfig
        {
            get => _SelectedModuleConfig;
            set => Set(ref _SelectedModuleConfig, value);
        }

    }
}
