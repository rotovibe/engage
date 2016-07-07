namespace Phytel.API.DataDomain.Contact.CareTeam
{
    public class CareTeamRepositoryFactory : ICareTeamRepositoryFactory
    {
        public ICareTeamRepository GetCareTeamRepository(Interface.IDataDomainRequest request, RepositoryType type)
        {
            var repo = new MongoCareTeamRepository(request.ContractNumber) { UserId = request.UserId };

            return repo;
            
        }
    }
}
