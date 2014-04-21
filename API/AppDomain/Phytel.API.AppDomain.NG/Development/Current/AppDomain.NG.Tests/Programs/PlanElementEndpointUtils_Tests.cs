using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.Test.Stubs;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.AppDomain.NG.DTO;
namespace Phytel.API.AppDomain.NG.Tests
{
    [TestClass()]
    public class PlanElementEndpointUtils_Tests
    {
        [TestMethod()]
        public void RequestPatientProgramDetailsSummary_Test()
        {
            StubPlanElementEndpointUtils peu = new StubPlanElementEndpointUtils { Client = new StubJsonRestClient() };
            GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest();
            GetProgramDetailsSummaryResponse response = peu.RequestPatientProgramDetailsSummary(request);
            Assert.IsNotNull(response);
        }
    }
}
