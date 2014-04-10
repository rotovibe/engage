using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.Observation;

namespace Phytel.API.AppDomain.NG.Test
{
    [TestClass]
    public class PatientObservationTest
    {
        static string userId = "000000000000000000000000";
        static string contract = "InHealth001";
        static int version = 1;
        
        [TestMethod]
        public void GetInitializeProblem_Test()
        {
            GetInitializeProblemRequest request = new GetInitializeProblemRequest { ContractNumber = contract, Token = "", PatientId = "5325da03d6a4850adcbba4fe", ObservationId = "533ed16dd4332307bc592bae", UserId = userId, Version = version };

            ObservationsManager oManager = new ObservationsManager();
            GetInitializeProblemResponse response = oManager.GetInitializeProblem(request);

            Assert.IsNotNull(response.PatientObservation);
        }
    }
}
