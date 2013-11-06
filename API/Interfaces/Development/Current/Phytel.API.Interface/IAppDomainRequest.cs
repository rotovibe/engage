using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.Interface
{
    public interface IAppDomainRequest
    {
        string VersionID { get; set; }
        string ContractNumber { get; set; }
        string Product { get; set; }
        string Token { get; set; }
    }
}
