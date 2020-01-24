using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TS.CodeGenerator
{
    public class NamespaceAssemblyReader : IAssemblyReader
    {
        public Stream GenerateTypingsStream()
        {
            MemoryStream ms = new MemoryStream();
            var sw = new StreamWriter(ms);
            sw.Write(GenerateTypingsString());
            sw.Flush();
            return ms;
        }

        public string GenerateTypingsString()
        {
            foreach (var tsModule in _namespaces)
            {
                tsModule.Value.Initialize();
            }

            return string.Join(",", _namespaces.Values.Select(n => n.ToTSString()));
        }

        private TSGenerator _generator;
       // private string _resolveDirectory;
        private Dictionary<string, TSModule> _namespaces;

        public NamespaceAssemblyReader(Assembly assembly)
        {
            _generator = new TSGenerator(assembly) { Modules = true };

            _namespaces = new Dictionary<string, TSModule>();
            foreach (var type in assembly.ExportedTypes)
            {
                var ns = type.Namespace.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                var root = ns.First();

                if (!_namespaces.ContainsKey(root))
                {
                    var module = new TSModule(root, _generator.GenerateLookupTypeName) {IsDeclared = true};
                    _namespaces.Add(root, module);
                }
                ns.RemoveAt(0);
                _namespaces[root].AddSubNamespaceType(ns, type);
            }

        }

        //private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        //{

        //    var di = new DirectoryInfo(_resolveDirectory);
        //    var files = di.GetFiles();
        //    var filename = args.Name.Split(new[] { ',' })[0];
        //    var module = files.FirstOrDefault(i => i.Name == filename + ".dll");
        //    if (module != null)
        //    {
        //        return Assembly.LoadFrom(module.FullName);
        //    }

        //    return null;
        //}
    }
}