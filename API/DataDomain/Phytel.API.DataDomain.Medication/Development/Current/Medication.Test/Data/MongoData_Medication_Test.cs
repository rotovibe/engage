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
    [TestClass]
    public class MongoData_Medication_Test
    {
        string context = "NG";
        string contractNumber = "InHealth001";
        string userId = "000000000000000000000000";
        double version = 1.0;
        string url = "http://localhost:8888/Medication";
        IRestClient client = new JsonServiceClient();

        [TestMethod]
        public void InitializeMedication_Test()
        {
            PutInitializeMedSuppDataRequest request = new PutInitializeMedSuppDataRequest {
                MedSuppName = "TestMed",
                Context = context,
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version
            };
            //[Route("/{Context}/{Version}/{ContractNumber}/Allergy/Initialize", "PUT")]
            PutInitializeMedSuppDataResponse response = client.Put<PutInitializeMedSuppDataResponse>(
    string.Format("{0}/{1}/{2}/{3}/MedSupp/Initialize", url, context, version, contractNumber), request);

            Assert.IsNotNull(response);
        }


        [TestMethod]
        public void UpdateMedication_Test()
        {

            MedicationData data = new MedicationData
            {
                DeleteFlag = false,
                Id = "54adb4ecd4332324dc0c77a3",
                Version = 1.0
            };

            PutMedicationDataRequest request = new PutMedicationDataRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                MedicationData = data,
                UserId = userId,
                Version = version
            };

            //[Route("/{Context}/{Version}/{ContractNumber}/Medication/Update", "PUT")]
            PutMedicationDataResponse response = client.Put<PutMedicationDataResponse>(
                string.Format("{0}/{1}/{2}/{3}/Medication/Update", url, context, version, contractNumber), request);
            Assert.IsNotNull(response);
        }
    }
}
