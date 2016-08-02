using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Medication.DTO;

namespace Phytel.API.DataDomain.Medication.Test
{
    [TestClass]
    public class StubMedication_Test
    {
        string context = "NG";
        string contractNumber = "InHealth001";
        string userId = "000000000000000000000000";
        double version = 1.0;
        IMedicationDataManager cm = new StubMedicationDataManager();

        [TestMethod]
        public void GetMedicationNDCs_Test()
        {
            GetMedicationNDCsDataRequest request = new GetMedicationNDCsDataRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version,
                Name = "ASPIRIN",
                Route = "ORAL",
                Strength = "30 mg",
                Form = "TABLET"
            };
            GetMedicationNDCsDataResponse response = cm.GetMedicationNDCs(request);
            Assert.IsTrue(response.NDCcodes.Count == 2);
        } 
    }
}
