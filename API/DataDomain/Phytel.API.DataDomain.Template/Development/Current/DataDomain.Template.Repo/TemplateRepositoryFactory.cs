using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDomain.Template.Repo;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Template
{
        public enum RepositoryType
        {
            Template
        }

    public abstract class TemplateRepositoryFactory
    {

        //public static ITemplateRepository GetTemplateRepository(IDataDomainRequest request, RepositoryType type)
        //{
        //    try
        //    {
        //        ITemplateRepository repo = null;

        //        switch (type)
        //        {
        //            case RepositoryType.Template:
        //                {
        //                    repo = new MongoTemplateRepository(request.ContractNumber);
        //                    break;
        //                }
        //        }

        //        repo.UserId = request.UserId;
        //        return repo;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
