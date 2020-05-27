using System;
using System.Collections.Generic;
using System.IO;
using Python.Runtime;


namespace Unison
{
    internal class Program
    {
        public static int Main(string[] args)
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

        private static List<string> GetExtraSitePackages()
        {
            var sitePackages = new List<string>();
            {
                var packageSitePackage = Path.GetFullPath("Packages/com.sho7noka.unison/clr_ext");
                packageSitePackage = packageSitePackage.Replace("\\", "/");
                sitePackages.Add(packageSitePackage);
            }
            
            if (Directory.Exists("Assets/Python/site-packages"))
            {
                var projectSitePackages = Path.GetFullPath("Assets/Python/site-packages");
                projectSitePackages = projectSitePackages.Replace("\\", "/");
                sitePackages.Add(projectSitePackages);
            }

            return sitePackages;
        }

        private static void clr_ext()
        {
            dynamic builtins = PythonEngine.ImportModule("__builtin__");
            // prepend to sys.path
            dynamic sys = PythonEngine.ImportModule("sys");
            dynamic syspath = sys.GetAttr("path");
            dynamic sitePackages = GetExtraSitePackages();
            dynamic pySitePackages = builtins.list();
            foreach (var sitePackage in sitePackages)
            {
                pySitePackages.append(sitePackage);
            }

            pySitePackages += syspath;
            sys.SetAttr("path", pySitePackages);
        }
    }
}