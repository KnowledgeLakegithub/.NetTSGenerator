using System;

namespace TS.CodeGenerator
{
    public class TSGenericParameter:IGenerateTS
    {
        private readonly Type _type;
        private readonly Func<Type, string> _mapType;

        public TSGenericParameter(Type type, Func<Type, string> mapType)
        {
            _type = type;
            _mapType = mapType;
            IsGenericParameter = type.IsGenericParameter;
        }
        public string TypeName { get; set; }
        public bool IsGenericParameter { get; set; }
        public void Initialize()
        {
            if (_type.IsGenericParameter)
            {
                TypeName = _type.Name;
            }
            else
            {
                TypeName = _mapType(_type);
            }
        }

        public string ToTSString()
        {
            return TypeName;
        }
    }
}