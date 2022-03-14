![Hexbot compiler logo](https://github.com/theAgingApprentice/hexbotCompiler/blob/main/img/compilerBanner.png)
[![LICENSE](https://img.shields.io/badge/license-MIT-blue.svg)](https://raw.githubusercontent.com/mmistakes/minimal-mistakes/master/LICENSE)
[![Doxygen Action](https://github.com/theAgingApprentice/aaChip/actions/workflows/main.yml/badge.svg?event=push)](https://github.com/theAgingApprentice/aaChip/actions/workflows/main.yml)

# hexbotCompiler - Bringing legs to life.

## Table of contents
* [Overview](#Overview)
* [Features](#Features)
* [Compatability](#Compatability)
* [Development environment](#Development-environment)
* [Code documentation](#Code-documentation)
* [Downloading](#Downloading)
* [Running VSC](#Running-VSC)
* [Compiling Code](#Compiling-Code)
* [Running Code](#Running-Code)
* [Testing](#Testing)
* [Further Help](#Further-Help)
* [Releases](#Releases)
* [Cloning this respository](#Cloning-this-respository)
* [Acknowledgements](#Acknowledgements)

## Overview

This repository contains C++ code intended to run on your computer in order to generate workflows
that can be fed into the Hexbot robot via MQTT in order to orchestrate complex movements for the
robot's six legs.

## Features

* Accepts command line arguments.
* Access to system variables.
* Repo auto generates online documentation
* MIT License

## Compatability 

* This code has only been built and tested on an iMac OSX Monterey. 

## Development environment

This library was written using the following tools:
* [<img src="/img/vscLogo.png" width="15" height="15">](https://code.visualstudio.com/docs) 
Microsoft's Visual Studio Code source code editor. 
* C++ plugins.

## Code documentation

This repository automatcally generates 
[online documentation](https://theagingapprentice.github.io/hexbotCompiler/html/index.html) 
each time code is merged into the main branch.

## Downloading

To download this code to your local repository please do the following. 
 
- Get into Visual Studio Code terminal window with no projects open.
- Navigate to the project folder on your local hard drive.
- Issue the command `git clone https://github.com/theAgingApprentice/{project name}` (you can paste the URL that you copied in the previous step)
- Navigate into the newly created directory `cd {project name}`
- Create a tasks.json file
 * If you use Windows OS then **copy and rename** ```./vscode/tasks.json-WIN``` to ```./vscode/tasks.json```. Do not just rename or you will end up including files in the repository that we do not want in there.
 * If you use OSX OS then **copy and rename** ```./vscode/tasks.json-OSX``` to ```./vscode/tasks.json```. Do not just rename or you will end up including files in the repository that we do not want in there.  
- See next section for special instructions on how to run Visual Studio Code for working with C++ for native code.

## Running VSC

If you are an OSX user then you can simply use the VSC icon to load VSC and use the hot ket combination `SHIFT + COMMAND + B` to build the executable. If you are a Windows user then do not use the VSC icon. In order for VSC to work propery with your compiler it must be started using a special VSC calling sequence. These instructions were taken from [here](https://code.visualstudio.com/docs/cpp/config-msvc).

VSC must be started from a special developer command prompt following these steps:

1. Manually, enter `devel` in Windows Start search window
2. A few options will appear. Run the option that mentions somehting about a command prompt
3. Then CD to the project level directory of your code, i.e. ....hexbotCompiler/
4. then enter `code .`
5. This will start VSC, and open the project folder within VSC
6. Now you can use VSC like you always do.

## Compiling Code 

1. Ensure that you have a tasks.json file
 * If you use Windows OS then copy and rename ```./vscode/tasks-WIN.json``` to ```./vscode/tasks.json```. 
 * If you use OSX OS then copy and rename ```./vscode/tasks-OSX.json``` to ```./vscode/tasks.json```.    
2. Go to the src folder
3. Enter the character `control-command-B` on a Mac, `control-shift-B` on a PC.

## Running Code

Binaries are located in the bin directory.

1. In the console terminal make sure that you are in the bin folder
2. To run code from the src directory, in a terminal window type ./filename. Note that this works for both Windows and OSX.  

## Testing

At this time we do not have a way to test this embedded code.

## Further Help

For more help using the C++ environment within VIsual STudio Code check [here](https://code.visualstudio.com/docs/cpp/config-clang-mac).
## Releases

1. We use the [SemVer](http://semver.org/) numbering scheme for our releases. 
2. The latest stable release is [v1.0.0](https://github.com/theAgingApprentice/underwear/releases/tag/v1.0.0).

## Cloning this respository

Detailed instructions on how to clone this template repository can bew viewed [here](./aaAdmin/newRepoTodo.md).

## Acknowledgements

1. The many example code sites found on the internet. Thank you all for sharing your knowledge so freely.
