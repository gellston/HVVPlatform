using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using VisionTool.View;

namespace VisionTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }

        protected override void OnStartup(StartupEventArgs e)
        {

            ActiproSoftware.Products.ActiproLicenseManager.RegisterLicense("hyvision system", "WPF211-6PW5H-108N4-0W3L4-0KCG");

            base.OnStartup(e);



            Helper.Extensions.EnsureAssociationsSet();


            String[] arguments = Environment.GetCommandLineArgs();


            // File Association Model On (스크립트 파일)
            if (arguments.GetLength(0) > 1)
            {
                foreach(var args in arguments)
                {
                    if (args.EndsWith(".vsjs"))
                    {
                        if (File.Exists(args))
                        {
                            string filePathFormMainArgs = arguments[1];
                            VisionTool.ViewModel.ViewModelLocator.IsAssosicationMode = true;
                            VisionTool.ViewModel.ViewModelLocator.AssosicationFilePath = filePathFormMainArgs;
                            VisionTool.ViewModel.ViewModelLocator.AssosicationMode = "Script";
                        }
                    }else if (args.EndsWith(".vsseq"))
                    {
                        if (File.Exists(args))
                        {
                            string filePathFormMainArgs = arguments[1];
                            VisionTool.ViewModel.ViewModelLocator.IsAssosicationMode = true;
                            VisionTool.ViewModel.ViewModelLocator.AssosicationFilePath = filePathFormMainArgs;
                            VisionTool.ViewModel.ViewModelLocator.AssosicationMode = "Sequence";
                        }
                    }
                    else if (args.EndsWith(".module"))
                    {
                        if (File.Exists(args))
                        {
                            string filePathFormMainArgs = arguments[1];
                            VisionTool.ViewModel.ViewModelLocator.IsAssosicationMode = true;
                            VisionTool.ViewModel.ViewModelLocator.AssosicationFilePath = filePathFormMainArgs;
                            VisionTool.ViewModel.ViewModelLocator.AssosicationMode = "Module";
                        }
                    }
                    else if (args.EndsWith(".diagram"))
                    {
                        if (File.Exists(args))
                        {
                            string filePathFormMainArgs = arguments[1];
                            VisionTool.ViewModel.ViewModelLocator.IsAssosicationMode = true;
                            VisionTool.ViewModel.ViewModelLocator.AssosicationFilePath = filePathFormMainArgs;
                            VisionTool.ViewModel.ViewModelLocator.AssosicationMode = "Diagram";
                        }
                    }

                }
            }


            // 멀티 인스턴스 실행 모드 온 
            bool isMultipleInstanceModel = false;
            if (arguments.GetLength(0) > 1)
            {
                foreach(var args in arguments)
                {
                    if (args == "/multi")
                        isMultipleInstanceModel = true;
                }
            }



            //중복 실행 방지 
            Process proc = Process.GetCurrentProcess();
            int count = Process.GetProcesses().Where(p =>
                p.ProcessName == proc.ProcessName).Count();

            if (count > 1 && isMultipleInstanceModel == false)
            {
                App.Current.Shutdown();
            }




            var splashViewModel = new DevExpress.Mvvm.DXSplashScreenViewModel
            {
                IsIndeterminate = true,
                Title = "HyVisionTeam Tool",
                Subtitle = "HVision Tool 1.0",
                Progress = 0,
                Status = "",
                Copyright = "© 2011 HyVisionSystem Vision Team All Rights Reserved"
            };
            SplashScreenManager.Create(() => new ModuleLoadingSplashWindow(), splashViewModel).ShowOnStartup();



        }

    }


    

}
