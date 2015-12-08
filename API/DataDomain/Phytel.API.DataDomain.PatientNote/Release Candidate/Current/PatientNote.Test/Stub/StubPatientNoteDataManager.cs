using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Common;
using Phytel.API.DataDomain.PatientNote.DTO;

namespace Phytel.API.DataDomain.PatientNote.Test.Stubs
{
    public class StubPatientNoteDataManager : IPatientNoteDataManager
    {
        public string InsertPatientNote(InsertPatientNoteDataRequest request)
        {
            throw new NotImplementedException();
        }

        public PatientNoteData GetPatientNote(GetPatientNoteDataRequest request)
        {
            throw new NotImplementedException();
        }

        public List<PatientNoteData> GetAllPatientNotes(GetAllPatientNotesDataRequest request)
        {
            throw new NotImplementedException();
        }

        public bool DeletePatientNote(DeletePatientNoteDataRequest request)
        {
            throw new NotImplementedException();
        }

        public DeleteNoteByPatientIdDataResponse DeleteNoteByPatientId(DeleteNoteByPatientIdDataRequest request)
        {
            throw new NotImplementedException();
        }

        public UndoDeletePatientNotesDataResponse UndoDeletePatientNotes(UndoDeletePatientNotesDataRequest request)
        {
            throw new NotImplementedException();
        }

        public RemoveProgramInPatientNotesDataResponse RemoveProgramInPatientNotes(RemoveProgramInPatientNotesDataRequest request)
        {
            throw new NotImplementedException();
        }

        public PatientNoteData UpdatePatientNote(UpdatePatientNoteDataRequest request)
        {
            throw new NotImplementedException();
        }

        public List<HttpObjectResponse<PatientNoteData>> InsertBatchPatientNotes(InsertBatchPatientNotesDataRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
