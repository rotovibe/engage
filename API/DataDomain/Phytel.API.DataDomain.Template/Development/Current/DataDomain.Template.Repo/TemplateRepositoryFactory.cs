using System;
using Phytel.API.DataDomain.Template.DTO;
using Phytel.API.Interface;
using ServiceStack.WebHost.Endpoints;

namespace DataDomain.Template.Repo
{
    public enum RepositoryType
    {
        Template
    }

    public abstract class TemplateRepositoryFactory
    {

        public static IMongoTemplateRepository GetTemplateRepository(IDataDomainRequest request, RepositoryType type)
        {
            try
            {
                IMongoTemplateRepository repo = null;

                switch (type)
                {
                    case RepositoryType.Template:
                    {
                        var context = new TemplateMongoContext(request.ContractNumber);
                        repo = new MongoTemplateRepository<TemplateMongoContext>(context){UserId = request.UserId,ContractDBName = request.ContractNumber};
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
