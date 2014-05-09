using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.API.AppDomain.NG.PlanCOR
{
    public class PlanElementEventArg :EventArgs
    {
        internal IPlanElement PlanElement {get; set;}
        internal Program Program { get; set; }
        internal string UserId { get; set; }
        internal string PatientId { get; set; }
        internal Actions Action { get; set; }
        internal IAppDomainRequest DomainRequest { get; set; }
    }
}
