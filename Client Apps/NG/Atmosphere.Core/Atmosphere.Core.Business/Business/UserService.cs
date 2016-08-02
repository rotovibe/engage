using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C3.Data;
using C3.Business.Interfaces;
using Phytel.Framework.SQL.Data;

namespace C3.Business
{
    public class UserService : SqlDataAccessor, IUserService
    {
        private string _dbConnName = "Phytel";

        public UserService()
        {
            _dbConnName = System.Configuration.ConfigurationManager.AppSettings.Get("PhytelServicesConnName");
        }

        public User GetUser(string userName, int currentContractId)
        {
            User user = Query<User>(null, _dbConnName, StoredProcedure.GetUser, User.Build, userName);
            user.Contracts = ContractService.Instance.GetByUser(user.UserId);
            user.HistoricalPasswords = PasswordHistoryService.Instance.GetByUser(user.UserId);
            user.Controls = ControlService.Instance.GetTabControlsByUser(user.UserId);
            user.LoginToken = GenerateToken(user.UserId);

            Contract userContract = user.Contracts[0];
            int defaultContractId = userContract.ContractId;

            if (user.Contracts.Count > 1)
            {
                foreach (Contract c in user.Contracts)
                {
                    if (c.DefaultContract)
                    {
                        userContract = c;
                        defaultContractId = c.ContractId;
                        break;
                    }
                }
            }

            if (currentContractId > 0)
            {
                user.Roles = GetUserRoles(user.UserId, currentContractId);
                user.ControlPermissions = new PermissionService().GetByUser(user.UserId, currentContractId);
            }
            else
            {
                user.Roles = GetUserRoles(user.UserId, defaultContractId);
                user.ControlPermissions = new PermissionService().GetByUser(user.UserId, defaultContractId);
            }

            if (userContract.ContractId > 0) //only do this if we have a valid contract that we can connect to (doing this when new user is created, we don't have the contract yet)
            {
                //user.Groups = GroupService.Instance.GetGroupsByUser(userContract, user.UserId);
                //user.SavedPageViews = PageViewService.Instance.GetPageViews(user.UserId, userContract.ContractId).ToList();
                user.OrgFunctions = OrgFunctionService.Instance.GetUserContractOrgFunctions(user.UserId,
                                                                                            userContract.ContractId);
            }
            return user;
        }

        public string GenerateToken(Guid userId)
        {
            string returnToken = Guid.NewGuid().ToString();

            try
            {
                new SqlDataExecutor().Execute(null, _dbConnName, StoredProcedure.GenerateUserToken, userId, returnToken);
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return returnToken;
        }

        public List<User> GetUsers(int userContractId, string firstName, string lastName, string status, string roleId, string sort)
        {
            return QueryAll<User>(null, _dbConnName, StoredProcedure.GetUsers, User.BuildLightweight, new object[] { userContractId, firstName, lastName, status, roleId, sort });
        }

        public List<User> GetUsers(int userContractId, string firstName, string lastName, string status, List<Role> roles)
        {
            return QueryAll<User>(null, _dbConnName, StoredProcedure.GetUsers, User.BuildLightweight, new object[] { userContractId, firstName, lastName, status, GetRolesXML(roles) });
        }

        public List<User> GetPhytelUsers(string firstName, string lastName, string status, string sort)
        {
            return QueryAll<User>(null, _dbConnName, StoredProcedure.GetPhytelUsers, User.BuildLightweight, new object[] { firstName, lastName, status, sort });
        }

        public string[] ValidateUser(string userName)
        {
            using (ITypeReader reader = QueryReader(null, _dbConnName, StoredProcedure.CheckUserValid, userName))
            {
                if (reader.Read())
                {
                    string condition = reader.GetString("Condition");
                    string message = reader.GetString("Message");
                    string daysToExpiration = reader.GetInt("DaysTillPasswordExpiration").ToString();
                    return new string[] { condition, message, daysToExpiration };
                }

                else
                    return new string[] { };
            }
        }

        public List<Role> GetUserRoles(Guid userId, int contractId)
        {
            return QueryAll<Role>(null, _dbConnName, StoredProcedure.GetUserRoles, User.BuildRoles, userId, contractId);
        }

        // from $/PhytelCode/Phytel.Net/Client Apps/C3/Development/Current/C3.Business/C3.Business copy
        //public List<UserRole> GetUserRoles(int contractId)
        //{
        //    return QueryAll<UserRole>(null, _dbConnName, StoredProcedure.GetUserRolesAll, User.BuildUserRoles, contractId);
        //}

        public List<string> GetStatuses()
        {
            return QueryAll<string>(null, _dbConnName, StoredProcedure.GetStatuses, User.BuildStatuses);
        }

        public DataTable GetStatusesDT()
        {
            return Query(null, _dbConnName, StoredProcedure.GetStatuses);
        }

        public DataTable GetAdminUsers(int contractId)
        {
            return Query(null, _dbConnName, StoredProcedure.GetAdminUsersByContract, contractId);
        }

        public DateTime? SaveSession(Guid userId, string sessionId, int sessionTimeOut, bool removeSession)
        {
            DateTime? expiration = null;
            using (ITypeReader reader = QueryReader(null, _dbConnName, StoredProcedure.SaveUserSession, userId, sessionId, sessionTimeOut, removeSession))
            {
                while (reader.Read())
                {
                    expiration = reader.GetDate("ExpirationDate");
                }
            }
            return expiration;
        }

        public bool SaveUserRoles(Guid userId, string roles)
        {
            bool saved = false;
            try
            {
                new SqlDataExecutor().Execute(null, _dbConnName, StoredProcedure.SaveUserRoles, userId, roles);
                saved = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return saved;
        }

        public bool DeleteUser(Guid userId)
        {
            bool deleted = false;
            if (userId != null)
            {
                try
                {
                    new SqlDataExecutor().Execute(null, _dbConnName, StoredProcedure.DeleteUserContractByUser, userId);
                    deleted = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return deleted;
        }

        public User SaveUser(User user)
        {
            if (user != null)
            {
                if (user.UserId != null)
                {
                    //This handles the transaction
                    //List<IDbCommand> commands = new List<IDbCommand>() 
                    //{
                    //    GetCommand(_dbConnName, StoredProcedure.UpdateUser, user.UserId
                    //        , user.FirstName
                    //        , user.MiddleName
                    //        , user.LastName
                    //        , user.Phone
                    //        , user.SessionTimeoutInterval
                    //        , user.PasswordExpiration
                    //        , user.AcceptedLatestTOS
                    //        ),
                    //    GetCommand(_dbConnName, StoredProcedure.DeleteUserFromContract, user.Contracts, user.UserId )
                    //};

                    
                    new SqlDataExecutor().Execute(_dbConnName, StoredProcedure.UpdateUser, new object []{
                        user.UserId
                        , user.FirstName
                        , user.MiddleName
                        , user.LastName
                        , user.Phone
                        , user.Ext
                        , user.SessionTimeoutInterval
                        , user.PasswordExpiration
                        , user.AcceptedLatestTOS
                        , user.UserTypeId
                        , user.StatusTypeId
                        , user.AdminUserId}
                        );
                }
            }
            return user;
        }

        public User SetFirstTimeUser(User user)
        {
            if (user != null)
            {
                if (user.UserId != null)
                {
                    new SqlDataExecutor().Execute(_dbConnName, StoredProcedure.SetFirstTimeUser, user.UserId);
                }
            }
            return user;
        }

        public User LockOutUser(User user)
        {
            if (user != null)
            {
                if (user.UserId != null)
                {
                    new SqlDataExecutor().Execute(_dbConnName, StoredProcedure.LockOutUser, user.UserId);
                }
            }
            return user;
        }

        public User SetPasswordExpiration(User user, string expirationDate)
        {
            if (user != null)
            {
                if (user.UserId != null)
                {
                    new SqlDataExecutor().Execute(_dbConnName, StoredProcedure.SetPasswordExpiration, user.UserId, expirationDate);
                    user.PasswordExpiration = DateTime.Parse(expirationDate);
                }
            }
            return user;
        }

        public User SetPasswordExpiration(User user)
        {
            if (user != null)
            {
                if (user.UserId != null)
                {
                    ITypeReader reader = new SqlDataExecutor().QueryReader(null, _dbConnName, StoredProcedure.SetPasswordExpiration, user.UserId, "");
                    reader.Read();
                    user.PasswordExpiration = reader.GetDate("ExpirationDate");
                }
            }
            return user;
        }

        public User SetFailedPasswordAttemptCount(User user, int newCount)
        {
            if (user != null)
            {
                if (user.UserId != null)
                {
                    new SqlDataExecutor().Execute(_dbConnName, StoredProcedure.SetFailedPasswordAttemptCount, user.UserId, newCount);
                    user.FailedPasswordAttemptCount = newCount;
                }
            }
            return user;
        }

        public User SetFailedPasswordAnswerAttemptCount(User user, int newCount)
        {
            if (user != null)
            {
                if (user.UserId != null)
                {
                    new SqlDataExecutor().Execute(_dbConnName, StoredProcedure.SetFailedPasswordAnswerAttemptCount, user.UserId, newCount);
                    user.FailedPasswordAnswerAttemptCount = newCount;
                }
            }
            return user;
        }

        public User ResetFailedAttemptCounts(User user)
        {
            if (user != null)
            {
                if (user.UserId != null)
                {
                    new SqlDataExecutor().Execute(_dbConnName, StoredProcedure.SetFailedPasswordAttemptCount, user.UserId, 0);
                    new SqlDataExecutor().Execute(_dbConnName, StoredProcedure.SetFailedPasswordAnswerAttemptCount, user.UserId, 0);
                    user.FailedPasswordAttemptCount = 0;
                    user.FailedPasswordAnswerAttemptCount = 0;
                }
            }
            return user;
        }

        public List<User> GetUsersToImpersonateByContract(int contractId)
        {
            List<User> _users = new List<User>();

            DataSet _impersonateUsers = QueryDataSet(null, _dbConnName, StoredProcedure.GetImpersonationUsersByContractId, contractId);

            _impersonateUsers.Tables[0].TableName = "Users";
            _impersonateUsers.Tables[1].TableName = "Roles";
            _impersonateUsers.Tables[2].TableName = "Contracts";

            foreach (DataRow row in _impersonateUsers.Tables["Users"].Rows)
            {
                User user = User.BuildImpersonationUsers(row);                

                if(_impersonateUsers.Tables["Roles"].Select(String.Format("UserId = '{0}'", user.UserId)).ToList().Count > 0)
                {
                    user.Roles = new List<Role>();
                    foreach (DataRow roleRow in _impersonateUsers.Tables["Roles"].Select(String.Format("UserId = '{0}'", user.UserId)))
                    {
                        user.Roles.Add(Role.BuildLight(roleRow));
                    }
                }

                if (_impersonateUsers.Tables["Contracts"].Select(String.Format("UserId = '{0}'", user.UserId)).ToList().Count > 0)
                {
                    user.Contracts = new List<Contract>();
                    foreach (DataRow contractRow in _impersonateUsers.Tables["Contracts"].Select(String.Format("UserId = '{0}'", user.UserId)))
                    {
                        user.Contracts.Add(Contract.BuildLight(contractRow));
                    }
                }

                _users.Add(user);
            }
            //ITypeReader read = QueryReader(null, _dbConnName, StoredProcedure.GetImpersonationUsersByContractId, contractId);

            //while (read.Read())
            //{
            //    _users.Add(User.BuildImpersonationUsers(read));
                
            //}
            //if (read.NextResult)
            //{
            //    while (read.Read())
            //    {

            //    }
            //}

            
            //_users = QueryAll<User>(null, _dbConnName, StoredProcedure.GetImpersonationUsersByContractId, User.BuildImpersonationUsers, contractId);



            return _users;
        }

        public string GetRolesXML(List<Role> roles)
        {
            string _roles = "";
            StringBuilder roleXML = new StringBuilder();

            if (roles != null)
            {
                //Beginning of XML 
                roleXML.Append("<roles>");

                List<Guid> ids = new List<Guid>();

                foreach (Role role in roles)
                {
                    if (!ids.Contains(role.RoleId))
                    {
                        roleXML.AppendFormat("<role>{0}</role>", role.RoleId);
                    }
                    ids.Add(role.RoleId);
                }

                roleXML.Append("</roles>");


                _roles = roleXML.ToString();
                return _roles;
            }
            else
            {
                return null;
            }

        }

		public Dictionary<string, string> GetContractProperties(Guid userId, int contractId)
		{
			Dictionary<string, string> properties = new Dictionary<string, string>();
			DataTable queryResults = Query(null, _dbConnName, StoredProcedure.GetUserContractProperty, userId, contractId);
			foreach (DataRow current in queryResults.Rows)
			{
				properties.Add(current["Key"].ToString(), current["Value"].ToString());
			}

			return properties;
		}

		public void SetContractProperty(Guid userId, int contractId, string key, string value)
		{
			new SqlDataExecutor().Execute(_dbConnName, StoredProcedure.SetUserContractProperty, userId, contractId, key, value);
		}
    }
}
