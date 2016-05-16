using System.Linq;
using Phytel.Engage.Integrations.Repo.DTO;
using Phytel.Engage.Integrations.Repo.DTOs;

namespace Phytel.Engage.Integrations.Repo.Bridge
{
    public interface IQueryImplementation
    {
        IQueryable<PatientInfo> GetPatientInfoQuery(ContractEntities ct);
        IQueryable<PCPPhone> GetPCPPhoneQuery(ContractEntities ct);
        IQueryable<PCPPhone> GetPCPPhoneQueryGeneral(ContractEntities db);
        IQueryable<PatientXref> GetPatientXrefQuery(ContractEntities ct);
        IQueryable<PatientNote> GetPatientNotesQuery(ContractEntities ct);
    }
}
