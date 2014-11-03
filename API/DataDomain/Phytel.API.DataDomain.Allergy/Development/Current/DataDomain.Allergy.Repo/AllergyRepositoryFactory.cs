using System;
using Phytel.API.DataDomain.Allergy.DTO;
using Phytel.API.Interface;
using ServiceStack.WebHost.Endpoints;

namespace DataDomain.Allergy.Repo
{
    public enum RepositoryType
    {
        Allergy,
        PatientAllergy
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
                        //var db = AppHostBase.Instance.Container.ResolveNamed<string>(Constants.Domain);
                        var context = new AllergyMongoContext(request.ContractNumber);
                        repo = new MongoAllergyRepository<AllergyMongoContext>(context) {UserId = request.UserId, ContractDBName = request.ContractNumber};
                        break;
                    }
                    case RepositoryType.PatientAllergy:
                    {
                        //var db = AppHostBase.Instance.Container.ResolveNamed<string>(Constants.Domain);
                        var context = new AllergyMongoContext(request.ContractNumber);
                        repo = new MongoPatientAllergyRepository<AllergyMongoContext>(context) { UserId = request.UserId, ContractDBName = request.ContractNumber };
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
