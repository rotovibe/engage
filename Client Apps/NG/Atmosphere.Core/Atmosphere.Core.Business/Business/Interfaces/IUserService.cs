using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C3.Data;
using Phytel.Framework.SQL.Data;

namespace C3.Business.Interfaces
{
    public interface IUserService
    {
        User GetUser(string userName, int currentContractId);
        List<User> GetUsers(int userContractId, string firstName, string lastName, string status, string roleId, string sort);
        List<User> GetUsers(int userContractId, string firstName, string lastName, string status, List<Role> roles);
        List<User> GetPhytelUsers(string firstName, string lastName, string status, string sort);
        string[] ValidateUser(string userName);
        List<Role> GetUserRoles(Guid userId, int contractId);
        List<string> GetStatuses();
        DataTable GetStatusesDT();
        DataTable GetAdminUsers(int contractId);
        DateTime? SaveSession(Guid userId, string sessionId, int sessionTimeOut, bool removeSession);
        bool SaveUserRoles(Guid userId, string roles);
        bool DeleteUser(Guid userId);
        User SaveUser(User user);
        User SetFirstTimeUser(User user);
        User LockOutUser(User user);
        User SetPasswordExpiration(User user, string expirationDate);
        User SetPasswordExpiration(User user);
        User SetFailedPasswordAttemptCount(User user, int newCount);
        User SetFailedPasswordAnswerAttemptCount(User user, int newCount);
        User ResetFailedAttemptCounts(User user);
		List<User> GetUsersToImpersonateByContract(int contractId);
		Dictionary<string, string> GetContractProperties(Guid userId, int contractId);
		void SetContractProperty(Guid userId, int contractId, string key, string value);
    }
}
