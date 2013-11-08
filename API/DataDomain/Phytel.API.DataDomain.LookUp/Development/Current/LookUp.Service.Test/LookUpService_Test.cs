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
        public void FindConditions_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string context ="NG";
            IRestClient client = new JsonServiceClient();

            // Act
            List<ConditionResponse> response = client.Get<List<ConditionResponse>>
                (string.Format("{0}/{1}/{2}/Contract/{3}/conditions",
                  "http://localhost:8888/LookUp/",context,version, contractNumber));

            // Assert
            Assert.AreNotEqual(0, response.Count);
        }

        [TestMethod]
        public void GetConditionByID_Test()
        {
            // Arrange
            string expectedValue = "Arthritis";
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            string conditionID = "527c1b1ad4332324ac199142";
            IRestClient client = new JsonServiceClient();

            // Act
            ConditionResponse response = client.Get<ConditionResponse>
                (string.Format("{0}/{1}/{2}/Contract/{3}/condition/{4}",
                  "http://localhost:8888/LookUp/", context, version, contractNumber, conditionID));

            string actualValue = response.DisplayName;
            // Assert
            Assert.AreEqual(expectedValue, actualValue, true);
        }
    }
}
