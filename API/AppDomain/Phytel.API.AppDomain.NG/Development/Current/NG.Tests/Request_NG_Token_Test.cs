using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.AppDomain.NG.Services.Test
{
    [TestClass]
    public class Request_NG_Token_Test
    {
        [TestMethod]
        public void GetPatientByID_Test()
        {
            // check to see if this id is registered in APISession in mongodb.
            // you will need to make sure there is a token registration for token '1234' 
            //with userid = 'BB241C64-A0FF-4E01-BA5F-4246EF50780E' in APIUserToken table for this to work.
            string token = "52792478fe7a592338e990a0";
            string lnControlValue = "Jones";
            string fnControlValue = "James";
            string lnsampleValue;
            string fnsampleValue;
            string patientID = "52781cd8fe7a5925fcee5bf3";

            IRestClient client = new JsonServiceClient();

            PatientResponse response = client.Post<PatientResponse>("http://localhost:888/v1/NG/Contract/InHealth001/patient",
                new PatientRequest { ID = patientID, Token= token } as object);

            lnsampleValue = response.LastName;
            fnsampleValue = response.FirstName;

            Assert.AreEqual(lnControlValue, lnsampleValue);
            Assert.AreEqual(fnControlValue, fnsampleValue);
        }

        [TestMethod]
        public void GetPatientByID_Failure_Test()
        {
            // check to see if this id is registered in APISession in mongodb.
            // you will need to make sure there is a token registration for token '1234' 
            //with userid = 'BB241C64-A0FF-4E01-BA5F-4246EF50780E' in APIUserToken table for this to work.
            string token = "52792478fe7a592338e990a1";
            string lnControlValue = "Jones";
            string fnControlValue = "James";
            string lnsampleValue;
            string fnsampleValue;
            string patientID = "52781cd8fe7a5925fcee5bf3";

            IRestClient client = new JsonServiceClient();

            PatientResponse response = client.Post<PatientResponse>("http://localhost:888/v1/NG/Contract/InHealth001/patient",
                new PatientRequest { ID = patientID, Token = token } as object);

            lnsampleValue = response.LastName;
            fnsampleValue = response.FirstName;

            Assert.AreEqual(lnControlValue, lnsampleValue);
            Assert.AreEqual(fnControlValue, fnsampleValue);
        }
    }
}
