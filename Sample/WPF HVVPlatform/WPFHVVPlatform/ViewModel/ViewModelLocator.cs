using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using WPFHVVPlatform.Service;

namespace WPFHVVPlatform.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            SimpleIoc.Default.Register<AppConfigService>();

            SimpleIoc.Default.Register<MainWindowViewModel>();
            SimpleIoc.Default.Register<ScriptEditViewModel>();
            
        }

        public ViewModelBase MainWindowViewModel
        {
            get
            {
                return SimpleIoc.Default.GetInstance<MainWindowViewModel>();
            }
        }
         
    }
}
