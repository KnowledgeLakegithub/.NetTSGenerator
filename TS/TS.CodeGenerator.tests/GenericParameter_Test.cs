using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TS.CodeGenerator.tests
{
    interface MyClass<T>
    {
        T GetT(T input);

        IEnumerable<int> data { get; set; }
    }
    [TestClass]
    public class GenericParameter_Test
    {
        [TestMethod]
        public void TestMethod1()
        {
            //arrange
            var c = typeof(MyClass<>);
            var gen = new TSGenerator(c.Assembly);

            //act
            gen.AddInterface(c);
            var res = gen.ToTSString();

            //assert
            Assert.IsTrue(res.Contains("GetT?(input:T/*T*/):JQueryPromise<T>;"));
        }
    
        [TestMethod]
        public void TestMethod2()
        {
            //arrange
            var c = typeof(MyClass<>);
            var gen = new TSGenerator(c.Assembly);

            //act
            gen.AddInterface(c);
            var res = gen.ToTSString();

            //assert
            Assert.IsTrue(res.Contains("number[]"));
        }
    }
}