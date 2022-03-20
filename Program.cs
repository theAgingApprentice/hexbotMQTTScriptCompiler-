/// Programmer notes:
/// Create: https://docs.microsoft.com/en-us/dotnet/core/tutorials/with-visual-studio-code?pivots=dotnet-6-0
/// Run: donet run in directory where *.csproj file resides or ./name where binary resides.
/// Publish: https://docs.microsoft.com/en-us/dotnet/core/tutorials/publishing-with-visual-studio-code?pivots=dotnet-6-0
/// Passing a variable number of parameters to a method: https://www.c-sharpcorner.com/UploadFile/manas1/params-in-C-Sharp-pass-variable-number-of-parameters-to-method/
/// Before running be sure to save the file. This is NOT done automatically in VSC!
/// To run be in src directry and type "dotnet run".
//using System;
//using System.IO;
namespace HexbotCompiler
{
   /// <summary>
   /// This class manages the Hexbot template file. Propertylist:
   /// <list type="Properties">
   /// <item>
   /// <description>getSrcContent</description>
   /// </item>
   /// <item>
   /// <description>templateName</description>
   /// </item>
   /// </list>
   /// </summary>
   class templateFile
   {
      const int MAX_SIZE = 200; // Maximum file size  
      private string[] srcLines = new string[MAX_SIZE]; // Holds lines from template source file.
      private string[] outLines = new string[MAX_SIZE]; // Holds translated lines of sourse file. 
      int outIndex = 0; // Index used for tracking lines translated. 
      public string srcFileName = "template.txt";
      private string robotName = "";
      /// <summary>
      /// Puts each line of the template file into an element of a string array.
      /// <summary>
      /// <param name="null">Does not return any values.</param>
      /// <returns>null</returns>
      /// <exception cref="DataException">Thrown if the file name provided does not exist.</exception>
      public void getSrcContent()
      {
         if (File.Exists(srcFileName)) 
         {
            try 
            {
               srcLines = System.IO.File.ReadAllLines(srcFileName);
            } // try 
            catch(FileNotFoundException e) 
            {
               Console.WriteLine($"Attempted read of {srcFileName} resulted in error {e}.");
            } // catch() 
         } // if
         else
         {
            Console.WriteLine($"Template file {srcFileName} does not exists.");
         } // else
      } // getSrcContent()

      /// <summary>
      /// Transform source file lines to output lines.
      /// <summary>
      /// <param name="null">Does not return any values.</param>
      /// <returns>null</returns>
      public void transformSrc()
      {
         string newLine = "";
         System.Console.WriteLine($"Transforming template source file to output strings.");
         bool invalidInput = true;
         while(invalidInput)
         {
            Console.WriteLine("Please enter the robot name [or shortcut Doug or Andrew]:");
            var userInput = Console.ReadLine();
            if(userInput != "")
            {
               robotName = userInput?.ToString() ?? ""; // Convert input to string (no null).
               if(robotName.ToUpper() == "ANDREW")
               {
                  robotName = "Hexbot94B97E5F4A40";
               } // if
               if(robotName.ToUpper() == "DOUG")
               {
                  robotName = "Hexbot3C61054ADD98";
               } // if
               invalidInput = false;
            } // if
         } // while
         foreach(string line in srcLines)
         {
            var parse = line.Split (' ', 2);
            var cmd = parse[0].Trim();  
            var text = parse[1].Trim();     
            if(cmd == "copy")
            {
               
               newLine = text;
//               Console.WriteLine($"---> {newLine}");
               outLines[outIndex] = newLine;
               outIndex++;
            } // if
            else if(cmd == "replace")
            {
               if(text == "<myBot>")
               {
                  string outLine = "mybot = \"" + robotName + "/commands\""; 
                  newLine = outLine;
//                  Console.WriteLine($"---> {newLine}"); 
                  outLines[outIndex] = newLine;
                  outIndex++;
               } // if
               else
               {
                  Console.WriteLine($"ERROR - replace command in file {srcFileName} given unknown argument of {text}."); 
                  newLine = "// COMPILER ERROR. Replace command in source file " + srcFileName;
                  newLine = newLine + " has unknow argument: " + text;
                  outLines[outIndex] = newLine;
                  outIndex++;
               } // else
            } // else if
            else if(cmd == "insert")
            {
               if(text == "<templateFile>")
               {
//                  Console.WriteLine($"---> {line}"); 
                  newLine = line;
                  outLines[outIndex] = newLine;
                  outIndex++;
               } // if
               else
               {
                  Console.WriteLine($"ERROR - insert command in file {srcFileName} given unknown argument of {text}."); 
                  newLine = "COMPILER ERROR. Insert command in source file " + srcFileName;
                  newLine = newLine + " as unknown argument: " + text;
                  outLines[outIndex] = newLine;
                  outIndex++;
               } // else
            } // else if
            else
            {
               Console.WriteLine($"ERROR - command {cmd} in file {srcFileName} unknown."); 
               newLine = "COMPILER ERROR. Command " + cmd;
               newLine = newLine + " in file " + srcFileName + " is unknown.";
               outLines[outIndex] = newLine;
               outIndex++;
            }
         } // foreach()
      } // transformSrc()

      /// <summary>
      /// Displays the content of the source template file to the console.
      /// <summary>
      /// <param name="null">Does not accept any arguments.</param>
      /// <returns>Does not return any values</returns>
      /// <exception cref="NoException">Does not throw any exceptions.</exception>
      public void displaySrcContent()
      {
         System.Console.WriteLine($"Content of template source file called {srcFileName}... ");
         foreach(string line in srcLines)
         {
            Console.WriteLine("[templateFile.displayContent] " + line);
         } // foreach()
      } // displayContent()
      
      /// <summary>
      /// Return string array of script file transformed.
      /// <summary>
      /// <param name="null">Does not accept any arguments.</param>
      /// <returns>Steing array containing the translated script file</returns>
      /// <exception cref="NoException">Does not throw any exceptions.</exception>
      public string[] getXfrmContent()
      {
         this.getSrcContent();
         this.transformSrc();
         return outLines;
      } // getXfrmContent()      
   } // class templateFile

   /// <summary>
   /// This class manages the Hexbot script file. Propertylist:
   /// <list type="Properties">
   /// <item>
   /// <description>getSrcContent</description>
   /// </item>
   /// </list>
   /// </summary>
   class scriptFile
   {
      const int MAX_SIZE = 200; // Maximum file size  
      private string[] srcLines = new string[MAX_SIZE]; // Holds lines from script source file.
      private string[] outLines = new string[MAX_SIZE]; // Holds translated lines of sourse file. 
      int outIndex = 0; // Index used for tracking lines translated. 
      private string srcFileName = "";


      /// <summary>
      /// Puts each line of the script file into an element of a string array.
      /// <summary>
      /// <param name="null">Does not return any values.</param>
      /// <returns>null</returns>
      /// <exception cref="DataException">Thrown if the file name provided does not exist.</exception>
      public void getSrcContent()
      {
         bool scriptFileNotFound  = true;
         while(scriptFileNotFound)
         {
            Console.WriteLine("Please enter the script file name:");
            var userInput = Console.ReadLine();
            srcFileName = userInput?.ToString() ?? ""; // Convert input to string (no null). 
            try 
            {
               srcLines = System.IO.File.ReadAllLines(srcFileName);
               scriptFileNotFound = false;    
            } // try 
            catch(FileNotFoundException e) 
            {
               Console.WriteLine($"ERROR. Attempted read of {srcFileName} resulted in error {e}.");
            } // catch() 
         } // while
      } // getSrcContent()

      /// <summary>
      /// Transform source file lines to output lines.
      /// <summary>
      /// <param name="null">Does not accept any arguments.</param>
      /// <returns>null. Does not return anything.</returns>
      public void transformSrc()
      {
         string tmr = "1000"; // How long to allow for move in ms.
         string newLine = "";
         newLine = "send(\"NEW_FLOW\")";
         outLines[outIndex] = newLine;
         outIndex++;
         foreach(string line in srcLines)
         {
            var parse = line.Split(' ', 2);
            var cmd = parse[0].Trim();  
            if(cmd == "symdef")
            {
               newLine = "// " + line + " // symdef syntax not yet implemented in compiler.";
               outLines[outIndex] = newLine;
               outIndex++;
            } // if
            else if(cmd == "MoveToHomePosition")
            {
               newLine = "send(\"Flow," + tmr + ",MLRH,10,0,0,0, 0,0,0, 0,0,0, 0,0,0, 0,0,0, 0,0,0, 0,0,0\")";
               outLines[outIndex] = newLine;
               outIndex++;
            } // if
            else if(cmd == "command")
            {
               newLine = "// " + line + " // command syntax not yet implemented in compiler.";
               outLines[outIndex] = newLine;
               outIndex++;
            } // if
            else if(cmd == "MoveRelHomeLocal")
            {
               string sendCmd = "send(\"Flow,"; // First part of send command. 
               string macro = ",MLRH,"; // Type of send command.
               string moveStright = "10,0,0,0, "; // Only support move toe in straight line for now.                
               newLine = sendCmd + tmr + "," + macro + "," + moveStright;
               var arg = parse[1].Split(',', 4);
               var leg = arg[0].Trim();
               var x = arg[1].Trim();
               var y = arg[2].Trim();
               var z = arg[3].Trim(); 
               if(z.EndsWith(","))
               {
                  z = z.Remove(z.Length - 1);
               } // if
               try
               {
                  int legNum = int.Parse(leg);
                  newLine = sendCmd + tmr + macro + moveStright;
                  switch(legNum)
                  {
                     case 1:
                        newLine = newLine + x + "," + y + "," + z + ", ";
                        newLine = newLine + "0,0,0, 0,0,0, 0,0,0, 0,0,0, 0,0,0\")"; 
                        break;
                     case 2:
                        newLine = newLine + "0,0,0, "; 
                        newLine = newLine + x + "," + y + "," + z + ", ";
                        newLine = newLine + "0,0,0, 0,0,0, 0,0,0, 0,0,0\")"; 
                        break;
                     case 3:
                        newLine = newLine + "0,0,0, 0,0,0, "; 
                        newLine = newLine + x + "," + y + "," + z + ", ";
                        newLine = newLine + "0,0,0, 0,0,0, 0,0,0\")"; 
                        break;
                     case 4:
                        newLine = newLine + "0,0,0, 0,0,0, 0,0,0, "; 
                        newLine = newLine + x + "," + y + "," + z + ", ";
                        newLine = newLine + "0,0,0, 0,0,0\")"; 
                        break;
                     case 5:
                        newLine = newLine + "0,0,0, 0,0,0, 0,0,0, 0,0,0, "; 
                        newLine = newLine + x + "," + y + "," + z + ", ";
                        newLine = newLine + "0,0,0\")"; 
                        break;
                     case 6:
                        newLine = newLine + "0,0,0, 0,0,0, 0,0,0, 0,0,0, 0,0,0, "; 
                        newLine = newLine + x + "," + y + "," + z + "\")";
                        break;
                     default:
                        Console.WriteLine($"ERROR transforming script file. Leg number {legNum} is invalid");
                        newLine = "// COMPILER ERROR! Parsing leg command in script src file. Illegal lg number: " + legNum;
                        outLines[outIndex] = newLine;
                        outIndex++;
                        break;
                  } // switch
                  outLines[outIndex] = newLine;
                  outIndex++;
               } // try
               catch (FormatException e)
               {
                  Console.WriteLine($"ERROR transforming script file. Parsing leg number caused {e.Message}");
                  newLine = "// COMPILER ERROR! Parsing leg command in script src file: " + e.Message;
                  outLines[outIndex] = newLine;
                  outIndex++;
               } // catch              
            } // if
            else if(cmd == "Doit")
            {
               newLine = "send(\"FLOW,49,50\")";
               outLines[outIndex] = newLine;
               outIndex++;
            } // if
            else
            {
               Console.WriteLine($"ERROR - command {cmd} in file {srcFileName} is unknown.");
               newLine = "// COMPILER ERROR! Unknown command in script src file: " + cmd;
               outLines[outIndex] = newLine;
               outIndex++;
            } // else
         } // foreach()
      } // transformSrc()

      /// <summary>
      /// Displays the content of the source template file to the console.
      /// <summary>
      /// <param name="null">Does not accept any arguments.</param>
      /// <returns>Does not return any values</returns>
      /// <exception cref="NoException">Does not throw any exceptions.</exception>
      public void displaySrcContent()
      {
         System.Console.WriteLine($"Content of script source file called {srcFileName}... ");
         foreach(string line in srcLines)
         {
            Console.WriteLine("[scriptFile.displaySrcContent] " + line);
         } // foreach()
      } // displayContent()      

      /// <summary>
      /// Return string array of script file transformed.
      /// <summary>
      /// <param name="null">Does not accept any arguments.</param>
      /// <returns>Steing array containing the translated script file</returns>
      /// <exception cref="NoException">Does not throw any exceptions.</exception>
      public string[] getXfrmContent()
      {
         this.getSrcContent();
         this.transformSrc();
         return outLines;
      } // getXfrmContent()      
   } // class scriptFile
   
   /// <summary>
   /// This class manages the Hexbot MQTTfx file. Propertylist:
   /// <list type="Properties">
   /// <item>
   /// <description>getSrcContent</description>
   /// </item>
   /// <item>
   /// <description>templateName</description>
   /// </item>
   /// </list>
   /// </summary>
   class fxFile
   {
      public void create(string[] scriptFile, string[] templateFile)
      {
         bool invalidInput = true;
         string outFile = "";
         while(invalidInput)
         {
            Console.WriteLine("Please enter the MQTTfx file name:");
            var userInput = Console.ReadLine();
            if(userInput != null)
            {
               if(File.Exists(userInput)) 
               {
                  Console.WriteLine("The file exists. Do you wish to overwite it (Y/N)");
                  if(Console.ReadKey().Key == ConsoleKey.Y)
                  {
                     outFile = userInput;
                     invalidInput = false;
                  } // if
               } // if
               else
               {
                  outFile = userInput;
                  invalidInput = false;
               } // else
            } // if
         } // While
         var currentDate = DateTime.Now;
         string newLine = "// This MQTTfx script for Hexbot robot created by hexbotScriptCompiler on ";
         newLine = newLine + currentDate.Month + "/" + currentDate.Day + "/" + currentDate.Year;
         newLine = newLine +  " at " + currentDate.TimeOfDay;
         using(FileStream aFile = new FileStream(outFile, FileMode.Create, FileAccess.Write))
         using(StreamWriter sw = new StreamWriter(aFile))
         {
            sw.WriteLine("// *****************************************************************************************************");
            sw.WriteLine(newLine);
            sw.WriteLine("// NOTE: Doug's robot = Hexbot3C61054ADD98, Andrew's robot = Hexbot94B97E5F4A40.");
            sw.WriteLine("// *****************************************************************************************************");
         } // using
         foreach(string lineTemplate in templateFile)
         {
            if(lineTemplate != null)
            {
               if(lineTemplate == "insert <templateFile>")
               {
                  foreach(string lineScript in scriptFile)
                  {
                     if(lineScript != null)
                     {
                        using(FileStream aFile = new FileStream(outFile, FileMode.Append, FileAccess.Write))
                        using(StreamWriter sw = new StreamWriter(aFile))
                        {
                           sw.WriteLine(lineScript);
                        } // using
                     } // if
                  } // foreach
               } // if
               else
               {
                  using(FileStream aFile = new FileStream(outFile, FileMode.Append, FileAccess.Write))
                  using(StreamWriter sw = new StreamWriter(aFile))
                  {
                     sw.WriteLine(lineTemplate);
                  } // using
               } // else
            } // if
         } // foreach
      } // create()
   } // class fxFile

   /// <summary>
   /// Generates MQTTfx files that control the Hexbot legs.
   /// <list type="Properties">
   /// <item>
   /// <description>Main</description>
   /// </item>
   /// </list>
   /// </summary>   
   class Program
   {
      /// <summary>
      /// This is the main logic driving this program.
      /// <summary>
      /// <param name="args">string array containing command line arguments.</param>
      /// <returns>null</returns>
      static void Main(string[] args)
      {
         var template = new templateFile(); // Object reference to template file.
         var script = new scriptFile(); // Object reference to script file.
         var mqttx = new fxFile(); // Object reference to MQTTfx file. 
         string[] scriptArray = new string[200]; // Content of transformed script file.
         string[] templateArray = new string[200]; // Content of transformed template file.  
         template.srcFileName = "template.txt";      
         Console.Clear();
         scriptArray = script.getXfrmContent(); // Get transformed script file content.
         templateArray = template.getXfrmContent(); // Get transformed template file content.
         mqttx.create(scriptArray, templateArray); // Create MQTTfx file.
      } // Main()
   } // class Program
} // namespace HexbotCompiler