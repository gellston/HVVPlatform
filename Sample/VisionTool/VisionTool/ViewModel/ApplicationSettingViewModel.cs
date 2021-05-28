using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Model;
using VisionTool.Service;

namespace VisionTool.ViewModel
{
    public class ApplicationSettingViewModel : ViewModelBase
    {
        private readonly SettingConfigService appConfigService;
        private readonly DiagramControlService diagramControlService;
        public ApplicationSettingViewModel(SettingConfigService _appConfigService,
                                           DiagramControlService _diagramControlService)
        {
            this.appConfigService = _appConfigService;
            this.diagramControlService = _diagramControlService;
            this.AppConfig = this.appConfigService.ApplicationSetting;
            this.DiagramCategoryCollection = this.appConfigService.ApplicationSetting.DiagramCategoryCollection;
            this.DiagramDataTypeCollection = this.appConfigService.ApplicationSetting.DiagramDataTypeCollection;
            this.DiagramPropertyDataTypeCollection = this.appConfigService.ApplicationSetting.DiagramPropertyDataTypeCollection;
        }


        private ApplicationSetting _AppConfig = null;
        public ApplicationSetting AppConfig
        {
            get => _AppConfig;
            set => Set<ApplicationSetting>(nameof(AppConfig), ref _AppConfig, value);
        }

        private ObservableCollection<string> _DiagramDataTypeCollection = null;
        public ObservableCollection<string> DiagramDataTypeCollection
        {
            get => _DiagramDataTypeCollection;
            set => Set(ref _DiagramDataTypeCollection, value);
        }


        private ObservableCollection<string> _DiagramCategoryCollection = null;
        public ObservableCollection<string> DiagramCategoryCollection
        {
            get => _DiagramCategoryCollection;
            set => Set(ref _DiagramCategoryCollection, value);
        }

        private ObservableCollection<string> _DiagramPropertyDataTypeCollection = null;
        public ObservableCollection<string> DiagramPropertyDataTypeCollection
        {
            get => _DiagramPropertyDataTypeCollection;
            set => Set(ref _DiagramPropertyDataTypeCollection, value);
        }



        private string _SelectedDataType = null;
        public string SelectedDataType
        {
            get => _SelectedDataType;
            set => Set(ref _SelectedDataType, value);
        }

        private string _SelectedCategoryType = null;
        public string SelectedCategoryType
        {
            get => _SelectedCategoryType;
            set => Set(ref _SelectedCategoryType, value);
        }

        private string _SelectedPropertyDataType = null;
        public string SelectedPropertyDataType
        {
            get => _SelectedPropertyDataType;
            set => Set(ref _SelectedPropertyDataType, value);
        }


        private string _CurrentDataType = null;
        public string CurrentDataType
        {
            get => _CurrentDataType;
            set => Set(ref _CurrentDataType, value);
        }

        private string _CurrentCategoryType = null;
        public string CurrentCategoryType
        {
            get => _CurrentCategoryType;
            set => Set(ref _CurrentCategoryType, value);
        }

        private string _CurrentPropertyDataType = null;
        public string CurrentPropertyDataType
        {
            get => _CurrentPropertyDataType;
            set => Set(ref _CurrentPropertyDataType, value);
        }


        public ICommand ResetApplicationSettingCommand
        {
            get => new RelayCommand(() =>
            {
                this.appConfigService.ResetApplicationSetting();
                this.AppConfig = this.appConfigService.ApplicationSetting;
                this.DiagramCategoryCollection = this.appConfigService.ApplicationSetting.DiagramCategoryCollection;
                this.DiagramDataTypeCollection = this.appConfigService.ApplicationSetting.DiagramDataTypeCollection;
                this.DiagramPropertyDataTypeCollection = this.appConfigService.ApplicationSetting.DiagramPropertyDataTypeCollection;

                this.diagramControlService.UpdateDiagramInfo();
            });
        }

        public ICommand SaveApplicationSettingCommand
        {
            get => new RelayCommand(() =>
            {
                this.appConfigService.SaveApplicationSetting();
            });
        }

        public ICommand LoadApplicationSettingCommand
        {
            get => new RelayCommand(() =>
            {
                this.appConfigService.LoadApplicationSetting();
                this.diagramControlService.UpdateDiagramInfo();
                
            });
        }

        public ICommand AddNewDiagramDataTypeCommand
        {
            get => new RelayCommand(() =>
            {
                if (this.CurrentDataType != null)
                    this.DiagramDataTypeCollection.Add(this.CurrentDataType);
            });
        }

        public ICommand DeleteDiagramDataTypeCommand
        {
            get => new RelayCommand(() =>
            {
                if (this.SelectedDataType != null)
                    this.DiagramDataTypeCollection.Remove(this.SelectedDataType);

            });
        }

        public ICommand AddNewDiagramCategoryCommand
        {
            get => new RelayCommand(() =>
            {
                if (this.CurrentCategoryType != null)
                    this.DiagramCategoryCollection.Add(CurrentCategoryType);

            });
        }

        public ICommand DeleteDiagramCategoryCommand
        {
            get => new RelayCommand(() =>
            {
                if (this.SelectedCategoryType != null)
                    this.DiagramCategoryCollection.Remove(this.SelectedCategoryType);

            });
        }


        public ICommand AddNewDiagramPropertyDataTypeCommand
        {
            get => new RelayCommand(() =>
            {
                if (this.CurrentPropertyDataType != null)
                    this.DiagramPropertyDataTypeCollection.Add(CurrentPropertyDataType);

            });
        }

        public ICommand DeleteDiagramPropertyDataTypeCommand
        {
            get => new RelayCommand(() =>
            {
                if (this.SelectedPropertyDataType != null)
                    this.DiagramPropertyDataTypeCollection.Remove(this.SelectedPropertyDataType);

            });
        }
    }
}
