using System.Collections.Generic;
using DataDomain.PatientNote.Repo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientNote.DTO;
using Phytel.API.DataDomain.PatientNote.Repo;

namespace Phytel.API.DataDomain.PatientNote.Test
{
    [TestClass]
    public class PatientNoteTest
    {
        IPatientNoteDataManager m = new PatientNoteDataManager(new PatientNoteRepositoryFactory("InHealth001", "123456789012345678901234") );
        
        [TestMethod]
        public void GetPatientNote_Test_Passes()
        {
            GetPatientNoteDataRequest request = new GetPatientNoteDataRequest { Id = "530fb4d9d433230ea4e8bdfa" };

            PatientNoteData response = m.GetPatientNote(request);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void GetPatientNote_Test_Fails()
        {
            GetPatientNoteDataRequest request = new GetPatientNoteDataRequest { Id = "52f5589c072ef709f84e7798" };

            PatientNoteData response = m.GetPatientNote(request);

            Assert.IsNull(response);
        }

        [TestMethod]
        public void GetAllPatientNotes_Test_Passes()
        {
            GetAllPatientNotesDataRequest request = new GetAllPatientNotesDataRequest { PatientId = "52f5589c072ef709f84e7798", Count = 3 };

            List<PatientNoteData> response = m.GetAllPatientNotes(request);

            Assert.IsTrue(response.Count > 0);
        }

        [TestMethod]
        public void GetAllPatientNotes_Test_Fails()
        {
            GetAllPatientNotesDataRequest request = new GetAllPatientNotesDataRequest { PatientId = "5307b343d433232dd4e94743", Count = 3 };

            List<PatientNoteData> response = m.GetAllPatientNotes(request);

            Assert.IsTrue(response.Count == 0);
        }

        [TestMethod]
        public void InsertPatientNote_Test()
        {
            PatientNoteData n = new PatientNoteData { Text = "Note D data domain", PatientId = "531f2dcf072ef727c4d2a150", CreatedById = "531f2df5072ef727c4d2a3bc" };
            InsertPatientNoteDataRequest request = new InsertPatientNoteDataRequest {
                Version = 1,
                PatientNote = n
            };
            string id = m.InsertPatientNote(request);

            Assert.IsNotNull(id);
        }

        [TestMethod]
        public void DeletePatientNote_Test()
        {
            DeletePatientNoteDataRequest request = new DeletePatientNoteDataRequest
            {
                UserId = "DD_Harness",
                Version = 1,
                Id = "5307c1f2d433232860709fef",
            };
            bool isDeleted = m.DeletePatientNote(request);

            Assert.IsTrue(isDeleted);
        }
    }
}
