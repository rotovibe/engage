using System;
using DataDomain.Allergy.Repo;
using Phytel.API.DataDomain.Allergy.DTO;
using Phytel.API.Interface;
using ServiceStack.WebHost.Endpoints;

namespace Phytel.API.DataDomain.Allergy.Test
{
    public abstract class StubRepositoryFactory
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
                        repo = new StubAllergyRepository<AllergyMongoContext>(context) {UserId = request.UserId, ContractDBName = request.ContractNumber};
                        break;
                    }
                    case RepositoryType.PatientAllergy:
                    {
                        //var db = AppHostBase.Instance.Container.ResolveNamed<string>(Constants.Domain);
                        var context = new AllergyMongoContext(request.ContractNumber);
                        repo = new StubPatientAllergyRepository<AllergyMongoContext>(context) { UserId = request.UserId, ContractDBName = request.ContractNumber };
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
