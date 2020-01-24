using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace TS.CodeGenerator.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = Path.GetFullPath(args[0]);
            var output = Path.GetFullPath(args[1]);

            System.Console.WriteLine($"Input Path {input}");
            System.Console.WriteLine($"Output Path {output}");

            if (!File.Exists(input))
            {
                System.Console.Error.WriteLine($"Could Not Find input {input}");
                return;
            }
           
            Settings.MethodReturnTypeFormatString = "{0}";
            Assembly asm = AssemblyLoadContext.Default.LoadFromAssemblyPath(input);
            var reader = new AssemblyReader(asm);

            if (File.Exists(output))
            {
                File.Delete(output);
            }
            using (var of = File.OpenWrite(output))
            using (var sw = new StreamWriter(of))
            {
                var types = reader.GenerateTypingsString();
               // sw.WriteLine(@"/// <reference path=""../jquery/jquery.d.ts"" />");
                sw.WriteLine(types);
                
            }
            System.Console.WriteLine("...");
            System.Console.WriteLine("Completed");
        }
    }
}