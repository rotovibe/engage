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
            AuthenticateResponse response = SecurityManager.ValidateCredentials("d33540ff-c382-4ec9-ba42-fb4c474b50dd", "12345", "NG");
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
            ValidateTokenRequest request = new ValidateTokenRequest { Context = "NG", Token = "abc", Version = "v1" };
            ValidateTokenResponse response = SecurityManager.ValidateToken(request);
        }

    }
}
