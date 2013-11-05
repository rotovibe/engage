using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using System;

namespace Phytel.API.AppDomain.NG.Test
{
    [TestClass]
    public class NGManager_Tests
    {
        [TestMethod]
        public void GetPatientByID_Test()
        {
            PatientResponse response = NGManager.GetPatientByID("1", "NG", "inHealth001");
            Assert.IsTrue(response.LastName == "DiGiorgio");
        }
    }
}
