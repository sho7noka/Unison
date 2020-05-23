using System;
using Python.Runtime;


namespace Unison
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var cmd = Environment.GetCommandLineArgs();
            // PyInterpreter.Set();

            PythonEngine.Initialize();
            var i = Runtime.Py_Main(cmd.Length, cmd);
            PythonEngine.Shutdown();
            return i;
        }
    }
    
    public class PyInterpreter
    {
        private static dynamic client;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static dynamic RpcClient(string address, int port)
        {
            using (Py.GIL())
            {
                client = Py.Import("xmlrpc.client");
                var proxy = client.ServerProxy($"http://{address}:{port}");
                return proxy;
            }
        }

        /// <summary>
        /// Interpreter. pipenv pyenv $ conda
        /// https://github.com/pythonnet/pythonnet/wiki/Using-Python.NET-with-Virtual-Environments
        /// </summary>
        /// <param name="pathToVirtualEnv"></param>
        public static void Set(string pathToVirtualEnv)
        {
            Environment.SetEnvironmentVariable("PATH", pathToVirtualEnv, EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PYTHONHOME", pathToVirtualEnv,
                EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PYTHONPATH",
                $"{pathToVirtualEnv}\\Lib\\site-packages;{pathToVirtualEnv}\\Lib", EnvironmentVariableTarget.Process);

            PythonEngine.PythonHome = pathToVirtualEnv;
            PythonEngine.PythonPath =
                Environment.GetEnvironmentVariable("PYTHONPATH", EnvironmentVariableTarget.Process);
        }
    }
}