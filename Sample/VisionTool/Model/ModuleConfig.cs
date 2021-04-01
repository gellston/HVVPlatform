using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class ModuleConfig
    {
        public ModuleConfig()
        {

        }

        public string ModuleName { get; set; }
        public string ModuleModifyDate { get; set; }
        public int ModuleVersion { get; set; }
        public string ModuleComment { get; set; }
    }
}
