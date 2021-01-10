using System;
using System.Threading.Tasks;

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


            HV.V1.Interpreter test1 = new HV.V1.Interpreter();

            HV.V1.Interpreter test2 = new HV.V1.Interpreter();


            var task1 = Task.Run(() =>
            {
                while (true)
                {
                    Console.Clear();
                    try
                    {

                        var watch = System.Diagnostics.Stopwatch.StartNew();

                        HV.V1.Object object_test = new HV.V1.Object("test", "test");
                        test1.RegisterExternalData("test_object", object_test);

                        test1.RunFile(currentDirecturoy + "script1.js");

                        watch.Stop();
                        var elapsedMs = watch.ElapsedMilliseconds;
                        System.Console.WriteLine("takt time = " + elapsedMs);


                        foreach (var object_set in test1.GlobalObjects)
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
                    catch (Exception e)
                    {
                        System.Console.WriteLine(e.ToString());
                    }
                    GC.Collect();
                    //Task.Delay(1000);
                }
            });


            var task2 = Task.Run(() =>
            {
                while (true)
                {
                    Console.Clear();
                    try
                    {

                        var watch = System.Diagnostics.Stopwatch.StartNew();

                        HV.V1.Object object_test = new HV.V1.Object("test", "test");
                        test2.RegisterExternalData("test_object", object_test);

                        test2.RunFile(currentDirecturoy + "script2.js");

                        watch.Stop();
                        var elapsedMs = watch.ElapsedMilliseconds;
                        System.Console.WriteLine("takt time = " + elapsedMs);


                        foreach (var object_set in test2.GlobalObjects)
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
                    catch (Exception e)
                    {
                        System.Console.WriteLine(e.ToString());
                    }
                    GC.Collect();
                    //Task.Delay(1000);
                }
            });

            task1.Wait();
            task2.Wait();
            //while (true)
            //{
            //    Console.Clear();
            //    try
            //    {

            //        var watch = System.Diagnostics.Stopwatch.StartNew();

            //        HV.V1.Object object_test = new HV.V1.Object("test", "test");
            //        test1.RegisterExternalData("test_object", object_test);

            //        test1.RunFile(currentDirecturoy + "script1.js");

            //        watch.Stop();
            //        var elapsedMs = watch.ElapsedMilliseconds;
            //        System.Console.WriteLine("takt time = " + elapsedMs);

                    
            //        foreach (var object_set in test1.GlobalObjects)
            //        {
            //            var key = object_set.Key;
            //            var value = object_set.Value;
            //            System.Console.WriteLine("===================================");
            //            System.Console.WriteLine("key : " + value.Name);
            //            System.Console.WriteLine("value : ");
            //            System.Console.WriteLine(value.ToString());
            //            System.Console.WriteLine("===================================");
            //        }
            //    }
            //    catch(Exception e)
            //    {
            //        System.Console.WriteLine(e.ToString());
            //    }
            //    System.Threading.Thread.Sleep(300);
            //}
            
        }
    }
}
