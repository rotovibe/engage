using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Atmosphere.Core.Data;
using C3.Business;
using C3.Business.Interfaces;
using C3.Data;
using Phytel.Framework.SQL.Data;

namespace C3.Business
{
    public class OrgFunctionService : SqlDataAccessor, IOrgFunctionService
    {

        #region Private Variables

        private static volatile OrgFunctionService _svc = null;
        private static readonly object SyncRoot = new Object();

        #endregion

        #region Constructors

        private string _dbConnName = "Phytel";

        public OrgFunctionService()
        {
            _dbConnName = System.Configuration.ConfigurationManager.AppSettings.Get("PhytelServicesConnName");
        }

        #endregion

        public static OrgFunctionService Instance
        {
            get
            {
                if (_svc == null)
                {
                    lock (SyncRoot)
                    {
                        if (_svc == null)
                            _svc = new OrgFunctionService();
                    }
                }

                return _svc;
            }
        }

        #region Public Methods

        public List<OrgFunction> GetContractOrgFunctions(int contractId)
        {
            var orgFunctions = new List<OrgFunction>();
            try
            {
                orgFunctions = QueryAll<OrgFunction>(null, _dbConnName, StoredProcedure.GetContractOrgFunctions, OrgFunctionBuilder.Build,
                                                     new object[] {contractId});
            }
            catch (SqlException sqlException)
            {
                AuditService.Instance.LogEvent(new AuditData
                {
                    ContractID = contractId,
                    EventDateTime = DateTime.Now,
                    Message = new Message { Id = null, Source = sqlException.Source, StackTrace = sqlException.StackTrace, Text = sqlException.Message },
                    Type = "OrgFunctions-ContractSqlError"
                });
            }
            catch (Exception exception)
            {
                AuditService.Instance.LogEvent(new AuditData
                {
                    ContractID = contractId,
                    EventDateTime = DateTime.Now,
                    Message = new Message { Id = null, Source = exception.Source, StackTrace = exception.StackTrace, Text = exception.Message },
                    Type = "SystemError"
                });
            }
            return orgFunctions;
        }

        public List<OrgFunction> GetUserContractOrgFunctions(Guid userId, int contractId)
        {
            var orgFunctions = new List<OrgFunction>();
            try
            {
                orgFunctions = QueryAll<OrgFunction>(null, _dbConnName, StoredProcedure.GetUserContractOrgFunctions,
                                                     OrgFunctionBuilder.Build,
                                                     new object[] {userId, contractId});
            }
            catch (SqlException sqlException)
            {
                AuditService.Instance.LogEvent(new AuditData
                {
                    ContractID = contractId,
                    EventDateTime = DateTime.Now,
                    Message = new Message { Id = null, Source = sqlException.Source, StackTrace = sqlException.StackTrace, Text = sqlException.Message },
                    Type = "OrgFunctions-UserContractSqlError",
                    UserId = userId
                });
            }
            catch (Exception exception)
            {
                AuditService.Instance.LogEvent(new AuditData
                {
                    ContractID = contractId,
                    EventDateTime = DateTime.Now,
                    Message = new Message { Id = null, Source = exception.Source, StackTrace = exception.StackTrace, Text = exception.Message },
                    Type = "SystemError",
                    UserId = userId
                });
            }
            return orgFunctions;
        }

        public void SaveUserContractOrgFunctions(Guid userId, int contractId, List<OrgFunction> orgFunctions)
        {
            try
            {
                new SqlDataExecutor().Execute(_dbConnName, StoredProcedure.SaveUserContractOrgFunctions,
                              new object[] { userId, contractId, orgFunctions.ToIdXml() });
            }
            catch (SqlException sqlException)
            {
                AuditService.Instance.LogEvent(new AuditData
                {
                    ContractID = contractId,
                    EventDateTime = DateTime.Now,
                    Message = new Message { Id = null, Source = sqlException.Source, StackTrace = sqlException.StackTrace, Text = sqlException.Message },
                    Type = "OrgFunctions-UserContractSqlError",
                    UserId = userId
                });
                throw;
            }
            catch (Exception exception)
            {
                AuditService.Instance.LogEvent(new AuditData
                {
                    ContractID = contractId,
                    EventDateTime = DateTime.Now,
                    Message = new Message { Id = null, Source = exception.Source, StackTrace = exception.StackTrace, Text = exception.Message },
                    Type = "SystemError",
                    UserId = userId
                });
                throw;
            }
        }

        #endregion
    }
}
