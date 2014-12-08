using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.Interface
{
    public interface IDomainResponse
    {
        double Version { get; set; }
        ResponseStatus Status { get; set; }
    }
}
