using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.PatientGoal.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientGoal
{
    public interface IAttributeRepositoryFactory
    {
        IAttributeRepository GetRepository(IDataDomainRequest request, RepositoryType type);
    }
}
