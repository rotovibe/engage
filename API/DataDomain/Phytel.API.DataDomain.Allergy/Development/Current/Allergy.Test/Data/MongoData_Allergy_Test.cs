using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Allergy.DTO;
using Phytel.API.DataDomain.Allergy.Test.Stubs;
using DataDomain.Allergy.Repo;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.Allergy.Test
{
    [TestClass]
    public class MongoData_Allergy_Test
    {
        string context = "NG";
        string contractNumber = "InHealth001";
        string userId = "000000000000000000000000";
        double version = 1.0;
        string url = "http://localhost:8888/Allergy";
        IRestClient client = new JsonServiceClient();

   //    [TestMethod]
   //    public void GetPatientAllergies_Test()
   //    {
   //        GetPatientAllergiesDataRequest request = new GetPatientAllergiesDataRequest
   //        {
   //            Context = context,
   //            ContractNumber = contractNumber,
   //            PatientId = "54087f43d6a48509407d69cb",
   //            UserId = userId,
   //            Version = version
   //        };
   //        //[Route("/{Context}/{Version}/{ContractNumber}/PatientAllergy/{PatientId}", "GET")]
   //        GetPatientAllergiesDataResponse response = client.Post<GetPatientAllergiesDataResponse>(
   //string.Format("{0}/{1}/{2}/{3}/PatientAllergy/{4}", url, context, version, contractNumber, request.PatientId), request);

   //        Assert.IsNotNull(response);
   //    }

        [TestMethod]
        public void InitializeAllergy_Test()
        {
            PutInitializeAllergyDataRequest request = new PutInitializeAllergyDataRequest {
                AllergyName = "testing name",
                Context = context,
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version
            };
            //[Route("/{Context}/{Version}/{ContractNumber}/Allergy/Initialize", "PUT")]
            PutInitializeAllergyDataResponse response = client.Put<PutInitializeAllergyDataResponse>(
    string.Format("{0}/{1}/{2}/{3}/Allergy/Initialize", url, context, version, contractNumber), request);

            Assert.IsNotNull(response);
        }
    }
}
