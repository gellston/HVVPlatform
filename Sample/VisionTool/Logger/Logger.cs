using Newtonsoft.Json;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;

namespace Logger
{

    public enum TYPE
    {
        ALL,
        DEVICE,
        UI
    }



    public class Logger
    {


        static Logger()
        {

          

        }



        public static Dictionary<TYPE, string> _Tag = null;
        public static Dictionary<TYPE, string> Tag
        {
            get
            {
                if(Logger._Tag == null)
                {
                    Logger._Tag = new Dictionary<TYPE, string>();
                    Logger._Tag[TYPE.ALL] = @"Log\{0}\all.log";
                    Logger._Tag[TYPE.DEVICE] = @"Log\{0}\device.log";
                    Logger._Tag[TYPE.UI] = @"Log\{0}\ui.log";
                }

                return Logger._Tag;
            }
        }


        private static ObservableCollection<Log> _LogCollection = null;
        public static ObservableCollection<Log> LogCollection
        {
            get
            {
                if(Logger._LogCollection == null)
                {

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Logger._LogCollection = new ObservableCollection<Log>();
                    });

                   
                }

                return Logger._LogCollection;
            }
        } 

        public static void Write(TYPE type, 
                                 string context,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
        {
            try
            {

               
                var timeObject = DateTime.UtcNow;
                var dateStamp = timeObject.ToString("yyy-MM-dd");
                var timeStamp = timeObject.ToString("yyyy-MM-dd HH:mm:ss:fff");
                string AllPathFormat = String.Format(Logger.Tag[TYPE.ALL], dateStamp);
                string TypePathFormat = String.Format(Logger.Tag[type], dateStamp);
                string AllJsonPathFormat = String.Format(@"Log\{0}\all.json", dateStamp);



                using (var log = new LoggerConfiguration()
                    .WriteTo.File(AllPathFormat, outputTemplate: "{Message}")
                    .CreateLogger())
                {


                    string fullContext = string.Format("[{0}]:[{1}]:[{2}]:{3}\n", timeStamp, memberName, sourceLineNumber, context);

                    log.Error(fullContext);
                }



                //using (var log = new LoggerConfiguration()
                //                //.WriteTo.File(AllJsonPathFormat,outputTemplate: "{Log}")
                //                .WriteTo.File(AllJsonPathFormat, new JsonFormatter())
                //                .CreateLogger())
                //{


                //    var customLogObject = new Log(timeObject, sourceFilePath, memberName, sourceLineNumber, context);
                //    var collection = Logger.LogCollection;
                //    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                //    {
                //        collection.Add(customLogObject);
                //    }));

                //    //string jsonString = JsonConvert.SerializeObject(customLogObject, Formatting.Indented);

                //    log.Error("{@Log}", customLogObject);
                //}


                if (type != TYPE.ALL)
                {
                    using (var log = new LoggerConfiguration()
                        .WriteTo.File(TypePathFormat, outputTemplate: "{Message}")
                        .CreateLogger())
                    {

                        string fullContext = string.Format("[{0}]:[{1}]:[{2}]:{3}\n", timeStamp, memberName, sourceLineNumber, context);

                        log.Error(fullContext);
                    }
                }

            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

        }
    }
}
