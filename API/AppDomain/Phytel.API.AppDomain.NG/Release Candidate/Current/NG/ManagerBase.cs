using Phytel.API.AppDomain.Security.DTO;
using Phytel.API.Common.Audit;
using Phytel.API.Common.Format;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;
using System.Configuration;
using System.Runtime.CompilerServices;
using Phytel.API.Common;

namespace Phytel.API.AppDomain.NG
{
    public abstract class ManagerBase
    {
        //static member constants for each of the AuditTypes
        protected const string GetPatientAction = "GetPatient";
        protected const string GetPatientProblemsAction = "PatientProblems"; 
        
        protected static readonly string ADSecurityServiceURL = ConfigurationManager.AppSettings["ADSecurityServiceUrl"];

        public ValidateTokenResponse IsUserValidated(string version, string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                    throw new ArgumentException("Token is null or empty.");

                IRestClient client = new JsonServiceClient();
                ValidateTokenResponse response = client.Post<ValidateTokenResponse>(string.Format("{0}/{1}/{2}/token", ADSecurityServiceURL, "NG", version),
                    new ValidateTokenRequest { Token = token } as object);

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected static void SendAuditDispatch(object request)
        {
            //DispatchEventArgs args = new DispatchEventArgs { payload = request};
            //AuditDispatcher.SendDispatchAsynch(args);
        }
    }
}
