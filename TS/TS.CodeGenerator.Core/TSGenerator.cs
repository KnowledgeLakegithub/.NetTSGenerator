using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TS.CodeGenerator
{
    public class TSGenerator : ITSGenerator
    {
        private Dictionary<Type, string> _typeMap;
        private Dictionary<Type, TSInterface> _interfaceMap;
        private Dictionary<Type, TSConstEnumeration> _enumerationsMap;
        private List<Assembly> _allowedAssemblies;
        public bool Modules { get; set; }
        public TSGenerator(Assembly currentAssembly):this()
        {
            _allowedAssemblies.Add(currentAssembly);

        }

        public TSGenerator()
        {
            _enumerationsMap = new Dictionary<Type, TSConstEnumeration>();
            _typeMap = new Dictionary<Type, string>(Settings.StartingTypeMap);
            _interfaceMap = new Dictionary<Type, TSInterface>();
            _allowedAssemblies = new List<Assembly>();
        }
        public void AddFollowAssembly(Assembly asm)
        {
            _allowedAssemblies.Add(asm);
        }

        private void _addTypeMap(Type type, string name)
        {
            if (_typeMap.ContainsKey(type))
                return;
            _typeMap.Add(type, name);
        }

        private bool _handleIfEnum(Type type)
        {
            var ti = type.GetTypeInfo();
            if (!ti.IsEnum)
                return false;


            if (!Settings.ConstEnumsEnabled)
                _typeMap.Add(type, Types.Any);

            if (!_typeMap.ContainsKey(type))
            {
                AddEnumeration(type);
                var e = _enumerationsMap[type];
                _addTypeMap(type, Modules ? e.ModuleName + "." + e.Name : e.Name);
            }

            return true;

        }

        private bool _handleIfGenericEnumerable(Type type)
        {
            var ti = type.GetTypeInfo();
            if (!IsGenericEnumerable(type))
                return false;

            //allow only generic enumerables
            //generic array first
            if (IsGenericEnumerable(type))
            {
                try
                {
                    var typeOfElements = type.IsArray
                        ? type.GetElementType()
                        : ti.GetGenericArguments().Single();
                    var name = GenerateLookupTypeName(typeOfElements) + "[]";

                    _addTypeMap(type, name);

                }
                catch
                {
                    if (!_typeMap.ContainsKey(type))
                        _addTypeMap(type, Types.Any);
                }
            }

            return true;
        }

        private bool _handleIfArray(Type type)
        {
            //standard array
            if (!type.IsArray)
                return false;


            var typeOfElements = type.GetElementType();
            var name = GenerateLookupTypeName(typeOfElements) + "[]";

            _addTypeMap(type, name);
            return true;
        }

        private bool _handleIfDict(Type type)
        {
            var ti = type.GetTypeInfo();
            if (!ti.IsGenericType)
                return false;


            var td = ti.GetGenericTypeDefinition();
            bool isDict = 
                        td == typeof(Dictionary<,>)
                        || td == typeof(ILookup<,>)
                        || td == typeof(IDictionary<,>)
                    ;
            if (!isDict)
                return false;

            var args = ti.GetGenericArguments();

            var p1 = GenerateLookupTypeName(args[0]);
            var p2 = GenerateLookupTypeName(args[1]);

            var t = string.Format(Settings.DictionaryFormat, p1, p2);

            _addTypeMap(type, t);
            return true;
        }


        private bool _handleIfGenericParameter(Type type)
        {
            if (!type.IsGenericParameter)
                return false;

            _addTypeMap(type,  type.Name);
            return true;
        }

        private bool _handleIfClassOrInterface(Type type)
        {
            var ti = type.GetTypeInfo();
            if (!_allowedAssemblies.Contains(ti.Assembly))
            {
                //anything from other assemblies are any
                _addTypeMap(type, Types.Any);
                return true;
            }

            AddInterface(type);
            var itf = _interfaceMap[type];
            _addTypeMap(type, Modules ? itf.ModuleName + "." + itf.InterFaceName : itf.InterFaceName);

            return true;
        }

        public string GenerateLookupTypeName(Type type)
        {
            //quickly map simple type maps
            if (_typeMap.ContainsKey(type))
                return _typeMap[type];

            if (_handleIfEnum(type))
                return _typeMap[type];

            if (_handleIfGenericEnumerable(type))
                return _typeMap[type];

            if (_handleIfArray(type))
                return _typeMap[type];

            if (_handleIfGenericParameter(type))
                return _typeMap[type];

            if (_handleIfDict(type))
                return _typeMap[type];

            if (_handleIfClassOrInterface(type))
                return _typeMap[type];

            return Types.Any;
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


            var strs = interfaces.Where(i => !Settings.IgnoreInterfaces.Contains(i.InterFaceName)).Select(v => v.ToTSString());
            var ints = string.Join(Settings.EndOfLine, strs);
            var enums = string.Join(Settings.EndOfLine, _enumerationsMap.Values.Select(en => en.ToTSString()));

            return ints + Settings.EndOfLine + enums;
        }


        static bool IsGenericEnumerable(Type t)
        {
            var ti = t.GetTypeInfo();
            if (!ti.IsGenericType)
                return false;

            var genType = t.GetGenericTypeDefinition();
            
            return genType == typeof(IEnumerable<>) 
                    || genType == typeof(IList<>)
                    || genType == typeof(List<>)
                    || genType == typeof(ICollection<>);

        }

        public void AddEnumeration(Type enumeration)
        {
            if (_enumerationsMap.ContainsKey(enumeration))
            {
                return;
            }

            var enumerationTS = new TSConstEnumeration(enumeration);
            _enumerationsMap.Add(enumeration, enumerationTS);
            enumerationTS.Initialize();
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