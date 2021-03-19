using System;
using System.Collections.Generic;
using System.Text;

namespace VisionTool.Model
{
    public class Diagram
    {
        public Diagram(string fileName, string filePath)
        {
            this.FileName = fileName;
            this.FilePath = filePath;
        }

        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}
