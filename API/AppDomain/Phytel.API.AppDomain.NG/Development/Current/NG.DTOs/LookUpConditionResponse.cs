using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class LookUpConditionsResponse
    {
        public List<LookUpCondition> Conditions { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class LookUpCondition
    {
        public string ConditionID { get; set; }
        public string DisplayName { get; set; }
    }
}
