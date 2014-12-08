using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Contract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Contract.Test.Stubs;
using Phytel.API.DataDomain.Contract.DTO;
namespace Phytel.API.DataDomain.Contract.Tests
{
    [TestClass()]
    public class ContractDataManagerTests
    {
        [TestClass()]
        public class AddRecentPatient
        {
            [TestMethod()]
            [TestCategory("NIGHT-911")]
            [TestProperty("TFS", "10409")]
            [TestProperty("Layer", "DD.ContractDataManager")]
            public void Add_One_Patient_To_Contract_Success()
            {
                ContractDataManager cm = new ContractDataManager { Factory = new StubContractRepositoryFactory() };
                PutRecentPatientRequest request = new PutRecentPatientRequest
                {
                    PatientId = "111156789012345678901111",
                    ContractId = "123456789012345678901234",
                    UserId = "666656789012345678906666",
                    Context = "NG",
                    ContractNumber = "InHealth001",
                    Version = 1.0
                };

                PutRecentPatientResponse response = cm.AddRecentPatient(request);
                bool result = response.SuccessData;
                Assert.IsTrue(result);
            }

            [TestMethod()]
            [TestCategory("NIGHT-911")]
            [TestProperty("TFS", "10409")]
            [TestProperty("Layer", "DD.ContractDataManager")]
            public void Add_One_Patient_To_Contract_DEV_Success()
            {
                ContractDataManager cm = new ContractDataManager { Factory = new StubContractRepositoryFactory() };
                PutRecentPatientRequest request = new PutRecentPatientRequest
                {
                    PatientId = "5325d9e7d6a4850adcbba4ad",
                    ContractId = "5325c81f072ef705080d347e",
                    UserId = "5325c81f072ef705080d347e",
                    Context = "NG",
                    ContractNumber = "InHealth001",
                    Version = 1.0
                };

                PutRecentPatientResponse response = cm.AddRecentPatient(request);
                
                Assert.IsNotNull(response);
            }


            [TestMethod()]
            [TestProperty("Layer", "DD.ContractDataManager")]
            public void DeletePatient()
            {
                ContractDataManager cm = new ContractDataManager { Factory = new ContractRepositoryFactory() };
                DeleteContractByPatientIdDataRequest request = new DeleteContractByPatientIdDataRequest
                {
                    PatientId = "5325db70d6a4850adcbba946",
                    UserId = "5325c81f072ef705080d347e",
                    Context = "NG",
                    ContractNumber = "InHealth001",
                    Version = 1.0
                };

                DeleteContractByPatientIdDataResponse response = cm.DeleteContractByPatientId(request);

                Assert.IsNotNull(response);
            }
        }
    }
}
