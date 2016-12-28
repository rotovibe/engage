using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Patient.DTO;

namespace NGDataImport.Test
{
    [TestClass]
    public class ImporterTest
    {
        Importer import = new Importer(@"http://localhost:8888/", "NG", 1.0, "Adventist001", "1234");
        [TestMethod]
        public void GetPatientData()
        {
            GetPatientDataByNameDOBRequest patientDataRequest = new GetPatientDataByNameDOBRequest
            {
                FirstName = "Wendell_01",
                LastName = "Pickering",
                DOB = "03/11/1960"                
            };
            GetPatientDataResponse patientDataResponse = import.GetPatientData(patientDataRequest);
            Assert.IsNotNull(patientDataResponse.Patient);
        }
    }
}
