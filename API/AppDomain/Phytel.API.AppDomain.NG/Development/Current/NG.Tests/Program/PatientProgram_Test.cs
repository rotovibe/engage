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
    }
}
