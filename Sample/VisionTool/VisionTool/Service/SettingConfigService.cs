using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Model;

namespace VisionTool.Service
{
    public class SettingConfigService
    {
        public SettingConfigService()
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
                    DiagramConfigPath = this.CurrentApplicationPath + "DiagramConfig" + Path.DirectorySeparatorChar,
                    DiagramImagePath = this.CurrentApplicationPath + "DiagramImage" + Path.DirectorySeparatorChar
                    
                };
            }
            else
            {
                // 파일 생성 코드 짜는게 필요함.
            }

            Directory.CreateDirectory(this.ApplicationSetting.ModulePath);
            Directory.CreateDirectory(this.ApplicationSetting.ModuleConfigPath);
            Directory.CreateDirectory(this.ApplicationSetting.ModuleMainPath);
            Directory.CreateDirectory(this.ApplicationSetting.ModuleThirdPartyDLLPath);
            Directory.CreateDirectory(this.ApplicationSetting.DiagramPath);
            Directory.CreateDirectory(this.ApplicationSetting.DiagramConfigPath);
            Directory.CreateDirectory(this.ApplicationSetting.DiagramImagePath);

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
