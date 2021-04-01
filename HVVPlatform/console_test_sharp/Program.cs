using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace console_test_sharp
{
    class Program
    {
 
        static void Trace(String test)
        {
      
            System.Console.WriteLine(test);
        }


        static void Trace2(String test)
        {

            System.Console.WriteLine(test);
        }

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
            test1.TraceEvent += Trace;
            test1.TraceEvent += Trace2;

            test1.RunScript("var test  = 1;");
            var globalobject = test1.GlobalObjects;

            
        }
    }
}
