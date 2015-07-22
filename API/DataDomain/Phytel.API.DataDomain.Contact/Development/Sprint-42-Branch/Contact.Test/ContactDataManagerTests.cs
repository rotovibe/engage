using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Contact;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Contact.Test.Stubs;
using Phytel.API.DataDomain.Contact.DTO;
namespace Phytel.API.DataDomain.Contact.Tests
{
    [TestClass()]
    public class ContactDataManagerTests
    {
        [TestClass()]
        public class AddRecentPatient
        {
            [TestMethod()]
            [TestCategory("NIGHT-911")]
            [TestProperty("TFS", "10409")]
            [TestProperty("Layer", "DD.ContactDataManager")]
            public void Add_One_Patient_To_Contact_Success()
            {
                ContactDataManager cm = new ContactDataManager { Factory = new StubContactRepositoryFactory() };
                PutRecentPatientRequest request = new PutRecentPatientRequest
                {
                    PatientId = "111156789012345678901111",
                    ContactId = "123456789012345678901234",
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
            [TestProperty("Layer", "DD.ContactDataManager")]
            public void Add_One_Patient_To_Contact_DEV_Success()
            {
                ContactDataManager cm = new ContactDataManager { Factory = new StubContactRepositoryFactory() };
                PutRecentPatientRequest request = new PutRecentPatientRequest
                {
                    PatientId = "5325d9e7d6a4850adcbba4ad",
                    ContactId = "5325c81f072ef705080d347e",
                    UserId = "5325c81f072ef705080d347e",
                    Context = "NG",
                    ContractNumber = "InHealth001",
                    Version = 1.0
                };

                PutRecentPatientResponse response = cm.AddRecentPatient(request);
                
                Assert.IsNotNull(response);
            }


            [TestMethod()]
            [TestProperty("Layer", "DD.ContactDataManager")]
            public void DeletePatient()
            {
                ContactDataManager cm = new ContactDataManager { Factory = new ContactRepositoryFactory() };
                DeleteContactByPatientIdDataRequest request = new DeleteContactByPatientIdDataRequest
                {
                    PatientId = "5325db70d6a4850adcbba946",
                    UserId = "5325c81f072ef705080d347e",
                    Context = "NG",
                    ContractNumber = "InHealth001",
                    Version = 1.0
                };

                DeleteContactByPatientIdDataResponse response = cm.DeleteContactByPatientId(request);

                Assert.IsNotNull(response);
            }
        }
    }
}
