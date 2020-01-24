using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace TS.CodeGenerator.tests
{
    

    public interface IMethodClass
    {
        string MyMethod(int parm1, string parm2, Guid? param3);
    }

  
    [TestClass]
    public class TSMethod_Test
    {
        [TestMethod]
        public void Test_MethodCLassMethod()
        {
            //arrange
              var gen = new TSInterface(typeof (IMethodClass), (t) => Settings.StartingTypeMap[t]);
            gen.Initialize();
            //act
            var res = gen.ToTSString();

            //assert
            Assert.IsTrue(res.Contains("MyMethod?(parm1:number/*Int32*/, parm2:string/*String*/, param3?:string/*Nullable`1*/):JQueryPromise<string>;"));
        }

        [TestMethod]
        public void Test_TSMethod()
        {
            //arrange
            var gen = new TSMethod(typeof(IMethodClass).GetTypeInfo().GetMethod("MyMethod"), (t) => Settings.StartingTypeMap[t]);
            gen.Initialize();
            //act
            var res = gen.ToTSString();

            //assert
            Assert.IsTrue(res.Contains("MyMethod?(parm1:number/*Int32*/, parm2:string/*String*/, param3?:string/*Nullable`1*/):JQueryPromise<string>;"));
        }
    
       


    }
}
