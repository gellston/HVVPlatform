using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;
using WPFHVVPlatform.Model;
using WPFHVVPlatform.Service;

namespace WPFHVVPlatform.ViewModel
{
    public class ApplicationSettingViewModel : ViewModelBase
    {
        private readonly AppConfigService appConfigService;
        public ApplicationSettingViewModel(AppConfigService _appConfigService)
        {
            this.appConfigService = _appConfigService;

            this.AppConfig = this.appConfigService.ApplicationSetting;
        }


        private ApplicationSetting _AppConfig = null;
        public ApplicationSetting AppConfig
        {
            get => _AppConfig;
            set => Set<ApplicationSetting>(nameof(AppConfig), ref _AppConfig, value);
        }
    }
}
