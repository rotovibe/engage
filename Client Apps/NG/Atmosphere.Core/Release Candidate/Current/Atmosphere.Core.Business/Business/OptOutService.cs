using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using C3.Data;
using Phytel.Framework.SQL.Data;

namespace C3.Business
{
    public class OptOutService : SqlDataAccessor
    {
        private static volatile OptOutService _instance;
        private static object _syncRoot = new Object();

        private OptOutService()
        {

        }

        public static OptOutService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                            _instance = new OptOutService();
                    }
                }
                return _instance;
            }
        }

        public DataTable GetOptOutTypes(Contract contract)
        {
            return CachedQuery(contract.ConnectionString, contract.Database, StoredProcedure.GetOptOutTypes, new CacheAccessor("C3Cache", string.Format("OptOutTypes{0}", contract.ContractId.ToString())));
        }

        public DataTable GetOptOutReasons(Contract contract)
        {
            return CachedQuery(contract.ConnectionString, contract.Database, StoredProcedure.GetOptOutReasons, new CacheAccessor("C3Cache", string.Format("OptOutReasons{0}", contract.ContractId.ToString())));
        }

        public DataTable GetOptOutByPatient(Contract contract, Patient patient)
        {
            return Query(contract.ConnectionString, contract.Database, StoredProcedure.GetOptOutByPatientId, new object[] { patient.PatientId });
        }

        public DataTable DeleteOptOut(Contract contract, Patient patient, string protocolOptOutIds, AuditData auditLog)
        {
            if (patient != null && (!string.IsNullOrEmpty(protocolOptOutIds)))
            {
                int ownerId = int.Parse(ApplicationSettingService.Instance.GetSetting("C3_SQL_ID").Value);
                new SqlDataExecutor().Execute(contract.ConnectionString, contract.Database, StoredProcedure.DeletePatientOptOut, new object[] { protocolOptOutIds, ownerId });

                auditLog.Type = "OptOutRemove";
                auditLog.Patients = new List<int> { patient.PatientId };
                AuditService.Instance.LogEvent(auditLog);
            }
            return GetOptOutByPatient(contract, patient);
        }

        public DataTable AddOptOut(Contract contract, Patient patient, string optOutData, AuditData auditLog)
        {
            DataTable dtPatientOptOut = new DataTable();

            if (patient != null && !string.IsNullOrEmpty(optOutData))
            {
                int ownerId = int.Parse(ApplicationSettingService.Instance.GetSetting("C3_SQL_ID").Value);
                SqlXml xml = new SqlXml(new XmlTextReader(new StringReader(optOutData)));
                new SqlDataExecutor().Execute(contract.ConnectionString, contract.Database, StoredProcedure.UpdatePatientOptOut, new object[] { patient.PatientId, ownerId, xml });
                dtPatientOptOut = GetOptOutByPatient(contract, patient);

                auditLog.Type = "OptOutAdd";
                auditLog.Patients = new List<int> { patient.PatientId };
                AuditService.Instance.LogEvent(auditLog);
            }
            else
                dtPatientOptOut = null;

            return dtPatientOptOut;
        }
    }
}
