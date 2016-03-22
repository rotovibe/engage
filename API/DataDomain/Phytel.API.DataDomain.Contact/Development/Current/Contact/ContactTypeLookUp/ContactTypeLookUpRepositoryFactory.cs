namespace Phytel.API.DataDomain.Contact.ContactTypeLookUp
{
    public class ContactTypeLookUpRepositoryFactory : IContactTypeLookUpRepositoryFactory 
    {
        public IContactTypeLookUpRepository GetContactTypeLookUpRepository(Interface.IDataDomainRequest request, RepositoryType type)
        {
            var repo = new MongoContactTypeLookUpRepository(request.ContractNumber) { UserId = request.UserId };

            return repo;
        }
    }
}
