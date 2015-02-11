using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TS.CodeGenerator.tests
{
    public interface ITestGenerics<T>
    {
        T MyMethod(T parm1, T parm2);
    }

    public class TestGenerics<T>
    {
        public IEnumerable<T> Items { get; set; } 
    }

  
    [TestClass]
    public class TSInterfact_test
    {
        [TestMethod]
        public void Test_InterfaceGenerics()
        {
            //arrange
              var gen = new TSInterface(typeof (ITestGenerics<>), (t) =>
              {
                  if (t.IsGenericParameter)
                      return t.Name;
                  return Settings.StartingTypeMap[t];
              });
            gen.Initialize();
            //act
            var res = gen.ToTSString();

            //assert
            Assert.IsTrue(res.Contains("interface ITestGenerics<T>"));
        }

        [TestMethod]
        public void Test_ClassGenerics()
        {
            //arrange
            var gen = new TSInterface(typeof(TestGenerics<>), (t) =>
            {
                if (t.Name.Contains("IEnumerable`1"))
                    return "T[]";
                return Settings.StartingTypeMap[t];
            });
            gen.Initialize();
            //act
            var res = gen.ToTSString();

            //assert
            Assert.IsTrue(res.Contains("interface ITestGenerics<T>"));
            Assert.IsTrue(res.Contains("Items: T[];"));
        }

       


    }
}
