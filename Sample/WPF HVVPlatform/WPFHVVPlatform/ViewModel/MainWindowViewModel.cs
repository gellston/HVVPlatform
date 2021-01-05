
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using WPFHVVPlatform.Model;
using WPFHVVPlatform.Service;

namespace WPFHVVPlatform.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {

        private readonly AppConfigService configService;

        public MainWindowViewModel(AppConfigService _configService,
                                   ScriptEditViewModel _scriptEditViewModel)
        {
            this.configService = _configService;

            this.MainMenuCollection.Add(new MainMenu()
            {

                Icon = WpfSvgRenderer.CreateImageSource(SvgImageHelper.CreateImage(new Uri("pack://application:,,,/DevExpress.Images.v20.4;component/SvgImages/Business Objects/BO_Opportunity.svg")), 1d, null, null, true),
                Name = "스크립트 편집",
                MenuAction = new RelayCommand(() =>
                {

                })

            });

            // 첫번째 메뉴 추가 
            this.CurrentContentViewModel = this.MainMenuCollection[0];

        }


        private ObservableCollection<MainMenu> _MainMenuCollection = null;
        ObservableCollection<MainMenu> MainMenuCollection
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
    }
}
