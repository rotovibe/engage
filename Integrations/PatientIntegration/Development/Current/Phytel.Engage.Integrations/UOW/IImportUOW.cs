using System.Collections.Generic;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientNote.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.Engage.Integrations.Repo.DTOs;

namespace Phytel.Engage.Integrations.UOW
{
    public interface IImportUow
    {
        List<PatientSystemData> PatientSystems { get; set; }
        List<PatientData> Patients { get; set; }
        List<PatientNoteData> PatientNotes { get; set; }
        List<PCPPhone> PCPPhones { get; set; } 
        void Commit(string contract);
        void LoadPatientSystems(Repo.Repositories.IRepository xrepo, List<PatientSystemData> systems);
        void LoadPatients(Repo.Repositories.IRepository repo, List<PatientData> pats);
        void LoadPatientNotes(Repo.Repositories.IRepository repo, List<PatientData> pats, List<PatientNoteData> notes);
        void LoadPcpPhones(Repo.Repositories.IRepository xrepo, List<PCPPhone> pcpPhones);
    }
}