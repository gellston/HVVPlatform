using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VisionTool.Model;

namespace VisionTool.Service
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
                    DiagramPath = this.CurrentApplicationPath + "Diagram" + Path.DirectorySeparatorChar,
                    DiagramConfigPath = this.CurrentApplicationPath + "DiagramConfig" + Path.DirectorySeparatorChar
                    
                };
            }

            Directory.CreateDirectory(this.ApplicationSetting.ModulePath);
            Directory.CreateDirectory(this.ApplicationSetting.ModuleConfigPath);
            Directory.CreateDirectory(this.ApplicationSetting.ModuleMainPath);
            Directory.CreateDirectory(this.ApplicationSetting.ModuleThirdPartyDLLPath);
            Directory.CreateDirectory(this.ApplicationSetting.DiagramPath);
            Directory.CreateDirectory(this.ApplicationSetting.DiagramConfigPath);

            Environment.SetEnvironmentVariable("PATH", Environment.GetEnvironmentVariable("PATH") + ";" + this.ApplicationSetting.ModuleThirdPartyDLLPath);
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

        public string TempDiagramPackagePath
        {
            get
            {
                var path = this.CurrentApplicationPath + "TempDiagramPackage" + Path.DirectorySeparatorChar;
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
