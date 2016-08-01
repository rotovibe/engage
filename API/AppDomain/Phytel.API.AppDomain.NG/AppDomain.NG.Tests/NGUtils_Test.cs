using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using MongoDB.Bson;
using Phytel.API.DataDomain.Program.DTO;
namespace Phytel.API.AppDomain.NG.Tests
{
    [TestClass()]
    public class NGUtils_Test
    {
        [TestClass()]
        public class GetADModules
        {
            [TestMethod()]
            [TestCategory("NIGHT-950")]
            [TestProperty("TFS", "11456")]
            [TestProperty("Layer", "NGUtils")]
            public void Get_With_AssignTo_Id()
            {
                ObjectId guid = ObjectId.GenerateNewId();
                List<Module> list = new List<Module> {new Module {AssignToId = guid.ToString()}};
                var nList = NGUtils.GetADModules(list);

                Assert.AreEqual(guid.ToString(), ((ModuleDetail)nList[0]).AssignTo);
            }
        }

        [TestClass()]
        public class GetADActions_Test
        {
            [TestMethod()]
            [TestCategory("NIGHT-877")]
            [TestProperty("TFS", "11456")]
            [TestProperty("Layer", "NGUtils")]
            public void Get_Action_With_AssignTo_Id()
            {
                ObjectId guid = ObjectId.GenerateNewId();
                List<Actions> list = new List<Actions> {new Actions {AssignToId = guid.ToString()}};
                var nList = NGUtils.GetADActions(list);

                Assert.AreEqual(guid.ToString(), ((ActionsDetail) nList[0]).AssignTo);
            }
        }
    }
}
