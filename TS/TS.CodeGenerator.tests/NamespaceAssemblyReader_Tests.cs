using lib.test.a.b.Vehicals.TwoWheeled.MotorCycles;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TS.CodeGenerator.tests
{
    [TestClass]
    public class NamespaceAssemblyReader_Tests
    {
        [TestMethod]
        public void TestMethod1()
        {
            //arrange
            Settings.ConstEnumsEnabled = true;
           
            var nsg = new NamespaceAssemblyReader(typeof (Harley<>).Assembly);
            //act
            var s = nsg.GenerateTypingsString();


            //assert
            Assert.IsNotNull(s);
        }
    }
}
