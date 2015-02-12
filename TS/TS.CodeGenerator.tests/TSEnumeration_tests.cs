using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TS.CodeGenerator.tests
{

    enum test1
    {
        First,
        Second
    }

    [TestClass]
    public class TSEnumeration_tests
    {
        [TestMethod]
        public void TestSimpleProperty()
        {

            //arrange
            var c = typeof(test1);
            var e = new TSConstEnumeration(c);
            e.Initialize();

            //act
            var res = e.ToTSString();

            //assert
            Assert.IsTrue(!string.IsNullOrEmpty(res));
        }

        

    }
}
