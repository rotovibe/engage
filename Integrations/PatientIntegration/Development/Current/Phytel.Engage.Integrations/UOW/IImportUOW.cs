using System.Collections.Generic;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientNote.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.Engage.Integrations.Repo.DTOs;
using Phytel.Engage.Integrations.UOW.Notes;

namespace Phytel.Engage.Integrations.UOW
{
    public interface IImportUow
    {
        List<PatientSystemData> PatientSystems { get; set; }
        List<PatientData> Patients { get; set; }
        List<PatientNoteData> PatientNotes { get; set; }
        List<PCPPhone> PCPPhones { get; set; } 
        void Commit(string contract);
        void Initialize(string contractDb);
        void LoadPatientSystems(Repo.Repositories.IRepository xrepo, List<PatientSystemData> systems);
        void LoadPatients(Repo.Repositories.IRepository repo, List<PatientData> pats);
        void LoadPatientNotes(List<PatientNote> patientNotes, List<PatientData> pats, List<PatientNoteData> notes, INoteMapper mapper);
        void LoadPcpPhones(Repo.Repositories.IRepository xrepo, List<PCPPhone> pcpPhones);
    }
}