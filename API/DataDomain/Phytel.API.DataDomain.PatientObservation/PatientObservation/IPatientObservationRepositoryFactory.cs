using Phytel.API.DataDomain.PatientObservation.DTO;
using Phytel.API.DataDomain.PatientObservation.MongoDB.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientObservation
{
    public interface IPatientObservationRepositoryFactory
    {
        IPatientObservationRepository GetRepository(IDataDomainRequest request, RepositoryType type);
    }
}