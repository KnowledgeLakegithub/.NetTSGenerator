using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace TS.CodeGenerator.tests
{
    interface MyClass<T>
    {
        T GetT(T input);

        IEnumerable<int> data { get; set; }
    }

    interface IMYGenericProperties<T>
    {
        IEnumerable<T> GenericList { get; set; } 
    }

    public class MyGenericPropertiesClass<T>
    {
        public IEnumerable<T> GenericList { get; set; }
    }

    [TestClass]
    public class GenericParameter_Test
    {
        [TestMethod]
        public void TestMethod1()
        {
            //arrange
            var c = typeof(MyClass<>);
            var gen = new TSGenerator(c.GetTypeInfo().Assembly);

            //act
            gen.AddInterface(c);
            var res = gen.ToTSString();

            //assert
            Assert.IsTrue(res.Contains("GetT?(input:T/*T*/):JQueryPromise<T>;"));
        }
        [TestMethod]
        public void TestGenericProperty()
        {
            //arrange
            var c = typeof(IMYGenericProperties<>);
            var gen = new TSGenerator(c.GetTypeInfo().Assembly);

            //act
            gen.AddInterface(c);
            var res = gen.ToTSString();

            //assert
            Assert.IsTrue(res.Contains("GenericList: T[];"));
        }
        [TestMethod]
        public void TestGenericPropertyClass()
        {
            //arrange
            var c = typeof(MyGenericPropertiesClass<>);
            var gen = new TSGenerator(c.GetTypeInfo().Assembly);
            
            //act
            gen.AddInterface(c);
            var res = gen.ToTSString();

            //assert
            Assert.IsTrue(res.Contains("GenericList: T[];"));
        }
    

        [TestMethod]
        public void TestMethod2()
        {
            //arrange
            var c = typeof(MyClass<>);
            var gen = new TSGenerator(c.GetTypeInfo().Assembly);

            //act
            gen.AddInterface(c);
            var res = gen.ToTSString();

            //assert
            Assert.IsTrue(res.Contains("number[]"));
        }
    }
}