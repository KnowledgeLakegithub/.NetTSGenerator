using System;
using System.Reflection;

namespace TS.CodeGenerator
{
    public class TSProperty:IGenerateTS
    {
        private readonly PropertyInfo _propertyInfo;
        private readonly Func<Type, string> _mapType;

        public TSProperty(PropertyInfo propertyInfo, Func<Type,string> mapType )
        {
            _propertyInfo = propertyInfo;
            _mapType = mapType;

            PropertyName = propertyInfo.Name;
            
            
        }

        public string PropertyName { get; private set; }
        public string PropertyType { get; private set; }

        public void Initialize()
        {
            PropertyType = _propertyInfo.PropertyType.IsGenericParameter
                            ? _propertyInfo.PropertyType.Name
                            : _mapType(_propertyInfo.PropertyType);
        }

        public string ToTSString()
        {
            return string.Format(isNullable() 
                    ? "{0}?: {1}; /*{2}*/" 
                    : "{0}: {1}; /*{2}*/", 
                    PropertyName, 
                    PropertyType, 
                    _propertyInfo.PropertyType);
        }

        private bool isNullable()
        {
            var pt = _propertyInfo.PropertyType;
            var pti = pt.GetTypeInfo();
            return pti.IsGenericType &&
                   pt.GetGenericTypeDefinition() == typeof (Nullable<>);
        }
    }

}
