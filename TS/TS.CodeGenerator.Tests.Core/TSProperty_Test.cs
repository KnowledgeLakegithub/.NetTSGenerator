using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace TS.CodeGenerator.tests
{
    public class GenericObj<T>
    {
        public T ID { get; set; }
    }

    public enum Sex
    {
        Bob,
        NotBob,
        NoSay
    }

    public class testClass1<T>:GenericObj<Guid>
    {
        public string Prop1 { get; set; }
        public int Prop2 { get; set; }
        public float Prop3 { get; set; }
        public DateTime Prop4 { get; set; }
        public DateTime? Prop41 { get; set; }
        public BigClassInherited Prop5 { get; set; }
        public List<BigClassInherited> Prop6 { get; set; }
        public List<int> Prop7 { get; set; }
        public float[] Prop8 { get; set; }
        public BigClassInherited[] Prop9 { get; set; }
        public IEnumerable<BigClassInherited> Prop10 { get; set; }
        public T Prop11 { get; set; }
        public IEnumerable<T> Prop12 { get; set; }
        public List<T> Prop121 { get; set; }
        public IList<T> Prop122 { get; set; }
        public ICollection<T> Prop123 { get; set; }

        public Sex Prop13 { get; set; }
        //not really properties

        public string Prop1_1;
        public int Prop2_1;
        public float Prop3_1;
        public DateTime Prop4_1;
        public DateTime? Prop41_1;
        public BigClassInherited Prop5_1;
        public List<BigClassInherited> Prop6_1;
        public List<int> Prop7_1;
        public float[] Prop8_1;
        public BigClassInherited[] Prop9_1;
        public IEnumerable<BigClassInherited> Prop10_1;
        public T Prop11_1;
        public IEnumerable<T> Prop12_1;
        public Sex Prop13_1;
        public Dictionary<string,bool> Prop14_1;
    }

    public class BigClassInherited : IInterfaceClass
    {
        public int Prop1 { get; set; }

        public string KillAllHumans(BigClassInherited c)
        {
            return null;
        }

        public BigClassInherited wannaKillALLHUMANS()
        {
            return null;
        }
    }

    public interface IInterfaceClass
    {
        int Prop1 { get; set; }
    }
    public interface Idictclass
    {
        Dictionary<string, bool> frank { get; }
        Dictionary<bool, string> frank2 { get; }
    }
    [TestClass]
    public class TSProperty_Test
    {
        [TestMethod]
        public void TestSimpleProperty()
        {

            //arrange
            var c = typeof(testClass1<>);
            var prop = new TSProperty(c.GetTypeInfo().GetProperties()[0], (t) => "string");
            prop.Initialize();

            //act
            var res = prop.ToTSString();

            //assert
            Assert.IsTrue(!string.IsNullOrEmpty(res));
        }

        [TestMethod]
        public void TestDictClass()
        {

            //arrange
            var c = typeof(Idictclass);
            var gen = new TSGenerator();
            var prop = new TSInterface(c, gen.GenerateLookupTypeName);
            prop.Initialize();

            //act
            var res = prop.ToTSString();

            //assert
            Assert.IsTrue(!string.IsNullOrEmpty(res));
        }



        [TestMethod]
        public void TestSimpleClass()
        {

            //arrange
            var c = typeof(testClass1<>);
            var prop = new TSInterface(c, (t) => "string");
            prop.Initialize();

            //act
            var res = prop.ToTSString();

            //assert
            Assert.IsTrue(!string.IsNullOrEmpty(res));
        }

        [TestMethod]
        public void TestSimpleGenerator()
        {

            //arrange
            Settings.ConstEnumsEnabled = true;
            var c = typeof(testClass1<>);
            var gen = new TSGenerator(c.GetTypeInfo().Assembly);

            //act
            gen.AddInterface(c);
            var res = gen.ToTSString();

            //assert
            Assert.IsTrue(!string.IsNullOrEmpty(res));
        }

       


    }
}
