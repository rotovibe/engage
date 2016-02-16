using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.PatientGoal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientGoal.DTO;
using Phytel.API.DataDomain.PatientGoal.Test.Stubs;

namespace Phytel.API.DataDomain.PatientGoal.Tests
{
    [TestClass()]
    public class PatientGoalDataManagerTests
    {
        [TestMethod()]
        public void GetPatientByTemplateIdGoalTest()
        {
            var pgm = new PatientGoalDataManager() {Factory =new StubPatientGoalRepositoryFactory()};
            var request = new GetPatientGoalByTemplateIdRequest
            {
                TemplateId = "545a91a1fe7a59218cef2d6d",
                PatientId = "5325db9cd6a4850adcbba9ca",
                Context = "NG",
                ContractNumber = "InHealth001",
                UserId = "1234",
                Version = 1
            };

            var response = pgm.GetPatientByTemplateIdGoal(request);

            Assert.IsNotNull(response);
        }
    }
}
