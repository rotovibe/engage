using System;
using System.Web;
using AutoMapper;
using Lucene.Net.Documents;
using Phytel.API.AppDomain.NG.Allergy;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Search;
using Phytel.API.AppDomain.NG.Medication;
using Phytel.API.AppDomain.NG.Observation;
using Phytel.API.AppDomain.NG.Programs;
using Phytel.API.AppDomain.NG.Search;
using Phytel.API.Common.Audit;
using Phytel.API.Common.CustomObject;
using Phytel.API.Common.Format;
using Phytel.API.DataAudit;
using Phytel.API.DataDomain.Allergy.DTO;
using Phytel.API.DataDomain.Medication.DTO;
using ServiceStack;
using ServiceStack.Common.Web;
using ServiceStack.MiniProfiler;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.Admin;
using ServiceStack.Text;
using ServiceStack.WebHost.Endpoints;
using ServiceStack.WebHost.Endpoints.Extensions;
using ServiceStack.WebHost.Endpoints.Support;

namespace Phytel.API.AppDomain.NG.Service
{
    public class Global : System.Web.HttpApplication
    {
        private static readonly bool Profile = Convert.ToBoolean(System.Web.Configuration.WebConfigurationManager.AppSettings.Get("Profiler"));

        public class NGAppHost : AppHostBase
        {

            //Tell Service Stack the name of your application and where to find your web services
            public NGAppHost()
                : base("Phytel NG Services", typeof(NGService).Assembly)
            {
            }

            public override void Configure(Funq.Container container)
            {
                //register any dependencies your services use, e.g:
                container.RegisterAutoWiredAs<SecurityManager, ISecurityManager>().ReusedWithin(Funq.ReuseScope.Request);
                container.RegisterAutoWiredAs<CommonFormatterUtil, ICommonFormatterUtil>().ReusedWithin(Funq.ReuseScope.Request);
                container.RegisterAutoWiredAs<EndpointUtils, IEndpointUtils>().ReusedWithin(Funq.ReuseScope.Request);
                container.RegisterAutoWiredAs<AllergyEndpointUtil, IAllergyEndpointUtil>().ReusedWithin(Funq.ReuseScope.Request);                
                container.RegisterAutoWiredAs<PlanElementUtils, IPlanElementUtils>().ReusedWithin(Funq.ReuseScope.Request);
                container.RegisterAutoWiredAs<NGManager, INGManager>().ReusedWithin(Funq.ReuseScope.Request);
                container.RegisterAutoWiredAs<AuditUtil, IAuditUtil>().ReusedWithin(Funq.ReuseScope.Request);
                container.RegisterAutoWiredAs<ObservationEndpointUtil, IObservationEndpointUtil>().ReusedWithin(Funq.ReuseScope.Request);
                container.RegisterAutoWiredAs<ObservationsManager, IObservationsManager>().ReusedWithin(Funq.ReuseScope.Request);
                container.RegisterAutoWiredAs<AllergyManager, IAllergyManager>().ReusedWithin(Funq.ReuseScope.Request);
                container.RegisterAutoWiredAs<MedicationManager, IMedicationManager>().ReusedWithin(Funq.ReuseScope.Request);
                container.RegisterAutoWiredAs<MedicationEndpointUtil, IMedicationEndpointUtil>().ReusedWithin(Funq.ReuseScope.Request);  
                // search
                container.RegisterAutoWiredAs<SearchManager, ISearchManager>().ReusedWithin(Funq.ReuseScope.Request);
                container.RegisterAutoWiredAs<SearchUtil, ISearchUtil>().ReusedWithin(Funq.ReuseScope.Request);

                // automapper configuration
                Mapper.CreateMap<AllergyData, DTO.Allergy>();
                Mapper.CreateMap<DTO.Allergy, AllergyData>();
                Mapper.CreateMap<PatientAllergyData, PatientAllergy>();
                Mapper.CreateMap<PatientAllergy, PatientAllergyData>();
                
                Mapper.CreateMap<Document, IdNamePair>()
                    .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Get("Id")))
                    .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Get("Name")));

                Mapper.CreateMap<Document, TextValuePair>()
                                    .ForMember(d => d.Value, opt => opt.MapFrom(src => src.Get("CompositeName").Trim()))
                                    .ForMember(d => d.Text, opt => opt.MapFrom(
                                        src => src.Get("CompositeName").Trim()));

                Mapper.CreateMap<Document, MedFieldsSearchDoc>()
                    .ForMember(d => d.ProductId, opt => opt.MapFrom(src => src.Get("Id")))
                    .ForMember(d => d.DosageFormname, opt => opt.MapFrom(src => src.Get("DosageFormname")))
                    .ForMember(d => d.CompositeName, opt => opt.MapFrom(src => src.Get("CompositeName")))
                    .ForMember(d => d.ProprietaryName, opt => opt.MapFrom(src => src.Get("ProprietaryName")))
                    .ForMember(d => d.RouteName, opt => opt.MapFrom(src => src.Get("RouteName")))
                    .ForMember(d => d.SubstanceName, opt => opt.MapFrom(src => src.Get("SubstanceName")))
                    .ForMember(d => d.Strength, opt => opt.MapFrom(src => src.Get("Strength")))
                    .ForMember(d => d.Unit, opt => opt.MapFrom(src => src.Get("Unit")));

                Mapper.CreateMap<DTO.Allergy, IdNamePair>().ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name.Trim().ToUpper().Replace("\"", "").Replace(",", "")));

                Mapper.CreateMap<PatientMedSuppData, PatientMedSupp>();
                Mapper.CreateMap<PatientMedSupp, PatientMedSuppData>();

                Plugins.Add(new RequestLogsFeature() { RequiredRoles = new string[] { } });

                var emitGlobalHeadersHandler = new CustomActionHandler((httpReq, httpRes) => httpRes.EndRequest());
                SetConfig(new EndpointHostConfig
                {
                    RawHttpHandlers = { (httpReq) => httpReq.HttpMethod == HttpMethods.Options ? emitGlobalHeadersHandler : null },
                    GlobalResponseHeaders = { 
                    //{"Access-Control-Allow-Origin", "*"},
                    { "Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS" }, 
                    { "Access-Control-Allow-Headers", "Content-Type" }, },
                    AllowJsonpRequests = true
                });

                // initialize datetime format
                JsConfig.DateHandler = JsonDateHandler.ISO8601;
            }
        }

        public class CustomActionHandler : IServiceStackHttpHandler, IHttpHandler
        {
            public Action<IHttpRequest, IHttpResponse> Action { get; set; }

            public CustomActionHandler(Action<IHttpRequest, IHttpResponse> action)
            {
                if (action == null)
                    throw new Exception("Action was not supplied to ActionHandler");

                Action = action;
            }

            public void ProcessRequest(IHttpRequest httpReq, IHttpResponse httpRes, string operationName)
            {
                Action(httpReq, httpRes);
            }

            public void ProcessRequest(HttpContext context)
            {
                ProcessRequest(context.Request.ToRequest(GetType().Name),
                    context.Response.ToResponse(),
                    GetType().Name);
            }

            public bool IsReusable
            {
                get { return false; }
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            new NGAppHost().Init();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (Profile)
                Profiler.Start();
        }

        protected void Application_EndRequest(object src, EventArgs e)
        {
            if (Profile)
                Profiler.Stop();
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}