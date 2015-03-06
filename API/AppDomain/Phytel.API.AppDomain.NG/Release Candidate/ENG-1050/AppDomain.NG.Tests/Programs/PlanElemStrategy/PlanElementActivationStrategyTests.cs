using MongoDB.Bson;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.Test.Stubs;

namespace Phytel.API.AppDomain.NG.Programs.PlanElemStrategy.Tests
{
    [TestClass()]
    public class PlanElementActivationStrategyTests
    {
        [TestClass()]
        public class RunTest
        {
            [TestMethod()]
            public void Run_Problem_Rule()
            {
                var se = new SpawnElement {ElementType = 101};
                var arg = new PlanElementEventArg {UserId = ObjectId.GenerateNewId().ToString()};
                var strat = new StubElementActivationStrategy().Run(arg, se, ObjectId.GenerateNewId().ToString(), new DataDomain.Program.DTO.ProgramAttributeData());
                Assert.AreEqual(strat, "Problems");
            }

            [TestMethod()]
            public void Run_ToDo_Rule()
            {
                var se = new SpawnElement { ElementType = 111 };
                var arg = new PlanElementEventArg { UserId = ObjectId.GenerateNewId().ToString() };
                var strat = new StubElementActivationStrategy().Run(arg, se, ObjectId.GenerateNewId().ToString(), new DataDomain.Program.DTO.ProgramAttributeData());
                Assert.AreEqual(strat, "ToDo");
            }
        }
    }
}
