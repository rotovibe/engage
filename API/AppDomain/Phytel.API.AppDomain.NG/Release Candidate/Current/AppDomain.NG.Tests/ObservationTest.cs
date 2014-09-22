using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.Observation;

namespace Phytel.API.AppDomain.NG.Test
{
    [TestClass]
    public class ObservationTest
    {
        string userId = "000000000000000000000000";
        string contract = "InHealth001";
        int version = 1;

        [TestMethod]
        public void GetAllowedStates_CorrectType_Test()
        {
            GetAllowedStatesRequest request = new GetAllowedStatesRequest { ContractNumber = contract, Token = "", UserId = userId, Version = version };

            ObservationsManager oManager = new ObservationsManager();
            GetAllowedStatesResponse response = oManager.GetAllowedObservationStates(request);

            Assert.IsNotNull(response.States);
        }

        [TestMethod]
        public void GetAllowedStates_InCorrectType_Test()
        {
            GetAllowedStatesRequest request = new GetAllowedStatesRequest { ContractNumber = contract, Token = "",  UserId = userId, Version = version };

            ObservationsManager oManager = new ObservationsManager();
            GetAllowedStatesResponse response = oManager.GetAllowedObservationStates(request);

            Assert.IsNull(response.States);
        }
    }
}
