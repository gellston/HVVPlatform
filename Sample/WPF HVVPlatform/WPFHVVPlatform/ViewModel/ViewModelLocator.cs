
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight;
//using GalaSoft.MvvmLight;
//using GalaSoft.MvvmLight.Ioc;
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

            //var test = SimpleIoc.Default.GetInstance<MainWindowViewModel>();
            
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
