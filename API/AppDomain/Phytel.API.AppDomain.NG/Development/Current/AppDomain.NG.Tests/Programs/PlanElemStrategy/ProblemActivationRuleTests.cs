using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.Test.Stubs;
using Phytel.API.DataDomain.Program.DTO;

namespace Phytel.API.AppDomain.NG.Programs.ElementActivation.Tests
{
    [TestClass()]
    public class ProblemActivationRuleTests
    {
        [TestClass()]
        public class ExecuteTest
        {
            [TestMethod()]
            public void Execute_Problem()
            {
                IElementActivationRule rule = new ProblemActivationRule
                {
                    EndpointUtil = new StubEndpointUtils(),
                    PlanUtils = new NG.Fakes.StubIPlanElementUtils()
                };
                SpawnElement se = new NG.DTO.Fakes.StubSpawnElement {ElementType = 101};
                PlanElementEventArg arg = new NG.PlanCOR.Fakes.StubPlanElementEventArg {};
                var userid = "999999999999999999999999";
                var result = rule.Execute(userid, arg, se, new ProgramAttributeData());
                Assert.AreEqual(result, "Problems");
            }

            [TestMethod()]
            public void Execute_ToDo()
            {
                IElementActivationRule rule = new ToDoActivationRule
                {
                    EndpointUtil = new StubEndpointUtils(),
                    PlanUtils = new NG.Fakes.StubIPlanElementUtils()
                };
                SpawnElement se = new NG.DTO.Fakes.StubSpawnElement { ElementType = 111 };
                PlanElementEventArg arg = new NG.PlanCOR.Fakes.StubPlanElementEventArg { };
                var userid = "999999999999999999999999";
                var result = rule.Execute(userid, arg, se, new ProgramAttributeData());
                Assert.AreEqual(result, "ToDo");
            }
        }
    }
}
