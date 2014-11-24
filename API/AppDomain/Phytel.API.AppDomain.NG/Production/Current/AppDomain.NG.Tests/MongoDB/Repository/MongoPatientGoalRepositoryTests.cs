using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.PatientGoal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientGoal.DTO;

namespace Phytel.API.DataDomain.PatientGoal.Tests
{
    [TestClass()]
    public class MongoPatientGoalRepositoryTests
    {
        [TestMethod()]
        public void FindByTemplateId_Status_Open_Or_Met()
        {
            var repo = new PatientGoalRepositoryFactory();
            var request = new GetPatientGoalByTemplateIdRequest
            {
                TemplateId = "545a91a1fe7a59218cef2d6d",
                PatientId = "5325db9cd6a4850adcbba9ca",
                Context = "NG",
                ContractNumber = "InHealth001",
                UserId = "1234",
            };
            var rep = repo.GetRepository(request, RepositoryType.PatientGoal);
            var patientGoal = rep.FindByTemplateId(request.PatientId, request.TemplateId);

            Assert.IsNotNull(patientGoal);
        }
    }
}
