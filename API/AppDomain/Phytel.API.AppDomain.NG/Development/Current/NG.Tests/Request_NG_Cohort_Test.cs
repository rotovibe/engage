using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.AppDomain.NG.Services.Test
{
    [TestClass]
    public class Request_NG_Cohort_Test
    {
        [TestMethod]
        public void GetCohorts_Test()
        {
            // check to see if this id is registered in APISession in mongodb.
            // you will need to make sure there is a token registration for token '1234' 
            //with userid = 'BB241C64-A0FF-4E01-BA5F-4246EF50780E' in APIUserToken table for this to work.
            string token = "52792478fe7a592338e990a0";
            string lnsampleValue;
            string fnsampleValue;
            string gnsampleValue;
            string dobsampleValue;
            string patientID = "527a933efe7a590ad417d3b0";

            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("APIToken: {0}", token));
            
            GetAllCohortsResponse response = client.Post<GetAllCohortsResponse>("http://localhost:888/Nightingale/v1/NG/InHealth001/cohort",
                new GetAllCohortsRequest { } as object);

        }

        [TestMethod]
        public void Get_Pateints_In_Cohort_Test()
        {
            string token = "528e3a62d6a4850c7463d5a7";
            string cohortID = "528aa055d4332317acc50978";

            IRestClient client = new JsonServiceClient();

            GetCohortPatientsResponse response = client.Get<GetCohortPatientsResponse>(
                string.Format("{0}/{1}/{2}/cohortpatients/{3}/{4}", "http://localhost:888/Nightingale", "v1", "InHealth001", cohortID, "?Skip=0&Take=1000")
                );
        }

    }
}
