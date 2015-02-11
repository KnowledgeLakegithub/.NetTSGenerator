using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TS.CodeGenerator
{
    public class TSGenerator : ITSGenerator
    {
        private readonly Assembly _currentAssembly;
        private Dictionary<Type, string> _typeMap;
        private Dictionary<Type, TSInterface> _interfaceMap;
        public TSGenerator(Assembly currentAssembly)
        {
            _typeMap = new Dictionary<Type, string>(Settings.StartingTypeMap);
            _interfaceMap = new Dictionary<Type, TSInterface>();
            _currentAssembly = currentAssembly;

        }
        public string GenerateLookupTypeName(Type type)
        {
            //todo generate enums?
            if (type.IsEnum)
                return Types.Any;

            //quickly map simple type maps
            if (_typeMap.ContainsKey(type))
            {
                return _typeMap[type];
            }

            //allow only generic enumerables
            //generic array first
            if (IsGenericEnumerable(type))
            {
                try
                {
                    var typeOfElements = type.IsArray
                        ? type.GetElementType()
                        : type.GetGenericArguments().Single();
                    var name = GenerateLookupTypeName(typeOfElements) + "[]";
                    if (!_typeMap.ContainsKey(type))
                        _typeMap.Add(type, name);
                    return name;
                }
                catch
                {
                    return Types.Any;
                }
            }
            //standard aray
            if (type.IsArray)
            {
                var typeOfElements = type.GetElementType();
                var name = GenerateLookupTypeName(typeOfElements) + "[]";
                if (!_typeMap.ContainsKey(type))
                    _typeMap.Add(type, name);
                return name;
            }
            if (type.IsGenericParameter)
            {
                return type.Name;
            }
            if (!Settings.FollowExternalAssemblies)
            {
                //anything from other assemblies are any
                if (type.Assembly != _currentAssembly)
                    return Types.Any;

            }
            if (_interfaceMap.ContainsKey(type))
            {
                return _interfaceMap[type].InterFaceName;
            }

            AddInterface(type);

            return _interfaceMap[type].InterFaceName;

        }

        class InterfaceComparer : IEqualityComparer<TSInterface>
        {
            public bool Equals(TSInterface x, TSInterface y)
            {
                return x.InterFaceName.Equals(y.InterFaceName);
            }

            public int GetHashCode(TSInterface obj)
            {
                return obj.InterFaceName.GetHashCode();
            }
        }

        public void Initialize()
        {
           
        }

        public string ToTSString()
        {
            var interfaces = _interfaceMap.Values
                .Where(i => !i.IsGenericMetaClass)//bob<string>, vs bob<T>
                .Distinct(new InterfaceComparer());


            var strs = interfaces.Select(v => v.ToTSString());
            return string.Join(Settings.EndOfLine, strs);
        }


        static bool IsGenericEnumerable(Type t)
        {
            if (!t.IsGenericType)
                return false;

            var genType = t.GetGenericTypeDefinition();
            return genType == typeof(IEnumerable<>) || genType == typeof(IList<>);

        }


        public void AddInterface(Type type)
        {
            if (_interfaceMap.ContainsKey(type))
                return;

            var tsInterface = new TSInterface(type, GenerateLookupTypeName);
            _interfaceMap.Add(type, tsInterface);
            tsInterface.Initialize();
        }
    }
}