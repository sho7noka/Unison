# -*- coding: utf-8 -*-
import sys

if sys.version_info > (3, 2):
    import xmlrpc.server as xmlrpc_server
    import xmlrpc.client as xmlrpc_client
else:
    import SimpleXMLRPCServer as xmlrpc_server
    import xmlrpclib as xmlrpc_client


def rpcClient():
    proxy = xmlrpc_client.ServerProxy('http://127.0.0.1:8080')  # サーバーに接続。
    # print (proxy.system.listMethods())  # サーバーが対応している関数の一覧を取得する。
    # print(proxy.Set("aaa"))
    return proxy


def rpcServer():
    client = xmlrpc_server.SimpleXMLRPCServer(("127.0.0.1", 8080), allow_none=True)
    # client.register_function(Set)
    client.register_introspection_functions()
    client.serve_forever()
