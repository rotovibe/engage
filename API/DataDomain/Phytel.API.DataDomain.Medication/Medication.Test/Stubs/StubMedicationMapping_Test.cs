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
    public class StubMedicationMapping_Test
    {
        string context = "NG";
        string contractNumber = "InHealth001";
        string userId = "000000000000000000000000";
        double version = 1.0;
        IMedicationMappingDataManager cm = new StubMedicationMappingDataManager();

        [TestMethod]
        public void InitializeMedicationMap_Test()
        {
            PutInitializeMedicationMapDataRequest request = new PutInitializeMedicationMapDataRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version,
                MedicationMapData = new MedicationMapData { FullName = "TESLA", Verified = false, Custom = true}
            };
            MedicationMapData data = cm.InitializeMedicationMap(request);
            Assert.IsNotNull(data.TTLDate);
        }


        [TestMethod]
        public void UpdateMedicationMap_Test()
        {
            PutMedicationMapDataRequest request = new PutMedicationMapDataRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version,
                MedicationMapData = new MedicationMapData { Id = "532371e4072ef721b8b05b97", FullName = "TESLA1", Verified = false, Custom = true }
            };
            MedicationMapData data = cm.UpdateMedicationMap(request);
            Assert.IsNull(data.TTLDate);
        } 
    }
}
