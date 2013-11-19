using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.LookUp.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.LookUp.Services.Test
{
    [TestClass]
    public class LookUpService_Test
    {
        [TestMethod]
        public void GetAllProblem_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string context ="NG";
            IRestClient client = new JsonServiceClient();

            // Act
            GetAllProblemResponse response = client.Get<GetAllProblemResponse>
                (string.Format("{0}/{1}/{2}/{3}/problems",
                  "http://localhost:8888/LookUp/",context,version, contractNumber));

            // Assert
            Assert.AreNotEqual(0, response.Problems.Count);
        }

        [TestMethod]
        public void GetProblemByID_Test()
        {
            // Arrange
            string expectedValue = "Arthritis";
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            string problemID = "528a66fdd4332317acc50960";
            IRestClient client = new JsonServiceClient();

            // Act
            GetProblemResponse response = client.Get<GetProblemResponse>
                (string.Format("{0}/{1}/{2}/{3}/problem/{4}",
                  "http://localhost:8888/LookUp/", context, version, contractNumber, problemID));

            string actualValue = response.Problem.Name;
            // Assert
            Assert.AreEqual(expectedValue, actualValue, true);
        }

        [TestMethod]
        public void SearchProblem_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            
            IRestClient client = new JsonServiceClient();

            // Act
            GetAllProblemResponse response = client.Post<GetAllProblemResponse>
                (string.Format("{0}/{1}/{2}/{3}/problems",
                  "http://localhost:8888/LookUp/", context, version, contractNumber),
                      new SearchProblemRequest
                      {
                          Active = true,
                          Type = "Chronic",
                          Version = version,
                          ContractNumber = contractNumber,
                          Context = context
                      }
                  );

            // Assert
            Assert.AreNotEqual(0, response.Problems.Count);
        }
    }
}
