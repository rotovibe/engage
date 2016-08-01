using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.Test.Stubs;
using Phytel.API.AppDomain.NG.DTO;

namespace Phytel.API.AppDomain.NG.Test.Patient
{
    /// <summary>
    /// This class tests all the methods that are related to Contact domain in INGManager class.
    /// </summary>
    [TestClass]
    public class Contact_Test
    {
        string userId = "5325c821072ef705080d3488";
        string contactId = "5325c821072ef705080d3488";
        string contractNumber = "InHealth001";
        double version = 1.0;
        string context = "NG";

        [TestMethod()]
        [TestCategory("NIGHT-911")]
        [TestProperty("TFS", "10417")]
        public void GetRecentPatientsForAContact_Test()
        {
            INGManager ngm = new StubNGManager(); // { PlanElementUtils = new StubPlanElementUtils(), EndPointUtils = new StubPlanElementEndpointUtils() };

            GetRecentPatientsRequest request = new GetRecentPatientsRequest
            {
                Version = version,
                ContractNumber = contractNumber,
                UserId = userId,
                Context = context,
                ContactId = contactId
            };

            GetRecentPatientsResponse response = ngm.GetRecentPatients(request);
            Assert.AreEqual(response.ContactId, request.ContactId);
        }

        [TestMethod()]
        [TestCategory("NIGHT-911")]
        [TestProperty("TFS", "10417")]
        public void GetRecentPatients_Limit_Test()
        {
            INGManager ngm = new StubNGManager(); // { PlanElementUtils = new StubPlanElementUtils(), EndPointUtils = new StubPlanElementEndpointUtils() };

            GetRecentPatientsRequest request = new GetRecentPatientsRequest
            {
                Version = version,
                ContractNumber = contractNumber,
                UserId = userId,
                Context = context,
                ContactId = contactId
            };

            GetRecentPatientsResponse response = ngm.GetRecentPatients(request);
            Assert.IsTrue(response.Limit >= response.Patients.Count);
        }

        [TestMethod()]
        public void InitializePatient_Test()
        {
            INGManager ngm = new NGManager();

            GetInitializePatientRequest request = new GetInitializePatientRequest
            {
                Version = version,
                ContractNumber = contractNumber,
                UserId = userId,
                Context = context
            };

            GetInitializePatientResponse response = ngm.GetInitializePatient(request);
            Assert.IsNotNull(response);
        }

        [TestMethod()]
        public void SavePatient_Test()
        {
            INGManager ngm = new NGManager();

            Phytel.API.AppDomain.NG.DTO.Patient p = new DTO.Patient {
                Id = "53ee51f8d433230bb8a9172d",
                FirstName = "farah",
                MiddleName = "f",
                LastName = "kahn",
                PreferredName = "farah kahn",
                Suffix = "IV",
                DOB = "02/07/1950",
                FullSSN = "333333333",
                Gender = "F",
                Priority = 3
            };

            PutPatientDetailsUpdateRequest request = new PutPatientDetailsUpdateRequest
            {
                Version = version,
                ContractNumber = contractNumber,
                UserId = userId,
                Token = string.Empty,
                Patient = p,
                Insert = true
            };

            PutPatientDetailsUpdateResponse response = ngm.UpsertPatient(request);
            Assert.IsNotNull(response);
        }
    }
}
