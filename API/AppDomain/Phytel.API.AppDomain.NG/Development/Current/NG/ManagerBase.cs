using Phytel.API.AppDomain.Security.DTO;
using Phytel.API.Common.Audit;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG
{
    public abstract class ManagerBase
    {
        protected static readonly string ADSecurityServiceURL = ConfigurationManager.AppSettings["ADSecurityServiceUrl"];

        public bool IsUserValidated(string version, string token)
        {
            bool result = false;
            IRestClient client = new JsonServiceClient();

            ValidateTokenResponse response = client.Post<ValidateTokenResponse>(string.Format("{0}/{1}/token", ADSecurityServiceURL, version),
                new ValidateTokenRequest { Token = token } as object);

            if (response.IsValid) result = true;

            return result;
        }

        //protected void SendAuditDispatch()
        //{
        //    //DispatchEventArgs args = new DispatchEventArgs { DispatchType = DispatchType.AUDIT, new object() };
        //    //AuditDispatcher.SendDispatchAsynch(args);
        //}
    }
}
