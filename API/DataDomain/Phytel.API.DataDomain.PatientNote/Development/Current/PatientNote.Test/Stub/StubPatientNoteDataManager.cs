using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.PatientNote.DTO;

namespace Phytel.API.DataDomain.PatientNote.Test.Stubs
{
    public class StubPatientNoteDataManager : IPatientNoteDataManager
    {

        public string InsertPatientNote(DTO.InsertPatientNoteDataRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.PatientNoteData GetPatientNote(DTO.GetPatientNoteDataRequest request)
        {
            throw new NotImplementedException();
        }

        public List<DTO.PatientNoteData> GetAllPatientNotes(DTO.GetAllPatientNotesDataRequest request)
        {
            throw new NotImplementedException();
        }

        public bool DeletePatientNote(DTO.DeletePatientNoteDataRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.DeleteNoteByPatientIdDataResponse DeleteNoteByPatientId(DTO.DeleteNoteByPatientIdDataRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.UndoDeletePatientNotesDataResponse UndoDeletePatientNotes(DTO.UndoDeletePatientNotesDataRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.RemoveProgramInPatientNotesDataResponse RemoveProgramInPatientNotes(DTO.RemoveProgramInPatientNotesDataRequest request)
        {
            throw new NotImplementedException();
        }

        public PatientNoteData UpdatePatientNote(DTO.UpdatePatientNoteDataRequest request)
        {
            throw new NotImplementedException();
        }


        public UpsertBatchPatientNotesDataResponse UpsertBatchPatientNotes(UpsertBatchPatientNotesDataRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
