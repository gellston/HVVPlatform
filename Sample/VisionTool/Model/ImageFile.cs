using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class ImageFile
    {
        public ImageFile(String fileName,
                     String filePath)
        {
            this.FileName = fileName;
            this.FilePath = filePath;
        }

        public String FileName
        {
            get; set;
        }


        public String FilePath
        {
            get; set;
        }
    }
}
