using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TS.CodeGenerator
{
    public class TSInterface : IGenerateTS
    {
        private const string formatInterface =
@"
/*{0}*/
{1}interface {2}{3}{{
{4}
{5}
}}
";
        private Type _type;
        private readonly Func<Type, string> _mapType;

        public TSInterface(Type type, Func<Type, string> mapType)
        {
            var ti = type.GetTypeInfo();
            _type = type;
            _mapType = mapType;
            ModuleName = type.Namespace;
            FullName = ti.FullName;
            //Methods = new List<TSMethod>();
            var name = ti.IsGenericType
                            ? type.Name.Split(new[] { '`' }).First()
                            : type.Name;
            InterFaceName = ti.IsInterface
                            ? name
                            : string.Format(Settings.InterfaceFormat, name);

            //interfaces
            Interfaces = ti.GetInterfaces().Where(i => i.GetTypeInfo().Assembly == ti.Assembly && i.Name != InterFaceName).ToList();
            if (ti.BaseType != null && ti.BaseType != typeof(object))
                Interfaces.Add(ti.BaseType);

            //properties
            var props = ti.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            Properties = props.Select(p => new TSProperty(p, _mapType)).ToList();

            //fields, not properties
            var fields = ti.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            var addp = fields.Select(f => new TSProperty(f, _mapType)).ToList();
            Properties.AddRange(addp);

            //generic
            //if (_type.IsGenericType)
            //{
            GenericParameters = ti.GetGenericArguments().Select(a => new TSGenericParameter(a, _mapType)).ToList();
            // }

            //methods
            var methods = ti.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).Where(x => !x.IsSpecialName);
            Methods = methods.Select(m => new TSMethod(m, _mapType)).ToList();
        }

        public List<Type> Interfaces { get; set; }
        public bool IsExported { get; set; }
        public string InterFaceName { get; private set; }
        public List<string> DerivedInterfaces { get; private set; } = new List<string>();
        public List<TSProperty> Properties { get; private set; }
        public List<TSMethod> Methods { get; private set; }
        public List<TSGenericParameter> GenericParameters { get; set; }
        public string ModuleName { get; set; }

        public string FullName { get; }

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
            var ti = _type.GetTypeInfo();
            //properties
            foreach (var tsProperty in Properties)
            {
                tsProperty.Initialize();
            }

            //base types
            var exc = new [] { Types.Any, Types.Boolean, Types.Number, Types.String, Types.Void };
            foreach (var intr in Interfaces)
            {
                var typ = _mapType(intr);
                if(!exc.Contains(typ))
                    DerivedInterfaces.Add(typ);
            }
            //DerivedInterfaces = Interfaces.Select(_mapType).Except(new[] { Types.Any, Types.Boolean, Types.Number, Types.String, Types.Void }).ToList();
            //foreach(var der in DerivedInterfaces) { }

            //generic
            foreach (var tsGenericParameter in GenericParameters)
            {
                tsGenericParameter.Initialize();
            }
            if (ti.IsGenericType)
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
            string result = string.Format(formatInterface,
                                            FullName,
                                            IsExported ? "export " : string.Empty,
                                            InterFaceName,
                                            derived,
                                            properties,
                                            methodes);
            return result;
        }
    }
}