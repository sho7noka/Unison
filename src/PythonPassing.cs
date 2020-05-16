using System;
using Python.Runtime;
using UnityEditor;


namespace Python.Passing
{
    /**
     * TODO: インタープリター位置が指定できないと実行時にコケる
     * <summary>
     * Interpreter. pipenv pyenv $ conda
     * https://github.com/pythonnet/pythonnet/wiki/Using-Python.NET-with-Virtual-Environments
     * </summary>
     */
    public class Interpreter
    {
        public static void PyConsole()
        {
            PythonNetBinder.Gen(typeof(DLLTest.MyUtilities)).Compile("Client.dll");
        }
        
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

        private static dynamic client;

        public static dynamic rpcServer()
        {
            using (Py.GIL())
            {
                client = Py.Import("xmlrpc.client");
                var proxy = client.ServerProxy("http://127.0.0.1:8080");
                proxy.Set("djsaklja");
                return proxy;
            }
        }

        public static void rpcClient()
        {
            using (Py.GIL())
            {
                Action<string> _ = Set;
                var def = _.ToPython();
                // dynamic Stop = client.server_close;

                try
                {
                    dynamic server = Py.Import("xmlrpc.server");
                    var param = new PyObject[] {new PyString("127.0.0.1"), new PyInt(8080)};
                    client = server.SimpleXMLRPCServer(
                        new PyTuple(param), server.SimpleXMLRPCRequestHandler, false, true);

                    client.register_function(def, "Set".ToPython());
                    client.register_multicall_functions();
                    client.serve_forever();
                }
                catch (PythonException e)
                {
                    client.server_close();
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}