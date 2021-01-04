using System;

namespace console_test_sharp
{
    class Program
    {


        static void Main(string[] args)
        {
            String currentDirecturoy = AppDomain.CurrentDomain.BaseDirectory;


            HV.V1.Interpreter.InitV8StartupData(currentDirecturoy);
            HV.V1.Interpreter.InitV8Platform();
            HV.V1.Interpreter.InitV8Engine();
            HV.V1.Interpreter.SetV8Flag("--use_strict");
            HV.V1.Interpreter.SetV8Flag("--max_old_space_size=8192");
            HV.V1.Interpreter.SetV8Flag("--expose_gc");


            HV.V1.Interpreter test = new HV.V1.Interpreter();


            while (true)
            {
                Console.Clear();
                try
                {

                    var watch = System.Diagnostics.Stopwatch.StartNew();

                    test.RunFile(currentDirecturoy + "script.js");

                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;
                    System.Console.WriteLine("takt time = " + elapsedMs);

                    
                    foreach (var object_set in test.GlobalObjects)
                    {
                        var key = object_set.Key;
                        var value = object_set.Value;
                        System.Console.WriteLine("===================================");
                        System.Console.WriteLine("key : " + value.Name);
                        System.Console.WriteLine("value : ");
                        System.Console.WriteLine(value.ToString());
                        System.Console.WriteLine("===================================");
                    }
                }
                catch(Exception e)
                {
                    System.Console.WriteLine(e.ToString());
                }
                System.Threading.Thread.Sleep(300);
            }
            
        }
    }
}
