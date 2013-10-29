using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.Security;
using Phytel.API.AppDomain.Security.DTOs;

namespace AppDomain.Security.Tests
{
    [TestClass]
    public class SecurityManager_Tests
    {
        [TestMethod]
        public void Validate_Credentials_Test()
        {
            AuthenticateResponse response = SecurityManager.ValidateCredentials("Mel", "bobadilla", "123456789");
        }

        [TestMethod]
        public void IsTokenExpired_Test()
        {
            bool result = SecurityManager.IsTokenExpired("testtoken");
        }

    }
}
