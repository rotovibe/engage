using Phytel.API.AppDomain.Audit.DTO;
using Phytel.API.Interface;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.Common.Audit
{
    public static class AuditDispatcher
    {
        static readonly string AuditSystemUrl = ConfigurationManager.AppSettings["AuditSystemUrl"];

        public static void SendDispatchAsynch(DispatchEventArgs args)
        {

#if Debug
            // synchronous for testing
            FireDispatch(args);
#else
            //asynch call
            Task.Factory.StartNew(() => FireDispatch(args));
#endif
        }

        private static void FireDispatch(DispatchEventArgs args)
        {
            IRestClient client = new JsonServiceClient();
            string className = args.payload.GetType().Name;
            string pathName = className.Substring(3, className.Length - 10).ToLower();

            //{Version}/{ContractNumber}/auditerror/{ObjectID} ??
            PutAuditResponse par = client.Post<PutAuditResponse>(string.Format("{0}/{1}/{2}/{3}",
                    AuditSystemUrl,
                    ((IAppDomainRequest)args.payload).Version,
                    ((IAppDomainRequest)args.payload).ContractNumber,
                    pathName)
                    , args.payload);
        }
    }
}
