using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Generators.Base.Helpers
{
    public static class LoggingHelper
    {
        private static List<(string, DateTime)> methods = new();

        static string Message = "";

        public static void WriteLine(string message)
        {
            Message += message + "\n";
        }

        public static string GetLog()
        {
            return Message;
        }

        public static void LogTime()
        {
            try
            {
                WriteLine("LogTime");
                var stackTrace = new StackTrace();
                var caller = stackTrace.GetFrame(1).GetMethod().Name;
                var time = DateTime.Now;
                if (methods.Any(x => x.Item1 == caller))
                {
                    var secondTime = methods.Last(x => x.Item1 == caller).Item2;
                    methods.Add((caller, time));
                    //WriteLine(
                    //    $"{caller}: Diff:{(secondTime - time).TotalSeconds}Secs, Time{time.ToLongTimeString()}"
                    //);

                    WriteLine($"Time{time.ToLongTimeString()}, {caller}");
                }
                else
                {
                    methods.Add((caller, time));
                    //WriteLine($"{caller}: Seconds:{time.Second}, Time{time.ToLongTimeString()}");
                    WriteLine($"Time{time.ToLongTimeString()}, {caller}");
                }
            }
            catch (Exception ex)
            {
                WriteLine("CrashLogTime");
            }
        }
    }
}
