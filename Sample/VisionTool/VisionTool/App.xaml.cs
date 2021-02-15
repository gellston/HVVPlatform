using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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

            base.OnStartup(e);


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
