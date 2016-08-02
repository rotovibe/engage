using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Allergy.DTO;
using Phytel.API.DataDomain.Allergy.Test;
using DataDomain.Allergy.Repo;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.Allergy.Test
{
    //[TestClass]
    public class Data_PatientAllergy_Test
    {
        string context = "NG";
        string contractNumber = "InHealth001";
        string userId = "000000000000000000000000";
        double version = 1.0;
        string url = "http://localhost:8888/Allergy";
        IRestClient client = new JsonServiceClient();

       [TestMethod]
       public void GetPatientAllergies_Test()
       {
           GetPatientAllergiesDataRequest request = new GetPatientAllergiesDataRequest
           {
               Context = context,
               ContractNumber = contractNumber,
               PatientId = "5458fdef84ac050ea472df8e",
               UserId = userId,
               Version = version
           };
           //[Route("/{Context}/{Version}/{ContractNumber}/PatientAllergy/{PatientId}", "GET")]
           GetPatientAllergiesDataResponse response = client.Post<GetPatientAllergiesDataResponse>(
   string.Format("{0}/{1}/{2}/{3}/PatientAllergy/{4}", url, context, version, contractNumber, request.PatientId), request);

           Assert.IsNotNull(response);
       }

        [TestMethod]
        public void InitializePatientAllergy_Test()
        {
            PutInitializePatientAllergyDataRequest request = new PutInitializePatientAllergyDataRequest {
                AllergyId = "54489a3dfe7a59146485bafe",
                Context = context,
                ContractNumber = contractNumber,
                PatientId = "54087f43d6a48509407d69cb",
                UserId = userId,
                Version = version
            };
            // [Route("/{Context}/{Version}/{ContractNumber}/PatientAllergy/{PatientId}/Initialize", "PUT")]
            PutInitializePatientAllergyDataResponse response = client.Put<PutInitializePatientAllergyDataResponse>(
    string.Format("{0}/{1}/{2}/{3}/PatientAllergy/Initialize", url, context, version, contractNumber), request);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void UpdatePatientAllergies_Test()
        {

            List<PatientAllergyData> data = new System.Collections.Generic.List<PatientAllergyData>();
            
            PatientAllergyData p1 = new PatientAllergyData
            {
                AllergyId = "54489a72fe7a59146485bce5",
                EndDate = DateTime.UtcNow,
                Id = "5452567ed433231b9c516d8e",
                Notes = "first note for patient allergy",
                PatientId = "54087f43d6a48509407d69cb",
                ReactionIds = new List<string> { "54494b5ad433232a446f7323", "54494b5dd433232a446f7324", "54494b60d433232a446f7325" },
                SeverityId = "54494a96d433232a446f7313",
                SourceId = "544e9976d433231d9c0330ae",
                StartDate = DateTime.UtcNow,
                StatusId = 1,
                SystemName = "Engage1",
                UpdatedOn = DateTime.UtcNow
            };

            PatientAllergyData p2 = new PatientAllergyData
            {
                AllergyId = "54489a79fe7a59146485bd1e",
                EndDate = DateTime.UtcNow,
                Id = "5452584ed4332305d8fa10b5",
                Notes = "asdasfddfjskdfjsldfugiosdgjksgj",
                PatientId = "54087f43d6a48509407d69cb",
                ReactionIds = new List<string> { "54494b5ad433232a446f7323" },
                SeverityId = "54494a96d433232a446f7313",
                SourceId = "544e9976d433231d9c0330ae",
                StartDate = DateTime.UtcNow,
                StatusId = 1,
                SystemName = "Engage2",
                UpdatedOn = DateTime.UtcNow
            };
            data.Add(p1);
            data.Add(p2);
            PutPatientAllergiesDataRequest request = new PutPatientAllergiesDataRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                PatientAllergiesData = data,
                UserId = userId,
                Version = version
            };

            //[Route("/{Context}/{Version}/{ContractNumber}/PatientAllergy/Update/Bulk", "PUT")]
            PutPatientAllergiesDataResponse response = client.Put<PutPatientAllergiesDataResponse>(
                string.Format("{0}/{1}/{2}/{3}/PatientAllergy/Update/Bulk", url, context, version, contractNumber), request);
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void DeletePatientAllergies_Test()
        {
            DeleteAllergiesByPatientIdDataRequest request = new DeleteAllergiesByPatientIdDataRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                PatientId = "5458fdef84ac050ea472df8e",
                UserId = userId,
                Version = version
            };

            //[Route("/{Context}/{Version}/{ContractNumber}/PatientAllergy/Patient/{PatientId}/Delete", "DELETE")]
            DeleteAllergiesByPatientIdDataResponse response = client.Delete<DeleteAllergiesByPatientIdDataResponse>(
                string.Format("{0}/{1}/{2}/{3}/PatientAllergy/Patient/{4}/Delete?UserId={5}", url, context, version, contractNumber, request.PatientId, request.UserId));
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void UndoDeletePatientAllergies_Test()
        {
            UndoDeletePatientAllergiesDataRequest request = new UndoDeletePatientAllergiesDataRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                Ids = new List<string> { "545920a184ac05124c984711", "5459271684ac05124ce6862a", "5459281584ac05124c362845", "54593ce684ac05124c3628cf" },
                UserId = userId,
                Version = version
            };

            //[Route("/{Context}/{Version}/{ContractNumber}/PatientAllergy/UndoDelete", "PUT")]
            UndoDeletePatientAllergiesDataResponse response = client.Put<UndoDeletePatientAllergiesDataResponse>(
                string.Format("{0}/{1}/{2}/{3}/PatientAllergy/UndoDelete", url, context, version, contractNumber), request);
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void DeletePatientAllergy_Test()
        {
            DeletePatientAllergyDataRequest request = new DeletePatientAllergyDataRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                Id = "54ef4da084ac050d0c615d5c",
                UserId = userId,
                Version = version
            };

            //[Route("/{Context}/{Version}/{ContractNumber}/PatientAllergy/{Id}", "DELETE")]
            DeletePatientAllergyDataResponse response = client.Delete<DeletePatientAllergyDataResponse>(
                string.Format("{0}/{1}/{2}/{3}/PatientAllergy/{4}?UserId={5}", url, context, version, contractNumber, request.Id, request.UserId));
            Assert.IsNotNull(response);
        }
    }
}
