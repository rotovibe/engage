using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientNote.DTO;

namespace Phytel.API.DataDomain.PatientNote.Test
{
    [TestClass]
    public class PatientNoteTest
    {
        //[TestMethod]
        //public void GetPatientNoteByID()
        //{
        //    GetPatientNoteRequest request = new GetPatientNoteRequest{ PatientNoteID = "5"};

        //    GetPatientNoteResponse response = PatientNoteDataManager.GetPatientNoteByID(request);

        //    Assert.IsTrue(response.PatientNote.PatientNoteID == "Tony");
        //}

        [TestMethod]
        public void InsertPatientNote_Test()
        {
            PatientNoteData n = new PatientNoteData { Text = "My first note through the data domain", CreatedBy = "53043e53d433231f48de8a7a", PatientId = "52f55877072ef709f84e69b0" };
            PutPatientNoteDataRequest request = new PutPatientNoteDataRequest {
                UserId = "DD_Harness",
                Version = "v1",
                PatientNote = n
            };
            bool isInserted = PatientNoteDataManager.InsertPatientNote(request);

            Assert.IsTrue(isInserted);
        }
    }
}
