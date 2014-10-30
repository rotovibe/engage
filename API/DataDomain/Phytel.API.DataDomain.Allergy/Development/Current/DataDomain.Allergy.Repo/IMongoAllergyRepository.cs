using Phytel.API.Interface;

namespace DataDomain.Allergy.Repo
{
    public interface IMongoAllergyRepository : IRepository
    {
        object Initialize(object newEntity);
    }
}