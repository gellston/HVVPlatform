using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;
using Model;
using VisionTool.Service;

namespace VisionTool.ViewModel
{
    public class ApplicationSettingViewModel : ViewModelBase
    {
        private readonly SettingConfigService appConfigService;
        public ApplicationSettingViewModel(SettingConfigService _appConfigService)
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
