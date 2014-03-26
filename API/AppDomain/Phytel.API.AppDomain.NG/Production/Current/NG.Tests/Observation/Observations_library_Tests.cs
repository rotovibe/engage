using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Patient.Service.Test
{
    [TestClass]
    public class Observations_library_Tests
    {
        [TestMethod]
        public void Get_Additional_Observation_Library_Test()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;
            string token = "53079028d2d8e91748f416cc";
            string patientId = "52f55875072ef709f84e68e5";
            string typeId = "53067453fe7a591a348e1b66";
            IRestClient client = new JsonServiceClient();

            GetAdditionalObservationLibraryResponse response = client.Get<GetAdditionalObservationLibraryResponse>(
                string.Format(@"http://localhost:888/Nightingale/{0}/{1}/Patient/{2}/Observation/Type/{3}/MatchLibrary/?Token={4}",
                version,
                contractNumber,
                patientId,
                typeId,
                token));
        }
    }
}
