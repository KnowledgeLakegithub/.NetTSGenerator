using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TS.CodeGenerator
{
    public class TSMethod
    {
        private readonly MethodInfo _mi;
        private readonly Func<Type, string> _mapType;

        public TSMethod(MethodInfo mi, Func<Type, string> mapType)
        {
            _mi = mi;
            _mapType = mapType;
            Parameters = _mi.GetParameters().Select(p => new TSParameter(p, _mapType)).ToList();
            MethodName = _mi.Name;
            MethodReturnType = mapType(_mi.ReturnType);
        }

        private const string formatMethodOptional = @"{0}?({1}):JQueryPromise<{2}>;";
        private const string formatMethodRequired = @"{0}({1}):JQueryPromise<{2}>;";

        public string MethodName { get; private set; }
        public string MethodReturnType { get; private set; }
        public List<TSParameter> Parameters { get; private set; }

        public override string ToString()
        {
            var paramStr = string.Join(", ", Parameters.Select(p => p.ToString()));
            var res = string.Format(Settings.MakeMethodsOptional ? formatMethodOptional : formatMethodRequired, MethodName, paramStr, MethodReturnType);
            return res;
        }
    }
}