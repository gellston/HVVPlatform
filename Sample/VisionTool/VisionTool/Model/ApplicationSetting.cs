using System;
using System.Collections.Generic;
using System.Text;

namespace VisionTool.Model
{
    public class ApplicationSetting
    {
        public ApplicationSetting()
        {

        }

        public string ModulePath { get; set; }
        public string ModuleConfigPath { get; set; }
        public string ModuleMainPath { get; set; }
        public string ModuleThirdPartyDLLPath { get; set; }
    }
}
