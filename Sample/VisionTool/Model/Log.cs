using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Log
    {
        public Log(string type, 
                   string content)
        {
            this.Type = type;
            this.Content = content;
        }
        public string Type
        {
            get;set;
        }

        public string Content
        {
            get;set;
        }
    }
}
