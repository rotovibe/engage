using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Phytel.Framework.SQL.Data;
using C3.Data;
using C3.Business.Interfaces;

namespace C3.Business
{
    public class PasswordHistoryService : SqlDataAccessor, IPasswordHistoryService
    {
         #region Private Variables

        private static volatile PasswordHistoryService _svc = null;
        private static object syncRoot = new Object();

        private string _dbConnName = "Phytel";

        public PasswordHistoryService()
        {
            _dbConnName = System.Configuration.ConfigurationManager.AppSettings.Get("PhytelServicesConnName");
        }

        #endregion

        #region Public Methods

        public static PasswordHistoryService Instance
        {
            get
            {
                if (_svc == null)
                {
                    lock (syncRoot)
                    {
                        if (_svc == null)
                            _svc = new PasswordHistoryService();
                    }
                }

                return _svc;
            }
        }

        public List<PasswordHistory> GetByUser(Guid userId)
        {
            return QueryAll<PasswordHistory>(null, _dbConnName, StoredProcedure.GetPasswordHistory, PasswordHistory.Build, userId);
        }

        public User SetByUser(User user, string newPassword)
        {
            if (user.UserId != null && !String.IsNullOrEmpty(newPassword))
            {
                try
                {
                    new SqlDataExecutor().Execute(_dbConnName, StoredProcedure.SetPasswordHistory, user.UserId, newPassword);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }            
            return user;
        }

        #endregion
    }
}
