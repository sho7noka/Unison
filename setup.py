from distutils.core import setup
from clr_ext import PythonNetExtension, PythonNetBuild

setup(
    name='Python.Passing',
    version='',
    packages=[''],
    url='',
    license='MIT',
    author='shosumioka',
    author_email='',
    description='',
    data_files=[],
    install_requires=[
        "pythonnet>=2.4.0",
    ],
    ext_package=[],
    py_modules=['clr_ext'],
    ext_modules=[PythonNetExtension('PythonPassing', ["Passing/PythonPassing.cs"])],
    cmdclass={'build_ext': PythonNetBuild},
    classifiers=['Development Status :: 4 - Beta',
                 "Operating System :: OS Independent",
                 'Programming Language :: Python :: 2.7',
                 'Programming Language :: Python :: 3.7',
                 ],
)
