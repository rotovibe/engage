using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG
{
    public class CohortRuleCheckData
    {
        public string PatientId { get; set; }
        public string ContactId { get; set; }
        public string ContractNumber { get; set; }
        public string UserId { get; set; }
        public double Version { get; set; }
        public List<string> UsersContactIds { get; set; }
    }    
}
