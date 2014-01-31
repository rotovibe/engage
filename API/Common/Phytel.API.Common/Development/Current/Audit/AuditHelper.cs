using System;
using System.Web;

namespace Phytel.API.Common.Audit
{
    public class AuditHelper
    {
        /// <summary>
        /// Gets an auditdata record for the current context
        /// </summary>
        /// <returns></returns>
        public static AuditData GetAuditLog(int auditTypeId, Phytel.API.Interface.IAppDomainRequest request, string methodCalledFrom)
        {
            var webrequest = HttpContext.Current.Request;
            
            ///{Version}/{ContractNumber}/patient/{PatientID}
            AuditData auditLog = new AuditData() 
            {
                //Type = auditType,
                AuditTypeId = 0, //derive this from the type passed in (lookup on PNG.AuditType.Name column)
                UserId = new Guid(request.UserId), //derive this from Mongo.APISessions.uid...lookup on Mongo.APISessions._id)
                SourcePage = methodCalledFrom, //derive this from querystring...utility function
                SourceIP = webrequest.UserHostAddress,
                Browser = webrequest.Browser.Type,
                SessionId = request.Token,
                ContractID = 0 //request.ContractNumber
            };

            return auditLog;
        }
    }
}
