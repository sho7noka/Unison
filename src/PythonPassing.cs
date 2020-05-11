using System;
using Python.Runtime;


namespace Python.Passing
{
    /**
     * TODO: インタープリター位置が指定できないと実行時にコケる
     * TODO: pth か py を生成
     * Interpreter. pipenv pyenv $ conda
     * https://github.com/pythonnet/pythonnet/wiki/Using-Python.NET-with-Virtual-Environments
     */
    public class Interpreter
    {
        public static void PyConsole()
        {
            PythonNetBinder.Gen(typeof(DLLTest.MyUtilities)).Compile("Client.dll");

            using (Py.GIL())
            {
                dynamic py = new PyExpandoObject();
                var socket = py.socket.socket(py.socket.AF_INET, py.socket.SOCK_DGRAM);
                socket.sendto("Hello UDP", "127.0.0.1", 50007);
                socket.close();
            }
        }

        public static void Set(string pathToVirtualEnv)
        {
            System.Environment.SetEnvironmentVariable("PATH", pathToVirtualEnv, EnvironmentVariableTarget.Process);
            System.Environment.SetEnvironmentVariable("PYTHONHOME", pathToVirtualEnv,
                EnvironmentVariableTarget.Process);
            System.Environment.SetEnvironmentVariable("PYTHONPATH",
                $"{pathToVirtualEnv}\\Lib\\site-packages;{pathToVirtualEnv}\\Lib", EnvironmentVariableTarget.Process);

            PythonEngine.PythonHome = pathToVirtualEnv;
            PythonEngine.PythonPath =
                System.Environment.GetEnvironmentVariable("PYTHONPATH", EnvironmentVariableTarget.Process);
        }
    }
}