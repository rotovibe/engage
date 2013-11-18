using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class ProblemsLookUpResponse
    {
        public List<ProblemLookUp> Problems { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class ProblemLookUp
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }
}
