using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TS.CodeGenerator.tests
{
    interface MyClass<T>
    {
        T GetT(T input);
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
    }
}