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

        [TestMethod]
        public void GetPatientsByProblem_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            string token = "1234";
            NGManager ngManager = new NGManager();
            GetAllPatientProblemRequest request = new GetAllPatientProblemRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                Token = token,
                Version = version,
                Category = "Chronic",
                Status = "Active",
                PatientID = "527a933efe7a590ad417d3b0"
            };
            // Act
            List<PatientProblem> response = ngManager.GetProblemsByPatientID(request);

            //Assert
            Assert.IsTrue(response.Count > 0);
        }
        
        [TestMethod]
        public void FindProblems_Test()
        { 
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            string token = "1234";
            NGManager ngManager = new NGManager();
            GetAllProblemLookUpRequest request = new GetAllProblemLookUpRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                Token = token,
                Version = version
            };
            // Act
            List<ProblemLookUp> response = ngManager.GetProblems(request);

            //Assert
            Assert.IsTrue(response.Count > 0);
        }
    }
}
