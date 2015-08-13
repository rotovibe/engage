using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.Test.Stubs;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using Phytel.API.AppDomain.NG.DTO.Internal;

namespace Phytel.API.AppDomain.NG.Test.PatientSystem
{
    /// <summary>
    /// This class tests all the methods that are related to Contact domain in INGManager class.
    /// </summary>
    [TestClass]
    public class PatientSystem_Test
    {
        string context = "NG";
        string contractNumber = "InHealth001";
        string userId = "000000000000000000000000";
        double version = 1.0;
        string url = "http://localhost:888/Nightingale";
        IRestClient client = new JsonServiceClient();
        string token = "55b003a6231e250f18f6b056";

        [TestMethod]
        public void GetPatientSystems_Test()
        {
            GetPatientSystemsRequest request = new GetPatientSystemsRequest
            {
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version,
                Token = token,
                PatientId = "5325dacad6a4850adcbba756"
            };

            JsonServiceClient.HttpWebRequestFilter = x =>
                x.Headers.Add(string.Format("{0}: {1}", "Token", token));

            //[Route("/{Version}/{ContractNumber}/Patient/{PatientId}PatientSystems", "GET")]
            GetPatientSystemsResponse response = client.Get<GetPatientSystemsResponse>(string.Format("{0}/{1}/{2}/Patient/{3}/PatientSystems", url, version, contractNumber, request.PatientId));
            Assert.IsNotNull(response);
        }

        
        [TestMethod]
        public void UpdatePatientsAndSystems_Test()
        {
            UpdatePatientsAndSystemsRequest request = new UpdatePatientsAndSystemsRequest
            {
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version,
                Token = token
            };

            JsonServiceClient.HttpWebRequestFilter = x =>
                x.Headers.Add(string.Format("{0}: {1}", "Token", token));

            //[Route("/{Version}/{ContractNumber}/Internal/PatientSystems/", "GET")]
            UpdatePatientsAndSystemsResponse response = client.Get<UpdatePatientsAndSystemsResponse>(string.Format("{0}/{1}/{2}/Internal/PatientSystems?UserId={3}", url, version, contractNumber, request.UserId));
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void InsertPatientSystems_Test()
        {
            List<DTO.PatientSystem> list = new List<DTO.PatientSystem>();
            list.Add(new DTO.PatientSystem { PatientId = "5325dacad6a4850adcbba756", Value = " 79876 ", StatusId = 1, Primary = false, SystemId = "559e8c70d4332320bc076f4f", DataSource = "Engage" });
            list.Add(new DTO.PatientSystem { PatientId = "5325dacad6a4850adcbba756", Value = " fkjhsdkfjhsdkfhksdjfhskdfyhsdfhjfhjkdddddddddddddddddddddddddduyyyyyyyyyyyyyyyyyyyhhhhhhhhhhhhhhhhhhhhhhhiiiiiiiiiiiiiiiiioooooooooossssssssssssss ", StatusId = 2, Primary = false, SystemId = "559e8c70d4332320bc076f4e", DataSource = "Import" });
            
            InsertPatientSystemsRequest request = new InsertPatientSystemsRequest
            {
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version,
                Token = token,
                PatientId = "5325dacad6a4850adcbba756",
                PatientSystems = list
            };

            JsonServiceClient.HttpWebRequestFilter = x =>
                x.Headers.Add(string.Format("{0}: {1}", "Token", token));

            //[Route("/{Version}/{ContractNumber}/Patient/{PatientId}/PatientSystems", "POST")]
            InsertPatientSystemsResponse response = client.Post<InsertPatientSystemsResponse>(string.Format("{0}/{1}/{2}/Patient/{3}/PatientSystems", url, version, contractNumber, request.PatientId), request as object);
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void UpdatePatientSystems_Test()
        {
            List<DTO.PatientSystem> list = new List<DTO.PatientSystem>();
            list.Add(new DTO.PatientSystem { Id = "55a82b5ad433263860362072", PatientId = "5325dacad6a4850adcbba756", Value = " aaaaaaaaaaaaaaaaaaaaaaaaaaabbbbbbbbbbbbbbbbbbbbbbbbbccccccccccccccccccccccccdddddddddddddddddddddeeeeeeeeeeeeeeeeeeeeeeefffffffffffffffffffffgggggggggggggggggghhhhhhhhhhhhhhhiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiijjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjkkkkkkkkkkkkkkkkkkkkkkkkkkkllllllllllllllllllllllllllnnnnnnnnnnnnnnnnnnnmmmmmmmmmmmmmmmmmmmmmmm ", StatusId = 2, Primary = false, SystemId = "559e8c70d4332320bc076f4e", DataSource = "Engage_Up" });
            list.Add(new DTO.PatientSystem { Id = "55a82b6ad43326386036207a", PatientId = "5325dacad6a4850adcbba756", Value = " zGHGEGGH  ", StatusId = 2, Primary = false, DataSource = "Import_Up", SystemId = "559e8c70d4332320bc076f4d" });

            UpdatePatientSystemsRequest request = new UpdatePatientSystemsRequest
            {
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version,
                Token = token,
                PatientId = "5325dacad6a4850adcbba756",
                PatientSystems = list
            };

            JsonServiceClient.HttpWebRequestFilter = x =>
                x.Headers.Add(string.Format("{0}: {1}", "Token", token));

            //[Route("/{Version}/{ContractNumber}/Patient/{PatientId}PatientSystems", "PUT")]
            UpdatePatientSystemsResponse response = client.Put<UpdatePatientSystemsResponse>(string.Format("{0}/{1}/{2}/Patient/{3}/PatientSystems", url, version, contractNumber, request.PatientId), request  as object);
            Assert.IsNotNull(response);
        }


        [TestMethod]
        public void DeletePatientSystems_Test()
        {
            DeletePatientSystemsRequest request = new DeletePatientSystemsRequest
            {
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version,
                Token = token,
                PatientId = "5325da9ed6a4850adcbba6ce",
                Ids = "55a038f7d43325251c8fbdb8,55a038f8d43325251c8fbdbf"
            };

            JsonServiceClient.HttpWebRequestFilter = x =>
                x.Headers.Add(string.Format("{0}: {1}", "Token", token));

            //[Route("/{Version}/{ContractNumber}/Patient/{PatientId}PatientSystems/{Ids}", "DELETE")]
            DeletePatientSystemsResponse response = client.Delete<DeletePatientSystemsResponse>(string.Format("{0}/{1}/{2}/Patient/{3}/PatientSystems/{4}", url, version, contractNumber, request.PatientId, request.Ids));
            Assert.IsNotNull(response);
        }
    }
}
