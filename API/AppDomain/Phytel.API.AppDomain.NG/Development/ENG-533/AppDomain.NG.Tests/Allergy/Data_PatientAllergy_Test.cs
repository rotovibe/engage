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
    //[TestClass]
    public class Data_PatientAllergy_Test
    {
        string context = "NG";
        string contractNumber = "InHealth001";
        string userId = "000000000000000000000000";
        double version = 1.0;
        string url = "http://localhost:888/Nightingale";
        IRestClient client = new JsonServiceClient();
        string token = "546bac6f60e4b90c7839b9eb";

        [TestMethod]
        public void GetPatientAllergies_Test()
        {
            GetPatientAllergiesRequest request = new GetPatientAllergiesRequest
            {
                ContractNumber = contractNumber,
                PatientId = "534685c160e4b90f8c8966a8",
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
        public void UpdatePatientAllergies_Test()
        {

            List<PatientAllergy> data = new System.Collections.Generic.List<PatientAllergy>();

            PatientAllergy p1 = new PatientAllergy
            {
                AllergyId = "54580a9b84ac05021485f632",
                IsNewAllergy = true,
                //AllergyName = "Cat dander",
                EndDate = DateTime.UtcNow,
                Id = "54580a9d84ac05021485f637",
                Notes = "AAAAAAAAA",
                PatientId = "5325daebd6a4850adcbba7be",
                ReactionIds = new List<string> { "54494b5ad433232a446f7323" },
                SeverityId = "54494a8fd433232a446f7311",
                SourceId = "544e9976d433231d9c0330ae",
                StartDate = DateTime.UtcNow,
                StatusId = 1,
                SystemName = "Engage",
                UpdatedOn = DateTime.UtcNow,
                DeleteFlag = false,
                AllergyTypeIds = new List<string> { "5447d6ddfe7a59146485b512", "5446db5efe7a591e74013b6b", "5446db5efe7a591e74013b6c" },
              
            };

            //PatientAllergy p2 = new PatientAllergy
            //{
            //    AllergyId = "5453e6bfd433230468567d33",
            //    IsNewAllergy = true,
            //    //AllergyName = "Cat dander",
            //    EndDate = DateTime.UtcNow,
            //    Id = "5453e7eb84ac0510a8f3ba88",
            //    Notes = "BBBBBBB",
            //    PatientId = "5325da1fd6a4850adcbba54a",
            //    //ReactionIds = new List<string> { "54494b5ad433232a446f7323" },
            //    //SeverityId = "54494a8fd433232a446f7311",
            //    UtilizationSourceId = "544e9976d433231d9c0330ae",
            //    StartDate = DateTime.UtcNow,
            //    StatusId = 2,
            //    SystemName = "Integration",
            //    UpdatedOn = DateTime.UtcNow,
            //    DeleteFlag = false,
            //    AllergyTypeIds = new List<string> { "5446db5efe7a591e74013b6d" },
            //};
            data.Add(p1);
            //data.Add(p2);
            PostPatientAllergiesRequest request = new PostPatientAllergiesRequest
            {
                ContractNumber = contractNumber,
                PatientAllergies = data,
                UserId = userId,
                Version = version
            };

            JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("{0}: {1}", "Token", token));
            //[Route("/{Version}/{ContractNumber}/PatientAllergy/Update", "POST")]
            PostPatientAllergiesResponse response = client.Post<PostPatientAllergiesResponse>(
                string.Format("{0}/{1}/{2}/PatientAllergy/Update", url, version, contractNumber), request);
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void DeletePatientAllergy_Test()
        {
            DeletePatientAllergyRequest request = new DeletePatientAllergyRequest
            {
                ContractNumber = contractNumber,
                Id = "54ef51b784ac050d0c615d6b",
                PatientId = "5325db24d6a4850adcbba85a",
                UserId = userId,
                Version = version
            };

            //[Route("/{Version}/{ContractNumber}/Patient/{PatientId}/PatientAllergy/{Id}", "DELETE")]
            DeletePatientAllergyResponse response = client.Delete<DeletePatientAllergyResponse>(
                string.Format("{0}/{1}/{2}/Patient/{3}/PatientAllergy/{4}?UserId={5}", url, version, contractNumber, request.PatientId, request.Id, request.UserId));
            Assert.IsNotNull(response);
        }
    }
}
