using Phytel.API.Common.Audit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.Test.Stubs
{
    public class StubAuditUtil : IAuditUtil
    {
        public void AuditAsynch(Interface.IAppDomainRequest request, string sqlUserID, List<string> patientids, string browser, string userIPAddress, string returnTypeName)
        {
            throw new NotImplementedException();
        }

        public void DataAuditAsynch(string userId, string collectionName, string entityId, string entityKeyField, Common.DataAuditType auditType, string contractNumber)
        {
            throw new NotImplementedException();
        }

        public string FindMethodType(string returnTypeName)
        {
            throw new NotImplementedException();
        }

        public AuditData GetAuditLog(int auditTypeId, Interface.IAppDomainRequest request, string sqlUserID, List<string> patientids, string browser, string userIPAddress, string methodCalledFrom, bool isError = false)
        {
            throw new NotImplementedException();
        }

        public int GetAuditTypeID(string callingMethod)
        {
            throw new NotImplementedException();
        }

        public int GetContractID(string contractNumber)
        {
            throw new NotImplementedException();
        }

        public Phytel.API.DataAudit.DataAudit GetDataAuditLog(string userId, string collectionName, string entityId, string entityKeyField, Common.DataAuditType auditType, string contractNumber)
        {
            throw new NotImplementedException();
        }

        public string GetMongoEntity(string contract, string collectionName, string entityId, string entityKeyField)
        {
            throw new NotImplementedException();
        }

        public void LogAuditData(Interface.IAppDomainRequest request, string sqlUserID, List<string> patientids, string browser, string hostAddress, string returnTypeName)
        {
           // pass this
        }

        public void LogDataAudit(string userId, string collectionName, List<string> entityIds, Common.DataAuditType auditType, string contractNumber)
        {
            throw new NotImplementedException();
        }

        public void LogDataAudit(string userId, string collectionName, string entityId, Common.DataAuditType auditType, string contractNumber)
        {
            throw new NotImplementedException();
        }

        public void LogDataAudit(string userId, string collectionName, string entityId, string entityKeyField, Common.DataAuditType auditType, string contractNumber)
        {
            throw new NotImplementedException();
        }

        public void LogDataAuditNoThread(string userId, string collectionName, string entityId, string entityKeyField, Common.DataAuditType auditType, string contractNumber)
        {
            throw new NotImplementedException();
        }
    }
}
