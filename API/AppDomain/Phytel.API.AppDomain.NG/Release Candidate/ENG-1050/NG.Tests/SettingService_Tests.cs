using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.AppDomain.NG.Services.Test
{
    [TestClass]
    public class SettingService_Tests
    {
        [TestMethod]
        public void GetAllSettings_Test()
        {

            // Arrange
            double version = 1.0;
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
