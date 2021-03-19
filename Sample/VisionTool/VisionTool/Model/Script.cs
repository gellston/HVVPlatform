using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;

namespace VisionTool.Model
{
    public class Script
    {
        public Script(String fileName,
                      String scriptContent,
                      String filePath)
        {
            this.FileName = fileName;
            this.ScriptContent = scriptContent;
            this.FilePath = filePath;
        }

        public String FileName
        {
            get;set;
        }

        public String ScriptContent
        {
            get;set;
        }

        public String FilePath
        {
            get;set;
        }
    }
}
