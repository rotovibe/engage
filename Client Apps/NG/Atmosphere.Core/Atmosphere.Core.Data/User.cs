using System;
using System.Data;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Atmosphere.Core.Data;
using C3.Data.Enum;
using Phytel.Framework.SQL.Data;
using Phytel.Framework.SQL;

namespace C3.Data
{
    [Serializable]
    [DataContract]
    public class User
    {

        #region Public Constants

        #endregion

        #region Public Properties
        
        [DataMember]
        public Guid UserId { get; set; }

        [DataMember]
        public UserTypes UserType {  get; set; }

        [DataMember]
        public string UserName { get; set; }

        public List<Permission> ControlPermissions { get; set; }

        public List<Control> Controls { get; set; }

        [DataMember]
        public List<Contract> Contracts { get; set; }

        [DataMember]
        public List<Role> Roles { get; set; }

        [DataMember]
        public string LoginToken { get; set; }

        [DataMember]
        public List<OrgFunction> OrgFunctions { get; set; }
        
        //[DataMember]
        //public string ContractString { get; set; }

        [DataMember]
        public List<PasswordHistory> HistoricalPasswords { get; set; }

        [DataMember]
        public bool AcceptedLatestTOS { get; set; }

        [DataMember]
        public int LastTOSVersion { get; set; }

        [DataMember]
        public bool FirstTimeUser { get; set; }

        [DataMember]
        public string DisplayName { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string MiddleName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public string Ext { get; set; }

        [DataMember]
        public int SessionTimeoutInterval { get; set; }

        [DataMember]
        public DateTime? SessionExpiration { get; set; }

        [DataMember]
        public DateTime? PasswordExpiration { get; set; }

        [DataMember]
        public DateTime CreateDate { get ; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public int StatusTypeId { get; set; }

        [DataMember]
        public string PasswordQuestion { get; set; }

        [DataMember]
        public string PasswordAnswer { get; set; }

        [DataMember]
        public int FailedPasswordAttemptCount { get; set; }

        [DataMember]
        public int FailedPasswordAnswerAttemptCount { get; set; }

        [DataMember]
        public Guid AdminUserId { get; set; }

        [DataMember]
        public string AdminUserName { get; set; }

        [DataMember]
        public int UserTypeId { get; set; }

        //public string RolesCommaSep { get; set; }

        //public List<PageView> SavedPageViews { get; set; }

        [DataMember]
        public DateTime? LastLoginDate { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public bool IsNewUser { get; set; }

        #endregion

        #region Public Methods
        
        public User()
        {
            ControlPermissions = new PermissionList();
            Contracts = new List<Contract>();
            HistoricalPasswords = new List<PasswordHistory>();
            Roles = new List<Role>();
        }

        public static User Build(ITypeReader reader)
        {
            User user = new User();
            user.UserId = reader.GetGuid("UserId");
            user.AcceptedLatestTOS = reader.GetBool("AcceptedLatestTOS");
            user.LastTOSVersion = reader.GetInt("LastTOSVersion");
            user.PasswordExpiration = reader.GetDate("PasswordExpiration");
            user.FirstTimeUser = reader.GetBool("NewUser");
            user.DisplayName = reader.GetString("DisplayName");
            user.FirstName = reader.GetString("FirstName");
            user.MiddleName = reader.GetString("MiddleName");
            user.LastName = reader.GetString("LastName");
            user.Phone = reader.GetString("Phone");
            user.Ext = reader.GetString("Ext");
            user.SessionTimeoutInterval = reader.GetInt("SessionTimeout");
            user.PasswordQuestion = reader.GetString("PasswordQuestion");
            user.PasswordAnswer = reader.GetString("PasswordAnswer");
            user.AdminUserId = reader.GetGuid("AdministratorUserId");
            user.AdminUserName = reader.GetString("AdminUserName");
            user.UserType = (UserTypes)reader.GetInt("UserTypeId");
            user.UserTypeId = reader.GetInt("UserTypeId");
            user.FailedPasswordAttemptCount = reader.GetInt("FailedPasswordAttemptCount");
            user.FailedPasswordAnswerAttemptCount = reader.GetInt("FailedPasswordAnswerAttemptCount");
            user.Status = reader.GetString("Status");
            user.StatusTypeId = reader.GetInt("StatusTypeId");
            return user;
        }

        public static User BuildLightweight(ITypeReader reader)
        {
            User user = new User();
            user.UserId = reader.GetGuid("UserId");
            user.UserName = reader.GetString("UserName");
            user.DisplayName = reader.GetString("DisplayName");
            user.CreateDate = reader.GetDate("CreateDate").Value;
            user.Status = reader.GetString("Status");
            user.StatusTypeId = reader.GetInt("StatusTypeId");
            user.UserType = (UserTypes)reader.GetInt("UserTypeId");
            user.UserTypeId = reader.GetInt("UserTypeId");
            user.LastLoginDate = reader.GetDate("LastLoginDate");
            user.Email = reader.GetString("Email");
            user.IsNewUser = reader.GetBool("IsNewUser");

            return user;
        }

        public static User BuildImpersonationUsers(ITypeReader reader)
        {
            User user = new User();

            user.UserId = reader.GetGuid("UserId");
            user.DisplayName = reader.GetString("DisplayName");
            user.UserName = reader.GetString("UserName");
            user.Status = reader.GetString("UserStatus");            

            return user;
        }

        public static User BuildImpersonationUsers(DataRow row)
        {
            User user = new User();

            user.UserId = Converter.ToGuid(row["UserId"].ToString());
            user.DisplayName = row["DisplayName"].ToString();
            user.UserName = row["UserName"].ToString();
            user.Status = row["UserStatus"].ToString();

            return user;
        }

        public static Role BuildRoles(ITypeReader reader)
        {   
            Role role = new Role();
            role.RoleId = reader.GetGuid("RoleId");
            role.RoleName = reader.GetString("RoleName");
            role.LoweredRoleName = reader.GetString("RoleName").ToLower();

            return role;
        }

		//public static UserRole BuildUserRoles(ITypeReader reader)
		//{
		//    return new UserRole {
		//                   UserId = reader.GetGuid("UserId"),
		//                   Role = BuildRoles(reader)
		//               };
		//}


        public static String BuildStatuses(ITypeReader reader)
        {
            return reader.GetString("Status");
        }

		#endregion

		#region Contract Property
		#endregion
	}
}
