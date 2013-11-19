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
            
            GetAllCohortResponse response = client.Post<GetAllCohortResponse>("http://localhost:888/Nightingale/v1/NG/InHealth001/cohort",
                new GetAllCohortRequest { } as object);

        }

        [TestMethod]
        public void Get_Pateints_In_Cohort_Test()
        {
            string token = "52792478fe7a592338e990a0";
            string patientID = "527a933efe7a590ad417d3b0";

            IRestClient client = new JsonServiceClient();

            GetCohortPatientsResponse response = client.Get<GetCohortPatientsResponse>("http://localhost:888/Nightingale/v1/NG/InHealth001/cohort/1");
        }

    }
}
