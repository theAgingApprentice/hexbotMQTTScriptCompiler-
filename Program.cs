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
   public class templateFile
   {
      const int MAX_SIZE = 200; // Maximum file size  
      private string[] srcLines = new string[MAX_SIZE]; // Holds lines from template source file.
      private string[] outLines = new string[MAX_SIZE]; // Holds translated lines of sourse file. 
      int outIndex = 0; // Index used for tracking lines translated. 
      public string srcFileName = "template.txt";
      private string robotName = "";

      public string[] symNames = new string[20];    // single character name for a symbol like $H
      public string[] symStrings = new string[20];  // string to be substituted for name with same index
      public string[] symLines = new string[100];  // holds lines from symbols file, including comments
      public int symCount = 0;
      public string[] errorLines = new string[100]; // holds warning & error messages, one line per array element
      public int errorNum = 0;   // index into errorLines[] 
      public string[] parse = new string[30];   // for prepForMove call
  
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

      // following routines, translateSymbols and symLookup, are duplicated from script object to avoid cross object method calls

      // symLookup(name) - look up a specified symbol's substitution string
      //
      // before this routine is called, the disk file symbols.txt has been processed to produce the following
      //   symCount - the number of symbols that were defined
      //   symNames[] - the uppercase single letters that are the symbol names
      //   sysStrings[] - the substitution string for the symbol name with the same array index

      public string symLookup(string name)   // look up argument name, and return its string (in Scriptfile class)
      {
            if(symCount == 0) // if we had no symbols defined..
            {
               return "";    // return the null string
            } // if(symCount == 0) 
            for( int j=0; j<symCount; j++)   // scan through all symbol names, seeking a match
            {
               if(symNames[j] == name.ToUpper())   // if we found one that matched the name we were given
               { 
                  return symStrings[j];   // return the corresponding string
               }
            } // for( int j=0; j<symCount; j++)
            return "";// there were symbols in table, but none matched
      } // void symLookup(string name)
      
      // translateSymbols( line) - replace any symbols in the form $X in line, with string from symStrings[]
      // called at beginning of per line processing of template and script

      public string translateSymbols(string lin)   // do symbol substution, returning expanded line (in Scriptfile class)
      {  
         int symLoc;
         string symString = "";  // also flags substitution was done if it ends up non-blank
         for (int i=1; i<=4; i++)      // substitute up to 4 symbols on the same line
         {
            if(lin.Contains("$"))     // a symbol is denoted by using a $ prefix to its name
            {
               symLoc = lin.IndexOf("$");  // location of $ in line (1st position is zero)
               string symName = lin.Substring(symLoc+1, 1); // the following char is the symbol name
               symString = symLookup(symName);    // find its corresponding substitution string
               lin = lin.Substring(0, symLoc) + symString + lin.Substring(symLoc+2); //rebuild line doing substitution
               // Console.WriteLine($"---line={line}");
            } // if line.contains $
         }// for (int i=1; i<=4; i++)
         return lin; // returned line has up to 4 symbols substituted

      } // string translateSymbols(string line)
 
      public string[] getErrors()   // main calls this to get any warnings generated during compilation
      {
         // for now, just test returning a string value
         return errorLines; // array with each error or warning described in one entry
      } // public string getWarnings()

      public void newError(int lin, string type, string msg)   // add a new error/warning to the list in errorLines[]
      {
         errorLines[errorNum] = lin.ToString() + "  " + type + " " + msg;
         errorNum ++ ;
         return;
      } // public void newError

      /// <summary>
      /// Transform source file lines to output lines.
      /// <summary>
      /// <param name="null">Does not return any values.</param>
      /// <returns>null</returns>
      public void transformSrc()      // we're in templateFile class
      {
         // first, read symbol definitions used in template and script files
         //  since we made need these symbols while doing the transformation
         // the symbol file is processed in both the template and the script objects,
         // but error reporting is only done here in the template

         if(File.Exists("symbols.txt")) 
         {
            errorNum = 0;       // index into list of errors we accumulate in errorLines[]
            int symLine = 0;        // line number within symbol file, for error message references

            symLines = System.IO.File.ReadAllLines("symbols.txt");
            foreach(string l in symLines)
            {
               string line = l ;       // create overwritable copy for error recovery
               symLine ++ ;      // count one more line in the symbol file
               if( line == "" | line.StartsWith("/") )
               {
                  // if line is a comment, or empty, or starts with a space, ignore it
               }  // if( line != ""...
               else  // otherwise it's a symbol definition
               {
                  if(line.Length < 2 | !line.Contains(' ')) // if the line is too short or ill formed...
                  {
                     // report an error
                     // newError(symLine,  "Error:", "In symbols.txt: line too short or missing space separator");
                     line = "- -";  // substitute so that parse[1] will exist nad not cause out of bounds errors
                  }// if(line.Length < 2 | !line.Contains(' '))
                  string[] parse = line.Split(' ',2);    // chop it into 2 pieces by the space after the symbol name
                  string sym = parse[0];
                  if(sym.Length != 1 | !Char.IsLetter(sym,0))  // symbol name must be exactly 1 letter
                  {
                     //Symbol name isn't valid
                     newError(symLine,  "Error:", "In symbols.txt file: Symbol name at start of line is not a single letter");
                     // for now, let things continue, with a strange symbol defined

                  }
                  if(parse[1].Length == 0)      // if there wasn't a substitution string, give a warning
                  {
                     newError(symLine,"Warning:", "In symbols.txt file: substitution string was empty");
                  } //  if(parse[1].Length == 0)
                  symNames[symCount] = parse[0].ToUpper();   // get the symbol, forced to upper case
                  symStrings[symCount] = parse[1];           // and store away the string that the symbol represents
                  symCount ++ ;        // count one more symbol
               } // else // if( line != ""...
            } // foreach(string line in srcLines)
         } // if File.Exists ("symbols.txt")

         // now on to your previously scheduled transform processing

         string newLine = "";
         bool invalidInput = true;
         while(invalidInput)
         {
            robotName = symLookup("H");      // see if symbol $H is defined as robot name
            if(robotName != "")              // if there's a defined robot name...
            {                                // use it rather than asking user to supply it
               Console.WriteLine($"  (using predefined Robot name: {robotName})");
               invalidInput = false;
            } //  if(robotName != "")
            else  // otherwise ask user to supply robot name, or a shortcut
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
               } // if userInput != "")
            } // else

         } // while
         int lineNum = 0;                       // line number within template file, for error messages
         foreach(string rawLine in srcLines)
         {
            lineNum ++;       // count one more line

            // before we do any processing on the line, do symbol substitution
            // for example, if the line contains $H, replace it with the value for H that was in the symbol file
            // (at this point the symbol file has been processed into arrays symNames and symStrings, for symCount symbols)
            
            string line = rawLine;        // we need a modifyable copy of the original line
            line = translateSymbols(line);       // we're in script object
   // if(line != rawLine) {Console.WriteLine($"<template.transformSrc> line before: {rawLine} ");}
   // if(line != rawLine) {Console.WriteLine($"<template.transformSrc> line  after: {line} ");}

            parse = line.Split (' ', 2);
            var cmd = parse[0].Trim();  
            var text = parse[1].Trim();     
            if(cmd == "copy")
            {
               
               newLine = text;
               outLines[outIndex] = newLine;
               outIndex++;
            } // if
            else if(cmd == "replace")
            {
               newError(lineNum,"Warning:", "Replace command is deprecated. Consider a copy command that references $H" );

               if(text == "<myBot>")
               {
                  string outLine = "mybot = \"" + robotName + "/commands\""; 
                  newLine = outLine;
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
   public class scriptFile
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
      public string[] symNames = new string[20];    // single character name for a symbol like $H
      public string[] symStrings = new string[20];  // string to be substituted for name with same index
      public string[] symLines = new string[100];  // holds lines from symbols file, including comments
      public string[] errorLines = new string[100]; // holds warning & error messages, one line per array element
      int errorNum = 0; // line counter in the accumulating error listing
      public int symCount = 0;
      int leg;       // often used as a loop index
      int groupNum;  // leg group alpha name translated to a number, 1 - 26
      int legNum;    // used in leg groups
      public string[] parse = new string[30];   // for prepForMove call
      double x, y, z;                           // to make prepForMove work
      int lineNum;   // keeping compiler happy
      string line = "";

      //string newLine;

      double fp_frontHipX = 3.82739F + 7.13528F * .707107F;  // = 8.872796
      double fp_frontHipY = 5.04750F;
      double fp_sideHipX = 0;
      double fp_sideHipY = 6.9423F;
      public int[] legMask = new int[7];     // binary mask that selects a particular leg
      public int[] legGroup = new int[27];   // definition of legGroups. index is letter of the alphabet,
                                             // value is bit encoded legs that are present in group
          //public var[] x, y, z 
      double localHomeX ;
      double localHomeY ;
      double localHomeZ ;
      public double[] globalHomeX = new double[7];   // home position in global coords for 6 legs
      public double[] globalHomeY = new double[7];
      public double[] globalHomeZ = new double[7];

      /// <summary>
      /// Puts each line of the script file into an element of a string array.
      /// <summary>
      /// <param name="null">Does not return any values.</param>
      /// <returns>null</returns>
      /// <exception cref="DataException">Thrown if the file name provided does not exist.</exception>
      public void getSrcContent()      // we're in scriptFile class
      {
         // some setup stuff for script processing, including symbol definitions from symbols.txt
         setupForScript();

         Console.WriteLine(" ");       // display a blank line to separate dotnet command from compiler output
         // see if input file name was predefind in the $I symbol
         string inFile = symLookup("I");  // see if $I has a definition
         if(inFile != "")     // if there was a non null definition..
         {
            Console.WriteLine($"  (using predefined script file name: {inFile})");
            if( !File.Exists(inFile))  // if the file named in $I doesn't actually exist
            {                          // tell user that, and fall back to asking him for filename
               Console.WriteLine("    ... but it doesn't exist. Ignoring the $I symbol value.");
            }// if(!File.Exists("symbols.txt"))
         } // if(inFile != "")

         bool scriptFileNotFound  = true;
         while(scriptFileNotFound)
         {
            if(inFile != "" & File.Exists(inFile)) // if there's a $I filename that exists
            {
               srcFileName = inFile;
            } // if(inFile != "")
            else
            {
               Console.WriteLine("Please enter the script file name:");
               var userInput = Console.ReadLine();
               srcFileName = userInput?.ToString() ?? ""; // Convert input to string (no null). 
            }
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

      // debugging version of Console.WriteLine(x)
      public void WC(string x)
      {
         Console.WriteLine(x);
      } //  public void WC(string x)


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
            newLine += " ," + Math.Round(legLocX[l],2).ToString();   // append X value
            newLine += "," + Math.Round(legLocY[l],2).ToString();   // append Y value
            newLine += "," + Math.Round(legLocZ[l],2).ToString();   // append Z value
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

         // global coordinates for home position for each leg
         globalHomeX[1] =  18.619;     // reference: hexbot/docs/MQTT scripts/creating-flows.odt ...
         globalHomeY[1] = -14.792;     // ... section 15.1, Global Coordinates of Key Locations, in creating-flows.odt
         globalHomeZ[1] = -10.595;

         globalHomeX[2] =   0.0;
         globalHomeY[2] = -20.709;
         globalHomeZ[2] = -10.595;

         globalHomeX[3] = -18.619;
         globalHomeY[3] = -14.792;
         globalHomeZ[3] = -10.595;

         globalHomeX[4] =  18.619;
         globalHomeY[4] =  14.792;
         globalHomeZ[4] = -10.595;

         globalHomeX[5] =   0.0;
         globalHomeY[5] =  20.709;
         globalHomeZ[5] = -10.595;

         globalHomeX[6] = -18.619;
         globalHomeY[6] =  14.792;
         globalHomeZ[6] = -10.595;

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
            legMask[leg] = Convert.ToInt32( Math.Pow(2, (leg) ));

         }  //  for(legAbsX = 1
         for( int i = 1; i <= 26; i++)   // initialize the 26 leg groups to having no members
         {
            legGroup[i] = 0 ;
         } // for( int i = 1;
         // now fill in the default groups
         // define symbolic names for the binary mask for each leg for clarity
         // note also the array legMask[l] specifies the binary mask for leg "l"
        
         int leg1 = 2;  // legMask[leg] = 2 raised to the (leg)
         int leg2 = 4;
         int leg3 = 8;
         int leg4 = 16;
         int leg5 = 32;
         int leg6 = 64;

         legGroup[1] = leg1 + leg2 + leg3 + leg4 + leg5 + leg6;   // a = 1 = all legs
         legGroup[2] = leg3 + leg6;                               // b = 2 = back legs
         legGroup[6] = leg1 + leg4;                               // f = 6 = front legs
         legGroup[12]= leg4 + leg5 + leg6;                        // l = 12 = left legs
         legGroup[18]= leg1 + leg2 + leg3;                        // r = 18 = right legs
         legGroup[5] = leg2 + leg4 + leg6;                        // e = 5 = even numbered legs
         legGroup[15]= leg1 + leg3 + leg5;                        // o = 15 = odd numbered legs
         legGroup[13]= leg2 + leg5;                               // m = 13 = middle legs

         // read symbol definitions used in template and script files

         symCount = 0;       // initially no symbols have been defined
         if(File.Exists("symbols.txt")) 
         {
            int symLine = 0;        // line number within symbol file for error messages
            symLines = System.IO.File.ReadAllLines("symbols.txt");
            foreach(string l in symLines)
            {
               string line = l ;       // create overwritable copy for error recovery
               symLine ++ ;      // count one more line in the symbol file
               if( line == "" | line.StartsWith("/") )
               {
                  // if line is a comment, or empty, or starts with a space, ignore it
               }  // if( line != ""...
               else  // otherwise it's a symbol definition
               {
                  if(line.Length < 2 | !line.Contains(' ')) // if the line is too short or ill formed...
                  {
                     // report an error
                     newError(symLine,  "Error:", "In symbols.txt: line too short or missing space separator");
                     line = "- -";  // substitute so that parse[1] will exist nad not cause out of bounds errors
                  }// if(line.Length < 2 | !line.Contains(' '))
                  parse = line.Split(' ',2);    // chop it into 2 pieces by the space after the symbol name
                  string sym = parse[0];
                  if(sym.Length != 1 | !Char.IsLetter(sym,0))  // symbol name must be exactly 1 letter
                  {
                     //Symbol name isn't valid
                     //newError( symLine,  "Error:", "In symbols.txt file: Symbol name at start of line is not a single letter");
                     // for now, let things continue, with a strange symbol defined

                  }
                  if(parse[1].Length == 0)      // if there wasn't a substitution string, give a warning
                  {
                     // newError(symLine,"Warning:", "In symbols.txt file: substitution string was empty");
                  }  //  if(parse[1].Length == 0)
                  symNames[symCount] = parse[0].ToUpper();   // get the symbol, forced to upper case
                  symStrings[symCount] = parse[1];           // and store away the string that the symbol represents
                  symCount ++ ;        // count one more symbol
               } // else // if( line != ""...
            } // foreach(string line in srcLines)
         } // if File.Exists ("symbols.txt")
      }  //  public void setupForScript()


      // symLookup(name) - look up a specified symbol's substitution string
      //
      // before this routine is called, the disk file symbols.txt has been processed to produce the following
      //   symCount - the number of symbols that were defined
      //   symNames[] - the uppercase single letters that are the symbol names
      //   sysStrings[] - the substitution string for the symbol name with the same array index

      public string symLookup(string name)   // look up argument name, and return its string (in Scriptfile class)
      {
            if(symCount == 0) // if we had no symbols defined..
            {
               return "";    // return the null string
            } // if(symCount == 0) 
            for( int j=0; j<symCount; j++)   // scan through all symbol names, seeking a match
            {
               if(symNames[j] == name.ToUpper())   // if we found one that matched the name we were given
               { 
                  return symStrings[j];   // return the corresponding string
               }
            } // for( int j=0; j<symCount; j++)
            return "";// there were symbols in table, but none matched
      } // void symLookup(string name)
      
      // translateSymbols( line) - replace any symbols in the form $X in line, with string from symStrings[]
      // called at beginning of per line processing of template and script

      public string translateSymbols(string lin)   // do symbol substution, returning expanded line (in Scriptfile class)
      {  
         int symLoc;
         string symString = "";  // also flags substitution was done if it ends up non-blank
         for (int i=1; i<=4; i++)      // substitute up to 4 symbols on the same line
         {
            if(lin.Contains("$"))     // a symbol is denoted by using a $ prefix to its name
            {
               symLoc = lin.IndexOf("$");  // location of $ in line (1st position is zero)
               string symName = lin.Substring(symLoc+1, 1); // the following char is the symbol name
               symString = symLookup(symName);    // find its corresponding substitution string
               lin = lin.Substring(0, symLoc) + symString + lin.Substring(symLoc+2); //rebuild line doing substitution
            } // if line.contains $
         }// for (int i=1; i<=4; i++)
         return lin; // returned line has up to 4 symbols substituted

      } // string translateSymbols(string line)

       public void newError(int lin, string type, string msg)   // add a new error/warning to the list in errorLines[]
      {
         errorLines[errorNum] = lin.ToString() + "  " + type + " " + msg;
         errorNum ++ ;
         return;
      } // public void newError
      /// Transform source file lines to output lines.

      public string[] getErrors()   // main calls this to get any warnings generated during compilation
      {
         // for now, just test returning a string value
         return errorLines; // array with each error or warning described in one entry
      } // public string[] getErrors()

      public void prepForMove()     // do standard prepareaion for move command with possible leg group
      {  // standard move command format: <move-command> leg, x, y, z
         //  where leg might be number to identify a single leg, or a letter to specify a leg group name
         //        x, y, z are coordinates or offsets, in local or global coordinates
         //
         // This routine processes the leg info:
         //  if it's a letter, figure the name, and index into the legGroup[] array (A - Z >  1 to 26)
         //  if it's a number, fake a group containing just that leg as group 0 for consistent processing
         //
         // Move commands just need to loop through the legs in the group identified by groupNum for processing

         var arg = parse[1].Split(',', 4);
         var leg = arg[0].Trim();
         string legName = leg.ToUpper(); 
         x = Convert.ToDouble(arg[1].Trim());
         y = Convert.ToDouble(arg[2].Trim());
         z = Convert.ToDouble(arg[3].Trim()); 
         // WC( $" x,y,z: {x}, {y}, {z}");

         // first argument can be alpha leg group name, or numeric leg number

         // if a $ is put in from of leg name, it likely is an undefined symbol, so check for this case
         if(string.IsNullOrEmpty(legName))     // was leg field empty (maybe after symbol substitution)
         {
            newError(lineNum,"  Error:",$"An empty leg number or group was given (bad symbol?): {line}");
            // substitute leg 1 so follow on code won't fail
            groupNum = 0;                    // fake a group, using the 0 index that precedes the alpha's in legGroup[]
            legGroup[groupNum] = legMask[1]; //  make it leg 1
         }
         else // if(string.IsNullOrEmpty(legName))  
         {

            if(Char.IsLetter(Convert.ToString(leg),0) )  // is Leg field a letter, i.e. a group name
            {  // yes. figure legGroup number from group name letter
                  groupNum = legName[0] - 64;          // convert ASCII to number. A = x41 > 1
            } //  if(Char.IsLetter(Convert.ToString(leg),0) )
            else // if(Char.IsLetter(Convert.ToString(leg),0) )
            {  // it wasn't a letter. check its a number, then fake group 0 containing just the specified leg
               groupNum = 0;   // fake a group, using the 0 index that preceded the alpha's in legGroup[]
               if(!string.IsNullOrEmpty(legName) & legName.All(char.IsDigit))  // is it all digits?
               {  // yes, safe to parse it as a number
                  legNum = int.Parse(legName);     // translate text to integer
                  if(legNum >=1 & legNum <= 6)     // if it's a legal number
                  {
                     // fake group 0 containing just the specified leg
                     legGroup[groupNum] = legMask[legNum];  // and make is a group containing just 1 leg
                  } // if(legNum >=1 & legNum <= 6)
                  else
                  {
                     newError(lineNum,  "Error",$"Invalid number for leg in: {line}");
                     // pick a leg so move routines don't get run time errors
                     legGroup[groupNum] = legMask[1];    // make it leg 1
                  }
               } // if(!string.IsNull....
               else // if(!string.IsNullOrEmpty(legName) & legName.All(char.IsDigit))
               {
               newError(lineNum,  "Error",$"Leg identifier is not valid in: {line}");
               // pick a leg so move routines don't get run time errors
               legGroup[groupNum] = legMask[1];    //  make it leg 1
               }
            } // else // if(Char.IsLetter(Convert.ToString(leg),0) )
         } // else // if(string.IsNullOrEmpty(legName))    
      } // public void prepForMove()

/*                     
            if(legNum < 0 || legNum > 6)        // range check given leg number
            {
               Console.WriteLine($"ERROR transforming script file. Leg number {legNum} is an invalid number");
               newLine = "// COMPILER ERROR! Parsing leg command in script src file. Illegal lg number: " + legNum;
               outLines[outIndex] = newLine;
               outIndex++;
               break;
            }
*/      

      /// <summary>
      /// Transform source file lines to output lines.
      /// <summary>
      /// <param name="null">Does not accept any arguments.</param>
      /// <returns>null. Does not return anything.</returns>
      public void transformSrc()      // we're in scriptFile class
      {
         errorNum = 0;       // index into list of errors we accumulate in errorLines[]
         string newLine = "";

         // force out a move to home position command, so we have a known starting point
         //  ... which will be used to initialize internal variables
         newLine = "send(\"Flow, 1,MLRH,10,0,0,0, 0,0,0, 0,0,0, 0,0,0, 0,0,0, 0,0,0, 0,0,0\")";
         outLines[outIndex] = newLine;
         outIndex++;
         lineNum = 0;        // line number within script file for error messages
         foreach(string rawLine in srcLines)
         { 
            lineNum++;           // count one more line in script file for error message info
            // before we do any processing on the line, do symbol substitution
            // for example, if the line contains $H, replace it with the value for H that was in the symbol file
            // (at this point the symbol file has been processed into arrays symNames and symStrings, for symCount symbols)
            
            line = rawLine;        // we need a modifyable copy of the original line
            line = translateSymbols(line);       // we're in script object
   // WC($"Line={line}");  // display line for debugging purposes

   // if(line != rawLine) {Console.WriteLine($"<template.transformSrc> line before: {rawLine} ");}
   // if(line != rawLine) {Console.WriteLine($"<template.transformSrc> line  after: {line} ");}

            parse = line.Split(' ', 2);
            var cmd = parse[0].Trim().ToUpper();  
// ========================================================================================================= "symdef"
            if(cmd == "SYMDEF")
            {
               newLine = "// " + line + " // symdef replaced by use of symbols.txt file.";
               outLines[outIndex] = newLine;
               outIndex++;
               newError(lineNum,"Warning:", "symdef command is obsolete, replaced by symbols in the symbol.txt file");
            } // if
// ===================================================================================================== "MoveToHomePosition"
            else if(cmd == "MoveToHomePosition".ToUpper())
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
            else if(cmd == "command".ToUpper())
            {
               newLine = "// " + line + " // command syntax not yet implemented in compiler.";
               outLines[outIndex] = newLine;
               outIndex++;
            } // if
// ====================================================================================================== "MoveRelHomeLocal"
            else if(cmd == "MoveRelHomeLocal".ToUpper())
            {
               prepForMove();  // process leg (group or number) and x,y,z from script command line
               for( legNum=1; legNum<=6; legNum++)  // check to see if each possible leg is in this group
               {
                  if((legGroup[groupNum] & legMask[legNum]) != 0)
                  {
                     // update the toe position for the specified leg as requested
                     legLocX[legNum] = localHomeX + Convert.ToDouble(x);
                     legLocY[legNum] = localHomeY + Convert.ToDouble(y);
                     legLocZ[legNum] = localHomeZ + Convert.ToDouble(z);
                     
                     // now update the equivalent global coordinates
                     transLocalToGlobal(legNum, legLocX[legNum], legLocY[legNum], legLocZ[legNum], ref legGloX[legNum], ref legGloY[legNum], ref legGloZ[legNum]);
                  } // if(legGroup[groupNum] && legMask[i] != 0)
               } // for( legNum=1; legNum<=6; legNum++)         
            } // if cmd == "MoveRelHomeLocal")

// ====================================================================================================== "MoveRelHomeGlobal"
            else if(cmd == "MoveRelHomeGlobal".ToUpper())
            {
               prepForMove();  // process leg (group or number) and x,y,z from script command line
               for( legNum=1; legNum<=6; legNum++)  // check to see if each possible leg is in this group
               {
                  if((legGroup[groupNum] & legMask[legNum]) != 0)
                  {
                     // update the toe position for the specified leg as requested
                     legGloX[legNum] = globalHomeX[legNum] + Convert.ToDouble(x);
                     legGloY[legNum] = globalHomeY[legNum] + Convert.ToDouble(y);
                     legGloZ[legNum] = globalHomeZ[legNum] + Convert.ToDouble(z);
                     
                     // now update the equivalent local coordinates
                  transGlobalToLocal(legNum, legGloX[legNum], legGloY[legNum], legGloZ[legNum], ref legLocX[legNum], ref legLocY[legNum], ref legLocZ[legNum]);
                  } // if(legGroup[groupNum] && legMask[i] != 0)
               } // for( legNum=1; legNum<=6; legNum++)         
            } // if cmd == "MoveRelHomeGlobal")
// ====================================================================================================== "MoveRelLastLocal"
// move leg (or leg group) a local coord offset in the x, y and z arguments from the last (i.e. current) leg position
            else if(cmd == "MoveRelLastLocal".ToUpper())
            {
               prepForMove();  // process leg (group or number) and x,y,z from script command line
               for( legNum=1; legNum<=6; legNum++)  // check to see if each possible leg is in this group
               {
                  if((legGroup[groupNum] & legMask[legNum]) != 0)
                  {
                     // update the local coord version of toe position for the specified leg as requested
                     legLocX[legNum] +=  Convert.ToDouble(x);
                     legLocY[legNum] +=  Convert.ToDouble(y);
                     legLocZ[legNum] +=  Convert.ToDouble(z);
                     
                     // now update the equivalent global coordinates
                     transLocalToGlobal(legNum, legLocX[legNum], legLocY[legNum], legLocZ[legNum], ref legGloX[legNum], ref legGloY[legNum], ref legGloZ[legNum]);
                  } // if(legGroup[groupNum] && legMask[i] != 0)
               } // for( legNum=1; legNum<=6; legNum++)         
            } // if cmd == "MoveRelLastLocal")
// ========================================================================================================== "MoveRelLastGlobal"
// move leg (or leg group) a global coord offset in the x, y and z arguments from the last (i.e. current) leg position
            else if(cmd == "MoveRelLastGlobal".ToUpper())
            {
               prepForMove();  // process leg (group or number) and x,y,z from script command line
               for( legNum=1; legNum<=6; legNum++)  // check to see if each possible leg is in this group
               {
                  if((legGroup[groupNum] & legMask[legNum]) != 0)
                  {
                     // update the global coord version of toe position for the specified leg as requested
                     legGloX[legNum] +=  Convert.ToDouble(x);
                     legGloY[legNum] +=  Convert.ToDouble(y);
                     legGloZ[legNum] +=  Convert.ToDouble(z);
                     
                     // now update the equivalent local coordinates
                     transGlobalToLocal(legNum, legGloX[legNum], legGloY[legNum], legGloZ[legNum], ref legLocX[legNum], ref legLocY[legNum], ref legLocZ[legNum]);
                  } // if(legGroup[groupNum] && legMask[i] != 0)
               } // for( legNum=1; legNum<=6; legNum++)         
            } // if cmd == "MoveRelLastGlobal")

// ========================================================================================================== "Doit"
            else if(cmd == "Doit".ToUpper())
            {
               // get the time interval from the doit command line
               if(parse.Length > 1 )   // was there a parse[1]? (if not, a reference causes runtime error)
               {
                  // yes. was it non trivial?
                  if(string.IsNullOrEmpty(parse[1]))       // if there wasn't a valid argument for Doit command...
                  {
                     newError(lineNum,  "Error:",$"Doit command's mandatory time duration argument was invalid: {line}");
                  } // if(parse[1] == "")
                  else // otherwise proceed to build a move command for the MQTT output file
                  {
                     var arg = parse[1].Split(',', 4);
                     var interval = arg[0].Trim();
                     int interval1 = int.Parse(interval);

                     doDoit(interval1);   // call common routine for doit processing
                  } // else
               }
               else // if(parse.Length > 1 ) 
               {
                  newError(lineNum,  "Error:",$"Doit command did not have mandatory time duration argument: {line}");
               }
            } // if

// ====================================================================================================== "CycleStart"
            else if(cmd == "CycleStart".ToUpper())
            {
               // the CycleStart command is used to define the start of a series of flow commands (a cycle)
               //  that will be repeated a number of times using the ExecuteCycle command
               // Format: CycleStart name
               //   where name is a single digit that identifies the cycle. (multiple cycles allowed)

               // get the numeric name of the cycle from the CycleStart command line
               var arg = parse[1].Split(',', 4);
               var cycleName = arg[0].Trim();
               int cycleName1 = int.Parse(cycleName);

               string sendCmd = "send(\"Flow,0,MCS," ; // First part of send command. MCS = Mark Cycle Start
               string cycleName2 = Convert.ToString(cycleName1); // append the numeric name of the cycle
               sendCmd = sendCmd + cycleName2 + ",0,0,0\")" ;        // tail end of MCS flow command

               outLines[outIndex] = sendCmd ;                  // and store the MCS flow command in the output array
               outIndex++;

            } // if(cmd = )

// ====================================================================================================== "CycleEnd"
            else if(cmd == "CycleEnd".ToUpper())
            {
               // the CycleEnd command is used to define the end of a series of flow commands (a cycle)
               //  that will be repeated a number of times using the ExecuteCycle command
               // Format: CycleEnd name
               //   where name is a single digit that identifies the cycle. (multiple cycles allowed)

               // get the numeric name of the cycle from the CycleEnd command line
               var arg = parse[1].Split(',', 4);
               var cycleName = arg[0].Trim();
               int cycleName1 = int.Parse(cycleName);

               string sendCmd = "send(\"Flow,0,MCE," ;            // First part of send command. MCE = Mark Cycle End
               string cycleName2 = Convert.ToString(cycleName1);  // append the numeric name of the cycle
               sendCmd = sendCmd + cycleName2 + ",0,0,0\")" ;     // and tail end of MCS flow command

               outLines[outIndex] = sendCmd ;                     // and store the MCS flow command in the output array
               outIndex++;

            } // if(cmd = )

// ====================================================================================================== "ExecuteCycle"
            else if(cmd == "ExecuteCycle".ToUpper())
            {
               // the ExecuteCycle command is used to execute a previously defined series of flow commands (a cycle)
               // Format: ExecuteCycle name, reps
               //   where name is a single digit that identifies the cycle. (multiple cycles allowed)
               //     and reps is the number of repetitions

               // get the numeric name of the cycle from the ExecuteCycle command line
               var arg = parse[1].Split(',', 4);
               var cycleName = arg[0].Trim();
               int cycleName1 = int.Parse(cycleName);

               // get the repetition count from the ExecuteCycle command line
               var reps  = arg[1].Trim();
               int reps1 = int.Parse(reps);

               string sendCmd = "send(\"Flow,0,DC," ; // First part of send command. DC = Do Cycle
               sendCmd = sendCmd + Convert.ToString(cycleName1) + "," + Convert.ToString(reps1); // append cycle name and rep count
               sendCmd = sendCmd + ",0,0,0\")" ;        // and tail end of MCS flow command

               outLines[outIndex] = sendCmd ;                  // and store the MCS flow command in the output array
               outIndex++;

            } // if(cmd = )

// ====================================================================================================== "LegGroup"
            else if(cmd == "LegGroup".ToUpper())
            {
               // Define a group of legs, represented by a letter, that all get the same commands when nme used
               // Format: LegGroup a,1,3,5
               //   where: a is that name of the group, case insensitive
               //          remaining numbers are legs that are members o the group
               // Predefined LegGroups:
               //   a - all - 1,2,3,4,5,6
               //   b - back - 3,6
               //   f - front - 1,4
               //   l - left - 4,5,6
               //   r - right - 1,2,3
               //   o - odds - 1,3,5
               //   e - evens - 2,4,6
               //   m - middles - 2,5
               // Datastructure: Int LegGroup[27] where:
               //    LegGroups[1] represents the name A...
               //    the value is bit encoded. If n is in the group, the 2^n column = 1 in value

               var arg = parse[1].Split(',', 7);
               int splitCount = parse[1].Split(',', 7).Count();
               string legName = arg[0].ToUpper();     // should do a bunch of error checking, range check
               int groupNum = legName[0] - 64;            // convert ASCII to number. A = x41 > 1
               // range check groupNum here
               if(groupNum >= 1 & groupNum <= 26)     // proceed if the group name was a letter
               {
                  if(legGroup[groupNum] != 0)                // was there already a definition for this legGroup? 
                  {
                     newError(lineNum,"Warning:",$"LegGroup definition overwrote a previous definition for group {legName}");
                  }  //  if(legGroup[groupNum] != 0) 
                  // should validate group name
                  // should issue warning if default leg group is overwritten
                  legGroup[groupNum] = 0;                    // initialize before we start adding bits represent leg members
                  for( int i = 1; i < splitCount; i++)   // may have up to 6 leg numbers in the command
                  {
                     // should do checks on leg number validity
                     legGroup[groupNum] += legMask[int.Parse(arg[i])]; //  add the bit for this leg to accumulating mask
                  } // for( int i = 1; i<=6; i++)
               } // if(groupNum >= 1 & groupNum >= 26)
               else
               {
                  // group name was not a letter. report and ignore bad LegGroup command
                  newError(lineNum,  "Error:",$"Group name was not a single letter in: {line}");
               }

            } // if(cmd = LegGroup)

/*  command handler template
            else if(cmd == "NewCmd".ToUpper())
            {
               // the NewCmd command is used for...

               // command handling, likely adding entries to outLines array...
                  outLines[outIndex] = newLine + "\")";
                  outIndex++;

            } // if(cmd = )
*/
            else
            {
               // Console.WriteLine($"ERROR - command {cmd} in file {srcFileName} is unknown.");
               newLine = "// COMPILER ERROR! Unknown command in script src file: " + cmd;
               outLines[outIndex] = newLine;
               outIndex++;
               newError(lineNum,  "Error:", "Unknown command in script source file: " + cmd);
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
      public void create(string[] scriptFile, string[] templateFile, string oFileName)
      {
         bool invalidInput = true;
         string outFile = "";
         while(invalidInput)
         {
            if(oFileName != "")       // if there was a filename in the symbol, use it, & overwrite
            {
               Console.WriteLine($"  (using predefined output MQTT file name: {oFileName})");
               outFile = oFileName;
               invalidInput = false;
            } // if(outFile != "")
            else  // otherwise get a filename from user
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
                  else // if(outFile != "")
                  {
                     outFile = userInput;
                     invalidInput = false;
                  } // else
               } // if(userInput != null)
            } // else 
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
         var script = new scriptFile(); // Object reference to script file.
         var template = new templateFile(); // Object reference to template file.
         var mqttx = new fxFile(); // Object reference to MQTTfx file. 
         string[] scriptArray = new string[200]; // Content of transformed script file.
         string[] templateArray = new string[200]; // Content of transformed template file. 
         string[] scriptErrors = new string[100];  // warning and error messages from script processing
         string[] templateErrors = new string[100];  // warning and error messages from template processing
         template.srcFileName = "template.txt";
         // leave C Sharp complier warnings on screen
         //Console.Clear();
         string test = script.symLookup("H");
         scriptArray = script.getXfrmContent(); // Get transformed script file content.
         templateArray = template.getXfrmContent(); // Get transformed template file content.
         // get the string for the $O symbol, which is output filename, if it's defined, null otherwise
         string oFile = script.symLookup("O");
         mqttx.create(scriptArray, templateArray, oFile); // Create MQTTfx file

         scriptErrors = script.getErrors();     // retrieve the list of error messages for the script
         templateErrors = template.getErrors(); // retrieve the list of error messages for the template

         bool didTemplateTitle = false;         // flag to generate template error title only if needed
         foreach(string line in templateErrors) // for the list of warnings and errors retrned by template object..
         {
            if( line != null)      // ignore unused lines
            {
               if(didTemplateTitle == false)    // if we have an error line, but haven't done title...
               {
                  Console.WriteLine("--- Errors in Template file --");  // now's the time
                  didTemplateTitle = true;         // only do title once
               } // if(didTemplateTitle == false)
               Console.WriteLine($"  Line {line}");       // array elements are in form "line type message"
                                                   // line is line number within template file
                                                   // type is either Warning or Error
                                                   // message is the diagnostic that was included by the template transformation
                                                   
            } // if( line != "")

         } // foreach(string line in templateErrors)

         // and same process for errors in the script file

         bool didScriptTitle = false;         // flag to generate template error title only if needed
         foreach(string line in scriptErrors) // for the list of warnings and errors retrned by script object..
         {
            if(line != null)
               {
               if(didScriptTitle == false)    // if we have an error line, but haven't done title...
               {
                  Console.WriteLine("--- Errors in Script file ---"); // now's the time
                  didScriptTitle = true;           // only do title once
               } // if(didScriptTitle == false)
               Console.WriteLine($"  Line {line}");       // array elements are in form "line type message"
                                                   // line is line number within template file
                                                   // type is either Warning or Error
                                                   // message is the diagnostic that was included by the template transformation
            } // if(line != null)
         } // foreach(string line in templateErrors)
         if( (didTemplateTitle == false) & (didScriptTitle == false) )     // if there weren't any errors reported
         {
               Console.WriteLine("--- Compilation was successful with no warnings or errors ---");
         } // if( (didTemplateTitle == false) & (didScriptTitle == false) )
         Console.WriteLine(" ");       // display a blank line to separate compiler messages from next command prompt

      } // Main()
   } // class Program
} // namespace HexbotCompiler