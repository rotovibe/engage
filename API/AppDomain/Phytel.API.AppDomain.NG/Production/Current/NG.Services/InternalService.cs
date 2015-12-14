using AutoMapper;
using Phytel.API.AppDomain.NG.Allergy;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.Security.DTO;
using Phytel.API.Common.Audit;
using Phytel.API.Common.CustomObject;
using Phytel.API.Common.Format;
using Phytel.API.DataAudit;
using ServiceStack.ServiceClient.Web;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Phytel.API.AppDomain.NG.DTO.Context;
using Phytel.API.AppDomain.NG.DTO.Internal;

namespace Phytel.API.AppDomain.NG.Service
{
    public partial class InternalService : ServiceStack.ServiceInterface.Service
    {
        public ISecurityManager Security {get; set;}
        public IPatientSystemManager PatientSystemManager { get; set; }
        public IAuditUtil AuditUtil { get; set; }
        public ICommonFormatterUtil CommonFormatterUtil { get; set; }
        public IServiceContext ServiceContext { get; set; }

        private const string unknownBrowserType = "Unknown browser";
        private const string unknownUserHostAddress = "Unknown IP";
    }
}