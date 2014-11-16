using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Medication
{
    public interface IMedicationRepositoryFactory
    {
        IMedicationRepository GetRepository(IDataDomainRequest request, RepositoryType type);
    }
}