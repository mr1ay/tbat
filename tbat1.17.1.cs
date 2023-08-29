 using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("tbat help =show all usage ");
            Console.WriteLine("tbat helpfile =show all usage ");

            return;
        }
 
        string input=args[0] ;

        if (args.Length > 0 && args[0] == "help")
        {
            Console.WriteLine(Help());
        }

        if (args.Length > 0 && args[0] == "helpfile")
        {
                   
                string textToWrite =  Help();

                    using (StreamWriter writer = new StreamWriter("output.txt"))
                    {
                        writer.WriteLine(textToWrite);
                    }

                    Console.WriteLine("Text has been written to output.txt.");
            
        }


        if (!File.Exists(input))
        {
            Console.WriteLine("Syntax:");
            Console.WriteLine("tbat a.txt");
            Console.WriteLine("tbat a.bat");
            return;
        }
        string newFileName2 = Path.GetFileNameWithoutExtension(input) + "_t.bat";
        string newFilePath2 = Path.Combine(Path.GetDirectoryName(input), newFileName2);

        string output=newFilePath2;
            

        List<string> codeList = ReadAndProcessCodeFile(input);

        //string outputFilePath = "output.txt";

        SpaceAdder3.Make(codeList, output);
 
        // Oluşturulan codeList listesini görüntülemek için kullanılır
        DisplayCodeList(codeList);
    }

    static List<string> ReadAndProcessCodeFile(string filePath)
    {
        List<string> resultList = new List<string>();
        List<string> sline = new List<string>();
        try
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                bool inSwitch = false;
                string switchVariable = "";

                bool firstCase = true;
                bool inDefault=false;
                bool last=false;
                bool comment=false;
                //bool i_saw_it=false;
                //bool atSwitch = false;
                //bool breakEnd = false; // Yeni değişken ekledik
                while ((line = sr.ReadLine()) != null)
                { 
                    
                    if (line.Contains("switch("))
                    {
                        inSwitch = true;
                        inDefault=false;
                        switchVariable = line.Replace("switch(", "").Replace(")", "").Trim();
                        line = "";
                        continue;
                    }

                    else if (inSwitch)
                    {
                        if (line.Contains("case")) 
                        {
                                string caseValue = line.Replace("case", "").Replace(":", "").Trim();

                             if (firstCase)
                             {
                                resultList.Add("/,/");
                                resultList.Add("/,/if %" + switchVariable + "% == " + caseValue + "  (");
                                firstCase=false;
                                continue;
                             }
                             else  
                             {
                                resultList.Add("/,/");
                                resultList.Add("/,/) else if %" + switchVariable + "% == " + caseValue + " (");
                                continue;

                             }
                        }
                        else if (line.Contains("default"))
                        {
                            inDefault=true;
                            last=true;
                            resultList.Add("/,/");
                            resultList.Add("/,/) else (");
                                continue;
                        }

                        else if (line.Contains("break"))
                        {
                            if (inDefault)
                            {
                            resultList.Add("/,/)");
                            resultList.Add("/,/");

                            inDefault=false;
                                    inSwitch=false;

                                continue;
                            }
                            else  
                            {
                                if(last)
                                {
                                    resultList.Add("goto :eof");
                                    inSwitch=false;
                                    last=false;
                                    continue;

                                }
                                else
                                {
                                continue;
                                }
                            }
                        }

                        else 
                        {
                           if(inSwitch==false){
                            resultList.Add("/,/    " + line.Trim());
                            //aradaki komutlarınki
                            continue;  
                           }
                           else
                           {
                            resultList.Add("/,/    " + line.Trim());
                            continue;  
                            
                           }
                        }

                    }
                    else if(line.Contains("break")&&inDefault==false)
                    {
                            resultList.Add("goto :eof"); // "goto :eof" ekle
                                continue;
                    }


                    else  if (line.StartsWith("//")){resultList.Add(line.Replace("//", "::"));continue;}
                    else if (line.StartsWith("/*")){comment=true; line=line.Replace("/*","");continue;}
                    else if (line.StartsWith("*/")){comment=false; line=line.Replace("*/","");continue;} 
                    else if (comment==true){   resultList.Add("::"+line); continue;}

                    else if (line.StartsWith("eff")){resultList.Add(line.Replace("eff", "@echo off"));continue;}
                    else if (line.StartsWith("ef>")){resultList.Add(line.Replace("ef>", "@echo off"));continue;}
                    else if (line.StartsWith("ef<")){resultList.Add(line.Replace("ef<", "@echo on"));continue;}
                    else if (line.StartsWith("ef")){resultList.Add(line.Replace("ef", "@echo on"));continue;}
                    else if (line.Contains("sss")){resultList.Add(line.Replace("sss", "SetLocal"));continue;}
                    else if (line.Contains("sse")){resultList.Add(line.Replace("sse", "EndLocal"));continue;}
                    else if (line.Contains("sed")){resultList.Add(line.Replace("sed", "SetLocal EnableDelayedExpansion"));continue;}
                    else if (line.Contains("sdd")){resultList.Add(line.Replace("sdd", "SetLocal DisableDelayedExpansion"));continue;}
                    else if (line.Contains("see")){resultList.Add(line.Replace("see", "SetLocal EnableExtensions"));continue;}
                    else if (line.Contains("sde")){resultList.Add(line.Replace("sde", "SetLocal DisableExtensions"));continue;}
                    else if (line.Contains("c<"))
                    {
                        if (line.Contains("c<m")){ resultList.Add(line.Replace("c<m", "mode"));}
                        else if (line.Contains("c<xy")){ resultList.Add(line.Replace("c<xy", "cmdwiz setwindowpos"));}
                        else if (line.Contains("c<t")){ resultList.Add(line.Replace("c<t", "title"));}
                        else if (line.Contains("c<c")){ resultList.Add(line.Replace("c<c", "color"));}
                        else{ resultList.Add(line.Replace("c<", "cls"));}
                        continue;
                    }

//////////////////////////////////
//////////////////////////////////
                    else if (line.Contains("package"))
                    {
                        string argfile = ""; // <> işaretleri arasındaki değeri saklamak için değişken
                        int startIndex = line.IndexOf("<");
                        int endIndex = line.IndexOf(">");
                        if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
                        {
                            argfile = line.Substring(startIndex + 1, endIndex - startIndex - 1);
                            resultList.Add("if not exist "+argfile+" md " +argfile); 
                        }
                        continue;
                    }   

                    else if (line.Contains("ife<")){resultList.Add(line.Replace("ife<", "if exist "));continue;}
                    else if (line.Contains("!ife<")){resultList.Add(line.Replace("!ife<", "if not exist "));continue;}
                    else if (line.Contains("ifd<")){resultList.Add(line.Replace("ifd<", "if defined "));continue;}
                    else if (line.Contains("!ifd<")){resultList.Add(line.Replace("!ifd<", "if not defined "));continue;}

                    else if (line.Contains("setp<"))
                    {
                        string argf = ""; // <> işaretleri arasındaki ilk değeri saklamak için değişken
                        string argh = ""; // <> işaretleri arasındaki ikinci değeri saklamak için değişken 
                        int startIndex = line.IndexOf("<");
                        int endIndex = line.IndexOf(">");
                        string argValues = line.Substring(startIndex + 1, endIndex - startIndex - 1);
                        string[] argsArray = argValues.Split(',');

                        if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
                        {
                            if (argsArray.Length == 2)
                            {
                                argf = argsArray[0];
                                argh = argsArray[1];
                                
                                resultList.Add("set /p " + argf + "=<" + argh );
                            }
                            else if(argsArray.Length == 1)
                            {
                                argf = argsArray[0]; 
                                
                                resultList.Add("set /p " + argf + "=" );
                            }
                            else
                            {
                                Console.WriteLine("setp komutunda hata var");
                            } 
                        }
                        continue;
                    }
  
                    if (line.Contains("seta<"))
                    {
                        int startIndex = line.IndexOf("<");
                        int endIndex = line.IndexOf(">"); 

                        if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
                        {
                            string argValues = line.Substring(startIndex + 1, endIndex - startIndex - 1);
                            string[] argsArray = argValues.Split(',');
                            string argf1 = argsArray[0]; 

                            if (argsArray.Length == 2)
                            {
                                string argh1 = argsArray[1];

                                if (argh1.Contains(argf1))
                                {
                                    argh1 = argh1.Replace(argf1, "%" + argf1 + "%"); // Replace sonucunu atamayı unutmayın

                                    resultList.Add("set /a " + argf1 + "=" + argh1);
                                }
                                else
                                {
                                    resultList.Add("set /a " + argf1 + "=" + argh1);
                                }
                            } 
                            else
                            {
                                Console.WriteLine("seta komutunda hata var");
                            } 
                        }
                        continue; 
                    }

                    else if (line.Contains("mdd<"))
                    {
                        string argfile = ""; // <> işaretleri arasındaki değeri saklamak için değişken
                        int startIndex = line.IndexOf("<");
                        int endIndex = line.IndexOf(">");
                        if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
                        {
                            argfile = line.Substring(startIndex + 1, endIndex - startIndex - 1);
                            resultList.Add("md "+argfile ); 
                            resultList.Add("cd /d "+argfile ); 
                        }
                        continue;
                    }  
 
                    else if (line.StartsWith("if<"))
                    {
                        iffunction(line, resultList);
                        continue;
                    }


                    else if (line.Contains(">import<"))
                    {
                        line=line.Replace(">import","");
                        int startIndex = line.IndexOf("<");
                        int endIndex = line.IndexOf(">"); 

                        if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
                        {
                            string argValues = line.Substring(startIndex + 1, endIndex - startIndex - 1);
                            string[] argsArray = argValues.Split(',');
                            
                            foreach (string argu in argsArray)
                            {
                                if (!string.IsNullOrWhiteSpace(argu))
                                {
                                    resultList.Add("if not exist " + argu + " echo "+argu+" is not found");
                                }
                                else
                                {
                                    Console.WriteLine("Geçersiz argüman.");
                                }
                            }
                        }
                        continue;
                    }
  
                    else if (line.Contains("<import<"))
                    {
                        line = line.Replace("<import", ""); // "<import<" kısmını çıkar

                        int startIndex = line.IndexOf("<");
                        int endIndex = line.IndexOf(">"); 
                        if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
                        {
                            string argValues = line.Substring(startIndex + 1, endIndex - startIndex - 1);
                            string[] argsArray = argValues.Split(',');
                            string lastArg = null;
                           // string updatedContent = ""; // Güncellenmiş içeriği saklamak için bir değişken

                                        sline.Add("/,/");
                                        sline.Add("/,/");
                                        sline.Add("/,/");
                                        sline.Add("->/,/goto :endf");
                                        sline.Add("->/,/::..................................");
                            foreach (string argu in argsArray)
                            {
                                if (!string.IsNullOrWhiteSpace(argu))
                                {
                                    string filePathh = Path.Combine("core", argu+".txt"); // Dosya yolu oluştur
                                    
                                    if (!File.Exists(filePathh))
                                    { 
                                        Console.WriteLine(argu+"is not found");
                                        Console.WriteLine("please add file to core folder");
                                    }
                                    else
                                    {
                                        string fileContent = File.ReadAllText(filePathh); 
                                        string[] lua = fileContent.Split('\n'); // Satırlara ayır
                                        for (int i = 0; i < lua.Length; i++)
                                        {
                                        sline.Add(  "->/,/" +lua[i]);
                                        }
                                        lastArg = argu; // Son terimi güncelle 
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Geçersiz argüman.");
                                }
                            }

                            if (lastArg != null)
                            {
                            sline.Add("->/,/::..................................");
                            sline.Add("->/,/:endf " ); // Son terimi kullanarak :endf eklenir

                            lastArg = null;
                            }
                        }
                        continue;
                    }

                    else if (line.Trim() == "") { continue; }
                    
                    else  {  resultList.Add("" + line.Trim());} 
                        
                }
                resultList.AddRange(sline);

//****************************

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Hata: " + ex.Message);
        }

        return resultList;
    }

    static void DisplayCodeList(List<string> codeList)
    {
        foreach (string codeLine in codeList)
        {
            Console.WriteLine(codeLine);
        }
    }
    static string Help()
    {

string helpp="example"+
"\n#switch case   ---input---  "+
"\n"+
"\nswitch(a)"+
"\ncase 3:"+
"\necho answer 3"+
"\nbreak"+
"\ncase 5:"+
"\necho answer 5"+
"\nbreak "+
"\ndefault:"+
"\necho nothing"+
"\nbreak"+
"\n"+
"\n#switch case   ---output---"+
"\n"+
"\n        if %a% == 3  ("+
"\n            echo answer 3"+
"\n        "+
"\n        ) else if %a% == 5 ("+
"\n            echo answer 5"+
"\n        "+
"\n        ) else ("+
"\n            echo nothing"+
"\n        )"+
"\n"+
"\n"+
"\nexample "+
"\n#startup        "+
"\n---input---   ---output--- "+
"\n"+
"\neff           @echo off"+
"\nef>           @echo off"+
"\nef<           @echo on"+
"\nef            @echo on"+
"\n"+
"\nsss           SetLocal "+
"\nsse           EndLocal "+
"\nsed           SetLocal EnableDelayedExpansion "+
"\nsdd           SetLocal DisableDelayedExpansion l"+
"\nsee           SetLocal EnableExtensions "+
"\nsde           SetLocal DisableExtensions "+
"\n "+
"\nc<            cls"+
"\nc<m           mode"+
"\nc<xy          cmdwiz setwindowpos"+
"\nc<t           title "+
"\nc<c           color"+
"\n"+
"\nexample"+
"\n#comment"+
"\n---input---   ---output--- "+
"\n//            ::"+
"\n/*            ::"+
"\n*/            ::"+
"\n"+
"\n"+
"\nexample"+
"\n#others"+
"\n---input---           ---output--- "+
"\npackage<alfafolder>   if not exist alfafolder   md alfafolder"+
"\n>import<batboxmouse>  if not exist batboxmouse  echo batboxmouse is not found"+
"\n<import<batboxmouse>  Adds the contents of the file in the core folder as a header to the bat file (warning if it doesn't exist)"+
"\n"+
"\nsetp<a,alfa.txt>      set /p a=<alfa.txt"+
"\nsetp<a>               set /p a="+
"\n"+
"\nseta<a,a+1>           set /a a=%a%+1"+
"\nseta<4+3>             set /a 4+3"+
"\n  "+
"\nmdd<alfa>             md alfa&&cd /d alfa"+
"\n"+
"\nife<alfa  command     if exist alfa"+
"\n!ife<alfa command     if not exist alfa"+
"\nifd<alfa  command     if defined alfa"+
"\n!ifd<alfa command     if not defined alfa"+
"\n"+
"\nif<a 0 4  command     if %a% equ 4  command"+
"\nif<a 1 4  command     if %a% leq 4  command"+
"\nif<a 2 4  command     if %a% lss 4  command"+
"\nif<a 3 4  command     if %a% geq 4  command"+
"\nif<a 4 4  command     if %a% gtr 4  command"+
"\nif<a 5 4  command     if %a% neq 4  command"+
"\n"+
"\n"+
"\nif<xy 00 4 5         command   if %x% equ 4  if %y% equ 5   command"+
"\nif<xxy 130 4 5 6     command   if %x% leq 4  if %x% gtr 5  if %y% equ 6  command"+
"\nif<xyy 013 4 5 6     command   if %x% leq 4  if %y% leq 5  if %y% gtr 6  command"+
"\nif<xxyy 1313 4 5 6 7 command   if %x% leq 4  if %x% gtr 5  if %y% leq 6  if %y% gtr 7  command"+
"\n"+
"";
return helpp;

    }
    static void iffunction(string line, List<string> resultList)
        {
        string[] parts = line.Substring(3).Split(' ');

        if (parts.Length >= 3)
        {
            string a1 = parts[0];
            string a2 = parts[1];
            string[] alfaa = parts.Skip(2).Take(a1.Length).ToArray();
            string[] beta = parts.Skip(2 + a1.Length).ToArray();

            string output = BuildIfStatement(a1, a2, alfaa) + "" + string.Join(" ", beta);
            resultList.Add(output);
        }
    }

    static string BuildIfStatement(string a1, string a2, string[] alfaa)
    {
        string[] comparisonOperators = { "equ", "leq", "lss", "geq", "gtr", "neq" };
        string output = "";

        for (int i = 0; i < a1.Length; i++)
        {
            string comparisonOperator = comparisonOperators[a2[i] - '0'];
            output += string.Format("if %{0}% {1} {2}   ", a1[i], comparisonOperator, alfaa[i]);
        }
        return output;
    }
}

class SpaceAdder3
{
        public static void Make(List<string> codeList, string outputFilePath) 
    {

        using (StreamWriter outputFile = new StreamWriter(outputFilePath, false))
        {
            outputFile.WriteLine("        ::Rewrited by Tbat v:1.7.1.x");
            outputFile.WriteLine("        ::Edited by SpaceAdder v:3.5.x");
            outputFile.WriteLine("        ::Thanks mr1ay ");
            outputFile.WriteLine("        ");

            bool i_see=false;
            
            foreach (string line in codeList)
            {
                string t = line.Trim();

                if (!string.IsNullOrWhiteSpace(t))
                {
                    if (t.StartsWith("::"))
                    {
                        outputFile.WriteLine(Metod(t, i_see));
                    }
                    else if (t.StartsWith(":"))
                    {
                        outputFile.WriteLine("        ");
                        outputFile.WriteLine(Metod(t, false));
                        i_see = true;
                    }
                    else if (t.StartsWith("->/,/"))
                    {
                        t = t.Replace("->/,/", "");
                        outputFile.WriteLine(Metod(t, false));
                    }
                    else if (t.StartsWith("/,/"))
                    {
                        t = t.Replace("/,/", "");
                        outputFile.WriteLine(Metod(t, i_see));
                    }
                    else if (t.StartsWith("\n/,/"))
                    {
                        t = t.Replace("\n/,/", "");
                        outputFile.WriteLine(Metod(t, i_see));
                    }
                    else
                    {
                        outputFile.WriteLine(Metod(t, i_see));
                    }
                }

}
}
}
public static string  Metod(string line,bool i_see){
 if (i_see)
    {
        line = "        " +"        " + line;
    }
    else
    {
        line = "        " + line;
    }

    return line;
}
}
