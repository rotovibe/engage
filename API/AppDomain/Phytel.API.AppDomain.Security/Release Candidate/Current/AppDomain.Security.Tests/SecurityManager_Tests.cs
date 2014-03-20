using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.Security.DTO;
using System;

namespace Phytel.API.AppDomain.Security.Test
{
    [TestClass]
    public class SecurityManager_Tests
    {
        private const string secToken = "PhytelSecurityKey";

        [TestMethod]
        public void Validate_Credentials_Test()
        {
            AuthenticateResponse response = SecurityManager.ValidateCredentials("bdd78ca7-405e-4999-be4c-602f3b0af783", secToken, "12345", "NG");
            Assert.IsTrue(response.UserId != string.Empty);
        }

        [TestMethod]
        public void Validate_Credentials_Test_Fail()
        {
            AuthenticateResponse response = SecurityManager.ValidateCredentials("abcxyz5", secToken, "12345", "NG");
            Assert.IsTrue(response.UserId == string.Empty);
        }

        [TestMethod]
        public void IsTokenExpired_Test()
        {
            ValidateTokenRequest request = new ValidateTokenRequest { Context = "NG", Token = "53287900d6a4850ebc395f67", Version = 1 };
            ValidateTokenResponse response = SecurityManager.ValidateToken(request, "Engineer");
        }

        [TestMethod]
        public void Logout_Test()
        {
            LogoutRequest request = new LogoutRequest { Context = "NG", Token = "531ddd4ed6a4850398308056" };
            LogoutResponse response = SecurityManager.Logout(request, "Engineer");
        }

    }
}
