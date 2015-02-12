using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS.CodeGenerator
{
    public static class Settings
    {
        static Settings()
        {
            MakeMethodsOptional = true;
            FollowExternalAssemblies = false;
            EndOfLine = "\r\n";
            InterfaceFormat = "I{0}";
            MethodReturnTypeFormatString = "JQueryPromise<{0}>";
            StartingTypeMap = new Dictionary<Type, string>
            {
                {typeof (string), Types.String},
                {typeof (DateTime), Types.String},
                {typeof (DateTime?), Types.String},
                {typeof (Guid), Types.String},
                {typeof (Guid?), Types.String},
                {typeof (bool), Types.Boolean},
                {typeof (bool?), Types.Boolean},
                {typeof (Double),Types.Number},
                {typeof (Int16),Types.Number},
                {typeof (Int32),Types.Number},
                {typeof (Int64),Types.Number},
                {typeof (UInt16),Types.Number},
                {typeof (UInt32),Types.Number},
                {typeof (UInt64),Types.Number},
                {typeof (Decimal),Types.Number},
                {typeof (Byte),Types.Number},
                {typeof (SByte),Types.Number},
                {typeof (Single),Types.Number},
                {typeof (Double?),Types.Number},
                {typeof (Int16?),Types.Number},
                {typeof (Int32?),Types.Number},
                {typeof (Int64?),Types.Number},
                {typeof (UInt16?),Types.Number},
                {typeof (UInt32?),Types.Number},
                {typeof (UInt64?),Types.Number},
                {typeof (Decimal?),Types.Number},
                {typeof (Byte?),Types.Number},
                {typeof (SByte?),Types.Number},
                {typeof (Single?),Types.Number},
                {typeof (void), Types.Void}
            };
        }

        public static string MethodReturnTypeFormatString { get; set; }
        public static bool MakeMethodsOptional { get; set; }
        public static bool FollowExternalAssemblies { get; set; }
        public static Dictionary<Type, string> StartingTypeMap { get; set; }
        public static string EndOfLine { get; set; }
        public static bool ConstEnumsEnabled { get; set; }
        public static string InterfaceFormat { get; set; }
    }

}
