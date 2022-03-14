#ifndef HEXFLOW_H // Start of macro to prevent file being included multiple times.
#define HEXFLOW_H // Variable to prevent file being included multiple times.

#include <iostream> // C++ standard library. Allows us to read/write to the standard input/output streams.
#include <fstream> // C++ standard library. Allows us to work with files. Needs iostream as well.
#include <algorithm>  // Required for std::toupper c++ macro.
//#include <stdio.h> 
// #include <vector> // C++ standard library. Dynamic array suppport.
// #include <string> // Handy character array tools.
bool appActive = true; // Flag controlling ending of execution of the app.
const int mainMenuActive = 1; // Numerical reference to main menu.
const int systemMenuActive = 2; // Numerical reference to system menu.
const int fileMenuActive = 3; // Numerical reference to file menu.
int currMenu = mainMenuActive; // State machine tracking current active menu.
const int ecNO_ERR = 0; // OK app exit code.
const int ecMENU_UNKNOWN = 1; // App exit code for falling through main loop menuing system.
const int ecNO_FILE_NAME = 2; // App exit code for failing to provide a file name in the command line.
const int ecNOT_VALID_TEMPLATE_FILE_NAME = 3; // App exit code for template file not found.
const int ecNOT_VALID_MOVEMENT_FILE_NAME = 4; // App exit code for movement file not found.
const int ecNO_BAD_MQTT_SCRIPT_FILE = 5; // App exit code for bad MQTT script file. 
const int ecBAD_ERR_ARG = 6; // App exit code for invalid debug argument. 
const int ecUNKNOWN_ERR = 99; // App exit code for unknown error. 
//int appExitCode = ecUNKNOWN_ERR; // Track app exit code.
const int maxFileNameSize = 40; // Number of bytes the file name can be.
bool debugFlag = false; // Flag used to control debug output

#endif // End of macro to prevent file being included mutiple times. 