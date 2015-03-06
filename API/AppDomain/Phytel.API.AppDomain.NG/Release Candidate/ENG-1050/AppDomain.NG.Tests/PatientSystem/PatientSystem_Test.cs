using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.Test.Stubs;
using Phytel.API.AppDomain.NG.DTO;

namespace Phytel.API.AppDomain.NG.Test.PatientSystem
{
    /// <summary>
    /// This class tests all the methods that are related to Contact domain in INGManager class.
    /// </summary>
    [TestClass]
    public class PatientSystem_Test
    {
        string userId = "000000000000000000000000";
        string contractNumber = "InHealth001";
        double version = 1.0;
        string context = "NG";

        [TestMethod()]
        public void GetPatientSystems_Test()
        {
            INGManager ngm = new NGManager();

            GetPatientSystemsRequest request = new GetPatientSystemsRequest
            {
                Version = version,
                ContractNumber = contractNumber,
                UserId = userId,
                PatientId = "5447e56684ac05125410e26b"
            };

            GetPatientSystemsResponse response = ngm.GetPatientSystems(request);
            Assert.IsTrue(response.PatientSystems.Count > 0);
        }

        [TestMethod()]
        public void SavePatientSystem_Test()
        {
            INGManager ngm = new NGManager();

            Phytel.API.AppDomain.NG.DTO.PatientSystem p = new DTO.PatientSystem
            {
                DeleteFlag  = false,
                //DisplayLabel = "ID",
                Id = "54481ddf84ac051254201e67",
                PatientId = "5447e56684ac05125410e26b",
                SystemId = "4444",
                SystemName = "Lamar"
            };

            PostPatientSystemRequest request = new PostPatientSystemRequest
            {
                Version = version,
                ContractNumber = contractNumber,
                UserId = userId,
                Token = string.Empty,
                PatientSystem = p,
                Insert = false,
            };

            PostPatientSystemResponse response = ngm.SavePatientSystem(request);
            Assert.IsNotNull(response);
        }
    }
}
