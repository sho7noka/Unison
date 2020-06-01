from distutils.core import setup
from clr_ext.build import PythonNetExtension, PythonNetBuild

setup(
    name='unison',
    version='0.9',
    packages=[''],
    url='git@github.com:sho7noka/Unison.git',
    license='MIT',
    author='sho7noka',
    author_email='',
    description='',
    data_files=[],
    ext_package=[],
    py_modules=['clr_ext'],
    install_requires=[
        # "pythonnet>=2.4.0",
    ],
    # ext_modules=[PythonNetExtension('Unison', ["Plugins/PyInterpreter.cs"])],
    # cmdclass={'build_ext': PythonNetBuild},
    classifiers=['Development Status :: 4 - Beta',
                 "Operating System :: OS Independent",
                 'Programming Language :: Python :: 2.7',
                 'Programming Language :: Python :: 3.7',
                 ],
)
