using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.Allergy;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.AppDomain.NG.Test
{
    [TestClass]
    public class Data_PatientAllergy_Test
    {
        string context = "NG";
        string contractNumber = "InHealth001";
        string userId = "000000000000000000000000";
        double version = 1.0;
        string url = "http://localhost:888/Nightingale";
        IRestClient client = new JsonServiceClient();
        string token = "5453c51984ac0510a8437c85";

        [TestMethod]
        public void GetPatientAllergies_Test()
        {
            GetPatientAllergiesRequest request = new GetPatientAllergiesRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                PatientId = "54087f43d6a48509407d69cb",
                StatusIds = new List<int>{1},
                TypeIds = new List<string> { "5447d6ddfe7a59146485b512", "5446db5efe7a591e74013b6b" },
                UserId = userId,
                Version = version
            };

            JsonServiceClient.HttpWebRequestFilter = x =>
                x.Headers.Add(string.Format("{0}: {1}", "Token", token));

            //[Route("/{Version}/{ContractNumber}/PatientAllergy/{PatientId}", "GET")]
            GetPatientAllergiesResponse response = client.Post<GetPatientAllergiesResponse>(
    string.Format("{0}/{1}/{2}/PatientAllergy/{3}", url, version, contractNumber, request.PatientId), request);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void InitializePatientAllergy_Test()
        {
           PostInitializePatientAllergyRequest request = new PostInitializePatientAllergyRequest {
                AllergyId = "54489a3efe7a59146485bb05",
                Context = context,
                ContractNumber = contractNumber,
                PatientId = "54087f43d6a48509407d69cb",
                UserId = userId,
                Version = version
            };

           JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("{0}: {1}", "Token", token));
            //[Route("/{Version}/{ContractNumber}/PatientAllergy/{PatientId}/Initialize", "GET")]
            PostInitializePatientAllergyResponse response = client.Post<PostInitializePatientAllergyResponse>(
    string.Format("{0}/{1}/{2}/PatientAllergy/Initialize", url, version, contractNumber), request);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void UpdatePatientAllergy_Test()
        {

            PatientAllergy data = new PatientAllergy
            {
                AllergyId = "54489a3efe7a59146485bb05",
                EndDate = DateTime.UtcNow,
                Id = "5452b4b7d4332303e459f08d",
                Notes = "AAAAAAAAA",
                PatientId = "54087f43d6a48509407d69cb",
                ReactionIds = new List<string> { "54494b5ad433232a446f7323" },
               // SeverityId = "54494a96d433232a446f7313",
                SourceId = "544e9976d433231d9c0330ae",
                StartDate = DateTime.UtcNow,
                StatusId = 2,
                SystemName = "Integration",
                UpdatedOn = DateTime.UtcNow
            };

            PostPatientAllergyRequest request = new PostPatientAllergyRequest
            {
                ContractNumber = contractNumber,
                PatientAllergy = data,
                UserId = userId,
                Version = version
            };
            JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("{0}: {1}", "Token", token));
            // [Route("/{Version}/{ContractNumber}/PatientAllergy/Update/Single", "POST")]
            PostPatientAllergyResponse response = client.Post<PostPatientAllergyResponse>(
                string.Format("{0}/{1}/{2}/PatientAllergy/Update/Single", url, version, contractNumber), request);
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void UpdatePatientAllergies_Test()
        {

            List<PatientAllergy> data = new System.Collections.Generic.List<PatientAllergy>();

            PatientAllergy p1 = new PatientAllergy
            {
                AllergyId = "54489a7afe7a59146485bd28",
                AllergyName = "Cat dander",
                EndDate = DateTime.UtcNow,
                Id = "5453c56384ac0510a8596c50",
              //  Notes = "AAAAAAAAA",
                PatientId = "53ed25aed433232e6c26182c",
              //  ReactionIds = new List<string> { "54494b5ad433232a446f7323" },
                SeverityId = "54494a8fd433232a446f7311",
              //  SourceId = "544e9976d433231d9c0330ae",
                StartDate = DateTime.UtcNow,
                StatusId = 1,
                SystemName = "XXXXXX",
                UpdatedOn = DateTime.UtcNow,
                DeleteFlag = false,
              
            };

            //PatientAllergy p2 = new PatientAllergy
            //{
            //    AllergyId = "54489a79fe7a59146485bd1e",
            //    EndDate = DateTime.UtcNow,
            //    Id = "5452584ed4332305d8fa10b5",
            //    Notes = "BBBBBBBBBB",
            //    PatientId = "54087f43d6a48509407d69cb",
            //    ReactionIds = new List<string> { "54494b5ad433232a446f7323" },
            //    SeverityId = "54494a96d433232a446f7313",
            //    SourceId = "544e9976d433231d9c0330ae",
            //    StartDate = DateTime.UtcNow,
            //    StatusId = 1,
            //    SystemName = "ZZZZZZ",
            //    UpdatedOn = DateTime.UtcNow
            //};
            data.Add(p1);
         //   data.Add(p2);
            PostPatientAllergiesRequest request = new PostPatientAllergiesRequest
            {
                ContractNumber = contractNumber,
                PatientAllergies = data,
                UserId = userId,
                Version = version
            };

            JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("{0}: {1}", "Token", token));
            //[Route("/{Version}/{ContractNumber}/PatientAllergy/Update/Bulk", "POST")]
            PostPatientAllergiesResponse response = client.Post<PostPatientAllergiesResponse>(
                string.Format("{0}/{1}/{2}/PatientAllergy/Update/Bulk", url, version, contractNumber), request);
            Assert.IsNotNull(response);
        }
    }
}
