using System;
using Python.Runtime;

namespace Unison
{
    internal class Program
    {
        public static int Main(string[] args)
        {
            var cmd = Environment.GetCommandLineArgs();
            
            PythonEngine.Initialize();
            PyInterpreter.Client("localhost", 8888);
            var i = Runtime.Py_Main(cmd.Length, cmd);
            PythonEngine.Shutdown();
            return i;
        }
    }
}