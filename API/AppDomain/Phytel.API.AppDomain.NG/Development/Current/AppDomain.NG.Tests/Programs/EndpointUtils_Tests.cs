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
using Phytel.API.AppDomain.NG.DTO.Observation;
using Phytel.API.Interface;
using Phytel.API.DataDomain.PatientObservation.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;
using MongoDB.Bson;

namespace Phytel.API.AppDomain.NG.Tests
{
    [TestClass()]
    public class EndpointUtils_Tests
    {
        [TestClass()]
        public class RequestPatientProgramDetailsSummary_Method
        {
            [TestMethod()]
            public void Valid_Response_Test()
            {
                StubPlanElementEndpointUtils peu = new StubPlanElementEndpointUtils { Client = new StubJsonRestClient() };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest();
                GetProgramDetailsSummaryResponse response = peu.RequestPatientProgramDetailsSummary(request);
                Assert.IsNotNull(response);
            }

            [TestMethod()]
            public void With_Description_Test()
            {
                StubPlanElementEndpointUtils peu = new StubPlanElementEndpointUtils { Client = new StubJsonRestClient() };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest();
                GetProgramDetailsSummaryResponse response = peu.RequestPatientProgramDetailsSummary(request);
                Assert.IsNotNull(response.Program.Description);
            }

            [TestMethod()]
            [TestCategory("NIGHT-917")]
            [TestProperty("TFS", "1886")]
            public void AD_With_Module_Description_Test()
            {
                string desc = "BSHSI - Outreach & Enrollment";
                StubPlanElementEndpointUtils peu = new StubPlanElementEndpointUtils { Client = new StubJsonRestClient() };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest();
                GetProgramDetailsSummaryResponse response = peu.RequestPatientProgramDetailsSummary(request);

                ModuleDetail module = response.Program.Modules.Find(m => m.SourceId == "532b5585a381168abe00042c");
                string mDesc = module.Description.Trim();
                Assert.AreEqual(desc, mDesc, true);
            }

            [TestMethod()]
            public void With_EligiblityRequirements_Test()
            {
                StubPlanElementEndpointUtils peu = new StubPlanElementEndpointUtils { Client = new StubJsonRestClient() };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest();
                GetProgramDetailsSummaryResponse response = peu.RequestPatientProgramDetailsSummary(request);
                Assert.IsNotNull(response.Program.EligibilityRequirements);
                Assert.IsNotNull(response.Program.EligibilityStartDate);
                Assert.IsNotNull(response.Program.EligibilityEndDate);
            }

            [TestMethod()]
            public void With_Attributes_AsNull_Test()
            {
                StubPlanElementEndpointUtils peu = new StubPlanElementEndpointUtils { Client = new StubJsonRestClient() };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest();
                GetProgramDetailsSummaryResponse response = peu.RequestPatientProgramDetailsSummary(request);
                Assert.IsNull(response.Program.Attributes);
            }

            [TestMethod()]
            [TestCategory("NIGHT-920")]
            [TestProperty("TFS", "6099")]
            public void GetActionIndividualAttributes_State_Test()
            {
                int actualValue = 4;
                StubPlanElementEndpointUtils peu = new StubPlanElementEndpointUtils { Client = new StubJsonRestClient() };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest();
                GetProgramDetailsSummaryResponse response = peu.RequestPatientProgramDetailsSummary(request);

                int expectedValue = response.Program.Modules[0].Actions[0].ElementState;
                Assert.AreEqual(expectedValue, actualValue);
            }

            [TestMethod()]
            [TestCategory("NIGHT-920")]
            [TestProperty("TFS", "6099")]
            public void GetActionIndividualAttributes_AssignedBy_Test()
            {
                StubPlanElementEndpointUtils peu = new StubPlanElementEndpointUtils { Client = new StubJsonRestClient() };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest();
                GetProgramDetailsSummaryResponse response = peu.RequestPatientProgramDetailsSummary(request);

                string expectedValue = response.Program.Modules[0].Actions[0].AssignBy;
                ObjectId objectId;
                bool success = ObjectId.TryParse(expectedValue, out objectId);
                Assert.IsTrue(success);
            }

            [TestMethod()]
            [TestCategory("NIGHT-832")]
            [TestProperty("TFS", "11170")]
            public void Get_Program_AssignedBy_Test()
            {
                StubPlanElementEndpointUtils peu = new StubPlanElementEndpointUtils { Client = new StubJsonRestClient() };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest { UserId = "123456789012345678901234" };
                GetProgramDetailsSummaryResponse response = peu.RequestPatientProgramDetailsSummary(request);

                string expectedValue = response.Program.AssignBy;
                ObjectId objectId;
                bool success = ObjectId.TryParse(expectedValue, out objectId);
                Assert.IsTrue(success);
            }

            [TestMethod()]
            [TestCategory("NIGHT-831")]
            [TestProperty("TFS", "11172")]
            public void Get_Program_AssignedDate_Test()
            {
                StubPlanElementEndpointUtils peu = new StubPlanElementEndpointUtils { Client = new StubJsonRestClient() };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest();
                GetProgramDetailsSummaryResponse response = peu.RequestPatientProgramDetailsSummary(request);

                DateTime expected = System.DateTime.UtcNow.Date;
                DateTime? value = response.Program.AssignDate;

                Assert.AreEqual(expected, ((DateTime)value).Date);
            }

            [TestMethod()]
            [TestCategory("NIGHT-920")]
            [TestProperty("TFS", "6099")]
            public void GetActionIndividualAttributes_AssignedTo_Test()
            {
                StubPlanElementEndpointUtils peu = new StubPlanElementEndpointUtils { Client = new StubJsonRestClient() };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest();
                GetProgramDetailsSummaryResponse response = peu.RequestPatientProgramDetailsSummary(request);

                string expectedValue = response.Program.Modules[0].Actions[0].AssignTo;
                ObjectId objectId;
                bool success = ObjectId.TryParse(expectedValue, out objectId);
                Assert.IsTrue(success);
            }

            [TestMethod()]
            [TestCategory("NIGHT-920")]
            [TestProperty("TFS", "6099")]
            public void GetActionIndividualAttributes_AttrStartDate_Test()
            {
                DateTime expectedValue = DateTime.UtcNow.Date;
                StubPlanElementEndpointUtils peu = new StubPlanElementEndpointUtils { Client = new StubJsonRestClient() };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest();
                GetProgramDetailsSummaryResponse response = peu.RequestPatientProgramDetailsSummary(request);

                DateTime actualValue = (DateTime)response.Program.Modules[0].Actions[0].AttrStartDate;
                Assert.AreEqual(expectedValue, actualValue.Date);
            }

            [TestMethod()]
            [TestCategory("NIGHT-920")]
            [TestProperty("TFS", "6099")]
            public void GetActionIndividualAttributes_AttrEndDate_Test()
            {
                DateTime expectedValue = DateTime.UtcNow.AddDays(10).Date;
                StubPlanElementEndpointUtils peu = new StubPlanElementEndpointUtils { Client = new StubJsonRestClient() };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest();
                GetProgramDetailsSummaryResponse response = peu.RequestPatientProgramDetailsSummary(request);

                DateTime actualValue = (DateTime)response.Program.Modules[0].Actions[0].AttrEndDate;
                Assert.AreEqual(expectedValue, actualValue.Date);
            }


            [TestMethod()]
            [TestCategory("NIGHT-924")]
            [TestProperty("TFS", "6108")]
            public void GetActionObjectives_Value_Test()
            {
                string expectedValue = "5325da08d6a4850adcbba50e";
                StubPlanElementEndpointUtils peu = new StubPlanElementEndpointUtils { Client = new StubJsonRestClient() };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest();
                GetProgramDetailsSummaryResponse response = peu.RequestPatientProgramDetailsSummary(request);

                string actualValue = response.Program.Modules[0].Actions[0].Objectives[0].Id;
                Assert.AreEqual(expectedValue, actualValue);
            }

            [TestMethod()]
            [TestCategory("NIGHT-924")]
            [TestProperty("TFS", "6108")]
            public void GetActionObjectives_Test()
            {
                StubPlanElementEndpointUtils peu = new StubPlanElementEndpointUtils { Client = new StubJsonRestClient() };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest();
                GetProgramDetailsSummaryResponse response = peu.RequestPatientProgramDetailsSummary(request);
                Assert.IsNotNull(response.Program.Modules[0].Actions[0].Objectives[0]);
            }
        }

        [TestClass()]
        public class UpdatePatientProblem_Method
        {
            [TestMethod()]
            public void Active_Is_True_Test()
            {
                string patientId = "5325da03d6a4850adcbba4fe";
                string userId = "531f2df9072ef727c4d2a3df";
                string elementId = "5346c582d6a48504b44b4f77";
                PatientObservation pod = new PatientObservation { Id = elementId, StateId = 2 };
                bool _active = true;
                IAppDomainRequest request = new GetActiveProgramsRequest { UserId = userId, Context = "NG", ContractNumber = "InHealth001", Version = 1.0 };

                PutUpdateObservationDataResponse result = PlanElementEndpointUtil.UpdatePatientProblem(patientId, userId, elementId, pod, _active, request);

                Assert.IsTrue(result.Result);
            }
        }

        [TestClass()]
        public class PutNewPatientProblem_Method
        {
            [TestMethod()]
            public void New_Patient_Problem_Test()
            {
                string patientId = "5325da03d6a4850adcbba4fe";
                string userId = "531f2df9072ef727c4d2a3df";
                string elementId = "533ed16cd4332307bc592baa";
                PatientObservation pod = new PatientObservation { Id = elementId, StateId = 2 };
                bool _active = true;
                IAppDomainRequest request = new GetActiveProgramsRequest { UserId = userId, Context = "NG", ContractNumber = "InHealth001", Version = 1.0 }; // request object is arbitrary. use any.

                PutRegisterPatientObservationResponse result = PlanElementEndpointUtil.PutNewPatientProblem(patientId, userId, elementId, request);

                //Assert.IsTrue(result.Result);
            }
        }

        [TestClass()]
        public class GetPatientProblem_Method
        {
            [TestMethod()]
            public void Get_Patient_Problem_Not_Null_Test()
            {
                PlanElementEventArg e = new PlanElementEventArg();
                PostProcessActionRequest dr = new PostProcessActionRequest
                {
                    ContractNumber = "InHealth001",
                    Version = 1.0,
                    PatientId = "5325db5ed6a4850adcbba912"
                };

                e.DomainRequest = dr as IAppDomainRequest;
                e.PatientId = "5325db5ed6a4850adcbba912";

                PatientObservation po = PlanElementEndpointUtil.GetPatientProblem("533ed16dd4332307bc592baf", e, "000000000000000000000000");
                Assert.IsNotNull(po);
            }
        }
    }
}
