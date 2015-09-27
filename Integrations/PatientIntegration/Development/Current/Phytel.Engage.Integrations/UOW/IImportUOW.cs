using System.Collections.Generic;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;

namespace Phytel.Engage.Integrations.UOW
{
    public interface IImportUow
    {
        List<PatientSystemData> PatientSystems { get; set; }
        List<PatientData> Patients { get; set; }
        void Commit(string contract);
        void LoadPatientSystems(Repo.Repositories.IRepository xrepo);
        void LoadPatients(Repo.Repositories.IRepository repo);
    }
}