using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WPFHVVPlatform.Model;

namespace WPFHVVPlatform.Service
{
    public class AppConfigService
    {
        public AppConfigService()
        {



            
            if (File.Exists(this.CurrentApplicationSettingPath) == false)
            {
                Directory.CreateDirectory(this.CurrentApplicationLocation + "Config" + Path.DirectorySeparatorChar);
                Directory.CreateDirectory(this.CurrentApplicationLocation + "Module" + Path.DirectorySeparatorChar);

                this.ApplicationSetting = new ApplicationSetting()
                {
                    ModuleLocation = this.CurrentApplicationLocation + "Module" + Path.DirectorySeparatorChar
                };
            }
        }

        public string CurrentApplicationLocation
        {
            get => AppDomain.CurrentDomain.BaseDirectory;
        }

        public string CurrentApplicationSettingPath
        {
            get => this.CurrentApplicationLocation + "Config" + Path.DirectorySeparatorChar + "config.xml";
        }

        public ApplicationSetting ApplicationSetting
        {
            get;set;
        }
        
    }
}
