using System;

namespace Phytel.API.Common.Audit
{
    public interface IAuditUtil
    {
        void AuditAsynch(Phytel.API.Interface.IAppDomainRequest request, string sqlUserID, System.Collections.Generic.List<string> patientids, string browser, string userIPAddress, string returnTypeName);
        void DataAuditAsynch(string userId, string collectionName, string entityId, string entityKeyField, Phytel.API.Common.DataAuditType auditType, string contractNumber);
        string FindMethodType(string returnTypeName);
        AuditData GetAuditLog(int auditTypeId, Phytel.API.Interface.IAppDomainRequest request, string sqlUserID, System.Collections.Generic.List<string> patientids, string browser, string userIPAddress, string methodCalledFrom, bool isError = false);
        int GetAuditTypeID(string callingMethod);
        int GetContractID(string contractNumber);
        Phytel.API.DataAudit.DataAudit GetDataAuditLog(string userId, string collectionName, string entityId, string entityKeyField, Phytel.API.Common.DataAuditType auditType, string contractNumber);
        string GetMongoEntity(string contract, string collectionName, string entityId, string entityKeyField);
        void LogAuditData(Phytel.API.Interface.IAppDomainRequest request, string sqlUserID, System.Collections.Generic.List<string> patientids, string browser, string userAddress, string returnTypeName);
        void LogDataAudit(string userId, string collectionName, System.Collections.Generic.List<string> entityIds, Phytel.API.Common.DataAuditType auditType, string contractNumber);
        void LogDataAudit(string userId, string collectionName, string entityId, Phytel.API.Common.DataAuditType auditType, string contractNumber);
        void LogDataAudit(string userId, string collectionName, string entityId, string entityKeyField, Phytel.API.Common.DataAuditType auditType, string contractNumber);
        void LogDataAuditNoThread(string userId, string collectionName, string entityId, string entityKeyField, Phytel.API.Common.DataAuditType auditType, string contractNumber);
    }
}
