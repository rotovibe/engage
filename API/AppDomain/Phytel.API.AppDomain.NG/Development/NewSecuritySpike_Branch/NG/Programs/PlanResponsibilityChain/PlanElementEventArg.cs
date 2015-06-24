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
        public IPlanElement PlanElement {get; set;}
        public Program Program { get; set; }
        public string UserId { get; set; }
        public string PatientId { get; set; }
        public Actions Action { get; set; }
        public IAppDomainRequest DomainRequest { get; set; }
    }
}
