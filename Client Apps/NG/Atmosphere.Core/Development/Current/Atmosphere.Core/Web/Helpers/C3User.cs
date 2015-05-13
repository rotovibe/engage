
using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C3.Data;
using C3.Business;
using System.Security.Principal;
using System.Web;
using System.Web.Security;


namespace C3.Web.Helpers
{
    [Serializable]
    public class C3User
    {
        public C3User(MembershipUser user)
        {
            AuthenticatedUser = user;
            datauser = new UserService().GetUser(AuthenticatedUser.UserName, 0);
            this.CurrentContract = UserHelper.GetDefaultContract(this);
        }

        public C3User(MembershipUser user, int currentContractId)
        {
            AuthenticatedUser = user;
            datauser = new UserService().GetUser(AuthenticatedUser.UserName, currentContractId);
            this.CurrentContract = UserHelper.GetDefaultContract(this);
        }

        public C3User()
        {
            datauser = new Data.User();

            datauser.FirstName = "";
            datauser.MiddleName = "";
            datauser.LastName = "";
            datauser.Phone = "";
            datauser.SessionTimeoutInterval = 0;
            datauser.Status = "";
        }

        #region Private Variables

        private MembershipUser AuthenticatedUser;
        private User datauser;

        #endregion

        #region Public Properties

        public bool IsBeingImpersonated { get; set; }

        public Guid ImpersonatingUserId { get; set; }

        public bool IsAdmin
        {
            get
            {
                Role userRole = datauser.Roles.Find(delegate(Role r) { return r.RoleName == "Administrator"; });
                return (userRole != null ? true : false);
            }
        }

        public string LoginToken
        {
            get { return datauser.LoginToken; }
            set { datauser.LoginToken = value; }
        }

        public bool IsDeleted
        {
            get { return datauser.Status == "Deleted"; }
        }

        public bool IsLocked
        {
            get { return datauser.Status == "Locked" || AuthenticatedUser.IsLockedOut; }
            set
            {
                if (!value)
                    AuthenticatedUser.UnlockUser();
                else
                    datauser = new UserService().LockOutUser(datauser);
            }
        }

        public Guid UserId
        {
            get { return new Guid(AuthenticatedUser.ProviderUserKey.ToString()); }
        }

        public string UserName
        {
            get { return AuthenticatedUser.UserName; }
        }

        public Data.Enum.UserTypes UserType
        {
            get { return datauser.UserType; }
            set { datauser.UserType = value; }
        }

        public int UserTypeId
        {
            get { return datauser.UserTypeId; }
            set { datauser.UserTypeId = value; }
        }

        public string Status
        {
            get { return datauser.Status; }
        }

        public int StatusTypeId
        {
            get { return datauser.StatusTypeId; }
            set { datauser.StatusTypeId = value; }
        }

        public string FirstName
        {
            get { return datauser.FirstName; }
            set { datauser.FirstName = value; }
        }

        public string MiddleName
        {
            get { return datauser.MiddleName; }
            set { datauser.MiddleName = value; }
        }

        public string Phone
        {
            //get { return string.Format("{0}:(###) ###-####}", datauser.Phone.Substring(1,3), datauser.Phone.Substring(4, 6), datauser.Phone.Substring(7,10)); }
            get { return datauser.Phone; }
            set { datauser.Phone = value; }
        }

        public string Ext
        {
            get { return datauser.Ext; }
            set { datauser.Ext = value; }
        }

        public string PhoneExt
        {
            get
            {
                string _phoneExt = "";
                //StringBuilder sb = new StringBuilder();
                //sb.Append(datauser.Phone);
                //sb.Append(datauser.Ext);
                //_phoneExt = sb.ToString();

                if (datauser.Phone.Length == 10)
                    _phoneExt = String.Format("{0:(###) ###-####}", Convert.ToInt64(datauser.Phone));
                else if (!string.IsNullOrEmpty(datauser.Phone.Trim()))
                    _phoneExt = String.Format("{0:###-####}", Convert.ToInt64(datauser.Phone));

                if (!string.IsNullOrEmpty(datauser.Ext.Trim()) && !string.IsNullOrEmpty(datauser.Phone.Trim()))
                    _phoneExt += " x" + datauser.Ext.Trim();

                //if (sb.ToString().Length > 10)
                //{
                //    _phoneExt = String.Format("{0:(###) ###-#### x}{1}", Convert.ToInt64(sb.ToString().Substring(0, 10)), Convert.ToInt64(sb.ToString().Substring(10)));
                //}
                //else if(_phoneExt.Length == 10)
                //{
                //    _phoneExt = String.Format("{0:(###) ###-####}", Convert.ToInt64(_phoneExt));
                //}
                return _phoneExt;
            }
        }

        public int SessionTimeoutInterval
        {
            get { return datauser.SessionTimeoutInterval; }
            set { datauser.SessionTimeoutInterval = value; }
        }

        public DateTime? SessionExpiration
        {
            get { return datauser.SessionExpiration; }
            set { datauser.SessionExpiration = value; }
        }

        public int LastTOSVersion
        {
            get { return datauser.LastTOSVersion; }
        }

        public bool AcceptedLatestTOS
        {
            get { return datauser.AcceptedLatestTOS; }
            set { datauser.AcceptedLatestTOS = value; }
        }

        public string LastName
        {
            get { return datauser.LastName; }
            set { datauser.LastName = value; }
        }

        public string PasswordQuestion
        {
            get { return datauser.PasswordQuestion; }
            set { datauser.PasswordQuestion = value; }
        }

        public bool IsActive
        {
            get { return datauser.Status == "Active"; }
        }

        public bool PasswordIsExpired
        {
            get { return datauser.PasswordExpiration == null || datauser.PasswordExpiration <= DateTime.Now; }
        }

        public DateTime? PasswordExpiration
        {
            get { return datauser.PasswordExpiration; }
        }

        public List<Permission> ControlPermissions
        {
            get { return datauser.ControlPermissions; }
            set { datauser.ControlPermissions = value; }
        }

        public List<Role> Roles
        {
            get { return datauser.Roles; }
            set { datauser.Roles = value; }
        }

        //public List<Group> Groups
        //{
        //    get { return datauser.Groups; }
        //    set { datauser.Groups = value; }
        //}

        public List<Contract> Contracts
        {
            get { return datauser.Contracts; }
            set { datauser.Contracts = value; }
        }

        public List<OrgFunction> OrgFunctions
        {
            get { return datauser.OrgFunctions ?? new List<OrgFunction>(); }
            set { datauser.OrgFunctions = value; }
        }

        public List<Control> Controls
        {
            get { return datauser.Controls; }
        }

        public bool FirstTimeUser
        {
            get { return datauser.FirstTimeUser; }
            set { datauser.FirstTimeUser = value; }
        }

        public string DisplayName
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(datauser.FirstName);
                sb.Append(" ");
                if (datauser.MiddleName.Trim().Length > 0)
                {
                    sb.Append(datauser.MiddleName);                
                    sb.Append(" ");
                }
                sb.Append(datauser.LastName);
                return sb.ToString();
            }
        }

        public string Email
        {
            get { return AuthenticatedUser.Email; }
            set { AuthenticatedUser.Email = value; }
        }

        public List<PasswordHistory> HistoricalPasswords
        {
            get { return datauser.HistoricalPasswords; }
        }

        public string CurrentPassword
        {
            get { return AuthenticatedUser.GetPassword(); }
        }

        public string PasswordAnswer
        {
            get { return datauser.PasswordAnswer; }
        }

        public Guid AdminUserId
        {
            get { return datauser.AdminUserId; }
            set { datauser.AdminUserId = value; }
        }

        public string AdminUserName
        {
            get { return datauser.AdminUserName; }
        }

        public PageInfo UserPageInfo
        {
            get { return (PageInfo)HttpContext.Current.Session["UserPageInfo"]; }
            set { HttpContext.Current.Session["UserPageInfo"] = value; }
        }

        public Contract CurrentContract { get; set; }

        public int FailedPasswordAttemptCount
        {
            get { return datauser.FailedPasswordAttemptCount; }
        }

        public int FailedPasswordAnswerAttemptCount
        {
            get { return datauser.FailedPasswordAnswerAttemptCount; }
        }

        //public IList<PageView> SavedPageViews
        //{
        //    get { return datauser.SavedPageViews; }
        //}

        #endregion

        #region Public Methods

        public bool Unlock()
        {
            return AuthenticatedUser.UnlockUser();
        }

        public bool ChangePassword(string oldPassword, string newPassword)
        {
            return AuthenticatedUser.ChangePassword(oldPassword, newPassword);
        }

        public string ResetPassword()
        {
            return AuthenticatedUser.ResetPassword();
        }

        public string GetPassword()
        {
            return AuthenticatedUser.GetPassword();
        }

        public void Save()
        {
            try
            {
                if (UserId != null)
                {
                    Membership.UpdateUser(AuthenticatedUser);
                    datauser = new UserService().SaveUser(datauser);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddToPasswordHistory(string newPassword)
        {
            try
            {
                datauser = PasswordHistoryService.Instance.SetByUser(datauser, newPassword);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetFirstTimeUser()
        {
            try
            {
                datauser = new UserService().SetFirstTimeUser(datauser);
                datauser.FirstTimeUser = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void LockOutUser()
        {
            try
            {
                datauser = new UserService().LockOutUser(datauser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetSecurityQuestion(string question, string answer)
        {
            try
            {
                datauser = new SecurityQuestionService().SetByUser(datauser, question, answer);
                datauser.PasswordQuestion = question;
                datauser.PasswordAnswer = answer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetPasswordExpiration()
        {
            try
            {
                datauser = new UserService().SetPasswordExpiration(datauser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetPasswordExpiration(string expiration)
        {
            try
            {
                datauser = new UserService().SetPasswordExpiration(datauser, expiration);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetFailedPasswordAttemptCount(int count)
        {
            try
            {
                datauser = new UserService().SetFailedPasswordAttemptCount(datauser, count);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetFailedPasswordAnswerAttemptCount(int count)
        {
            try
            {
                datauser = new UserService().SetFailedPasswordAnswerAttemptCount(datauser, count);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ResetFailedAttemptCounts()
        {
            try
            {
                datauser = new UserService().ResetFailedAttemptCounts(datauser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool HasPermission(Data.Enum.Permissions checkPermission)
        {
            Permission perm = ControlPermissions.Find(delegate(Permission p) { return p.PermissionId == (int)checkPermission; });
            return (perm != null);
        }

        public bool HasRole(string roleName)
        {
            Role role = Roles.Find(delegate(Role r) { return r.LoweredRoleName == roleName.ToLower(); });
            return (role != null);
        }
        #endregion
    }
}
