using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WoaW.RnD.LinQProvider;
using System.IO;
using System.Linq;

namespace WoaW.RnD.MMF.UnitTests
{
    /// <summary>
    /// Summary description for UnitTest2
    /// </summary>
    [TestClass]
    public class FileSystemLinQProviderUnitTests
    {
        public FileSystemLinQProviderUnitTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestMethod1()
        {
            var query = from element in new FileSystemContext(@"C:\Temp")
                        where element.ElementType == ElementType.File && element.Path.EndsWith(".zip")
                        orderby element.Path ascending
                        select element;
            Assert.AreEqual(2, query.Count());

            ////it is standard working code 
            //var files = from file in new DirectoryInfo(@"C:\Downloads").GetFiles()
            //            where file.Extension == "zip"
            //            select file;
        }
    }
}
