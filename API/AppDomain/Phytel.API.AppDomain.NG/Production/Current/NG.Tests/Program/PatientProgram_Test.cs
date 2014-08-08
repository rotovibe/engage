using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.Patient.Service.Test
{
    /// <summary>
    /// Summary description for PatientProgram_Test
    /// </summary>
    [TestClass]
    public class PatientProgram_Test
    {
        [TestMethod]
        public void GetPatientActionDetailsTest()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;
            string token = "534ee052d6a48504b03b4a9a";
            IRestClient client = new JsonServiceClient();

            GetPatientActionDetailsRequest request = new GetPatientActionDetailsRequest { PatientId = "5325d9f2d6a4850adcbba4ca", PatientModuleId = "534c4fb2d6a48504b05335c2", PatientProgramId = "534c4fb2d6a48504b053346f", PatientActionId = "534c4fb2d6a48504b05335c3", UserId = "000000000000000000000000" };
            JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("Token: {0}", token));
            //   [Route("/{Version}/{ContractNumber}/Patient/{PatientId}/Program/{PatientProgramId}/Module/{PatientModuleId}/Action/{PatientActionId}", "GET")]	
            GetPatientActionDetailsResponse response = client.Get<GetPatientActionDetailsResponse>(
                string.Format(@"http://localhost:888/Nightingale/{0}/{1}/Patient/{2}/Program/{3}/Module/{4}/Action/{5}?Context={6}",
                version,
                contractNumber,
                request.PatientId,
                request.PatientProgramId,
                request.PatientModuleId,
                request.PatientActionId,
                context));

            Assert.IsNotNull(response.Action);
        }

        [TestMethod]
        public void GetPatientProgramDetailsSummary_Test()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;
            string token = "53693860d6a485044ccb0f0b";
            IRestClient client = new JsonServiceClient();

            GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest { PatientId = "5325d9f2d6a4850adcbba4ca", UserId = "000000000000000000000000" };
            JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("Token: {0}", token));
            // [Route("/{Version}/{ContractNumber}/Patient/{PatientId}/Program/{PatientProgramId}/", "GET")]
            GetPatientProgramDetailsSummaryResponse response = client.Get<GetPatientProgramDetailsSummaryResponse>(
                string.Format(@"http://localhost:888/Nightingale/{0}/{1}/Patient/{2}/Program/{3}?Context={4}",
                version,
                contractNumber,
                request.PatientId,
                request.PatientProgramId,
                context));

            Assert.IsNotNull(response.Program.Modules[0].Objectives);
        }

        [TestMethod]
        public void RemoveProgram_Test()
        {
            PostRemovePatientProgramRequest request = new PostRemovePatientProgramRequest
            {
                ContractNumber = "InHealth001",
                Id = "53d1442fd6a4850d589a2d5a",
                PatientId = "5325db0cd6a4850adcbba81a",
                Reason = "Just liked that.",
                ProgramName = "BSHSI - Healthy Weight",
                UserId = "5325c821072ef705080d3488",
                Token = "53d13d16d6a4850d5889f443",
                Version = 1.0
            };
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("Token: {0}", request.Token));
            //[Route("/{Version}/{ContractNumber}/Patient/{PatientId}/Program/{Id}/Remove", "POST")]
            PostRemovePatientProgramResponse response = client.Post<PostRemovePatientProgramResponse>(
                string.Format(@"http://localhost:56187/{0}/{1}/Patient/{2}/Program/{3}/Remove",
                request.Version,
                request.ContractNumber,
                request.PatientId,
                request.Id), request);

            Assert.IsNotNull(response);
        }
    }
}
