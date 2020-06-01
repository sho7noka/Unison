using System;
using System.Collections.Generic;
using Python.Runtime;

namespace Unison
{
    public class RPCServer
    {
        private dynamic server;
        private readonly Dictionary<Action<string>, string> functions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="functions"></param>
        public RPCServer(Dictionary<Action<string>, string> functions)
        {
            this.functions = functions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="port"></param>
        public void Start(string address, int port)
        {
            using (Py.GIL())
            {
                dynamic server = Py.Import("xmlrpc.server");
                var param = new PyObject[] {new PyString(address), new PyInt(port)};
                server = server.SimpleXMLRPCServer(
                    new PyTuple(param), server.SimpleXMLRPCRequestHandler, false, true);

                foreach (var func in functions)
                {
                    server.register_function(func.Key, func.Value.ToPython());
                }

                server.register_multicall_functions();
                server.serve_forever();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            using (Py.GIL())
            {
                server.server_close();
            }
        }
    }
}