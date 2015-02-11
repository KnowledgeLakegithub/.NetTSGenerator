using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TS.CodeGenerator.tests
{
    public class GenericObj<T>
    {
        public T ID { get; set; }
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

    [TestClass]
    public class TSProperty_Test
    {
        [TestMethod]
        public void TestSimpleProperty()
        {

            //arrange
            var c = typeof(testClass1<>);
            var prop = new TSProperty(c.GetProperties()[0], (t) => "string");

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

            //act
            var res = prop.ToTSString();

            //assert
            Assert.IsTrue(!string.IsNullOrEmpty(res));
        }

        [TestMethod]
        public void TestSimpleGenerator()
        {

            //arrange
            var c = typeof(testClass1<>);
            var gen = new TSGenerator(c.Assembly);

            //act
            gen.AddInterface(c);
            var res = gen.ToTSString();

            //assert
            Assert.IsTrue(!string.IsNullOrEmpty(res));
        }

       


    }
}
