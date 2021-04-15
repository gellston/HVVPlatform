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
using VisionTool.Message;
using VisionTool.Service;

namespace VisionTool.ViewModel
{
    public class ModulePackageManagementViewModel : ViewModelBase
    {

        
        private readonly ModuleControlService moduleControlService;
        private readonly ScriptControlService scriptControlService;

        public ModulePackageManagementViewModel(ModuleControlService _moduleControlService,
                                                ScriptControlService _scriptControlService)
        {
        
            this.moduleControlService = _moduleControlService;
            this.scriptControlService = _scriptControlService;

            this.ModuleCollection = this.moduleControlService.ModuleCollection;
            this.ModuleConfigCollection = this.moduleControlService.ModuleConfigCollection;
            this.DependentDLLCollection = this.moduleControlService.DependentDLLCollection;
          


            this.moduleControlService.SetCallbackCurrentModuleComment(data => this.ModuleComment = data);
            this.moduleControlService.SetCallbackCurrentModuleMainPath(data => this.ModuleMainPath = data);
            this.moduleControlService.SetCallbackCurrentModuleModifyDate(data => this.ModuleModifyDate = data);
            this.moduleControlService.SetCallbackCurrentModuleName(data => this.ModuleName = data);
            this.moduleControlService.SetCallbackCurrentModuleVersion(data => this.ModuleVersion = data);

            this.MessengerInstance.Register<AssociationModeMessage>(this, FileAssociationCallback);
        }

        private void FileAssociationCallback(AssociationModeMessage message)
        {
            if (message.AssociationMode == "Module")
            {

                try
                {
                    this.moduleControlService.ImportModule(message.FilePath);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }


            }
        }

        public ICommand ImportModuleCommand
        {
            get => new RelayCommand(() =>
            {
                this.moduleControlService.ImportModule();
            });
        }

        public ICommand ModifyUpdateModuleCommand
        {
            get => new RelayCommand(() =>
            {
                if (this.SelectedModuleConfig == null) return;

                try
                {
                    
                    this.moduleControlService.LoadModuleInfoFromConfig(this.SelectedModuleConfig);

                    this.RaisePropertyChanged("DependentDLLCollection");
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }

            });
        }

        public ICommand ModifyModuleCommand
        {
            get => new RelayCommand(() =>
            {
                try
                {
                    this.ModuleModifyDate = DateTime.Now.ToString("yyyy-MM-HH hh:mm:ss");

                    try
                    {
                        if (this.moduleControlService.CheckModuleExists(this.ModuleName) == false) return;
                        this.scriptControlService.ClearNativeModules();
                        this.moduleControlService.CreateModulePackage(this.ModuleName,
                                                                      this.ModuleModifyDate,
                                                                      this.ModuleVersion + 1,
                                                                      this.ModuleComment,
                                                                      this.ModuleMainPath,
                                                                      true);




                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                    }
                }
                catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            });
        }

        public ICommand DeleteModuleCommand
        {
            get => new RelayCommand(() =>
            {
                try
                {
                    this.moduleControlService.DeleteModule(this.SelectedModule);
                    this.scriptControlService.ClearNativeModules();
                    this.moduleControlService.UpdateModuleInfo();

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
                this.moduleControlService.UpdateModuleInfo();
            });
        }

        public ICommand DeleteDependentDLLCommand
        {
            get => new RelayCommand<DependentDLL>((dll) =>
            {
                this.moduleControlService.DeleteDeleteDependentDLL(dll);

            });
        }

        public ICommand ClearModuleCommand
        {
            get => new RelayCommand(() =>
            {

                this.moduleControlService.ClearMoudle();

            });
        }


        public ICommand AddMainDLLCommand
        {
            get => new RelayCommand(() =>
            {
                try
                {
                    this.moduleControlService.LoadLibraryFromPath();
                }
                catch(Exception e)
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
                    this.moduleControlService.Load3rdLibrariesFromPath();


                }
                catch(Exception e)
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
                    this.scriptControlService.ClearNativeModules();
                    this.moduleControlService.CreateModulePackage(this.ModuleName,
                                                                  this.ModuleModifyDate,
                                                                  this.ModuleVersion,
                                                                  this.ModuleComment,
                                                                  this.ModuleMainPath,
                                                                  false);


                    this.scriptControlService.ClearNativeModules();
                    this.moduleControlService.UpdateModuleInfo();

                }
                catch(Exception e)
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
            get => _DependentDLLCollection;
            set => Set(ref _DependentDLLCollection, value);
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
