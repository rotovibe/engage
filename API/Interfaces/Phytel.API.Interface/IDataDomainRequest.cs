using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.Interface
{
    public interface IDataDomainRequest
    {
        double Version { get; set; }
        string ContractNumber { get; set; }
        string Context { get; set; }
        string UserId { get; set; }
    }
}
