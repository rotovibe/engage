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
            AuthenticateResponse response = SecurityManager.ValidateCredentials("b25eaff2-35f4-4d79-9ea8-5dc8b7a4cfe1", secToken, "12345", "NG");
            Assert.IsTrue(response.UserID != Guid.Empty);
        }

        [TestMethod]
        public void Validate_Credentials_Test_Fail()
        {
            AuthenticateResponse response = SecurityManager.ValidateCredentials("abcxyz5", secToken, "12345", "NG");
            Assert.IsTrue(response.UserID == Guid.Empty);
        }

        [TestMethod]
        public void IsTokenExpired_Test()
        {
            ValidateTokenRequest request = new ValidateTokenRequest { Context = "NG", Token = "531a1fedd6a4850f643204e0", Version = "v1" };
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
