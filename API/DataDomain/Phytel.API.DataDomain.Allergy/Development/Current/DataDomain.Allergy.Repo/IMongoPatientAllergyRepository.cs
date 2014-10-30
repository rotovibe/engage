using Phytel.API.Interface;

namespace DataDomain.Allergy.Repo
{
    public interface IMongoPatientAllergyRepository : IRepository
    {
        object Initialize(object newEntity);
        object FindByPatientId(string entityID);
    }
}