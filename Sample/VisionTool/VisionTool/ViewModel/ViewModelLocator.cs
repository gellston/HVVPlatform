
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight;
//using GalaSoft.MvvmLight;
//using GalaSoft.MvvmLight.Ioc;
using VisionTool.Service;
using System;
using System.Runtime.InteropServices;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;


namespace VisionTool.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {

            //if (InitializeScript() == false)
            //{
            //    System.Console.WriteLine("error");
            //}

         
            



            SimpleIoc.Default.Register<SettingConfigService>();
            //SimpleIoc.Default.Register<FileDialogService>();
            //SimpleIoc.Default.Register<DialogHelper>();
            SimpleIoc.Default.Register<ScriptControlService>();
            SimpleIoc.Default.Register<ModuleControlService>();
            SimpleIoc.Default.Register<DiagramControlService>();
            //SimpleIoc.Default.Register<DiagramEditService>();
            
            
            
            SimpleIoc.Default.Register<MainWindowViewModel>();
            SimpleIoc.Default.Register<ScriptEditViewModel>();
            SimpleIoc.Default.Register<ModulePackageManagementViewModel>();
            SimpleIoc.Default.Register<ApplicationSettingViewModel>();
            SimpleIoc.Default.Register<DiagramEditViewModel>();
            SimpleIoc.Default.Register<DiagramPackageManagementViewModel>();


            //SimpleIoc.Default.Register<HV.V1.Interpreter>();




            //미리생성
            SimpleIoc.Default.GetInstance<ModulePackageManagementViewModel>();
            SimpleIoc.Default.GetInstance<DiagramPackageManagementViewModel>();
            SimpleIoc.Default.GetInstance<DiagramEditViewModel>();


            Messenger.Default.Send<NotificationMessage>(new NotificationMessage("UpdateModule"));
            Messenger.Default.Send<NotificationMessage>(new NotificationMessage("InitialDiagramCollection"));
            Messenger.Default.Send<NotificationMessage>(new NotificationMessage("ReloadDiagramCollection"));

            

        }

        ~ViewModelLocator()
        {
           


        }

        public ViewModelBase MainWindowViewModel
        {
            get
            {
                return SimpleIoc.Default.GetInstance<MainWindowViewModel>();
            }
        }

        public ViewModelBase ModulePackageManagementViewModel
        {
            get
            {
                return SimpleIoc.Default.GetInstance<ModulePackageManagementViewModel>();
            }
        }

        public ViewModelBase ScriptEditViewModel
        {
            get
            {
                return SimpleIoc.Default.GetInstance<ScriptEditViewModel>();
            }
        }

        //static public bool InitializeScript()
        //{
        //    bool check = true;
        //    String currentDirecturoy = AppDomain.CurrentDomain.BaseDirectory;
        //    check = HV.V1.Interpreter.InitV8StartupData(currentDirecturoy);
        //    HV.V1.Interpreter.InitV8Platform();
        //    check = HV.V1.Interpreter.InitV8Engine();
        //    HV.V1.Interpreter.SetV8Flag("--use_strict");
        //    HV.V1.Interpreter.SetV8Flag("--max_old_space_size=8192");
        //    HV.V1.Interpreter.SetV8Flag("--expose_gc");

        //    return check;
        //}

        //static class Nativemethods
        //{
        //    [DllImport("kernel32")]
        //    public static extern bool AllocConsole();
        //}

    }
}
