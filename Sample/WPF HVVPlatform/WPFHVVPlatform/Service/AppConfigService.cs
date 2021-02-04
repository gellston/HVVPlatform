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




            if (File.Exists(this.CurrentApplicationSettingFilePath) == false)
            {
                this.ApplicationSetting = new ApplicationSetting()
                {
                    ModulePath = this.CurrentApplicationPath + "Module" + Path.DirectorySeparatorChar,
                    ModuleConfigPath = this.CurrentApplicationPath + "ModuleConfig" + Path.DirectorySeparatorChar,
                    ModuleMainPath = this.CurrentApplicationPath + "ModuleMain" + Path.DirectorySeparatorChar,
                    ModuleThirdPartyDLLPath = this.CurrentApplicationPath + "ModuleThirdParty" + Path.DirectorySeparatorChar,
                };
            }

            Directory.CreateDirectory(this.ApplicationSetting.ModulePath);
            Directory.CreateDirectory(this.ApplicationSetting.ModuleConfigPath);
            Directory.CreateDirectory(this.ApplicationSetting.ModuleMainPath);
            Directory.CreateDirectory(this.ApplicationSetting.ModuleThirdPartyDLLPath);
        }

        public string CurrentApplicationPath
        {
            get => AppDomain.CurrentDomain.BaseDirectory;
        }

        public string CurrentApplicationSettingPath
        {
            get{
                var path = this.CurrentApplicationPath + "Config" + Path.DirectorySeparatorChar;
                Directory.CreateDirectory(path);
                return path;
            }
        }

        public string TempModulePackagePath
        {
            get
            {
                var path = this.CurrentApplicationPath + "TempModulePackage" + Path.DirectorySeparatorChar;
                Directory.CreateDirectory(path);
                return path;
            }
        }

        public string CurrentApplicationSettingFilePath
        {
            get
            {
                return CurrentApplicationSettingPath + "config.xml";
            }
        }


        public string SecurityPassword
        {
            get
            {
                return "Qhdkso!88";
            }
        }

        public ApplicationSetting ApplicationSetting
        {
            get;set;
        }

        
        
    }
}
