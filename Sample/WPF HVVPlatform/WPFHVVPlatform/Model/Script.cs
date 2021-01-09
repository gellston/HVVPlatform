using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;

namespace WPFHVVPlatform.Model
{
    public class Script
    {
        public Script()
        {

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
