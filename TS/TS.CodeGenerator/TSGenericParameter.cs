using System;

namespace TS.CodeGenerator
{
    public class TSGenericParameter
    {
        private readonly Type _type;
        private readonly Func<Type, string> _mapType;

        public TSGenericParameter(Type type, Func<Type, string> mapType)
        {
            _type = type;
            _mapType = mapType;
            if (type.IsGenericParameter)
            {
                TypeName = type.Name;
            }
            else
            {
                TypeName = mapType(type);
            }
            
            IsGenericParameter = type.IsGenericParameter;
        }
        public string TypeName { get; set; }
        public bool IsGenericParameter { get; set; }
    }
}