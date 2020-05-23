# -*- coding: utf-8 -*-
import sys
import clr_ext as clr

if sys.version_info > (3, 2):
    import xmlrpc.server as xmlrpc_server
    import xmlrpc.client as xmlrpc_client
else:
    import SimpleXMLRPCServer as xmlrpc_server
    import xmlrpclib as xmlrpc_client


def client(address="121.0.0.1", port=8888):
    proxy = xmlrpc_client.ServerProxy('http://%d:%s' %(address, port))
    return proxy


def server(address="121.0.0.1", port=8888):
    client = xmlrpc_server.SimpleXMLRPCServer((address, port), allow_none=True)
    # client.register_function(Set)
    client.register_introspection_functions()
    client.serve_forever()
