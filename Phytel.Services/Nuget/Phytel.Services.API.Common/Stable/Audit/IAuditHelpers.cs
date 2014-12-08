using System;
namespace Phytel.API.Common.Audit
{
    public interface IAuditHelpers
    {
        string FindMethodType(string returnTypeName);
        AuditData GetAuditLog(int auditTypeId, Phytel.API.Interface.IAppDomainRequest request, string sqlUserID, System.Collections.Generic.List<string> patientids, string browser, string userIPAddress, string methodCalledFrom, bool isError = false);
        int GetAuditTypeID(string callingMethod);
        void LogAuditData(Phytel.API.Interface.IAppDomainRequest request, string sqlUserID, System.Collections.Generic.List<string> patientids, System.Web.HttpRequest webreq, string returnTypeName);
        void LogDataAudit(string userId, string collectionName, System.Collections.Generic.List<string> entityIds, Phytel.API.Common.DataAuditType auditType, string contractNumber);
        void LogDataAudit(string userId, string collectionName, string entityId, Phytel.API.Common.DataAuditType auditType, string contractNumber);
        void LogDataAudit(string userId, string collectionName, string entityId, string entityKeyField, Phytel.API.Common.DataAuditType auditType, string contractNumber);
        void LogDataAuditNoThread(string userId, string collectionName, string entityId, string entityKeyField, Phytel.API.Common.DataAuditType auditType, string contractNumber);
    }
}
