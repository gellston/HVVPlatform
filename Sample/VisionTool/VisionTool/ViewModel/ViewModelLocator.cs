﻿
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight;
using VisionTool.Service;
using GalaSoft.MvvmLight.Messaging;
using VisionTool.Message;

namespace VisionTool.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {


            SimpleIoc.Default.Register<SettingConfigService>();

            SimpleIoc.Default.Register<ScriptControlService>();
            SimpleIoc.Default.Register<ModuleControlService>();
            SimpleIoc.Default.Register<DiagramControlService>();
            SimpleIoc.Default.Register<SequenceControlService>();
            
            
            
            SimpleIoc.Default.Register<MainWindowViewModel>();
            SimpleIoc.Default.Register<ScriptEditViewModel>();
            SimpleIoc.Default.Register<ModulePackageManagementViewModel>();
            SimpleIoc.Default.Register<ApplicationSettingViewModel>();
            SimpleIoc.Default.Register<DiagramEditViewModel>();
            SimpleIoc.Default.Register<DiagramPackageManagementViewModel>();



            //미리생성
            SimpleIoc.Default.GetInstance<ScriptEditViewModel>();
            SimpleIoc.Default.GetInstance<ModulePackageManagementViewModel>();
            SimpleIoc.Default.GetInstance<DiagramPackageManagementViewModel>();
            SimpleIoc.Default.GetInstance<DiagramEditViewModel>();
            SimpleIoc.Default.GetInstance<ApplicationSettingViewModel>();
            SimpleIoc.Default.GetInstance<MainWindowViewModel>();


            if(ViewModelLocator.IsAssosicationMode == true)
            {
                var message = new AssociationModeMessage()
                {
                    FilePath = ViewModelLocator.AssosicationFilePath,
                    AssociationMode = ViewModelLocator.AssosicationMode,
                    
                };

                switch(message.AssociationMode)
                {
                    case "Script":
                        message.CurrentViewModel = SimpleIoc.Default.GetInstance<ScriptEditViewModel>();
                        break;
                    case "Sequence":
                        message.CurrentViewModel = SimpleIoc.Default.GetInstance<DiagramEditViewModel>();
                        break;
                    case "Module":
                        message.CurrentViewModel = SimpleIoc.Default.GetInstance<ModulePackageManagementViewModel>();
                        break;
                    case "Diagram":
                        message.CurrentViewModel = SimpleIoc.Default.GetInstance<DiagramPackageManagementViewModel>();
                        break;
                }

                Messenger.Default.Send<AssociationModeMessage>(message);
            }

            

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


        public ViewModelBase ScriptEditViewModel
        {
            get
            {
                return SimpleIoc.Default.GetInstance<ScriptEditViewModel>();
            }
        }

        public ViewModelBase ModulePackageManagementViewModel
        {
            get
            {
                return SimpleIoc.Default.GetInstance<ModulePackageManagementViewModel>();
            }
        }

        public ViewModelBase DiagramEditViewModel
        {
            get
            {
                return SimpleIoc.Default.GetInstance<DiagramEditViewModel>();
            }
        }

        public ViewModelBase DiagramPackageManagementViewModel
        {
            get
            {
                return SimpleIoc.Default.GetInstance<DiagramPackageManagementViewModel>();
            }
        }

        public ViewModelBase ApplicationSettingViewModel
        {
            get
            {
                return SimpleIoc.Default.GetInstance<ApplicationSettingViewModel>();
            }
        }


        static public bool IsAssosicationMode { get; set; } = false;
        static public string AssosicationFilePath { get; set; } = "";
        static public string AssosicationMode { get; set; } = "";


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
