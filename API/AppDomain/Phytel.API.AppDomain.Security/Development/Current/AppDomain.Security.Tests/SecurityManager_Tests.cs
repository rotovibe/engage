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
        public void TestMethod1()
        {
            AuthenticateResponse response = SecurityManager.GetToken("Mel", "bobadilla", "123456789");
        }
    }
}
