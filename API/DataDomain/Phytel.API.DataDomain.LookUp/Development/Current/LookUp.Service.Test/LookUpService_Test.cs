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
        public void FindProblems_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string context ="NG";
            IRestClient client = new JsonServiceClient();

            // Act
            List<Problem> response = client.Get<List<Problem>>
                (string.Format("{0}/{1}/{2}/{3}/problems",
                  "http://localhost:8888/LookUp/",context,version, contractNumber));

            // Assert
            Assert.AreNotEqual(0, response.Count);
        }

        [TestMethod]
        public void GetProblemByID_Test()
        {
            // Arrange
            string expectedValue = "Arthritis";
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            string problemID = "527c1b1ad4332324ac199142";
            IRestClient client = new JsonServiceClient();

            // Act
            Problem response = client.Get<Problem>
                (string.Format("{0}/{1}/{2}/{3}/problem/{4}",
                  "http://localhost:8888/LookUp/", context, version, contractNumber, problemID));

            string actualValue = response.Name;
            // Assert
            Assert.AreEqual(expectedValue, actualValue, true);
        }
    }
}
