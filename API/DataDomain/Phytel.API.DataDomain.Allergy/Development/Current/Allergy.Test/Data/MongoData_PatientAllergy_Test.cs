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
    public class MongoData_PatientAllergy_Test
    {
        string context = "NG";
        string contractNumber = "InHealth001";
        string userId = "000000000000000000000000";
        double version = 1.0;
        string url = "http://localhost:8888/Allergy";
        IRestClient client = new JsonServiceClient();
            
       [TestMethod]
        public void GetAllergyByID()
        {
            //GetAllergyRequest request = new GetAllergyRequest{ AllergyID = "5"};

            //GetAllergyResponse response = new StubAllergyDataManager().GetAllergyByID(request);

            //Assert.IsTrue(response.DdAllergy.AllergyID == "??");
        }



       //[TestClass]
       //public class PatientNote_Test
       //{
       //    [TestMethod]
       //    public void InsertPatientNote_Test()
       //    {
       //        string url = "http://localhost:8888/PatientNote";
       //        PatientNoteData note = new PatientNoteData { Text = "DD_Service test note 2", CreatedById = "53043e53d433231f48de8a7a", PatientId = "52f55877072ef709f84e69b0" };
       //        string contractNumber = "InHealth001";
       //        string context = "NG";
       //        double version = 1.0;
       //        IRestClient client = new JsonServiceClient();
       //        JsonServiceClient.HttpWebRequestFilter = x =>
       //                        x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

       //        //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Note/Insert", "PUT")]
       //        PutPatientNoteDataResponse response = client.Put<PutPatientNoteDataResponse>(
       //            string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Note/Insert", url, context, version, contractNumber, note.PatientId),
       //            new PutPatientNoteDataRequest { Context = context, ContractNumber = contractNumber, PatientId = "52f55877072ef709f84e69b0", PatientNote = note, UserId = "53043e53d433231f48de8a7a", Version = version } as object);

       //        Assert.IsNotNull(response.Id);
       //    }
       //}


       [TestMethod]
       public void GetPatientAllergies_Test()
       {
           GetPatientAllergiesDataRequest request = new GetPatientAllergiesDataRequest
           {
               Context = context,
               ContractNumber = contractNumber,
               PatientId = "54087f43d6a48509407d69cb",
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
        public void UpdatePatientAllergy_Test()
        {

            PatientAllergyData data = new PatientAllergyData {
                AllergyId = "54489a3dfe7a59146485bafe",
                EndDate = DateTime.UtcNow,
                Id = "5452b3f1d4332303e459f08a",
                Notes = "AAAAAAAAAAAAAAA",
                PatientId = "54087f43d6a48509407d69cb",
                ReactionIds = new List<string>{"54494b5ad433232a446f7323"},
               // SeverityId = "54494a96d433232a446f7313",
                SourceId = "544e9976d433231d9c0330ae",
                StartDate = DateTime.UtcNow,
                StatusId = 1,
                SystemName = "Integration",
                UpdatedOn  = DateTime.UtcNow 
            };
            
            PutPatientAllergyDataRequest request = new PutPatientAllergyDataRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                PatientAllergyData = data,
                UserId = userId,
                Version = version
            };

            // [Route("/{Context}/{Version}/{ContractNumber}/PatientAllergy/Update/Single", "PUT")]
            PutPatientAllergyDataResponse response = client.Put<PutPatientAllergyDataResponse>(
                string.Format("{0}/{1}/{2}/{3}/PatientAllergy/Update/Single", url, context, version, contractNumber), request);
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
    }
}
