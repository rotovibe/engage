using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using System;
using System.Collections.Generic;
using Phytel.API.AppDomain.NG;
using Phytel.API.AppDomain.NG.Test.Stubs;

namespace Phytel.API.AppDomain.NG.Test.Programs
{
    [TestClass]
    public class ProgramTest
    {
        [TestMethod()]
        [TestCategory("NIGHT-921")]
        [TestProperty("TFS", "4956")]
        public void AD_Get_Action_Name_Test()
        {
            string patientId = "5325db5dd6a4850adcbba90e";
            string programId = "536aa669d6a485044cdd41cd";
            string userId = "0000000000000000000000000";
            string name = "Program Completion";
            string patientModuleId = "536aa669d6a485044cdd4345";
            string patientActionId = "536aa669d6a485044cdd4380";

            INGManager ngm = new NGManager { PlanElementUtils = new StubPlanElementUtils(), EndpointUtils = new StubPlanElementEndpointUtils() };

            GetPatientActionDetailsRequest request = new GetPatientActionDetailsRequest
            {
                Version = 1.0,
                ContractNumber = "InHealth001",
                PatientId = patientId,
                PatientProgramId = programId,
                UserId = userId,
                PatientModuleId = patientModuleId,
                PatientActionId = patientActionId
            };

            GetPatientActionDetailsResponse response = ngm.GetPatientActionDetails(request);
            Assert.AreEqual(name, response.Action.Name, true);
        }
    }
}
