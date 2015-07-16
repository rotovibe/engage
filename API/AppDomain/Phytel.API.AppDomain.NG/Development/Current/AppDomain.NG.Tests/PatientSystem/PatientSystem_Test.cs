using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.Test.Stubs;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

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
        string token = "54e7970984ac0727006941bd";

        [TestMethod]
        public void GetPatientSystems_Test()
        {
            GetPatientSystemsRequest request = new GetPatientSystemsRequest
            {
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version,
                Token = token,
                PatientId = "5325da9ed6a4850adcbba6ce"
            };

            JsonServiceClient.HttpWebRequestFilter = x =>
                x.Headers.Add(string.Format("{0}: {1}", "Token", token));

            //[Route("/{Version}/{ContractNumber}/Patient/{PatientId}PatientSystems", "GET")]
            GetPatientSystemsResponse response = client.Get<GetPatientSystemsResponse>(string.Format("{0}/{1}/{2}/Patient/{3}/PatientSystems", url, version, contractNumber, request.PatientId));
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void InsertPatientSystems_Test()
        {
            List<DTO.PatientSystem> list = new List<DTO.PatientSystem>();
            list.Add(new DTO.PatientSystem { PatientId = "5325db88d6a4850adcbba98a", Value = " AAAAAAAAAAAAAAAAAAAAAAAAAABBBBBBBBBBBBBBBBBBBBBBBBBBBBBBCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHKKKKKKKKKKKKKKKKKKKKKKKKKKKKLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMNNNNNNNNNNNNNNNNNNNNNNNNNOOOOOOOOOOOOOOOOOOO ", StatusId = 1, Primary = true, SystemId = "559e8c70d4332320bc076f4d" });
            list.Add(new DTO.PatientSystem { PatientId = "5325db88d6a4850adcbba98a", Value = "  ", StatusId = 1, Primary = true, SystemId = "559e8c70d4332320bc076f4d" });
            
            InsertPatientSystemsRequest request = new InsertPatientSystemsRequest
            {
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version,
                Token = token,
                PatientId = "5325db88d6a4850adcbba98a",
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
            //list.Add(new DTO.PatientSystem { Id = "55a56cb9d433253144dabce5", PatientId = "5325db88d6a4850adcbba98a", Value = " aaaaaaaaaaaaaaaaaaaaaaaaaaabbbbbbbbbbbbbbbbbbbbbbbbbccccccccccccccccccccccccdddddddddddddddddddddeeeeeeeeeeeeeeeeeeeeeeefffffffffffffffffffffgggggggggggggggggghhhhhhhhhhhhhhhiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiijjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjkkkkkkkkkkkkkkkkkkkkkkkkkkkllllllllllllllllllllllllllnnnnnnnnnnnnnnnnnnnmmmmmmmmmmmmmmmmmmmmmmm ", StatusId = 2, Primary = false, SystemId = "559e8c70d4332320bc076f4e" });
            list.Add(new DTO.PatientSystem { Id = "55a014bbd4332720a4bf5093", PatientId = "546d0d0684ac0508e43299d2", Value = "  ", StatusId = 2, Primary = false, SystemId = "559e8c70d4332320bc076f4f" });

            UpdatePatientSystemsRequest request = new UpdatePatientSystemsRequest
            {
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version,
                Token = token,
                PatientId = "5325db88d6a4850adcbba98a",
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
