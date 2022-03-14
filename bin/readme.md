# About the bin directory

This directory contains the binary files output by the c++ compiler. It also contains the hexflow.dSYM/Contents directory tree. The binaries output by the build process end up here because this destination is specified in the **./.vscode/tasks.json** file using the `/Fe:` arg for the windows flavour and the `-o` arg for the OSX flavour of the file. Both the windows executable hexflow (OSX binary) and hexflow.exe (Windows binary) are found here. Type `./hexflow` in the VSC terminal to execute the binary on both Windows and OSX. 
