using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using WPFHVVPlatform.Model;

namespace WPFHVVPlatform.ViewModel
{
    public class ModulePackageManagementViewModel : ViewModelBase
    {

        public ModulePackageManagementViewModel()
        {

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

            });
        }

        public ICommand RelfreshModuleCommand
        {
            get => new RelayCommand(() =>
             {

             });
        }

        public ICommand AddMainDLLCommand
        {
            get => new RelayCommand(() =>
            {

            });
        }

        public ICommand AddDependentDLLCommand
        {
            get => new RelayCommand(() =>
            {

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

        private string _ModuleVersion= "";
        public string ModuleVersion { 
            get => _ModuleVersion;
            set => Set<string>(nameof(ModuleVersion), ref _ModuleVersion, value);
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
    }
}
