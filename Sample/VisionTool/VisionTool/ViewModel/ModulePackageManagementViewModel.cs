using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows.Input;
using DevExpress.Xpf.CodeView;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Model;
using VisionTool.Service;

namespace VisionTool.ViewModel
{
    public class ModulePackageManagementViewModel : ViewModelBase
    {

        private readonly SettingConfigService appConfigService;
        private readonly ModuleControlService modulePackageService;
        private readonly ScriptControlService scriptControlService;

        public ModulePackageManagementViewModel(SettingConfigService _appConfigService,
                                                ModuleControlService _modulePackageService,
                                                ScriptControlService _scriptControlService)
        {
            this.appConfigService = _appConfigService;
            this.modulePackageService = _modulePackageService;
            this.scriptControlService = _scriptControlService;

            this.ModuleCollection = this.modulePackageService.ModuleCollection;
            this.ModuleConfigCollection = this.modulePackageService.ModuleConfigCollection;

            

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
                try
                {
                    this.modulePackageService.DeleteModule(this.SelectedModule);
                    this.scriptControlService.ClearNativeModules();
                    this.modulePackageService.UpdateModuleInfo();

                }catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }

            });
        }

        public ICommand ReLoadModuleCommand
        {
            get => new RelayCommand(() =>
            {
                this.scriptControlService.ClearNativeModules();
                this.modulePackageService.UpdateModuleInfo();
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
                try
                {
                    this.ModuleMainPath = this.modulePackageService.GetLibraryFromPath();
                }catch(Exception e)
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
                    this.DependentDLLCollection.Clear();
                    this.DependentDLLCollection.AddRange(this.modulePackageService.Get3rdLibrariesFromPath());


                }catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }

            });
        }

        public ICommand PackageModuleCommand
        {
            get => new RelayCommand(() =>
            {

                this.ModuleModifyDate = DateTime.Now.ToString("yyyy-MM-HH hh:mm:ss");

                try
                {
                    this.modulePackageService.CreateModulePackage(this.ModuleName,
                                                                  this.ModuleModifyDate,
                                                                  this.ModuleVersion,
                                                                  this.ModuleComment,
                                                                  this.ModuleMainPath,
                                                                  this.DependentDLLCollection);

                    this.modulePackageService.UpdateModuleInfo();
                    

                }catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
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
            get => _ModuleConfigCollection;
            set => Set(ref _ModuleConfigCollection, value);
        }

        private ObservableCollection<Module> _ModuleCollection = null;
        public ObservableCollection<Module> ModuleCollection
        {
            get => _ModuleCollection;
            set => Set(ref _ModuleCollection, value);
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
