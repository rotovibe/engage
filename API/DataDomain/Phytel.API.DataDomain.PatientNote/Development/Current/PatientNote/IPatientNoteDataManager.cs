using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.PatientNote.DTO;

namespace Phytel.API.DataDomain.PatientNote
{
    public interface IPatientNoteDataManager
    {
        string InsertPatientNote(PutPatientNoteDataRequest request);
        PatientNoteData GetPatientNote(GetPatientNoteDataRequest request);
        List<PatientNoteData> GetAllPatientNotes(GetAllPatientNotesDataRequest request);
        bool DeletePatientNote(DeletePatientNoteDataRequest request);
        DeleteNoteByPatientIdDataResponse DeleteNoteByPatientId(DeleteNoteByPatientIdDataRequest request);
        UndoDeletePatientNotesDataResponse UndoDeletePatientNotes(UndoDeletePatientNotesDataRequest request);
        RemoveProgramInPatientNotesDataResponse RemoveProgramInPatientNotes(RemoveProgramInPatientNotesDataRequest request);
    }
}
