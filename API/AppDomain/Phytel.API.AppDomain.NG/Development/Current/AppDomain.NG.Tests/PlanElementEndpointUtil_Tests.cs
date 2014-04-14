using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO.Observation;
using Phytel.API.Interface;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.PatientObservation.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;
namespace Phytel.API.AppDomain.NG.Tests
{
    [TestClass()]
    public class PlanElementEndpointUtil_Tests
    {
        [TestMethod()]
        public void UpdatePatientProblem_Test()
        {
            string patientId = "5325da03d6a4850adcbba4fe";
            string userId = "531f2df9072ef727c4d2a3df";
            string elementId = "5346c582d6a48504b44b4f77";
            PatientObservation pod = new PatientObservation { Id = elementId, StateId = 2 };
            bool _active = true;
            IAppDomainRequest request = new GetActiveProgramsRequest { UserId = userId, Context = "NG", ContractNumber="InHealth001", Version = 1.0 };

            PutUpdateObservationDataResponse result = PlanElementEndpointUtil.UpdatePatientProblem(patientId, userId, elementId, pod, _active, request);

            Assert.IsTrue(result.Result);
        }

        [TestMethod()]
        public void PutNewPatientProblem_Test()
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

        [TestMethod()]
        public void GetPatientProblem_Test()
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
