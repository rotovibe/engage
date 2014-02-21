using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientNote.DTO;

namespace Phytel.API.DataDomain.PatientNote.Test
{
    [TestClass]
    public class PatientNoteTest
    {
        [TestMethod]
        public void GetPatientNote_Test_Passes()
        {
            GetPatientNoteDataRequest request = new GetPatientNoteDataRequest { Id = "5307b343d433232dd4e94743" };

            PatientNoteData response = PatientNoteDataManager.GetPatientNote(request);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void GetPatientNote_Test_Fails()
        {
            GetPatientNoteDataRequest request = new GetPatientNoteDataRequest { Id = "52f5589c072ef709f84e7798" };

            PatientNoteData response = PatientNoteDataManager.GetPatientNote(request);

            Assert.IsNull(response);
        }

        [TestMethod]
        public void GetAllPatientNotes_Test_Passes()
        {
            GetAllPatientNotesDataRequest request = new GetAllPatientNotesDataRequest { PatientId = "52f5589c072ef709f84e7798", Count = 3 };

            List<PatientNoteData> response = PatientNoteDataManager.GetAllPatientNotes(request);

            Assert.IsTrue(response.Count > 0);
        }

        [TestMethod]
        public void GetAllPatientNotes_Test_Fails()
        {
            GetAllPatientNotesDataRequest request = new GetAllPatientNotesDataRequest { PatientId = "5307b343d433232dd4e94743", Count = 3 };

            List<PatientNoteData> response = PatientNoteDataManager.GetAllPatientNotes(request);

            Assert.IsTrue(response.Count == 0);
        }

        [TestMethod]
        public void InsertPatientNote_Test()
        {
            PatientNoteData n = new PatientNoteData { Text = "Note D data domain", CreatedById = "BB241C64-A0FF-4E01-BA5F-4246EF50780E", PatientId = "52f5589c072ef709f84e7798" };
            PutPatientNoteDataRequest request = new PutPatientNoteDataRequest {
                UserId = "DD_Harness",
                Version = "v1",
                PatientNote = n
            };
            string id = PatientNoteDataManager.InsertPatientNote(request);

            Assert.IsNotNull(id);
        }

        [TestMethod]
        public void DeletePatientNote_Test()
        {
            DeletePatientNoteDataRequest request = new DeletePatientNoteDataRequest
            {
                UserId = "DD_Harness",
                Version = "v1",
                Id = "5307c1f2d433232860709fef",
            };
            bool isDeleted = PatientNoteDataManager.DeletePatientNote(request);

            Assert.IsTrue(isDeleted);
        }
    }
}
