﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            AuthenticateResponse response = SecurityManager.ValidateCredentials("b25eaff2-35f4-4d79-9ea8-5dc8b7a4cfe1", "12345", "NG");
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

        [TestMethod]
        public void Logout_Test()
        {
            LogoutRequest request = new LogoutRequest { Context = "NG", Token = "52a87fe2d6a4850a8cd99ed5" };
            LogoutResponse response = SecurityManager.Logout(request);
        }

    }
}
