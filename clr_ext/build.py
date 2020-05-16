# -*- coding: utf-8 -*-
import platform
import shutil
import subprocess
from distutils import sysconfig
from setuptools import Extension
from setuptools.command import build_ext


class PythonNetExtension(Extension):
    """
    python build system for csc

    >>> ext_modules=[PythonNetExtension('package_name', ["test.cs"])],
    """

    def __init__(self, name, sources=[], is_lib=True, compiler=None):
        """
        Args:
            name: 
            sourcedir: 
        """
        Extension.__init__(self, name, sources=sources)
        self.runtime = sysconfig.get_python_lib() + "Python.Runtime.dll"
        self.is_lib = is_lib
        self.compiler = compiler


class PythonNetBuild(build_ext.build_ext):
    """
    pythonnet 用 setuptoolの拡張
    Python.Runtime.dll と python.dll, python.zip
    https://docs.python.org/ja/3/distutils/apiref.html

    >>> cmdclass={'build_ext': PythonNetBuild},
    """

    if platform.platform() == "Windows":
        csc = "C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe"
    else:
        csc = "csc"

    def run(self):
        try:
            _ = subprocess.check_output([self.csc, '/help'])
        except OSError:
            raise RuntimeError("csc must be installed to build the following extensions: " +
                               ", ".join(e.name for e in self.extensions))
        for ext in self.extensions:
            self.build_extension(ext)

    def build_extension(self, ext):
        """
        https://qiita.com/toshirot/items/dcf7809007730d835cfc
        Returns:
        """
        cmd = [
            ext.compiler or self.csc,
            "-nologo",
            "/t:library" if ext.is_lib else "/t:exe",
            "/out:%s.dll" % ext.name if ext.is_lib else "/out:%s.exe" % ext.name,
            "/r:%s" % ext.runtime,
            ext.sources,
        ]
        subprocess.check_call(cmd, shell=True)

        # package
        dllib = [".dll", ".a", ".so"]
        pythonz = sysconfig.get_python_lib(True, True)
        shutil.make_archive(pythonz, "zip", root_dir=pythonz)
