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
                Directory.CreateDirectory(this.CurrentApplicationPath + "Config" + Path.DirectorySeparatorChar);
                Directory.CreateDirectory(this.CurrentApplicationPath + "Module" + Path.DirectorySeparatorChar);

                this.ApplicationSetting = new ApplicationSetting()
                {
                    ModulePath = this.CurrentApplicationPath + "Module" + Path.DirectorySeparatorChar,
                    ModuleConfigPath = this.CurrentApplicationPath + "ModuleConfig" + Path.DirectorySeparatorChar,
                    ModuleUnZipPath = this.CurrentApplicationPath + "ModuleUnzip" + Path.DirectorySeparatorChar,
                    ModuleThirdPartyDLLPath = this.CurrentApplicationPath + "ModuleThirdParty" + Path.DirectorySeparatorChar,
                };
            }
        }

        public string CurrentApplicationPath
        {
            get => AppDomain.CurrentDomain.BaseDirectory;
        }

        public string CurrentApplicationSettingPath
        {
            get => this.CurrentApplicationPath + "Config" + Path.DirectorySeparatorChar + "config.xml";
        }

        public ApplicationSetting ApplicationSetting
        {
            get;set;
        }
        
    }
}
