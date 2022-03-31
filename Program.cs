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

            public void getSrcContent()      // we're in templateFile class
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
      public void transformSrc()      // we're in templateFile class
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
      public void displaySrcContent()      // we're in templateFile class
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
      public string[] getXfrmContent()      // we're in templateFile class
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
      // arrays to track running absolute locations of 6 legs, each with X, Y and Z
      private double[] legGloX = new double[7], legGloY = new double[7], legGloZ = new double[7];
      // arrays to track running local locations of 6 legs, each with X, Y and Z
      private double[] legLocX = new double[7], legLocY = new double[7], legLocZ = new double[7];
      public double[] f_hipX = new double[7], f_hipY = new double[7];
      int leg;       // often used as a loop index
      //string newLine;

      double fp_frontHipX = 3.82739F + 7.13528F * .707107F;  // = 8.872796
      double fp_frontHipY = 5.04750F;
      double fp_sideHipX = 0;
      double fp_sideHipY = 6.9423F;
      public int[] legMask = new int[7];     // binary mask that selects a particular leg
      public int[] legGroup = new int[27];   // definition of legGroups. index is letter of the alphabet,
                                             // value is bit encoded legs that are present in group
         double localHomeX ;
         double localHomeY ;
         double localHomeZ ;


      /// <summary>
      /// Puts each line of the script file into an element of a string array.
      /// <summary>
      /// <param name="null">Does not return any values.</param>
      /// <returns>null</returns>
      /// <exception cref="DataException">Thrown if the file name provided does not exist.</exception>
      public void getSrcContent()      // we're in scriptFile class
      {
         // some setup stuff for script processing
         setupForScript();

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
               // scriptFileNotFound  = true;

            } // catch() 
         } // while
      } // getSrcContent()

      // C# seems to be missing a RADIANS function, so...

      double rad(double angle)
      {
         return (Math.PI / 180F) * angle;
      }


      // translate global coordinates to local coordinates for a specified leg
      private void transGlobalToLocal( int leg, double gx, double gy, double gz, ref double lx, ref double ly, ref double lz)
      {
      // rotating a vector(X,Y) thru counter clockwise angle B, to get (Xr,Yr)
      //    Xr = cos(B) * X - sin(B) * Y
      //    Yr = sin(B) * X + cos(B) * Y

         double Xrt, Yrt ;       //temp variables for rotated global X & Y coords
         lz = fp_sideHipX;       //  null statement so compiler won't complain fp_sideHipX is unreferenced
         ly = gz;      // height above robot is global Z, local Y
         switch (leg) 
         {
            case 1:
            // Front Right leg
            Xrt = Math.Cos(rad(-45)) * (gx - fp_frontHipX) - Math.Sin(rad(-45)) * (gy + fp_frontHipY);  // rotated (Xg,Yg)
            Yrt = Math.Sin(rad(-45)) * (gx - fp_frontHipX) + Math.Cos(rad(-45)) * (gy + fp_frontHipY);
            lx = -1 * Yrt;
            lz = Xrt;
            break;
         
            case 2:
            // Middle Right leg
            lx = -1 * gy - fp_sideHipY;
            lz = gx;
            break;

            case 3:
            // Back Right leg
            Xrt = Math.Cos(rad(45)) * (gx + fp_frontHipX) - Math.Sin(rad(45)) * (gy + fp_frontHipY);  // rotated (Xg,Yg)
            Yrt = Math.Sin(rad(45)) * (gx - fp_frontHipX) + Math.Cos(rad(45)) * (gy + fp_frontHipY);
            lx = -1 * Yrt;
            lz = Xrt;        
            break;

            case 4:
            // Front Left leg
            Xrt = Math.Cos(rad(45)) * (gx - fp_frontHipX) - Math.Sin(rad(45)) * (gy - fp_frontHipY);  // rotated (Xg,Yg)
            Yrt = Math.Sin(rad(45)) * (gx - fp_frontHipX) + Math.Cos(rad(45)) * (gy - fp_frontHipY);
            lx = Yrt;
            lz = -1 * Xrt;         
            break;
            
            case 5:  
            // Middle Left leg
            lx = gy - fp_sideHipY;
            lz = -1 * gx ;
            break;

            case 6:
            // Back Left leg
            Xrt = Math.Cos(rad(-45)) * (gx + fp_frontHipX) - Math.Sin(rad(-45)) * (gy - fp_frontHipY);  // rotated (Xg,Yg)
            Yrt = Math.Sin(rad(-45)) * (gx + fp_frontHipX) + Math.Cos(rad(-45)) * (gy - fp_frontHipY);
            lx = Yrt;
            lz = -1* Xrt;        
            break;
         
            default:
            lx = 0;        // give a vague hint of an error
            ly = 0;
            lz = 0;
            return ;
         }
         return ;
      }  // private void transGlobalToLocal( 

      // translate local coords into global coords for a specified leg

      public void transLocalToGlobal(int leg, double lx, double ly, double lz, ref double gx, ref double gy, ref double gz)
      {
         gz = ly;        // height off floor is easy. 
         switch (leg)
         {
            case 1:
            // Front Right leg
            gx = f_hipX[leg] + lx * Math.Cos(rad(45)) + lz * Math.Cos(rad(45));
            gy = f_hipY[leg] - lx * Math.Cos(rad(45)) + lz * Math.Cos(rad(45));
            break;

            case 2:
            // Middle Right leg
            gx = f_hipX[leg] + lz;
            gy = f_hipY[leg] - lx;
            break;

            case 3:
            // Back Right leg
            gx = f_hipX[leg] - lx * Math.Cos(rad(45)) + lz * Math.Cos(rad(45));
            gy = f_hipY[leg] - lx * Math.Cos(rad(45)) - lz * Math.Cos(rad(45));
            break;

            case 4:
            // Front Left leg
            gy = f_hipX[leg] + lx * Math.Cos(rad(45)) - lz * Math.Cos(rad(45));
            gy = f_hipY[leg] + lx * Math.Cos(rad(45)) + lz * Math.Cos(rad(45));
            break;

            case 5:
            // Middle Left leg
            gx = f_hipX[leg] - lz;
            gy = f_hipY[leg] + lx;
            break;

            case 6:
            // Back Left leg
            gx = f_hipX[leg] - lx * Math.Cos(rad(45)) - lz * Math.Cos(rad(45));
            gy = f_hipY[leg] + lx * Math.Cos(rad(45)) - lz * Math.Cos(rad(45));
            break;

            default:
            break;
         }
         return;
      }   // public transLocalToGlobal

            public void doDoit(int tmr)       // the doit command processing is done if there is a doit command in the script
      {                                // but also implicitly from some commands like MoveToHomePosition
         string sendCmd = "send(\"Flow,"; // First part of send command. 
         string macro = "MLC"; // Type of send command.
         string moveStright = "10,0,0,0"; // Only support move toe in straight line for now.                
         string newLine = sendCmd + tmr + "," + macro + "," + moveStright;
         // now output 3 numbers representing local coords, for each leg
         for(int l=1; l<=6; l++)
         {
            newLine += " ," + legLocX[l].ToString();   // append X value
            newLine += "," + legLocY[l].ToString();   // append Y value
            newLine += "," + legLocZ[l].ToString();   // append Z value
         }  // for l=
         outLines[outIndex] = newLine + "\")";
         outIndex++;
      } // public void doDoit() 


      // setupForScript - Setup preparations for processing the script file
      // called from script.getSrcContent()

      public void setupForScript()
      {
         // define locations of each leg's hip, in global coordinates
         
         f_hipX[1] = 8.87;  f_hipY[1] = -5.05;     // global coordinates for hip for each leg    
         f_hipX[2] =   0 ;  f_hipY[2] = -6.94;
         f_hipX[3] =-8.87;  f_hipY[3] = -5.05;
         f_hipX[4] = 8.87;  f_hipY[4] =  5.05;
         f_hipX[5] =   0 ;  f_hipY[5] =  6.94;
         f_hipX[6] =-8.87;  f_hipY[6] =  5.05;

         localHomeX = 13.78 ;      // by definition, local coords for home position are the same for all legs
         localHomeY = -10.60 ;
         localHomeZ = 0 ;

         // a "move to home" command is added to start of script so we have a known starting point
         // ...based on that, we can fill in the current toe locations (after that command is executed)

         for(leg = 1; leg <= 6; leg++)
         {
            legLocX[leg] = localHomeX;  // the local coords are same for each leg, being local and all
            legLocY[leg] = localHomeY;
            legLocZ[leg] = localHomeZ;

            // now translate these to global coords. We'll track current position in both coord systems
            transLocalToGlobal( leg, legLocX[leg], legLocY[leg], legLocZ[leg], ref legGloX[leg], ref legGloY[leg], ref legGloZ[leg]);

            // and initialize the masks that select the bit representing a leg in the legGroup
            legMask[leg] = Convert.ToInt32( Math.Pow(2, (leg - 1) ));

         }  //  for(legAbsX = 1
         for( int i = 1; i <= 26; i++)   // initialize the 26 leg groups to having no members
         {
            legGroup[i] = 0 ;
         } // for( int i = 1;
         // now fill in the default groups
         // define symbolic names for the binary mask for each leg for clarity
         // note also the array legMask[l] specifies the binary mask for leg "l"
         int leg1 = 1;
         int leg2 = 2;
         int leg3 = 4;
         int leg4 = 8;
         int leg5 = 16;
         int leg6 = 32;

         legGroup[1] = leg1 + leg2 + leg3 + leg4 + leg5 + leg6;   // a = 1 = all legs
         legGroup[2] = leg3 + leg6;                               // b = 2 = back legs
         legGroup[6] = leg1 + leg4;                               // f = 6 = front legs
         legGroup[12]= leg4 + leg5 + leg6;                        // l = 12 = left legs
         legGroup[18]= leg1 + leg2 + leg3;                        // r = 18 = right legs
         legGroup[5] = leg2 + leg4 + leg6;                        // e = 5 = even numbered legs
         legGroup[15]= leg1 + leg3 + leg5;                        // o = 15 = odd numbered legs
         legGroup[13]= leg2 + leg5;                               // m = 13 = middle legs

      }  //  public void setupForScript()

      /// <summary>
      /// Transform source file lines to output lines.
      /// <summary>
      /// <param name="null">Does not accept any arguments.</param>
      /// <returns>null. Does not return anything.</returns>
      public void transformSrc()      // we're in scriptFile class
      {
         string newLine = "";

         // force out a move to home position command, so we have a known starting point
         //  ... which will be used to initialize internal variables
         newLine = "send(\"Flow, 1,MLRH,10,0,0,0, 0,0,0, 0,0,0, 0,0,0, 0,0,0, 0,0,0, 0,0,0\")";
         outLines[outIndex] = newLine;
         outIndex++;
         foreach(string line in srcLines)
         {
                     
//   Console.WriteLine($"<script.transformSrc> processing script line: {line} ");

            var parse = line.Split(' ', 2);
            var cmd = parse[0].Trim();  
// ========================================================================================================= "symdef"
            if(cmd == "symdef")
            {
               newLine = "// " + line + " // symdef syntax not yet implemented in compiler.";
               outLines[outIndex] = newLine;
               outIndex++;
            } // if
// ===================================================================================================== "MoveToHomePosition"
            else if(cmd == "MoveToHomePosition")
            {
               for(leg = 1; leg <= 6; leg++)
               {
                  legLocX[leg] = localHomeX;  // the local coords are same for each leg, being local and all
                  legLocY[leg] = localHomeY;
                  legLocZ[leg] = localHomeZ;

                  // now translate these to global coords. We'll track current position in both coord systems
                  transLocalToGlobal( leg, legLocX[leg], legLocY[leg], legLocZ[leg], ref legGloX[leg], ref legGloY[leg], ref legGloZ[leg]);
               }  //  for(leg = 1

               doDoit(500);     // force a doit command with a time interval of 500 ( its a quick servo jump anyway)

            } // if
// ========================================================================================================== "command"
            else if(cmd == "command")
            {
               newLine = "// " + line + " // command syntax not yet implemented in compiler.";
               outLines[outIndex] = newLine;
               outIndex++;
            } // if
// ====================================================================================================== "MoveRelHomeLocal"
            else if(cmd == "MoveRelHomeLocal")
            {
               
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
                  if(legNum < 0 || legNum > 6)        // range check given leg number
                  {
                     Console.WriteLine($"ERROR transforming script file. Leg number {legNum} is an invalid number");
                     newLine = "// COMPILER ERROR! Parsing leg command in script src file. Illegal lg number: " + legNum;
                     outLines[outIndex] = newLine;
                     outIndex++;
                     break;
                  }
                  else
                  {
                     // update the toe position for the specified leg as requested
                     legLocX[legNum] = localHomeX + Convert.ToDouble(x);
                     legLocY[legNum] = localHomeY + Convert.ToDouble(y);
                     legLocZ[legNum] = localHomeZ + Convert.ToDouble(z);
                     
                     // now update the equivalent global coordinates
                     transLocalToGlobal(legNum, legLocX[legNum], legLocY[legNum], legLocZ[legNum], ref legGloX[legNum], ref legGloY[legNum], ref legGloZ[legNum]);

                  } // else legNum check
               
               } // try
               catch (FormatException e)
               {
                  Console.WriteLine($"ERROR transforming script file. Parsing leg number caused {e.Message}");
               } // catch            
            } // if

// ====================================================================================================== "MoveRelLastLocal"
// move leg (or leg group) an offset in the x, y and z arguments from the last (i.e. current) leg position
            else if(cmd == "MoveRelLastLocal")
            {
               // this code largely copied from MoveRelHomeLocal case  
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
                  if(legNum < 0 || legNum > 6)        // range check given leg number
                  {
                     Console.WriteLine($"ERROR transforming script file. Leg number {legNum} is an invalid number");
                     newLine = "// COMPILER ERROR! Parsing leg command in script src file. Illegal leg number: " + legNum;
                     outLines[outIndex] = newLine;
                     outIndex++;
                     break;
                  }
                  else
                  {
                     // update the toe position for the specified leg as requested
                     legLocX[legNum] += Convert.ToDouble(x);
                     legLocY[legNum] += Convert.ToDouble(y);
                     legLocZ[legNum] += Convert.ToDouble(z);
                     
                     // now update the equivalent global coordinates
                     transLocalToGlobal(legNum, legLocX[legNum], legLocY[legNum], legLocZ[legNum], ref legGloX[legNum], ref legGloY[legNum], ref legGloZ[legNum]);

                  } // else legNum check
               
               } // try
               catch (FormatException e)
               {
                  Console.WriteLine($"ERROR transforming script file. Parsing leg number caused {e.Message}");
               } // catch            
            } // if


// ========================================================================================================== "Doit"
            else if(cmd == "Doit")
            {
               // get the time interval from the doit command line
               var arg = parse[1].Split(',', 4);
               var interval = arg[0].Trim();
               int interval1 = int.Parse(interval);

               doDoit(interval1);   // call common routine for doit processing
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
      public void displaySrcContent()      // we're in scriptFile class
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
      public string[] getXfrmContent()      // we're in scriptFile class
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
         // leave complier warnings on screen
         //Console.Clear();
         scriptArray = script.getXfrmContent(); // Get transformed script file content.
         templateArray = template.getXfrmContent(); // Get transformed template file content.
         mqttx.create(scriptArray, templateArray); // Create MQTTfx file.
      } // Main()
   } // class Program
} // namespace HexbotCompiler