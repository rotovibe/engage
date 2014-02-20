using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientNote.DTO;

namespace Phytel.API.DataDomain.PatientNote.Test
{
    [TestClass]
    public class PatientNoteTest
    {
        [TestMethod]
        public void GetPatientNoteByID()
        {
            GetPatientNoteRequest request = new GetPatientNoteRequest{ PatientNoteID = "5"};

            GetPatientNoteResponse response = PatientNoteDataManager.GetPatientNoteByID(request);

            Assert.IsTrue(response.PatientNote.PatientNoteID == "Tony");
        }
    }
}
