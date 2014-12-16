using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Contract.DTO;
using Phytel.API.Interface;
using MongoDB.Bson;
using Phytel.API.Common.Audit;

namespace Phytel.API.DataDomain.Contract.Repository
{
    public interface IContractRepository : IRepository
    {
        object FindContracts(GetContractsDataRequest request);
    }
}
