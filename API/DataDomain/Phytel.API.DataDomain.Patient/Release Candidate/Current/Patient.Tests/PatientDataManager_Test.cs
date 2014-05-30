using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.Patient.Test.Stub;

namespace Phytel.API.DataDomain.Patient.Test
{
    [TestClass]
    public class PatientDataManager_Test
    {
        [TestMethod()]
        [TestCategory("NIGHT-911")]
        [TestProperty("TFS", "10563")]
        [TestProperty("Layer", "DD.DataManager")]
        public void GetPatientsDetails()
        {
            StubDataPatientManager pm = new StubDataPatientManager { Factory = new StubPatientRepositoryFactory() };
            GetPatientsDataRequest request = new GetPatientsDataRequest
            {
                Version = 1.0,
                UserId = "000000000000000000000000",
                ContractNumber = "InHealth001",
                Context = "NG"
            };
            GetPatientsDataResponse response = pm.GetPatients(request);
            Assert.IsTrue(response.Patients.Count > 0);
        }
    }
}
