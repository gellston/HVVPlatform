using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WPFHVVPlatform.Service
{
    public class ScriptFileService
    {
        public ScriptFileService()
        {

        }

        public void SaveScriptFile(string _path, string _content)
        {
            File.WriteAllText(_path, _content, Encoding.UTF8);
        }


    }
}
