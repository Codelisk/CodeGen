using System;
using System.Collections.Generic;
using System.Text;

namespace Generators.Base
{
    public static class TestLog
    {
        public static string Log { get; set; } = "";

        public static void Add(string message)
        {
            Log += message + "\n";
        }
    }
}
