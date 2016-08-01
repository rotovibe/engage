using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.LookUp
{
    public interface ILookUpRepositoryFactory
    {
        ILookUpRepository GetRepository(IDataDomainRequest request, RepositoryType type);
    }
}
