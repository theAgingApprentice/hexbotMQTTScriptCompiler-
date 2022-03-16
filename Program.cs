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
         Console.WriteLine($"Transforming script source file to output strings.");
/*
         foreach(string line in srcLines)
         {
            var parse = line.Split (' ', 2);
            var cmd = parse[0].Trim();  
            var text = parse[1].Trim();   
            Console.WriteLine($"---> cmd = {cmd}");  
            Console.WriteLine($"---> text = {text}");  
         } // foreach
*/
         Console.WriteLine($"-->send(\"NEW_FLOW\")"); // First line of script
         foreach(string line in srcLines)
         {
            var parse = line.Split (' ', 2);
            var cmd = parse[0].Trim();  
            Console.WriteLine("-->" + cmd);
            if(cmd == "symdef")
            {
               string newLine = "// " + line + " // symdef syntax not yet implemented in compiler.";
               Console.WriteLine($"-->{newLine}");
            } // if
            else if(cmd == "MoveToHomePosition")
            {
               Console.WriteLine("-->send(\"Flow,1000,MLRH,10,0,0,0, 0,0,0, 0,0,0, 0,0,0, 0,0,0, 0,0,0, 0,0,0\")");
            } // if
            else if(cmd == "command")
            {
               string newLine = "// " + line + " // command syntax not yet implemented in compiler.";
               Console.WriteLine($"-->{newLine}");
            } // if
            else
            {
               Console.WriteLine($"-->ERROR - command {cmd} in file {srcFileName} is unknown.");
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

/*
//         string scriptName = uInput?.ToString() ?? "";


//         var name = Console.ReadLine();
//         var currentDate = DateTime.Now;
//         Console.WriteLine($"{Environment.NewLine}Hello, {name}, on {currentDate:d} at {currentDate:t}!");
//         Console.Write($"{Environment.NewLine}Press any key to exit...");
//         Console.ReadKey(true);
*/