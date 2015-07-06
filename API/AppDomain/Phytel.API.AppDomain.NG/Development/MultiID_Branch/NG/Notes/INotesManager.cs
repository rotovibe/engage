using System;
using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO;

namespace Phytel.API.AppDomain.NG.Notes
{
    public interface INotesManager
    {
        IUtilizationManager UtilManager { get; set; }
        PatientNote GetPatientNote(GetPatientNoteRequest request);
        List<PatientNote> GetAllPatientNotes(GetAllPatientNotesRequest request);
        PostPatientNoteResponse InsertPatientNote(PostPatientNoteRequest request);
        PostDeletePatientNoteResponse DeletePatientNote(PostDeletePatientNoteRequest request);
        UpdatePatientNoteResponse UpdatePatientNote(UpdatePatientNoteRequest request);
        void LogException(Exception ex);
    }
}