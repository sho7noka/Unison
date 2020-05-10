import clr as _clr
import os
import sys


def AddReferenceFrom(dll):
    """
    フルパスからlibを直接Reference
    戻値にモジュールを返す
    .NET AddReferenceToFileAndPath 互換

    Args:
        dll: マネージコードのパス

    Returns:
        object: 
        System, ManageCode モジュール

    >>> import clr_ext as clr
    >>> clr.AddReferenceFrom("assembly.dll")
    """

    # unity helper
    if dll.startswith("Asset"):
        _clr.AddReference("UnityEditor")
        import UnityEditor
        path = UnityEditor.AssetDatabase.GetAssetPath(dll)
        dll = System.IO.Path.GetFullPath(path)

    _clr.setPreload(True)
    _clr.AddReference("System")
    import System

    if os.path.exists(dll):
        dir_name = os.path.dirname(dll)
        sys.path.append(dir_name)

    if dll.endswith(".dll"):
        asm = os.path.basename(dll).split(".")[0]
        _clr.AddReference(asm)
        ref = System.Reflection.Assembly.LoadFile(dll)
        name_space = ref.GetTypes()[0].ToString().split(".")[0]

    try:
        Client = __import__(name_space)
    except ModuleNotFoundError:
        Client = None

    return System, Client


def AddReferenceUnManage(dll):
    """
    manage & unmanage 両用のリファレンス参照
    Args:
        dll: unmanage code

    Returns:

    >>> import clr_ext as clr
    >>> clr.AddReferenceUnmanage()
    """
    clr.setPreload(True)
    clr.AddReference("System")
    import System

    try:
        from ctypes import cdll
        lib = cdll.LoadLibrary(dll)
        return System, lib
    except OSError:
        return AddReferenceFrom(dll)
