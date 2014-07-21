using System;
using System.Reflection;

namespace TS.CodeGenerator
{
    public class TSParameter
    {
        private readonly ParameterInfo _pi;

        public TSParameter(ParameterInfo pi, Func<Type, string> mapType)
        {
            _pi = pi;
            ParameterName = pi.Name;
            ParameterType = mapType(pi.ParameterType);
        }
        public string ParameterName { get; private set; }
        public string ParameterType { get; private set; }

        private const string formatParameter = @"{0}:{1}/*{2}*/";
        public override string ToString()
        {
            var res = string.Format(formatParameter, ParameterName, ParameterType, _pi.ParameterType.Name);
            return res;
        }
    }
}