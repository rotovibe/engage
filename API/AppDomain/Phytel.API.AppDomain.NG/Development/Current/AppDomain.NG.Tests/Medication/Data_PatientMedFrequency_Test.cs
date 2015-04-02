using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.AppDomain.NG.Test
{
    [TestClass]
    public class Data_PatientMedFrequency_Test
    {
        string context = "NG";
        string contractNumber = "InHealth001";
        string userId = "000000000000000000000000";
        double version = 1.0;
        string url = "http://localhost:888/Nightingale";
        IRestClient client = new JsonServiceClient();
        string token = "54b81d0b84ac050580839a18";

        [TestMethod]
        public void Insert_PatientMedFrequency_Test()
        {
            PostPatientMedFrequencyRequest request = new PostPatientMedFrequencyRequest
            {
                PatientMedFrequency = new PatientMedFrequency { Name = "prn (as needed)", PatientId = "5325db63d6a4850adcbba922" },
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version,
                Token = token
            };

            JsonServiceClient.HttpWebRequestFilter = x =>
                x.Headers.Add(string.Format("{0}: {1}", "Token", token));

            //[Route("/{Version}/{ContractNumber}/PatientMedSupp/Frequency/Insert", "POST")]
            PostPatientMedFrequencyResponse response = client.Post<PostPatientMedFrequencyResponse>(string.Format("{0}/{1}/{2}/PatientMedSupp/Frequency/Insert", url, version, contractNumber), request);

            Assert.IsNotNull(response);
        }
    }
}
