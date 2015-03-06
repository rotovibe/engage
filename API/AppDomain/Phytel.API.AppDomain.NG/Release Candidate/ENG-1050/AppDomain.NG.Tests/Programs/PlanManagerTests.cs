using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using Phytel.API.AppDomain.NG;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.Test.Factories;

namespace Phytel.API.AppDomain.NG.Tests
{
    [TestClass()]
    public class PlanManagerTests
    {
        [TestMethod()]
        public void ReplaceSpawnElementReferencesTest()
        {
            var pm = new PlanManager {};
            var origId = "999999999999999999999999";
            var newId = "000000000000000000000000";

            var list = new List<DTO.SpawnElement>
            {
                new DTO.SpawnElement
                {
                    ElementId = "53cfeea9d6a4850d58939849"
                },
                new DTO.SpawnElement
                {
                    ElementId = "532caae9a38116ac18000846"
                },
                new DTO.SpawnElement
                {
                    ElementId = "999999999999999999999999"
                }
            };

            pm.ReplaceSpawnElementReferences(list, origId, newId);

            Assert.AreEqual(list[2].ElementId, newId);
        }

        [TestMethod()]
        public void InsertActionIntoModuleTest()
        {
            var pm = new PlanManager();
            var actionId = ObjectId.GenerateNewId().ToString();
            var step1Id = ObjectId.GenerateNewId().ToString();
            var step2Id = ObjectId.GenerateNewId().ToString();
            var selectedRespId = ObjectId.GenerateNewId().ToString();
            var repeatId = "999999999999999999999999";
            var module = SampleFactory.CreateModule(actionId, step1Id, step2Id, selectedRespId);

            var action = SampleFactory.CreateAction(repeatId);

            pm.ReplaceActionReferenceInModule(actionId, action, module);
            Assert.AreEqual(module.Actions[0].Next, repeatId);
            Assert.AreEqual(module.Actions[0].Previous, repeatId);
            Assert.AreEqual(module.Actions[0].SpawnElement[0].ElementId, repeatId);
            Assert.AreEqual(module.Actions[0].Steps[0].SpawnElement[0].ElementId, repeatId);
            Assert.AreEqual(module.Actions[0].Steps[0].Responses[0].SpawnElement[0].ElementId, repeatId);
            Assert.AreEqual(module.Actions.Count, 3);
            Assert.AreEqual(module.Actions[2].Id, repeatId);
        }
    }
}
