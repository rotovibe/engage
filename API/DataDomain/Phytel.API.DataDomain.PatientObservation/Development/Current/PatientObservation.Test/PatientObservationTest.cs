using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientObservation.DTO;

namespace Phytel.API.DataDomain.PatientObservation.Test
{
    [TestClass]
    public class PatientObservationTest
    {
        static string userId = "000000000000000000000000";
        static string contractNumber = "InHealth001";
        static string context = "NG";
        static int version = 1;
        
        [TestMethod]
        public void InitializePatientProblem()
        {
            GetInitializeProblemDataRequest request = new GetInitializeProblemDataRequest { Context = context, ContractNumber = contractNumber, ObservationId = "533ed16ed4332307bc592bb9", PatientId = "5325db00d6a4850adcbba802", UserId = userId, Version = version };

            GetInitializeProblemDataResponse response = PatientObservationDataManager.GetInitializeProblem(request);

            Assert.IsNotNull(response.PatientObservation);
        }
    }
}
