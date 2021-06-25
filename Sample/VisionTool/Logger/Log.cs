using System;
using System.Collections.Generic;
using System.Text;

namespace Logger
{
    public class Log
    {

        public Log(DateTime timeStamp,
                   string sourceFilePath,
                   string memberFunctionName,
                   int sourceLineNumber,
                   string context)
        {
            this.TimeStamp = timeStamp;
            this.SourceFilePath = sourceFilePath;
            this.MemberFunctionName = memberFunctionName;
            this.SourceLineNumber = sourceLineNumber;
            this.Context = context;
        }

        public DateTime TimeStamp { get; set; }

        public string SourceFilePath { get; set; } = "";

        public string MemberFunctionName { get; set; } = "";
        public int SourceLineNumber { get; set; } = 0;
        public string Context { get; set; } = "";
        
    }
}
