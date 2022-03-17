/// Programmer notes:
/// Create: https://docs.microsoft.com/en-us/dotnet/core/tutorials/with-visual-studio-code?pivots=dotnet-6-0
/// Run: donet run in directory where *.csproj file resides or ./name where binary resides.
/// Publish: https://docs.microsoft.com/en-us/dotnet/core/tutorials/publishing-with-visual-studio-code?pivots=dotnet-6-0
/// Passing a variable number of parameters to a method: https://www.c-sharpcorner.com/UploadFile/manas1/params-in-C-Sharp-pass-variable-number-of-parameters-to-method/
/// Before running be sure to save the file. This is NOT done automatically in VSC!
/// To run be in src directry and type "dotnet run".
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
      private string[] srcLines = {""}; 
      private string[] outLines = {""};   
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
         System.Console.WriteLine($"Transforming template source file to output strings.");
         bool invalidInput = true;
         while(invalidInput)
         {
            Console.WriteLine("Please enter the robot name:");
            var userInput = Console.ReadLine();
            if(userInput != "")
            {
               robotName = userInput?.ToString() ?? ""; // Convert input to string (no null).
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
               Console.WriteLine($"---> {text}");
            } // if
            else if(cmd == "replace")
            {
               if(text == "<myBot>")
               {
                  string outLine = "mybot = \"" + robotName + "/commands\""; 
                  Console.WriteLine($"---> {outLine}"); 
               } // if
               else
               {
                  Console.WriteLine($"ERROR - replace command in file {srcFileName} given unknown argument of {text}."); 
               } // else
            } // else if
            else if(cmd == "insert")
            {
               if(text == "<templateFile>")
               {
                  Console.WriteLine($"---> {line}"); 
               } // if
               else
               {
                  Console.WriteLine($"ERROR - insert command in file {srcFileName} given unknown argument of {text}."); 
               } // else
            } // else if
            else
            {
               Console.WriteLine($"ERROR - command {cmd} in file {srcFileName} unknown."); 
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
            Console.WriteLine("-->" + line);
         } // foreach()
      } // displayContent()

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
      private string[] srcLines = {""}; 
      private string[] outLines = {""};   
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
               Console.WriteLine($"Attempted read of {srcFileName} resulted in error {e}.");
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
         Console.WriteLine($"Transforming script source file to output strings.");
         var currentDate = DateTime.Now;
         string newLine = "// MQTTfx script for Hexbot robot created by hexbotScriptCompiler on ";
         newLine = newLine + currentDate.Day + " at " + currentDate.TimeOfDay;
         Console.WriteLine($"-->{newLine}"); // First line of script
         Console.WriteLine($"-->send(\"NEW_FLOW\")"); // First line of script
         foreach(string line in srcLines)
         {
            var parse = line.Split(' ', 2);
            var cmd = parse[0].Trim();  
            if(cmd == "symdef")
            {
               newLine = "// " + line + " // symdef syntax not yet implemented in compiler.";
               Console.WriteLine($"-->{newLine}");
            } // if
            else if(cmd == "MoveToHomePosition")
            {
               newLine = "send(\"Flow," + tmr + ",MLRH,10,0,0,0, 0,0,0, 0,0,0, 0,0,0, 0,0,0, 0,0,0, 0,0,0\")";
               Console.WriteLine($"-->{newLine}");
            } // if
            else if(cmd == "command")
            {
               newLine = "// " + line + " // command syntax not yet implemented in compiler.";
               Console.WriteLine($"-->{newLine}");
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
                        break;
                  } // switch
                  Console.WriteLine($"-->{newLine}");
               } // try
               catch (FormatException e)
               {
                  Console.WriteLine($"ERROR transforming script file. Parsing leg number caused {e.Message}");
               } // catch              
            } // if
            else if(cmd == "Doit")
            {
               newLine = "send(\"DO_FLOW,49,50\")";
               Console.WriteLine($"-->{newLine}");
            } // if
            else
            {
               Console.WriteLine($"ERROR - command {cmd} in file {srcFileName} is unknown.");
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
            Console.WriteLine("-->" + line);
         } // foreach()
      } // displayContent()      
   } // class scriptFile

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
         var template = new templateFile();
         var script = new scriptFile(); 
         Console.Clear();
         template.srcFileName = "template.txt";
         script.getSrcContent();
         script.transformSrc();
//         script.displaySrcContent();
         template.getSrcContent();
         template.transformSrc();
//         template.displaySrcContent();

      } // Main()
   } // class Program
} // namespace HexbotCompiler