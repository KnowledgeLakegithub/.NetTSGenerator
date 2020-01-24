using System;
using System.Reflection;

namespace TS.CodeGenerator
{
    public class TSProperty : IGenerateTS
    {
        private readonly Func<Type, string> _mapType;
        private Type _type;

        public TSProperty(PropertyInfo propertyInfo, Func<Type, string> mapType)
        {
            _mapType = mapType;

            _type = propertyInfo.PropertyType;
            PropertyName = propertyInfo.Name;


        }
        public TSProperty(FieldInfo finfo, Func<Type, string> mapType)
        {
            _mapType = mapType;
            _type = finfo.FieldType;
            PropertyName = finfo.Name;


        }

        public string PropertyName { get; private set; }
        public string PropertyType { get; private set; }

        public void Initialize()
        {
            PropertyType = _type.IsGenericParameter
                ? _type.Name
                : _mapType(_type);
        }

        public string ToTSString()
        {
            return string.Format(isNullable()
                    ? "{0}?: {1}; /*{2}*/"
                    : "{0}: {1}; /*{2}*/",
                    PropertyName,
                    PropertyType,
                    _type);
        }

        private bool isNullable()
        {
            var pt = _type;
            var pti = pt.GetTypeInfo();
            return pti.IsGenericType &&
                   pt.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }

}
