using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Common;
using Phytel.API.DataDomain.PatientNote.DTO;

namespace Phytel.API.DataDomain.PatientNote
{
    public interface IPatientNoteDataManager
    {
        string InsertPatientNote(InsertPatientNoteDataRequest request);
        PatientNoteData GetPatientNote(GetPatientNoteDataRequest request);
        List<PatientNoteData> GetAllPatientNotes(GetAllPatientNotesDataRequest request);
        bool DeletePatientNote(DeletePatientNoteDataRequest request);
        DeleteNoteByPatientIdDataResponse DeleteNoteByPatientId(DeleteNoteByPatientIdDataRequest request);
        UndoDeletePatientNotesDataResponse UndoDeletePatientNotes(UndoDeletePatientNotesDataRequest request);
        RemoveProgramInPatientNotesDataResponse RemoveProgramInPatientNotes(RemoveProgramInPatientNotesDataRequest request);
        PatientNoteData UpdatePatientNote(UpdatePatientNoteDataRequest request);
        List<HttpObjectResponse<PatientNoteData>> InsertBatchPatientNotes(InsertBatchPatientNotesDataRequest request);
    }
}
