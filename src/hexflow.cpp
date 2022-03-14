/** @file hexflow.cpp
 * A command line utility for making Hexbot flows.
 * Hexflow is a command line utitlity that outputs flows that can be used to 
 * issue leg movement commands to the  hexbot robot via MQTT messages.   
 * 
 * # Getting started
 * To get your C++ envionment up and running in Visual Studio Code please
 * check out the link for 
 * [MacOsx](https://code.visualstudio.com/docs/cpp/config-clang-mac)]or this 
 * link for [Windows](https://code.visualstudio.com/docs/cpp/config-mingw).
 * 
 * # Building
 * To build this code open the project in Visual Studio Code (see readme file
 * for this project regarding speccial Windows considerations runing VSC) and 
 * press the hot key combination to trigger a build. For OSX users this is 
 * SHIFT + COMMAND + B, for Windows users this is SHIFT + CONTROL + B. 
 * 
 * # Running the program
 * To run this program go into a terminal shell and type ./hexflow
 * 
 * # Notes
 * The include of the hexflow.h file shows up as an error by the linter and it
 * underlines the line as if it is an error. The same goes for the include 
 * iostream.h line in hexflow.h. Despite this the code compiles cleanly and 
 * runs just fine. To avoid confusion about false bugs being underlined the 
 * option to underline include files that are not found has been disabled.
 ******************************************************************************/
#include "hexflow.h"

using namespace std; // Scope identifiers like functions and variables etc. 

/**
 * @brief Display the application help message.
 * @param null. 
 * @return null.
 ******************************************************************************/
void displayHelpMessage()
{
   printf("<displayHelpMessage> ------------\n");
   printf("<displayHelpMessage> hexflow help\n");
   printf("<displayHelpMessage> ------------\n");
   printf("<displayHelpMessage> Hexflow is a native c++ binary application that requires\n");
   printf("<displayHelpMessage> you to provide at least four command line arguments.\n");
   printf("<displayHelpMessage> Calling format: ./hexflow <botName> <input_template> <input_movement> <output_script> <debug>\n");
   printf("<displayHelpMessage> Where:\n");
   printf("<displayHelpMessage> <botName> is the name assigned to the target hexbot.\n");
   printf("<displayHelpMessage> <input_template> is an input file containing the instructions to build the script.\n");
   printf("<displayHelpMessage> <input_movement> is an input file containing the moves we want to make.\n");
   printf("<displayHelpMessage> <output_script> is the resulting script to be run by MQTTfx.\n");
   printf("<displayHelpMessage> <debug> is an optional argument that turns on debugging output.\n");
} // displayHelpMessage()

/**
 * @brief Display the command line arguments passed into the application.
 * @param argc Number of argument passed.
 * @param argv array of char arrays containing the name of the application as 
 * well as any other command line parameters.
 * @return null.
 ******************************************************************************/
void displayArgs(int argc, char** argv)
{
   int argCount = argc - 1; // Ignore first argument. That is the app name.
   if(argCount < 1)
   {
      printf("<displayArgs> No command line arguments were provided.\n");
   } // if
   else
   {
      printf("<displayArgs> Command line argument count is %d.\n", argc);
      for(int i = 1; i < argc; ++i)
      {
         printf("<displayArgs> ... %d) = %s\n", i, argv[i]);
      } // for
   } // else
} // displayArgs()

/**
 * @brief Display the shell environment variables available to this application.
 * @param env_var_ptr pointer to array of environment variables.
 * @return null.
 ******************************************************************************/
void displayEnvVars(char **env_var_ptr)
{
   printf("<displayEnvVars> Available environment variables:\n");
   int i = 0;
   while(*env_var_ptr != NULL) 
   {
      i++;
      printf("<displayEnvVars> ... %d) %s\n",i, *(env_var_ptr++));
   } // while
} // displayEnvVars()

/**
 * @brief Send a test message to the MQTT broker.
 * @param null.
 * @return null.
 ******************************************************************************/
void sendMqttMessage()
{
   printf("<sendMqttMessage> This feature is not yet implemented.\n");
} // sendMqttMessage()

/**
 * @brief Append a line of text to a file.
 * @param outputFile is the MQTTfx script file being created.
 * @param text is the line of text to append to the file.
 * @return null.
 ******************************************************************************/
void appendText(char *line, char *outputFile)
{
   if(debugFlag == true)
   {
      printf("<appendText> Append file %s with this line: %s\n", outputFile, line);
   } // if
   ofstream a_file(outputFile, ios::app); // Opens file for appending data.
   a_file << line << '\n'; // Appends data to file. Add new line to end.
   a_file.close(); // Close the file stream explicitly.  
   return;
} // appendText()

/**
 * @brief Append one file to another file after doing some content processing.
 * @param inputFile is the movement file containing flow data.
 * @param outputFile is the MQTTfx script file being created.
 * @return null.
 ******************************************************************************/
void appendFile(char *inputFile, char *outputFile)
{
   fstream myFile; // Reference to file input.
   const int maxFileLineSize = 80; // Max num characters in one line of file.
   char lineFromFile[maxFileLineSize]; // Hold input from file.  
   int lineCount = 0; // Enumerate lines of file.
   myFile.open(inputFile, ios::in); // Open file for reading.
   if(myFile.is_open()) //Check whether the file is open.
   {   
      printf("<appendFile> Append file %s with parsed content from file %s\n", inputFile, outputFile);
      while(myFile.getline(lineFromFile, maxFileLineSize))
      {
         lineCount ++;
         printf("<appendFile> %d) %s.\n", lineCount, lineFromFile);  
         // Parse line here
         // Check comand syntax
         // MoveToHomePosition sends: send("Flow,1000,MLRH,10,0,0,0, 0,0,0, 0,0,0, 0,0,0, 0,0,0, 0,0,0, 0,0,0")       
         ofstream a_file(outputFile, ios::app); // Opens file for appending data.
         a_file << lineFromFile << '\n'; // Appends data to file. Add new line to end.
         a_file.close(); // Close the file stream explicitly.  
      } // while    
      myFile.close(); // Close the file object.
   } // if
   else // Cannot open file.
   {
      printf("<appendFile> ERROR - Could not open input file %s.\n", inputFile);
   } // else
   return;
} // appendFile()

/**
 * @brief Display content of file to terminal.
 * @param argv Array of character strings containing each command line argument.
 * @return null.
 ******************************************************************************/
void displayFile(char** argv)
{
   char fileName[maxFileNameSize] = "test.txt";
   const int maxFileLineSize = 80; // Max num characters in one line of file.
   char lineFromFile[maxFileLineSize]; // Hold input from file.  
   int lineCount = 0; // Enumerate lines of file.
   fstream myFile; // Reference to file input.
   printf("<displayFile> Content of file %s:\n", argv[2]);
   myFile.open(fileName, ios::in); // Open file for reading.
   if(myFile.is_open()) //Check whether the file is open.
   {   
      while(myFile.getline(lineFromFile, maxFileLineSize))
      {
         lineCount ++;
         printf("<displayFile> %d) %s.\n", lineCount, lineFromFile);         
      } // while    
      myFile.close(); // Close the file object.
   } // if
   else // Cannot open file.
   {
      printf("<displayFile> Could not open file.\n");
   } // else
   return;
} // displayFile()

/**
 * @brief See if the named file exists. 
 * @param checkFile Pointer to character array containing the file name.
 * @return TRUE if the file exists. FALSE if it does not.
 ******************************************************************************/
bool checkFile(char *checkFile)
{
   fstream inFile; 
   inFile.open(checkFile);
   if(inFile.is_open()) 
   {
      if(debugFlag == true)
      {
         printf("<checkFile> Success! File name %s found.\n", checkFile); 
      } // if
      return true;     
   } // if
   else 
   {
      if(debugFlag == true)
      {
         printf("<checkFile> Error! File name %s NOT found.\n", checkFile);  
      } // if
      return false;    
   } // else      
} // checkFile()

/**
 * @brief Parse an array of characters from a line of the script file. 
 * @param lineToParse A character array pointer to character array containing the file name.
 * @param robotName Pointer to character array containing the robot name.
 * @param movementFile Pointer to character array containing a file name.
 * @param outFile Pointer to character array containing file name to write to.
 * @return TRUE if syntax is good. FALSE if syntax is bad.
 ******************************************************************************/
bool parseScriptLine(char *line, char *robotName, char *movementFile, char *outFile)
{
   string lineToParse(line); // Move character array to string.
   string moveFile(movementFile); // Move char array to string.
   string outputFile(outFile); // Move character array to string.
   string rName(robotName); // Move character array to string.
   std::string cmdTopic = std::string("myBot = \"") + 
                          std::string(rName) + 
                          std::string("/commands\"");
   char topic[cmdTopic.size() + 1];
   strcpy(topic, cmdTopic.c_str());
   std::string s = lineToParse;
   std::string delimiter = " ";
   size_t pos = 0;
   std::string token;
   int index = 0;
   int MaxNumElements = 10;
   string lineElements[MaxNumElements];
   while((pos = s.find(delimiter)) != std::string::npos) 
   {
      token = s.substr(0, pos);
      lineElements[index] = token;
      index ++;
      s.erase(0, pos + delimiter.length());
   }
   token = s;
   lineElements[index] = token; 
   if(debugFlag == true) // If debug is on because of command line.
   {
      for(int x = 0; x<= index; x++) // Loop and show each element of line.
      {
         printf("<parseScriptLine> Element %d = %s\n", x, lineElements[x].c_str());      
      } // for
   } // if
   transform(lineElements[0].begin(), lineElements[0].end(), lineElements[0].begin(), ::toupper); // Convert to upper case.
   if(lineElements[0] == "COPY") // Insert line verbatirm to file.
   {
      string tmp(line); // Convert char array 'line' to string 'tmp' 
      const int START_CHAR = 5;
      const int END_CHAR = 80;
      string outputLine = tmp.substr(START_CHAR, END_CHAR); // Drop the word COPY off the front of this line.
      strcpy(line, outputLine.c_str());
      appendText(line, outFile);     
   } // if
   if(lineElements[0] == "REPLACE") // Insert robot specific MQTT topic name into file now.
   {
      transform(lineElements[1].begin(), lineElements[1].end(), lineElements[1].begin(), ::toupper); // Convert to upper case.
      if(lineElements[1] == "<MYBOT>") // Insert template.
      {
         appendText(topic, outFile); 
      } // if
      else
      {
         printf("<parseScriptLine> ERROR - Invalid insert object type specified: %s\n", lineElements[1].c_str());
         return false;
      } // else
   } // if
   if(lineElements[0] == "INSERT") // Insert object into file now.
   {
      transform(lineElements[1].begin(), lineElements[1].end(), lineElements[1].begin(), ::toupper); // Convert to upper case.
      if(lineElements[1] == "<TEMPLATEFILE>") // Insert template.
      {
         appendFile(movementFile, outFile);
      } // if
      else
      {
         printf("<parseScriptLine> ERROR - Invalid insert object type specified: %s\n", lineElements[1].c_str());
         return false;
      } // else
   } // if
   return true;
} // parseScriptLine()

/**
 * @brief Read through the template file. 
 * @param robotName Pointer to character array containing the robot name.
 * @param templateFile Pointer to character array containing a file name.
 * @param movementFile Pointer to character array containing a file name.
 * @return TRUE if no errors were encountered. FALSE if errors are.
 ******************************************************************************/
bool processTemplateFile(char * robotName, char *templateFile, char *movementFile, char *outFile)
{
   const int maxFileLineSize = 80; // Max num characters in one line of file.
   char lineFromFile[maxFileLineSize]; // Hold input from file.  
   int lineCount = 0; // Enumerate lines of file.
   fstream myFile; // Reference to file input.
   if(debugFlag == true)
   {
      printf("<processTemplateFile> Dumpint content of file %s to terminal...\n", templateFile);
   } // if
   myFile.open(templateFile, ios::in); // Open file for reading.
   if(myFile.is_open()) //Check whether the file is open.
   {   
      // Clear output file
      ofstream outputFile;
      outputFile.open(outFile, ofstream::out | ofstream::trunc);
      outputFile.close();
      // Loop through input file
      while(myFile.getline(lineFromFile, maxFileLineSize))
      {
         lineCount ++;
         if(debugFlag == true)
         {
            printf("<processTemplateFile> %d) %s.\n", lineCount, lineFromFile); 
         } // if
         bool rtn = parseScriptLine(lineFromFile, robotName, movementFile, outFile);        
      } // while    
      myFile.close(); // Close the file object.
   } // if
   else // Cannot open file.
   {
      printf("<processTemplateFile> ERROR - Could not open file %s.\n", templateFile);
      return false;
   } // else
   return true;                              
} // processTemplateFile()

/**
 * @brief Main function where execution begins. 
 * @param argc Count of all command line elements including the program name.
 * @param argv Array of character strings containing each command line argument.
 * @param env_var_ptr Array of characters containing all environment variables.
 * @return appExitCode which contains the app exit code (0 = OK, non 0 = error).
 ******************************************************************************/
int main(int argc, char** argv, char **env_var_ptr)
{
   char inputTemplateFile[30];
   char inputMovementFile[30];
   char robotName[30];
   char outputFileName[30];
   char debugSetting[30];
   bool isValidinputTemplateFile;
   bool isValidinputMovementFile;

   if(argc < 5) // Check for mandatory cmd line arguments.
   {
      printf("<main> ERROR - You have not provided enough command line parameters.\n");
      displayArgs(argc, argv);
      displayHelpMessage();
      return ecNO_FILE_NAME; 
   } // if

   strcpy(robotName, argv[1]); // Check for optional debug argument.
   strcpy(inputTemplateFile, argv[2]);
   strcpy(inputMovementFile, argv[3]);
   strcpy(outputFileName, argv[4]);

   if(argc >= 6) // Check for debug flag
   {
      strcpy(debugSetting, argv[5]);
      if(std::string{debugSetting} == "debug")
      {
         debugFlag = true;
         printf("\n<main> Number of command line arguments provided = %d.\n", argc - 1); // Do not count binary file name.
         printf("<main> 1. Robot name provided = %s\n", robotName);      
         printf("<main> 2. template input file name provided = %s\n", inputTemplateFile);      
         printf("<main> 3. movement input file name provided = %s\n", inputMovementFile);   
         printf("<main> 4. MQTTfx output file name provided = %s\n", outputFileName);   
         printf("<main> 5. Debugging is turned on.\n");  
      } // if
      else
      {
         printf("<main> Error - invalid debug flag value = %s\n", debugSetting);
         return ecBAD_ERR_ARG;      
      } // else
   }  // if

   isValidinputTemplateFile = checkFile(inputTemplateFile); // Validate template file name.
   if(!isValidinputTemplateFile) 
   {
      printf("<main> ERROR - template file name cannot be found.\n");      
      displayHelpMessage();
      return ecNOT_VALID_TEMPLATE_FILE_NAME; 
   } // else

   isValidinputMovementFile = checkFile(inputMovementFile); // Validate movement file name.
   if(!isValidinputMovementFile) 
   {
      printf("<main> ERROR - movement file name cannot be found.\n");      
      displayHelpMessage();
      return ecNOT_VALID_MOVEMENT_FILE_NAME; 
   } // else

   bool rtn = processTemplateFile(robotName, inputTemplateFile, inputMovementFile, outputFileName);
   if(rtn == false)
   {
      return ecNO_BAD_MQTT_SCRIPT_FILE;
   } // if
   if(debugFlag == true)
   {
      printf("<main> Program ending without any errors.\n");
   } // if
   return ecNO_ERR;
} // main()