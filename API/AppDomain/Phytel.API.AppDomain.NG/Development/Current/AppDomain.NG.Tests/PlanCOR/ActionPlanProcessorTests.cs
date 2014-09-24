using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.PlanCOR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.Test.Stubs;

namespace Phytel.API.AppDomain.NG.PlanCOR.Tests
{
    [TestClass()]
    public class ActionPlanProcessorTests
    {
        [TestMethod()]
        public void PlanElementHandler_Test()
        {
            var actionPlan = new ActionPlanProcessor
            {
                PeUtils = new StubPlanElementUtils(),
                PEUtils = new StubPlanElementUtils()
            };
            Assert.IsNotNull(actionPlan);
        }
    }
}
