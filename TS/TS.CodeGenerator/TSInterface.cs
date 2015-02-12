using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TS.CodeGenerator
{
    public class TSInterface:IGenerateTS
    {
        private const string formatInterface =
@"
/*{0}*/
interface {1}{2}{{
{3}
{4}
}}
";
        private Type _type;
        private readonly Func<Type, string> _mapType;

        public TSInterface(Type type, Func<Type, string> mapType)
        {
            _type = type;
            _mapType = mapType;

            //Methods = new List<TSMethod>();
            var name = type.IsGenericType
                            ? type.Name.Split(new[] { '`' }).First()
                            : type.Name;
            InterFaceName = type.IsInterface
                            ? name
                            : string.Format(Settings.InterfaceFormat, name);

            //interfaces
            Interfaces = _type.GetInterfaces().Where(i => i.Assembly == _type.Assembly && i.Name != InterFaceName).ToList();
            if (_type.BaseType != null && _type.BaseType != typeof(object))
                Interfaces.Add(_type.BaseType);

            //properties
            var props = _type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            Properties = props.Select(p => new TSProperty(p, _mapType)).ToList();

            //generic
            //if (_type.IsGenericType)
            //{
                GenericParameters = _type.GetGenericArguments().Select(a => new TSGenericParameter(a, _mapType)).ToList();
           // }

            //methods
            var methods = _type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).Where(x => !x.IsSpecialName);
            Methods = methods.Select(m => new TSMethod(m, _mapType)).ToList();
        }

        public List<Type> Interfaces { get; set; }

        public string InterFaceName { get; private set; }
        public List<string> DerivedInterfaces { get; private set; }
        public List<TSProperty> Properties { get; private set; }
        public List<TSMethod> Methods { get; private set; }
        public List<TSGenericParameter> GenericParameters { get; set; }

        public bool IsGenericMetaClass
        {
            get
            {
                if (GenericParameters == null)
                    return false;
                return GenericParameters.Any(p => !p.IsGenericParameter);
            }
        }

        public void Initialize()
        {
            //properties
            foreach (var tsProperty in Properties)
            {
                tsProperty.Initialize();
            }

            //base types
            DerivedInterfaces = Interfaces.Select(_mapType).Except(new[] { Types.Any, Types.Boolean, Types.Number, Types.String, Types.Void }).ToList();


            //generic
            foreach (var tsGenericParameter in GenericParameters)
            {
                tsGenericParameter.Initialize();
            }
            if (_type.IsGenericType)
            {
                InterFaceName += string.Format("<{0}>", string.Join(", ", GenericParameters.Select(p => p.ToTSString())));
            }

            //methods
            foreach (var tsMethod in Methods)
            {
                tsMethod.Initialize();
            }
        }

        public string ToTSString()
        {
          
            string derived = string.Empty;

            if (DerivedInterfaces != null && DerivedInterfaces.Count > 0)
            {
                derived = " extends " + string.Join(",", DerivedInterfaces);
            }

            string properties = Properties == null || Properties.Count == 0
                                ? string.Empty
                                : "  /*properties*/" + Settings.EndOfLine 
                                + "\t" + string.Join(Settings.EndOfLine + "\t", Properties.Select(p => p.ToTSString()));
            string methodes = Methods == null || Methods.Count == 0
                                ? string.Empty
                                : "  /*methods*/" + Settings.EndOfLine 
                                + "\t" + string.Join(Settings.EndOfLine + "\t", Methods.Select(m => m.ToTSString()));
            string result = string.Format(formatInterface, _type.FullName, InterFaceName, derived, properties, methodes);
            return result;
        }
    }
}