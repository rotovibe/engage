using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Contact
{
    public class ContactRepositoryFactory : IContactRepositoryFactory
    {
        public IContactRepository GetRepository(IDataDomainRequest request, RepositoryType type)
        {
            IContactRepository repo = null;

            switch (type)
            {
                case RepositoryType.Contact:
                    {
                        repo = new MongoContactRepository(request.ContractNumber) as IContactRepository;
                        break;
                    }
            }

            repo.UserId = request.UserId;
            return repo;
        }

        //public IContactRepository GetContactRepository(string dbName, string productName, string userId)
        //{
        //    try
        //    {
        //        IContactRepository repo = null;

        //        //We only have 1 repository at this time, just return it
        //        repo = new MongoContactRepository(dbName) as IContactRepository;
        //        repo.UserId = userId;
        //        return repo;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
