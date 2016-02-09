using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceHost;

namespace Phytel.API.Interface
{
    public interface ISortableRequest
    {
        int Skip { get; set; }     
        int Take { get; set; }
        string Sort { get; set; }
    }
}
