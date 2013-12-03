﻿using Phytel.API.AppDomain.Security.DTO;
using Phytel.API.Common.Audit;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;
using System.Configuration;

namespace Phytel.API.AppDomain.NG
{
    public abstract class ManagerBase
    {
        protected static readonly string ADSecurityServiceURL = ConfigurationManager.AppSettings["ADSecurityServiceUrl"];

        public bool IsUserValidated(string version, string token)
        {
            bool result = false;

            if(string.IsNullOrEmpty(token))
                throw new ArgumentException("Token is null or empty.");

            IRestClient client = new JsonServiceClient();
            ValidateTokenResponse response = client.Post<ValidateTokenResponse>(string.Format("{0}/{1}/{2}/token", ADSecurityServiceURL, "NG", version),
                new ValidateTokenRequest { Token = token } as object);

            if (response.IsValid)
                result = true;
            else
                throw new ArgumentException("Token is not valid.");

            return result;
        }

        protected static void SendAuditDispatch(object request)
        {
            DispatchEventArgs args = new DispatchEventArgs { payload = request};
            AuditDispatcher.SendDispatchAsynch(args);
        }
    }
}