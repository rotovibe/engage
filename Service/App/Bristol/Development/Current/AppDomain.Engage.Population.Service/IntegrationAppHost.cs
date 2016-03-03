﻿using System;
using System.Reflection;
using System.Web;
using AppDomain.Engage.Population.Service.Container;
using Phytel.API.Interface;
using ServiceStack;
using ServiceStack.Api.Swagger;
using ServiceStack.Common;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.Text;
using ServiceStack.WebHost.Endpoints;
using ServiceStack.WebHost.Endpoints.Extensions;
using ServiceStack.WebHost.Endpoints.Support;

namespace AppDomain.Engage.Population.Service
{
    public class IntegrationAppHost : AppHostBase
    {
        public IntegrationAppHost(): base("Phytel Search Data Domain Services", Assembly.GetExecutingAssembly())
        {
        }

        public override void Configure(Funq.Container container)
        {
            Plugins.Add(new SwaggerFeature());

            HttpServiceContainer.Build(container);

            // request filtering for setting global vals.
            RequestFilters.Add((req, res, requestDto) =>
            {
                HostContext.Instance.Items.Add("Contract", ((IAppDomainRequest)requestDto).ContractNumber);
                HostContext.Instance.Items.Add("Version", ((IAppDomainRequest)requestDto).Version);
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

            // initialize datetime format
            JsConfig.DateHandler = JsonDateHandler.ISO8601;
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