using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;

namespace MongoTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            MongoDatabase db = Phytel.Services.Mongo.MongoService.Instance.GetDatabase("TEST", false);
            MongoUser user = db.FindUser("tony");
            Assert.IsFalse(user.Username == "tony");
        }
    }
}
