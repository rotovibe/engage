using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientProblem;
using Phytel.API.DataDomain.PatientProblem.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.PatientProblem.Service.Test
{
    [TestClass]
    public class GetPatientProblemServicesTest
    {
        [TestMethod]
        public void Get_Patient_Problem_True_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            string patientID = "528f6d46072ef708ecd78728";
            string problemID = "528a66fdd4332317acc50960";
            IRestClient client = new JsonServiceClient();

            // Act
            GetPatientProblemsDataResponse response = client.Get<GetPatientProblemsDataResponse>(
                string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Problem/?ProblemId={5}", 
                "http://localhost:8888/PatientProblem", 
                context, 
                version, 
                contractNumber,
                patientID,
                problemID));

           // Assert
            Assert.IsNotNull(response.PatientProblem);
        }

        [TestMethod]
        public void Get_Patient_Problem_False_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            string patientID = "528f6d46072ef708ecd78711";
            string problemID = "528a66fdd4332317acc50960";
            IRestClient client = new JsonServiceClient();

            // Act
            GetPatientProblemsDataResponse response = client.Get<GetPatientProblemsDataResponse>(
                string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Problem/?ProblemId={5}",
                "http://localhost:8888/PatientProblem",
                context,
                version,
                contractNumber,
                patientID,
                problemID));

            // Assert
            Assert.IsNull(response.PatientProblem);
        }

        [TestMethod]
        public void Update_Patient_Problem_Test()
        {
            //"_id" : ObjectId("52e26f5b072ef7191c11ef78"),
            //"pid" : ObjectId("52e26f5b072ef7191c11ef73"),
            //"prbid" : ObjectId("528a66e3d4332317acc5095e"),
            //"act" : true,
            //"f" : true,
            //"l" : 1,
            //"v" : "v1",
            //"uby" : null,
            //"del" : false,
            //"uon" : ISODate("2014-01-24T13:49:15.345Z")

            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            string patientID = "52e26f5b072ef7191c11ef73";
            string problemID = "528a66f4d4332317acc5095f";
            bool active = false;
            bool featured = true;
            string id = "52eace5bfe7a5920449cdd7f";
            int level = 1;

            IRestClient client = new JsonServiceClient();

            // Act
            PutUpdatePatientProblemResponse response = client.Put<PutUpdatePatientProblemResponse>(
                string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Problem/Update/",
                "http://localhost:8888/PatientProblem",
                context,
                version,
                contractNumber,
                patientID),
                new PutUpdatePatientProblemRequest
                {
                    Active = active,
                    Featured = featured,
                    Id = id,
                    Level = level,
                    ProblemId = problemID
                } as object);

            // Assert
            Assert.IsNotNull(response.Outcome);
        }
    }
}
