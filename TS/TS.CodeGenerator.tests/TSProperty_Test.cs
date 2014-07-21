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

        [TestMethod]
        public void TestAssemblyGenerator()
        {

            //arrange
            var c = typeof(testClass1);
           

            //act
            var path = Path.Combine(Directory.GetCurrentDirectory(), "KnowledgeLake.Index.Web.Contracts.dll");
            var rdr = new AssemblyReader(path);
            var res = rdr.GenerateTypingsString();


            //assert
            Assert.IsTrue(!string.IsNullOrEmpty(res));
        }


        //[TestMethod]
        //public void TestLocal()
        //{

        //    //arrange
        //    var c = typeof(testClass1);
           

        //    //act
        //    var path = Path.Combine(@"C:\Development\MaintenanceSuite\Maintenance.Contracts\bin\Debug\", "Maintenance.Contracts.dll");
        //    var rdr = new AssemblyReader(path);
        //    var res = rdr.GenerateTypingsString();


        //    //assert
        //    Assert.IsTrue(!string.IsNullOrEmpty(res));
        //}
       // [TestMethod]
        public void TestALLTHETHINGS()
        {

            //arrange
            var c = typeof(testClass1);
            
            //act

            foreach (string file in Directory.EnumerateFiles("D://Development", "*.contracts.dll", SearchOption.AllDirectories))
            {
                string res;
                try
                {
                    var rdr = new AssemblyReader(file);

                    res = rdr.GenerateTypingsString();
                }
                catch (FileNotFoundException e)
                {
                    
                }
            }
          
            //assert
            Assert.IsTrue(!string.IsNullOrEmpty(""));
        }


    }
}
