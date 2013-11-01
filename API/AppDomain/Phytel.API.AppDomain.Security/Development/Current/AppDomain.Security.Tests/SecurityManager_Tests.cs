using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.Security.DTO;
using System;

namespace Phytel.API.AppDomain.Security.Test
{
    [TestClass]
    public class SecurityManager_Tests
    {
        [TestMethod]
        public void Validate_Credentials_Test()
        {
            AuthenticateResponse response = SecurityManager.ValidateCredentials("abcxyz", "12345", "NG");
            Assert.IsTrue(response.UserID != Guid.Empty);
        }

        [TestMethod]
        public void Validate_Credentials_Test_Fail()
        {
            AuthenticateResponse response = SecurityManager.ValidateCredentials("abcxyz5", "12345", "NG");
            Assert.IsTrue(response.UserID == Guid.Empty);
        }

        [TestMethod]
        public void IsTokenExpired_Test()
        {
            ValidateTokenResponse response = SecurityManager.ValidateToken("abc", "NG");
        }

    }
}
