using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack;

namespace Phytel.API.AppDomain.NG.Services.Test
{
    [TestClass]
    public class SettingService_Tests
    {
        [TestMethod]
        public void GetAllSettings_Test()
        {

            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string token = "52936c88d6a48509e8d30632";
            IRestClient client = new JsonServiceClient();
            // Act
            //[Route("/{Version}/{ContractNumber}/settings", "GET")]
            GetAllSettingsResponse response = client.Get<GetAllSettingsResponse>
                (string.Format("{0}/{1}/{2}/settings?Token={3}",
                  "http://localhost:888/Nightingale/", version, contractNumber, token));

            // Assert
            Assert.AreNotEqual(0, response.Settings.Count);
        }
    }
}
