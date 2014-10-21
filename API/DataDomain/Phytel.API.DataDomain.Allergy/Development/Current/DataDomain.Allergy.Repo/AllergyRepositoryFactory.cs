using System;
using Phytel.API.DataDomain.Allergy.DTO;
using Phytel.API.Interface;
using ServiceStack.WebHost.Endpoints;

namespace DataDomain.Allergy.Repo
{
    public enum RepositoryType
    {
        Allergy
    }

    public abstract class AllergyRepositoryFactory
    {

        public static IMongoAllergyRepository GetAllergyRepository(IDataDomainRequest request, RepositoryType type)
        {
            try
            {
                IMongoAllergyRepository repo = null;

                switch (type)
                {
                    case RepositoryType.Allergy:
                    {
                        var db = AppHostBase.Instance.Container.ResolveNamed<string>(Constants.Domain);
                        var context = new AllergyMongoContext(db);
                        repo = new MongoAllergyRepository<AllergyMongoContext>(context) {UserId = request.UserId};
                        break;
                    }
                }
                return repo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
