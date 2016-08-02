using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.AppDomain.NG.Services.Test.Contact
{
    [TestClass]
    public class ContactService_Test
    {
        [TestMethod]
        public void GetRecentPatientsForAContact_Test()
        {
            string contractNumber = "InHealth001";
            double version = 1.0;
            string token = "53750ca2d6a4850854d33c42";
            string contactId = "5325c821072ef705080d3488";

            JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("Token: {0}", token));

            IRestClient client = new JsonServiceClient();
            //[Route("/{Version}/{ContractNumber}/Contact/{ContactId}/RecentPatients", "GET")]
            GetRecentPatientsResponse response = client.Get<GetRecentPatientsResponse>(
                string.Format(@"http://localhost:888/Nightingale/{0}/{1}/Contact/{2}/RecentPatients",
                version,
                contractNumber,
                contactId));

            Assert.IsNotNull(response.Limit);
        }
    }
}
