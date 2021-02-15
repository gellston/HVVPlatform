using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VisionTool.Service
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

        public string LoadScriptFile(string _path)
        {
            return File.ReadAllText(_path, Encoding.UTF8);
        }

    }
}
