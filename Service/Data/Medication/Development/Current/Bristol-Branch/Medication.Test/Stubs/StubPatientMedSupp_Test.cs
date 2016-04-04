using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Medication.DTO;

namespace Phytel.API.DataDomain.Medication.Test.Stubs
{
    [TestClass]
    public class StubPatientMedSupp_Test
    {
        string context = "NG";
        string contractNumber = "InHealth001";
        string userId = "000000000000000000000000";
        double version = 1.0;
        IPatientMedSuppDataManager cm = new StubPatientMedSuppDataManager();

        [TestMethod]
        public void GetPatientMedSupps_Test()
        {
            GetPatientMedSuppsDataRequest request = new GetPatientMedSuppsDataRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version,
                PatientId = "",
                StatusIds = new List<int> { 1, 2}
            };
            List<PatientMedSuppData> response = cm.GetPatientMedSupps(request);
            Assert.IsTrue(response.Count == 2);
        }

        [TestMethod]
        public void SavePatientMedSupps_Test()
        {
            PutPatientMedSuppDataRequest request = new PutPatientMedSuppDataRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version,
                Insert = true,
                PatientMedSuppData = new PatientMedSuppData { 
                    CategoryId = 1,
                    TypeId = "545bdfa6d433232248966639",
                    DeleteFlag = false,
                    Dosage = "2",
                    EndDate = DateTime.Now.AddDays(3),
                    StartDate = DateTime.Now,
                    Form = "TABLET",
                    Name = "ASPIRIN",
                    Notes = "This is a test note",
                    PatientId = "54bdde96fe7a5b27384a9551",
                    PrescribedBy = "Dr. Louis",
                    Reason = "This is a test reason",
                    Route = "ORAL",
                    SystemName = "Engage",
                    Strength = "30 mg"
                }
            };
            PatientMedSuppData response = cm.SavePatientMedSupps(request);
            Assert.IsTrue(response.Id == "54bdde96fe7a5b27384aaad9");
        }

        [TestMethod]
        public void DeletePatientMedSupps_Test()
        {
            DeleteMedSuppsByPatientIdDataRequest request = new DeleteMedSuppsByPatientIdDataRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version,
                PatientId = "54bdde96fe7a5b27384a9b76",
                Id = "54c2bd30d433271e40fb9f11"
            };
            DeleteMedSuppsByPatientIdDataResponse response = cm.DeletePatientMedSupps(request);
            Assert.IsTrue(response.DeletedIds.Count == 1);
        }

        [TestMethod]
        public void UndoDeletePatientMedSupps_Test()
        {
            UndoDeletePatientMedSuppsDataRequest request = new UndoDeletePatientMedSuppsDataRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version,
                Ids = new List<string> { "54c2bd30d433271e40fb9f11"}
            };
            UndoDeletePatientMedSuppsDataResponse response = cm.UndoDeletePatientMedSupps(request);
            Assert.IsTrue(response.Success);
        }
    }
}
