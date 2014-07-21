using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TS.CodeGenerator
{
    public class AssemblyReader : IAssemblyReader
    {
        private TSGenerator _generator;
        private string _resolveDirectory;

        public AssemblyReader(string dllPath)
        {
            _resolveDirectory = Path.GetDirectoryName(dllPath);
            var files = Directory.EnumerateFiles(_resolveDirectory, "*.dll");
            var fi = new FileInfo(dllPath);
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            //AppDomain.CurrentDomain.SetupInformation.PrivateBinPath = dir;
            AppDomain.CurrentDomain.AppendPrivatePath(_resolveDirectory);
            Assembly asm = Assembly.LoadFile(dllPath);//null;
            //foreach (var file in files)
            //{

            //    var tasm =  Assembly.LoadFile(file);
            //    if (fi.FullName == new FileInfo(file).FullName)
            //    {
            //        asm = tasm;
            //    }
            //}


            //var asm = Assembly.LoadFile(dllPath);
            //AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            _generator = new TSGenerator(asm);


            foreach (var type in asm.GetTypes().Where(t => !t.IsEnum && t.IsPublic))
            {
                _generator.AddInterface(type);
            }

        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {

            var di = new DirectoryInfo(_resolveDirectory);
            var files = di.GetFiles();
            var filename = args.Name.Split(new[] { ',' })[0];
            var module = files.FirstOrDefault(i => i.Name == filename + ".dll");
            if (module != null)
            {
                return Assembly.LoadFrom(module.FullName);
            }

            return null;
        }


        public Stream GenerateTypingsStream()
        {
            MemoryStream ms = new MemoryStream();
            var sw = new StreamWriter(ms);
            sw.Write(_generator.ToTSString());
            sw.Flush();
            return ms;
        }

        public string GenerateTypingsString()
        {
            return _generator.ToTSString();
        }
    }
}