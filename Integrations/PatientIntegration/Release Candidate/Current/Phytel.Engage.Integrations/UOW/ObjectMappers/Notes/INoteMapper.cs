using Phytel.API.DataDomain.PatientNote.DTO;
using Phytel.Engage.Integrations.Repo.DTOs;

namespace Phytel.Engage.Integrations.UOW.Notes
{
    public interface INoteMapper
    {
        PatientNoteData MapPatientNote(string patientMongoId, PatientNote n);
        string GetNoteType(int actionId, int categoryId);
        string SetNoteTypeForGeneral(int categoryId);
    }
}