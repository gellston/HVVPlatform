
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using VisionTool.Model;
using VisionTool.Service;


namespace VisionTool.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {

        private readonly AppConfigService appConfigService;
        
        public MainWindowViewModel(ScriptEditViewModel _scriptEditViewModel,
                                   AppConfigService _appConfigService,
                                   ModulePackageManagementViewModel _modulePackageViewModel,
                                   ApplicationSettingViewModel _applicationSettingViewModel,
                                   DiagramEditViewModel _diagramEditViewModel)
        {

            this.appConfigService = _appConfigService;
            //this.CurrentContentViewModel = _scriptEditViewModel;
            this.MainMenuCollection.Add(new MainMenu()
            {
                Icon = WpfSvgRenderer.CreateImageSource(SvgImageHelper.CreateImage(new Uri("pack://application:,,,/DevExpress.Images.v20.2;component/SvgImages/XAF/Action_ShowScript.svg")), 1d, null, null, true),
                Name = "스크립트 편집",
                ViewModel = _scriptEditViewModel
            });

            this.MainMenuCollection.Add(new MainMenu()
            {
                Icon = WpfSvgRenderer.CreateImageSource(SvgImageHelper.CreateImage(new Uri("pack://application:,,,/DevExpress.Images.v20.2;component/SvgImages/Icon Builder/Business_Diagram.svg")), 1d, null, null, true),
                Name = "다이어그램 편집",
                ViewModel = _diagramEditViewModel
            });


            this.MainMenuCollection.Add(new MainMenu()
            {
                Icon = WpfSvgRenderer.CreateImageSource(SvgImageHelper.CreateImage(new Uri("pack://application:,,,/DevExpress.Images.v20.2;component/SvgImages/Icon Builder/Shopping_Box.svg")), 1d, null, null, true),
                Name = "모듈 패키지",
                ViewModel = _modulePackageViewModel
            });

            this.MainMenuCollection.Add(new MainMenu()
            {
                Icon = WpfSvgRenderer.CreateImageSource(SvgImageHelper.CreateImage(new Uri("pack://application:,,,/DevExpress.Images.v20.2;component/SvgImages/XAF/ModelEditor_Settings.svg")), 1d, null, null, true),
                Name = "어플리케이션 설정",
                ViewModel = _applicationSettingViewModel
            });

            


            this.CurrentContentViewModel = this.MainMenuCollection[0];


        }


        private ObservableCollection<MainMenu> _MainMenuCollection = null;
        public ObservableCollection<MainMenu> MainMenuCollection
        {
            get
            {
                if (_MainMenuCollection == null)
                    _MainMenuCollection = new ObservableCollection<MainMenu>();

                return _MainMenuCollection;
            }

        }

        private MainMenu _CurrentContentViewModel = null;
        public MainMenu CurrentContentViewModel
        {
            get => _CurrentContentViewModel;
            set => Set<MainMenu>(nameof(CurrentContentViewModel), ref _CurrentContentViewModel, value);

        }

        public ICommand OpenMainMenuCommand
        {
            get => new RelayCommand(() =>
            {
                this.IsOpenMenu = !this.IsOpenMenu;
            });
        }


        private bool _IsOpenMenu = false;
        public bool IsOpenMenu
        {
            get => _IsOpenMenu;
            set => Set<bool>(nameof(IsOpenMenu), ref _IsOpenMenu, value);
        }
    }
}
