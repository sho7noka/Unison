from distutils.core import setup
from clr_ext.build import PythonNetExtension, PythonNetBuild

setup(
    name='Unison',
    version='',
    packages=[''],
    url='',
    license='MIT',
    author='sho7noka',
    author_email='',
    description='',
    data_files=[],
    install_requires=[
        # "pythonnet>=2.4.0",
    ],
    ext_package=[],
    py_modules=['clr_ext'],
    ext_modules=[PythonNetExtension('PythonPassing', ["src/PythonPassing.cs"])],
    cmdclass={'build_ext': PythonNetBuild},
    classifiers=['Development Status :: 4 - Beta',
                 "Operating System :: OS Independent",
                 'Programming Language :: Python :: 2.7',
                 'Programming Language :: Python :: 3.7',
                 ],
)
