using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.PatientGoal.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientGoal
{
    public class AttributeRepositoryFactory : IAttributeRepositoryFactory
    {
        public IAttributeRepository GetRepository(IDataDomainRequest request, RepositoryType type)
        {
            try
            {
                IAttributeRepository repo = null;

                switch (type)
                {
                    case RepositoryType.AttributeLibrary:
                    {
                        repo = new MongoAttributeLibraryRepository(request.ContractNumber);
                        break;
                    }
                }

                repo.UserId = request.UserId;
                return repo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
