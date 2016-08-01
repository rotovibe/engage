using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Contact.CareTeam
{
    public interface ICareTeamRepositoryFactory
    {
        ICareTeamRepository GetCareTeamRepository(IDataDomainRequest request, RepositoryType type);
    }
}
