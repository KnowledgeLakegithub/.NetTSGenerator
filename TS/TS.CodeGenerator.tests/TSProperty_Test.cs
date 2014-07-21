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

    public class testClass1:GenericObj<Guid>
    {
        public string Prop1 { get; set; }
        public int Prop2 { get; set; }
        public float Prop3 { get; set; }
        public DateTime Prop4 { get; set; }
        public DateTime? Prop41 { get; set; }
        public testClass2 Prop5 { get; set; }
        public List<testClass2> Prop6 { get; set; }
        public List<int> Prop7 { get; set; }
        public float[] Prop8 { get; set; }
        public testClass2[] Prop9 { get; set; }
    }

    public class testClass2 : Itestc2
    {
        public int Prop1 { get; set; }

        public string KillAllHumans(testClass2 c)
        {
            return null;
        }

        public testClass2 wannaKillALLHUMANS()
        {
            return null;
        }
    }

    public interface Itestc2
    {
        int Prop1 { get; set; }
    }

    [TestClass]
    public class TSProperty_TSInterface_Test
    {
        [TestMethod]
        public void TestSimpleProperty()
        {

            //arrange
            var c = typeof(testClass1);
            var prop = new TSProperty(c.GetProperties()[0], (t) => "string");

            //act
            var res = prop.ToString();

            //assert
            Assert.IsTrue(!string.IsNullOrEmpty(res));
        }

        [TestMethod]
        public void TestSimpleClass()
        {

            //arrange
            var c = typeof(testClass1);
            var prop = new TSInterface(c, (t) => "string");

            //act
            var res = prop.ToString();

            //assert
            Assert.IsTrue(!string.IsNullOrEmpty(res));
        }

        [TestMethod]
        public void TestSimpleGenerator()
        {

            //arrange
            var c = typeof(testClass1);
            var gen = new TSGenerator(c.Assembly);

            //act
            gen.AddInterface(c);
            var res = gen.ToTSString();

            //assert
            Assert.IsTrue(!string.IsNullOrEmpty(res));
        }

       


    }
}
