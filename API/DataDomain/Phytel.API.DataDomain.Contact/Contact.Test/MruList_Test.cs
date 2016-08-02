using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Contact;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
namespace Phytel.API.DataDomain.Contact.Tests
{
    [TestClass()]
    public class MruList_Test
    {
        [TestClass()]
        public class AddPatient
        {
            [TestMethod()]
            [TestCategory("NIGHT-911")]
            [TestProperty("TFS", "10409")]
            [TestProperty("Layer", "DD.MRUList")]
            public void Add_New_Patient()
            {
                IMruList mrulist = new MruList { Limit = 5, RecentList = new List<string>() };
                string newP = ObjectId.GenerateNewId().ToString();

                mrulist.AddPatient(newP.ToString());

                Assert.AreEqual(1, mrulist.RecentList.Count);
            }

            [TestMethod()]
            [TestCategory("NIGHT-911")]
            [TestProperty("TFS", "10409")]
            [TestProperty("Layer", "DD.MRUList")]
            public void Add_Existing_Patient_to_Three()
            {
                IMruList mrulist = new MruList { Limit = 5, RecentList = new List<string>() };

                ObjectId newP = ObjectId.GenerateNewId();
                ObjectId newP2 = ObjectId.GenerateNewId();
                ObjectId newP3 = ObjectId.GenerateNewId();

                mrulist.AddPatient(newP.ToString());
                mrulist.AddPatient(newP2.ToString());
                mrulist.AddPatient(newP3.ToString());
                int index = mrulist.RecentList.IndexOf(newP.ToString()) +1;

                mrulist.AddPatient(newP.ToString());
                int result = mrulist.RecentList.IndexOf(newP.ToString()) + 1;
                Assert.AreNotEqual(index, result);
            }

            [TestMethod()]
            [TestCategory("NIGHT-911")]
            [TestProperty("TFS", "10409")]
            [TestProperty("Layer", "DD.MRUList")]
            public void Add_Existing_Patient_to_Three_Top_Of_Stack()
            {
                IMruList mrulist = new MruList { Limit = 5, RecentList = new List<string>() };

                ObjectId newP = ObjectId.GenerateNewId();
                ObjectId newP2 = ObjectId.GenerateNewId();
                ObjectId newP3 = ObjectId.GenerateNewId();

                mrulist.AddPatient(newP.ToString());
                mrulist.AddPatient(newP2.ToString());
                mrulist.AddPatient(newP3.ToString());

                mrulist.AddPatient(newP.ToString());
                int result = mrulist.RecentList.IndexOf(newP.ToString());
                Assert.AreEqual(0 + 1, result + 1);
            }

            [TestMethod()]
            [TestCategory("NIGHT-911")]
            [TestProperty("TFS", "10409")]
            [TestProperty("Layer", "DD.MRUList")]
            public void Add_Existing_6th_Patient_Not_There()
            {
                IMruList mrulist = new MruList { Limit = 5, RecentList = new List<string>() };

                ObjectId newP = ObjectId.GenerateNewId();
                ObjectId newP2 = ObjectId.GenerateNewId();
                ObjectId newP3 = ObjectId.GenerateNewId();
                ObjectId newP4 = ObjectId.GenerateNewId();
                ObjectId newP5 = ObjectId.GenerateNewId();
                ObjectId newP6 = ObjectId.GenerateNewId();

                mrulist.AddPatient(newP.ToString());
                mrulist.AddPatient(newP2.ToString());
                mrulist.AddPatient(newP3.ToString());
                mrulist.AddPatient(newP4.ToString());
                mrulist.AddPatient(newP5.ToString());
                mrulist.AddPatient(newP6.ToString());

                string found = mrulist.RecentList.Find(o => o.Equals(newP.ToString()));
                Assert.IsNull( found);
            }
        }
    }
}
