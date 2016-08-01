using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Medication.DTO;
using Phytel.API.DataDomain.Medication.Test;
using DataDomain.Medication.Repo;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.Allergy.Test
{
    //[TestClass]
    public class Data_Medication_Test
    {
        string context = "NG";
        string contractNumber = "InHealth001";
        string userId = "000000000000000000000000";
        double version = 1.0;
        string url = "http://localhost:8888/Medication";
        IRestClient client = new JsonServiceClient();

        [TestMethod]
        public void InitializeMedicationMap_Test()
        {
            PutInitializeMedicationMapDataRequest request = new PutInitializeMedicationMapDataRequest {
                MedicationMapData = new MedicationMapData { FullName  = "testname"},
                Context = context,
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version
            };
            //[Route("/{Context}/{Version}/{ContractNumber}/MedicationMap/Initialize", "PUT")]
            PutInitializeMedicationMapDataResponse response = client.Put<PutInitializeMedicationMapDataResponse>(
    string.Format("{0}/{1}/{2}/{3}/MedicationMap/Initialize", url, context, version, contractNumber), request);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void GetMedicationMap_Test()
        {
            GetMedicationMapDataRequest request = new GetMedicationMapDataRequest
            {
                Name = "ADVIL",
                Context = context,
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version
            };
            //[Route("/{Context}/{Version}/{ContractNumber}/MedicationMap/{Name}", "POST")]
            GetMedicationMapDataResponse response = client.Get<GetMedicationMapDataResponse>(string.Format("{0}/{1}/{2}/{3}/MedicationMap/{4}", url, context, version, contractNumber, request.Name));
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void UpdateMedicationMap_Test()
        {
            MedicationMapData data = new MedicationMapData
            {
                Id = "54b82870d433262acc525a17",
                FullName = "testname123",
                SubstanceName = "67894",
                Verified = true
            };

            PutMedicationMapDataRequest request = new PutMedicationMapDataRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                MedicationMapData = data,
                UserId = userId,
                Version = version
            };

            //[Route("/{Context}/{Version}/{ContractNumber}/MedicationMap/Update", "PUT")]
            PutMedicationMapDataResponse response = client.Put<PutMedicationMapDataResponse>(
                string.Format("{0}/{1}/{2}/{3}/MedicationMap/Update", url, context, version, contractNumber), request);
            Assert.IsNotNull(response);
        }
    }
}
