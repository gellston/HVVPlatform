
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight;
//using GalaSoft.MvvmLight;
//using GalaSoft.MvvmLight.Ioc;
using WPFHVVPlatform.Service;
using System;
using System.Runtime.InteropServices;

namespace WPFHVVPlatform.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            if (Nativemethods.AllocConsole() == false)
            {
                System.Console.WriteLine("error");
            }

            if (InitializeScript() == false)
            {
                System.Console.WriteLine("error");
            }


            SimpleIoc.Default.Register<AppConfigService>();
            SimpleIoc.Default.Register<FileDialogService>();
            SimpleIoc.Default.Register<MessageDialogService>();
            SimpleIoc.Default.Register<ScriptFileService>();
            //SimpleIoc.Default.Register<>
            
            SimpleIoc.Default.Register<MainWindowViewModel>();
            SimpleIoc.Default.Register<ScriptEditViewModel>();


            SimpleIoc.Default.Register<HV.V1.Interpreter>();
        }

        public ViewModelBase MainWindowViewModel
        {
            get
            {
                return SimpleIoc.Default.GetInstance<MainWindowViewModel>();
            }
        }

        static public bool InitializeScript()
        {
            bool check = true;
            String currentDirecturoy = AppDomain.CurrentDomain.BaseDirectory;
            check = HV.V1.Interpreter.InitV8StartupData(currentDirecturoy);
            HV.V1.Interpreter.InitV8Platform();
            check = HV.V1.Interpreter.InitV8Engine();
            HV.V1.Interpreter.SetV8Flag("--use_strict");
            HV.V1.Interpreter.SetV8Flag("--max_old_space_size=8192");
            HV.V1.Interpreter.SetV8Flag("--expose_gc");

            return check;
        }

        static class Nativemethods
        {
            [DllImport("kernel32")]
            public static extern bool AllocConsole();
        }

    }
}
