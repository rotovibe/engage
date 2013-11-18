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
            string lnControlValue = "Johnson";
            string fnControlValue = "Greg";
            string gnControlValue = "M";
            string dobControlValue = "2/15/1975";
            string lnsampleValue;
            string fnsampleValue;
            string gnsampleValue;
            string dobsampleValue;
            string patientID = "527a933efe7a590ad417d3b0";

            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("APIToken: {0}", token));
            
            PatientResponse response = client.Post<PatientResponse>("http://localhost:888/Nightingale/v1/NG/Contract/InHealth001/patient",
                new PatientRequest { PatientID = patientID } as object);

            lnsampleValue = response.LastName;
            fnsampleValue = response.FirstName;
            gnsampleValue = response.Gender;
            dobsampleValue = response.DOB;

            Assert.AreEqual(lnControlValue, lnsampleValue);
            Assert.AreEqual(fnControlValue, fnsampleValue);
            Assert.AreEqual(fnControlValue, fnsampleValue);
            Assert.AreEqual(fnControlValue, fnsampleValue);
            Assert.AreEqual(gnControlValue, gnsampleValue);
            Assert.AreEqual(dobControlValue, dobsampleValue);
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
                new PatientRequest { PatientID = patientID, Token = token } as object);

            lnsampleValue = response.LastName;
            fnsampleValue = response.FirstName;

            Assert.AreEqual(lnControlValue, lnsampleValue);
            Assert.AreEqual(fnControlValue, fnsampleValue);
        }


        [TestMethod]
        public void GetPatientProblemsByPatientIDService_POST_Test()
        {

            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            string token = "1234";
            string category = "Chronic";
            string status = "Active";
            string patientID = null;// "527a933efe7a590ad417d3b0";
            IRestClient client = new JsonServiceClient();

            // Act
             //[Route("/{Context}/{Version}/Contract/{ContractNumber}/patientproblems/{PatientID}", "GET")]
            //[Route("/{Context}/{Version}/Contract/{ContractNumber}/patientproblems", "POST")]
            PatientProblemsResponse response = client.Post<PatientProblemsResponse>
                (string.Format("{0}/{1}/{2}/Contract/{3}/patientproblems",
                  "http://localhost:888/Nightingale/", context, version, contractNumber),
                  new PatientProblemRequest {
                   Context = context,
                   ContractNumber = contractNumber,
                   Version = version,
                   Token = token,
                   Category = category,
                   Status = status,
                   PatientID = patientID
                  } 
                  as object
                  );

            // Assert
            Assert.AreNotEqual(0, response.PatientProblems.Count);
        }


        

        [TestMethod]
        public void FindProblemsService_Test()
        {

            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            string token = "1234";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("APIToken: {0}", token));
            // Act
            //[Route("/{Context}/{Version}/Contract/{ContractNumber}/problemslookup", 
            ProblemsLookUpResponse response = client.Get<ProblemsLookUpResponse>
                (string.Format("{0}/{1}/{2}/Contract/{3}/problemslookup",
                  "http://localhost:888/Nightingale/", context, version, contractNumber));

            // Assert
            Assert.AreNotEqual(0, response.Problems.Count);
        }
    }
}
