using System;
using System.Collections.Generic;
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


        [TestMethod]
        public void GetCurrentPatientObservation_Test()
        {
            GetCurrentPatientObservationsRequest request = new GetCurrentPatientObservationsRequest { ContractNumber = contract, PatientId = "5323762f231e250d5c0c62a7", UserId = userId, Version = version };

            ObservationsManager oManager = new ObservationsManager();
            GetCurrentPatientObservationsResponse response = oManager.GetCurrentPatientObservations(request);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void UpdatePatientObservation_Test()
        {
            List<PatientObservation> po = new List<PatientObservation>();
            po.Add(new PatientObservation {
                DeleteFlag = true,
                DisplayId = 1,
                EndDate = null,
                GroupId = null,
                Id = "53e907dcd6a4850bc4f863ba",
                Name = "GERD",
                ObservationId = "533ed16ed4332307bc592bb8",
                PatientId = "5325db15d6a4850adcbba82a",
                Source = "CM",
                Standard = false, 
                StartDate = DateTime.Now,
                StateId = 2,
                TypeId = "533d8278d433231deccaa62d",
                Units = null,
                Values = null
            });

            PostUpdateObservationItemsRequest request = new PostUpdateObservationItemsRequest { ContractNumber = contract, PatientId = "5325db15d6a4850adcbba82a", UserId = userId, Version = version, Context = "NG", PatientObservations = po };

            ObservationsManager oManager = new ObservationsManager();
            PostUpdateObservationItemsResponse response = oManager.SavePatientObservations(request);

            Assert.IsNotNull(response);
        }
    }
}
