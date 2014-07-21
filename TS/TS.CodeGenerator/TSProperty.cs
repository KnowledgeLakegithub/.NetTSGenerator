using System;
using System.Reflection;

namespace TS.CodeGenerator
{
    public class TSProperty
    {
        private readonly PropertyInfo _propertyInfo;

        public TSProperty(PropertyInfo propertyInfo, Func<Type,string> mapType )
        {
            _propertyInfo = propertyInfo;

            PropertyName = propertyInfo.Name;
            
            PropertyType = propertyInfo.PropertyType.IsGenericParameter
                            ? propertyInfo.PropertyType.Name
                            : mapType(propertyInfo.PropertyType);
        }

        public string PropertyName { get; private set; }
        public string PropertyType { get; private set; }

        public override string ToString()
        {
            if (isNullable())
            {
                return string.Format("{0}?: {1}; //{2}", PropertyName, PropertyType, _propertyInfo.PropertyType);
            }
            
            return string.Format("{0}: {1}; //{2}", PropertyName, PropertyType, _propertyInfo.PropertyType);
        }

        private bool isNullable()
        {
            return _propertyInfo.PropertyType.IsGenericType &&
                   _propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof (Nullable<>);
        }
    }

}
