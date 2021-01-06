
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using WPFHVVPlatform.Model;
using WPFHVVPlatform.Service;

namespace WPFHVVPlatform.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {

        private readonly AppConfigService appConfigService;
        
        public MainWindowViewModel(ScriptEditViewModel _scriptEditViewModel,
                                   AppConfigService _appConfigService)
        {

            this.appConfigService = _appConfigService;
            this.CurrentContentViewModel = _scriptEditViewModel;
            this.MainMenuCollection.Add(new MainMenu()
            {

                Icon = WpfSvgRenderer.CreateImageSource(SvgImageHelper.CreateImage(new Uri("pack://application:,,,/DevExpress.Images.v20.2;component/SvgImages/XAF/Action_ShowScript.svg")), 1d, null, null, true),
                Name = "스크립트 편집",
                MenuAction = new RelayCommand(() =>
                {
                    this.CurrentContentViewModel = _scriptEditViewModel;
                })

            });



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

        private ViewModelBase _CurrentContentViewModel = null;
        public ViewModelBase CurrentContentViewModel
        {
            get => _CurrentContentViewModel;
            set => Set<ViewModelBase>(nameof(CurrentContentViewModel), ref _CurrentContentViewModel, value);

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
