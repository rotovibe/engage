using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.AppDomain.NG.Service.Tests
{
    [TestClass()]
    public class PatientSystemServiceTests
    {
        string context = "NG";
        string contractNumber = "InHealth001";
        string userId = "000000000000000000000000";
        double version = 1.0;
        string url = "http://localhost:888/Nightingale";
        IRestClient client = new JsonServiceClient();
        string token = "54b81d0b84ac050580839a18";
        string patientId = ObjectId.GenerateNewId().ToString();

        [TestMethod()]
        public void PutTest()
        {
            UpdatePatientSystemsRequest request = new UpdatePatientSystemsRequest
            {
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version,
                Token = token,
                PatientId = patientId,
                PatientSystems =
                    new List<DTO.PatientSystem>()
                    {
                        new DTO.PatientSystem
                        {
                            Id = ObjectId.GenerateNewId().ToString(),
                            PatientId = patientId,
                            Primary = true
                        },
                        new DTO.PatientSystem
                        {
                            Id = ObjectId.GenerateNewId().ToString(),
                            PatientId = patientId,
                            Primary = true
                        },
                        new DTO.PatientSystem
                        {
                            Id = ObjectId.GenerateNewId().ToString(),
                            PatientId = patientId,
                            Primary = false
                        }
                    }
            };

            JsonServiceClient.HttpWebRequestFilter = x =>
                x.Headers.Add(string.Format("{0}: {1}", "Token", token));

            //[Route("/{Version}/{ContractNumber}/Patient/{PatientId}/PatientSystems", "PUT")]
            UpdatePatientSystemsResponse response =
                client.Put<UpdatePatientSystemsResponse>(
                    string.Format("{0}/{1}/{2}/Patient/{3}/PatientSystems", url, version, contractNumber, patientId),
                    request);

            Assert.IsNotNull(response);
        }
    }
}
