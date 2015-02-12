using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TS.CodeGenerator
{
    public class TSConstEnumeration : IGenerateTS
    {
        public List<string> EnumNames;
        public string Name;
        private Type _type;
        public bool IsExported { get; set; }
        public string ModuleName { get; set; }

        public TSConstEnumeration(Type enumType)
        {
            if (!enumType.IsEnum)
                throw new Exception("Must be an enum");
            _type = enumType;
            Name = enumType.Name;
            ModuleName = enumType.Namespace;
            EnumNames = enumType.GetEnumNames().ToList();
        }

        public void Initialize()
        {

        }

        public string ToTSString()
        {
            var enums = string.Join("," + Settings.EndOfLine + "\t", EnumNames);
            var formatString = "/*{0}*/"
                + Settings.EndOfLine
                + (IsExported ? "export " : string.Empty)
                + "const enum {1} {{" + Settings.EndOfLine
                + "\t{2}"
                + Settings.EndOfLine
                + "}}" + Settings.EndOfLine;
            return string.Format(formatString, _type.FullName, Name, enums);
        }

    }
}
