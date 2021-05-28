using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using Model;
using Newtonsoft.Json;

namespace VisionTool.Service
{
    public class SettingConfigService
    {
        public SettingConfigService()
        {

            this.LoadApplicationSetting();
        }

        public void LoadApplicationSetting()
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
                string jsonContent = File.ReadAllText(this.CurrentApplicationSettingFilePath, Encoding.UTF8);
                var applicationSetting = JsonConvert.DeserializeObject<ApplicationSetting>(jsonContent);
                this.ApplicationSetting = applicationSetting;
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

        public void SaveApplicationSetting()
        {
            if(DialogHelper.ShowConfirmMessage("프로그램 설정을 저장하시겠습니까?") == true)
            {

                try
                {

                    string jsonString = JsonConvert.SerializeObject(this.ApplicationSetting, Formatting.Indented);
                    File.WriteAllText(this.CurrentApplicationSettingFilePath, jsonString, Encoding.UTF8);
                }
                catch(Exception e)
                {
                    DialogHelper.ShowToastErrorMessage("설정 저장", "설정 저장이 실패했습니다.");
                }
            }
        }

        public void ResetApplicationSetting()
        {
            if (DialogHelper.ShowConfirmMessage("프로그램 설정을 초기화 하시겠습니까?") == true)
            {

                try
                {
                    File.Delete(this.CurrentApplicationSettingFilePath);
                    this.LoadApplicationSetting();
                }
                catch (Exception e)
                {
                    DialogHelper.ShowToastErrorMessage("설정 초기화", "설정 초기화가 실패했습니다.");
                }
            }
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

        public string TempModuleModPackagePath
        {
            get
            {
                var path = this.CurrentApplicationPath + "TempModuleModPackage" + Path.DirectorySeparatorChar;
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
                return CurrentApplicationSettingPath + "Config.json";
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
