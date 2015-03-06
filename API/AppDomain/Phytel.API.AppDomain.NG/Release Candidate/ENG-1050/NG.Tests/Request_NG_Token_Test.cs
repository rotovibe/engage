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
        public void PostPatientByID_Test()
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
            string patientID = "528f6dc2072ef708ecd90e87";

            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("APIToken: {0}", token));

            GetPatientResponse response = client.Post<GetPatientResponse>("http://localhost:888/Nightingale/1.0/InHealth001/patient",
                new GetPatientRequest { PatientID = patientID, Token = token } as object);

            lnsampleValue = response.Patient.LastName;
            fnsampleValue = response.Patient.FirstName;
            gnsampleValue = response.Patient.Gender;
            dobsampleValue = response.Patient.DOB;

            Assert.AreEqual(lnControlValue, lnsampleValue);
            Assert.AreEqual(fnControlValue, fnsampleValue);
            Assert.AreEqual(fnControlValue, fnsampleValue);
            Assert.AreEqual(fnControlValue, fnsampleValue);
            Assert.AreEqual(gnControlValue, gnsampleValue);
            Assert.AreEqual(dobControlValue, dobsampleValue);
        }

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
            string patientID = "528f6dc2072ef708ecd90e87";

            IRestClient client = new JsonServiceClient();
            //JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("APIToken: {0}", token));

            GetPatientResponse response = client.Get<GetPatientResponse>(string.Format("http://localhost:888/Nightingale/1.0/InHealth001/patient/{0}?token={1}", patientID, token));

            lnsampleValue = response.Patient.LastName;
            fnsampleValue = response.Patient.FirstName;
            gnsampleValue = response.Patient.Gender;
            dobsampleValue = response.Patient.DOB;

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

            GetPatientResponse response = client.Post<GetPatientResponse>("http://localhost:888/1.0/NG/InHealth001/patient",
                new GetPatientRequest { PatientID = patientID, Token = token } as object);

            lnsampleValue = response.Patient.LastName;
            fnsampleValue = response.Patient.FirstName;

            Assert.AreEqual(lnControlValue, lnsampleValue);
            Assert.AreEqual(fnControlValue, fnsampleValue);
        }


        #region PatientProblem
        [TestMethod]
        public void GetAllPatientProblems_Test()
        {

            // Arrange
            double version = 1.0;
            string contractNumber = "InHealth001";
            string token = "528cc924d6a4850fe05b3afa";
            string patientID = "528bdccc072ef7071c2e22ae";
            IRestClient client = new JsonServiceClient();

            GetAllPatientProblemsResponse response = client.Get<GetAllPatientProblemsResponse>
                (string.Format("{0}/{1}/{2}/patientproblems/{3}?Token={4}",
                  "http://localhost:888/Nightingale/", version, contractNumber, patientID, token)
                  );

            // Assert
            Assert.AreNotEqual(0, response.PatientProblems.Count);
        }




        [TestMethod]
        public void GetAllProblems_Test()
        {

            // Arrange
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            string token = "528cc924d6a4850fe05b3afa";
            IRestClient client = new JsonServiceClient();
            // Act
            //[Route("/{Context}/{Version}/{ContractNumber}/lookup/problems", 
            GetAllProblemsResponse response = client.Get<GetAllProblemsResponse>
                (string.Format("{0}/{1}/{2}/{3}/lookup/problems?Token={4}",
                  "http://localhost:888/Nightingale/", context, version, contractNumber, token));

            // Assert
            Assert.AreNotEqual(0, response.Problems.Count);
        }
    } 
        #endregion
}
