using System;
using System.Reflection;
using System.Web;
using AppDomain.Engage.Population.Service.Container;
using Phytel.API.AppDomain.Platform.Security.DTO;
using Phytel.API.ASE.Client.Interface;
using Phytel.API.Interface;
using Phytel.Services.API.Platform;
using Phytel.Services.API.Platform.Filter;
using ServiceStack;
using ServiceStack.Api.Swagger;
using ServiceStack.Common;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.Admin;
using ServiceStack.ServiceInterface.Validation;
using ServiceStack.Text;
using ServiceStack.WebHost.Endpoints;
using ServiceStack.WebHost.Endpoints.Extensions;
using ServiceStack.WebHost.Endpoints.Support;

namespace AppDomain.Engage.Population.Service
{
    public class IntegrationAppHost : AppHostBase
    {
        public IntegrationAppHost() : base("Engage Integration Services", Assembly.GetExecutingAssembly()) { }

        public override void Configure(Funq.Container container)
        {
            
            Plugins.Add(new ValidationFeature());
            HttpServiceContainer.Build(container);
            PlatformServiceContainer.Build(container);

            var auditLogger = container.TryResolve<IAuditLogger>() ??
                                  new AuditLogger(container.TryResolve<IASEClient>(), container.TryResolve<ITokenManager>());

            auditLogger.SupressAuditingForRequestDtoTypes = new Type[] { typeof(GetNotificationsBySessionIdRequest) };
            auditLogger.DomainName = "Engage Integration Services";

            Plugins.Add(new RequestLogsFeature
            {
                RequestLogger = auditLogger

            });

            // request filtering for setting global vals.
            RequestFilters.Add((req, res, requestDto) =>
            {
            //{ServiceStack.Api.Swagger.Resources}
                if (req.OperationName != "Resources" && req.OperationName != "ResourceRequest")
                {
                    if (requestDto.GetType() == typeof (Resources) ||
                        requestDto.GetType() == typeof (ResourceRequest))
                        return;
                    HostContext.Instance.Items.Add("Contract", ((IAppDomainRequest) requestDto).ContractNumber);
                    HostContext.Instance.Items.Add("Version", ((IAppDomainRequest) requestDto).Version);
                }
            });

            RequestFilters.Add((req, res, requestDto) =>
            {
                var obj = req.ResponseContentType;
            });

            var emitGlobalHeadersHandler = new CustomActionHandler((httpReq, httpRes) => httpRes.EndRequest());
            SetConfig(new EndpointHostConfig
            {
                RawHttpHandlers =
                    {
                        (httpReq) => httpReq.HttpMethod == HttpMethods.Options ? emitGlobalHeadersHandler : null
                    },
                GlobalResponseHeaders =
                    {
                        //{"Access-Control-Allow-Origin", "*"},
                        {"Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS"},
                        {"Access-Control-Allow-Headers", "Content-Type"},
                    },
                AllowJsonpRequests = true
            });

            Plugins.Add(new SwaggerFeature());
            // initialize datetime format
            JsConfig.DateHandler = JsonDateHandler.ISO8601;
            JsConfig.EmitCamelCaseNames = true;
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
    }
}