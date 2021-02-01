using System;
using System.Collections.Generic;
using System.Text;

namespace WPFHVVPlatform.Model
{
    public class ApplicationSetting
    {
        public ApplicationSetting()
        {

        }

        public string ModulePath { get; set; }
        public string ModuleConfigPath { get; set; }
        public string ModuleUnZipPath { get; set; }
        public string ModuleThirdPartyDLLPath { get; set; }
    }
}
