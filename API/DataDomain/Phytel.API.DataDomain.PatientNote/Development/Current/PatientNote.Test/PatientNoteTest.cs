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
            PatientNoteData n = new PatientNoteData { Text = "My second note", CreatedBy = "53043e53d433231f48de8a7a" };
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
