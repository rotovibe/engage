using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using System;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.Test
{
    [TestClass]
    public class NGManager_Tests
    {
        [TestMethod]
        public void GetPatientByID_Test()
        {
            //PatientResponse response = NGManager.GetPatientByID("1", "NG", "inHealth001");
            //Assert.IsTrue(response.LastName == "DiGiorgio");
        }

        #region PatientProblem
        [TestMethod]
        public void GetAllPatientProblems_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string token = "1234";
            NGManager ngManager = new NGManager();
            GetAllPatientProblemsRequest request = new GetAllPatientProblemsRequest
            {
                ContractNumber = contractNumber,
                Token = token,
                Version = version,
                PatientID = "528bdccc072ef7071c2e22ae"
            };
            // Act
            List<PatientProblem> response = ngManager.GetPatientProblems(request);

            //Assert
            Assert.IsTrue(response.Count > 0);
        }

        [TestMethod]
        public void GetAllProblems_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string token = "1234";
            NGManager ngManager = new NGManager();
            GetAllProblemsRequest request = new GetAllProblemsRequest
            {
                ContractNumber = contractNumber,
                Token = token,
                Version = version
            };
            // Act
            List<LookUp> response = ngManager.GetProblems(request);

            //Assert
            Assert.IsTrue(response.Count > 0);
        } 
        #endregion

        [TestMethod]
        public void GetAllCohorts_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string token = "1234";
            NGManager ngManager = new NGManager();
            GetAllCohortsRequest request = new GetAllCohortsRequest
            {
                ContractNumber = contractNumber,
                Token = token,
                Version = version
            };
            // Act
            List<Cohort> response = ngManager.GetCohorts(request);

            //Assert
            Assert.IsTrue(response.Count > 0);
        }

        [TestMethod]
        public void GetAllSettings_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string token = "52936c88d6a48509e8d30632";
            NGManager ngManager = new NGManager();
            GetAllSettingsRequest request = new GetAllSettingsRequest
            {
                ContractNumber = contractNumber,
                Token = token,
                Version = version
            };
            // Act
            GetAllSettingsResponse response = ngManager.GetAllSettings(request);

            //Assert
            Assert.IsTrue(response.Settings.Count > 0);
        }
    }
}
