using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.Common.Audit
{
    [Route("/{Context}/{Version}/{ContractNumber}/patient", "POST")]
    public class DispatchRequest
    {
        [ApiMember(Name = "ObjectID", Description = "The object ID for the entity", ParameterType = "path", DataType = "string")]
        public string ObjectID { get; set; }
    }
}
