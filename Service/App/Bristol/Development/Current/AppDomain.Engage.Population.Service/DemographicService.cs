using System;
using System.Collections.Generic;
using System.Web.Services.Description;
using AppDomain.Engage.Population.DTO.Context;
using AppDomain.Engage.Population.DTO.Demographics;
using Phytel.API.AppDomain.Platform.Security.DTO;
using Phytel.API.ASE.Client.Interface;
using Phytel.API.Common.Audit;
using Phytel.API.DataAudit;
using Phytel.Services.API.Platform.Filter;
using Phytel.Services.API.Platform.Filter.Attributes;
using Phytel.Services.API.Provider;
using ServiceStack.ServiceInterface.ServiceModel;

namespace AppDomain.Engage.Population.Service
{
    [IsAuthenticatedFilter("br")]
    public class DemographicService : ServiceBase
    {
        protected readonly IHostContextProxy _hostContextProxy;
        protected readonly IAuditLogger _auditLogger;
        private readonly IASEClient _client;
        private UserContext _userContext { get; set; }
        public IDemographicsManager Manager { get; set; }
        public IAuditHelpers AuditHelper { get; set; }

        public DemographicService(IHostContextProxy hostContextProxy, IAuditLogger auditLogger, IASEClient client)
        {
            _hostContextProxy = hostContextProxy;
            _userContext = BuildUserContext(_hostContextProxy.GetItem(FilterAttributesConstants.UserSessionName) as AppDomainSessionInfo);
            _auditLogger = auditLogger;
            _client = client;
        }

        public PostPatientDemographicsResponse Post(PostPatientDemographicsRequest request)
        {
            Manager.UserContext = _userContext; // initialize this for every service handler.
            try
            {
                //Manager.DoSomething();
                var status = new ResponseStatus {Message = "success!"};
                var response = new PostPatientDemographicsResponse();
                response.ResponseStatus = status;

                // use this to log any patients that you might see.
                //_auditLogger.Patients = new List<string> { "patient1", "patient2", "patient3" };

                return response;
            }
            catch (Exception ex)
            {
                FormatException(request, new PostPatientDemographicsResponse(), _client, ex);
            }
            return new PostPatientDemographicsResponse();
        }
    }
}