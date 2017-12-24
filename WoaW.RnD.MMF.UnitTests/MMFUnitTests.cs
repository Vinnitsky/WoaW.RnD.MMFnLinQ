using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace WoaW.RnD.MMF.UnitTests
{
    [TestClass]
    public class MMFUnitTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            InMemDb.Update();

        }
        [TestMethod]
        public void Write_SuccessTest()
        {
            InMemDb.Write();
        }
        [TestMethod]
        public void Add_SuccessTest()
        {
            var db = new InMemDb();
            db.ConnectionString = @"..\..\..\db.data";
            db.Connect();

            db.Add(new MyData() { Id="1", Name="1", Description="1"});
            Assert.AreEqual(1, db._data.Count);
            List<object> setTemp;
            db._data.TryGetValue(typeof(MyData), out setTemp);
            Assert.AreEqual(1, setTemp.Count);
            Assert.AreEqual(1, db._records.Count);
                
            db.Add(new MyData() { Id="2", Name="2", Description="2"});
            Assert.AreEqual(1, db._data.Count);
            db._data.TryGetValue(typeof(MyData), out setTemp);
            Assert.AreEqual(2, setTemp.Count);
            Assert.AreEqual(2, db._records.Count);

            db.Add(new MyData() { Id="3", Name="3", Description="3"});
            Assert.AreEqual(1, db._data.Count);
            db._data.TryGetValue(typeof(MyData), out setTemp);
            Assert.AreEqual(3, setTemp.Count);
            Assert.AreEqual(3, db._records.Count);

            db.Add(new MyData() { Id="4", Name="4", Description="4"});
            Assert.AreEqual(1, db._data.Count);
            db._data.TryGetValue(typeof(MyData), out setTemp);
            Assert.AreEqual(4, setTemp.Count);
            Assert.AreEqual(4, db._records.Count);

            db.Add(new MyData() { Id="5", Name="5", Description="5"});
            Assert.AreEqual(1, db._data.Count);
            db._data.TryGetValue(typeof(MyData), out setTemp);
            Assert.AreEqual(5, setTemp.Count);
            Assert.AreEqual(5, db._records.Count);

        }
        [TestMethod]
        public void Save_SuccessTest()
        {
            var db = new InMemDb();
            db.ConnectionString = @"..\..\..\db.data";
            db.Connect();

            db.Add(new MyData() { Id="1", Name="1", Description="1"});
            db.Add(new MyData() { Id="2", Name="2", Description="2"});
            db.Add(new MyData() { Id="3", Name="3", Description="3"});
            db.Add(new MyData() { Id="4", Name="4", Description="4"});
            db.Add(new MyData() { Id="5", Name="5", Description="5"});
            db.Save< MyData>();
        }
        [TestMethod]
        public void Load_SuccessTest()
        {
            var db = new InMemDb();
            db.ConnectionString = @"..\..\..\db.data";
            db.Connect();

            db.Load();

            List<object> setTemp;
            Assert.AreEqual(1, db._data.Count);
            db._data.TryGetValue(typeof(MyData), out setTemp);
            Assert.AreEqual(5, setTemp.Count);
        }
    }
}
