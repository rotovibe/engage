using System;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;

namespace Phytel.Services.Network
{
    public sealed class ADService
    {
        private static volatile ADService instance;
        private static object syncRoot = new Object();
 
        #region Instance Methods
        private ADService() {}

        public static ADService Instance
        {
            get 
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ADService();
                    }
                }

                return instance;
            }
        }
        #endregion

       #region Public Methods
       public DirectoryEntry GetOrgUnit(string ouName, bool createIfNotExists)
        {
            return GetOrgUnit(ouName, createIfNotExists, System.Environment.UserDomainName, string.Empty, string.Empty);
        }

        public DirectoryEntry GetOrgUnit(string ouName, bool createIfNotExists, string domainName, string domainUser, string domainPassword)
        {
            DirectoryEntry _OrgUnit = null;

            try
            {
                _OrgUnit = GetLDAPEntry(domainName, domainUser, domainPassword).Children.Find("OU=" + ouName, "organizationalUnit");
            }
            catch (DirectoryServicesCOMException)
            {
                _OrgUnit = null;
            }
            catch (Exception)
            {
                throw;
            }
            if (_OrgUnit == null && createIfNotExists)
                _OrgUnit = CreateOrgUnit(ouName, domainName, domainUser, domainPassword);

            return _OrgUnit;
        }

        public DirectoryEntry CreateOrgUnit(string ouName)
        {
            return CreateOrgUnit(ouName, System.Environment.UserDomainName, string.Empty, string.Empty);
        }

        public DirectoryEntry CreateOrgUnit(string ouName, string domainName, string domainUser, string domainPassword)
        {
            DirectoryEntry _OrgUnit = null;

            try
            {
                //First make sure the Organizational Unit doesn't already exist. If so, log it and get out 
                _OrgUnit = GetOrgUnit(ouName, false, domainName, domainUser, domainPassword);

                if (_OrgUnit == null)
                {
                    _OrgUnit = GetLDAPEntry(domainName, domainUser, domainPassword).Children.Add("OU=" + ouName, "organizationalUnit");
                    _OrgUnit.CommitChanges();
                }
                else
                    throw new ActiveDirectoryObjectExistsException(string.Format("Organizational Unit '{0}' already exists!", ouName));
            }
            catch (Exception)
            {
                throw;
            }
            return _OrgUnit;
        }

        public DirectoryEntry GetSecurityGroup(string ouName, string secGroupName, bool createIfNotExists)
        {
            return GetSecurityGroup(ouName, secGroupName, createIfNotExists, System.Environment.UserDomainName, string.Empty, string.Empty);
        }

        public DirectoryEntry GetSecurityGroup(string ouName, string secGroupName, bool createIfNotExists, string domainName, string domainUser, string domainPassword)
        {
            DirectoryEntry _OrgUnit = null;
            DirectoryEntry _secGroup = null;

            _OrgUnit = GetOrgUnit(ouName, createIfNotExists, domainName, domainUser, domainPassword);

            if (_OrgUnit == null)
                throw new System.DirectoryServices.ActiveDirectory.ActiveDirectoryObjectNotFoundException(string.Format("Organizational Unit '{0}' does not exist!", ouName));

            //Now try to locate the security group within the AD 
            try
            {
                _secGroup = _OrgUnit.Children.Find("CN=" + secGroupName, "group");
            }
            catch (DirectoryServicesCOMException)
            {
                _secGroup = null;
            }
            catch (Exception)
            {
                throw;
            }

            if (_secGroup == null && createIfNotExists)
                _secGroup = CreateSecurityGroup(ouName, secGroupName, domainName, domainUser, domainPassword);

            return _secGroup;
        }

        public DirectoryEntry CreateSecurityGroup(string ouName, string secGroupName)
        {
            return CreateSecurityGroup(ouName, secGroupName, System.Environment.UserDomainName, string.Empty, string.Empty);
        }

        public DirectoryEntry CreateSecurityGroup(string ouName, string secGroupName, string domainName, string domainUser, string domainPassword)
        {
            DirectoryEntry _OU = null;
            DirectoryEntry _secGroup = null;

            try
            {
                _OU = GetOrgUnit(ouName, false, domainName, domainUser, domainPassword);
                if(_OU == null)
                    throw new ActiveDirectoryObjectExistsException(string.Format("The Organizational Unit '{0}' does not exist!", ouName));

                //Now try to locate the security group within the AD 
                try
                {
                    _secGroup = _OU.Children.Find("CN=" + secGroupName, "group");
                }
                catch (DirectoryServicesCOMException)
                {
                    _secGroup = null;
                }
                catch (Exception)
                {
                    throw;
                }

                if (_secGroup == null)
                {
                    _secGroup = _OU.Children.Add("CN=" + secGroupName, "group");
                    _secGroup.Properties["displayName"].Add(secGroupName);
                    _secGroup.Properties["description"].Add(secGroupName);
                    _secGroup.Properties["sAMAccountName"].Add(secGroupName);
                    _secGroup.CommitChanges();
                }
                else
                    throw new ActiveDirectoryObjectExistsException(string.Format("The Security Group '{0}' already exists!", secGroupName));
            }
            catch (Exception)
            {
                throw;
            }
            return _secGroup;
        }

        public DirectoryEntry GetUser(string ouName, string userID)
        {
            return GetUser(ouName, userID, System.Environment.UserDomainName, string.Empty, string.Empty);
        }

        public DirectoryEntry GetUser(string ouName, string userID, string domainName, string domainUser, string domainPassword)
        {
            DirectoryEntry _OrgUnit = null;
            DirectoryEntry _User = null;

            _OrgUnit = GetOrgUnit(ouName, false, domainName, domainUser, domainPassword);

            if (_OrgUnit == null)
                throw new System.DirectoryServices.ActiveDirectory.ActiveDirectoryObjectNotFoundException(string.Format("Organizational Unit '{0}' does not exist!", ouName));

            //Now try to locate the User within the AD 
            try
            {
                _User = _OrgUnit.Children.Find("CN=" + userID, "user");
            }
            catch (DirectoryServicesCOMException)
            {
                _User = null;
            }
            catch (Exception)
            {
                throw;
            }

            return _User;
        }

        public DirectoryEntry CreateUser(string ouName, string userID, string userFirstName, string userLastName, string userPassword, bool shouldPasswordExpire, string[] secGroups)
        {
            return CreateUser(ouName, userID, userFirstName, userLastName, userPassword, shouldPasswordExpire, secGroups, System.Environment.UserDomainName, string.Empty, string.Empty);
        }

        public DirectoryEntry CreateUser(string ouName, string userID, string userFirstName, string userLastName, string userPassword, bool shouldPasswordExpire, string[] secGroups, string domainName, string domainUser, string domainPassword)
        {
            DirectoryEntry _LDAP = null;
            DirectoryEntry _OU = null;
            DirectoryEntry _User = null;
            string _principalName = string.Empty;

            try
            {
                _LDAP = GetLDAPEntry(domainName, domainUser, domainPassword);
                _OU = GetOrgUnit(ouName, false, domainName, domainUser, domainPassword);
                if (_OU == null)
                    throw new ActiveDirectoryObjectExistsException(string.Format("The Organizational Unit '{0}' does not exist!", ouName));

                //Now try to locate the User within the AD 
                try
                {
                    _User = _OU.Children.Find("CN=" + userID, "user");
                }
                catch (DirectoryServicesCOMException)
                {
                    _User = null;
                }
                catch (Exception)
                {
                    throw;
                }

                if (_User == null)
                {
                    _principalName = userID + "@" + domainName.ToLower() + ".phytel.com";

                    _User = _OU.Children.Add("CN=" + userID, "user");
                    _User.Properties["sn"].Add(userFirstName);
                    _User.Properties["givenName"].Add(userLastName);
                    _User.Properties["displayName"].Add(userLastName + ", " + userFirstName);
                    _User.Properties["sAMAccountName"].Add(userID);
                    _User.Properties["userPrincipalName"].Add(_principalName);
                    _User.CommitChanges();
                    _User.RefreshCache();

                    System.Threading.Thread.Sleep(10000); //Give system time to finalize creating the user.

                    SetUserPassword(_User, userPassword, shouldPasswordExpire);

                    _LDAP.CommitChanges();
                    _LDAP.RefreshCache();
                }

                //now get the user in the groups requested
                foreach (string secGroup in secGroups)
                    PutUserInGroup(secGroup, _OU, _User, domainName, domainUser, domainPassword);
            }
            catch (Exception)
            {
                throw;
            }
            return _User;
        }

        public void SetUserPassword(string ouName, string userID, string userPassword, bool shouldPasswordExpire)
        {
            SetUserPassword(ouName, userID, userPassword, shouldPasswordExpire, string.Empty, string.Empty, string.Empty);
        }

        public void SetUserPassword(string ouName, string userID, string userPassword, bool shouldPasswordExpire, string domainName, string domainUser, string domainPassword)
        {
            DirectoryEntry _LDAP = null;
            DirectoryEntry _OU = null;
            DirectoryEntry _User = null;

            _LDAP = GetLDAPEntry(domainName, domainUser, domainPassword);
            _OU = GetOrgUnit(ouName, false, domainName, domainUser, domainPassword);

            //Now try to locate the User within the AD 
            try
            {
                _User = _OU.Children.Find("CN=" + userID, "user");
                if (_User != null)
                    SetUserPassword(_User, userPassword, shouldPasswordExpire);
            }
            catch (Exception)
            {
                _User = null;
            }
            _LDAP.CommitChanges();
            _LDAP.RefreshCache();

        }

        public void SetUserPassword(DirectoryEntry _User, string userPassword, bool shouldPasswordExpire)
        {
            if (_User != null)
            {
                _User.Invoke("SetPassword", new Object[] { userPassword });
                _User.CommitChanges();

                if (!shouldPasswordExpire)
                    _User.Properties["userAccountControl"].Value = 512 | 65536 | 32;
                else
                    _User.Properties["userAccountControl"].Value = 512 | 32;

                _User.CommitChanges();
            }
        }

        public void PutUserInGroup(string securityGroup, DirectoryEntry orgUnit, DirectoryEntry userAccount)
        {
            PutUserInGroup(securityGroup, orgUnit, userAccount, System.Environment.UserDomainName, string.Empty, string.Empty);
        }

        public void PutUserInGroup(string securityGroup, DirectoryEntry orgUnit, DirectoryEntry userAccount, string domainName, string domainUser, string domainPassword)
        {
            bool userExistsInGroup = false;

            if (securityGroup.Trim() != string.Empty)
            {
                DirectoryEntry secGroupDE = null;
                try
                {
                    secGroupDE = orgUnit.Children.Find("CN=" + securityGroup, "group");
                }
                catch (Exception)
                {
                    secGroupDE = null;
                }

                if (secGroupDE != null)
                {
                    //Before we add the user, let's make sure the user isn't already in the group.
                    DirectorySearcher dirSearcher = new System.DirectoryServices.DirectorySearcher(secGroupDE);
                    dirSearcher.Filter = "(&(objectClass=user))";

                    foreach (SearchResult result in dirSearcher.FindAll()) //FYI: This method does not always return valid results (AD Issue)
                    {
                        DirectoryEntry resultEntry = result.GetDirectoryEntry();
                        if (resultEntry.Properties["sAMAccountName"].Value.ToString().ToUpper().Equals(userAccount.Properties["sAMAccountName"].Value.ToString().ToUpper()))
                        {
                            userExistsInGroup = true;
                            break;
                        }
                    }
                    if (!userExistsInGroup)
                    {
                        try
                        {
                            secGroupDE.Invoke("Add", new Object[] { userAccount.Path.ToString() });
                            secGroupDE.CommitChanges();
                        }
                        catch (Exception) { } //Nothing to do here, the user is in the group
                    }
                }
                else
                    throw new Exception(string.Format("Security Group '{0}' was not found", securityGroup));
            }
        }

        public string[] GetDomainComputers(string domainName)
        {
            return GetDomainComputers(domainName, string.Empty, string.Empty);
        }

        public void ShowDomainComputers()
        {
            DirectoryEntry domain = new DirectoryEntry("LDAP://domain.com/CN=Computers,DC=Domain,DC=com");

            foreach (DirectoryEntry child in domain.Children)
            {
                Console.WriteLine(child.Name);
            }
        }

        public string[] GetDomainComputers(string domainName, string domainUser, string domainPassword)
        {
            string returnComputers = string.Empty;
            DirectoryEntry domain;

            domain = new DirectoryEntry("WinNT://" + domainName, domainUser, domainPassword);
            domain.Children.SchemaFilter.Add("computer");

            foreach (DirectoryEntry computer in domain.Children)
                returnComputers += computer.Name + ",";

            if(returnComputers.Contains(","))
                returnComputers = returnComputers.Substring(0, returnComputers.Length - 1);

            return returnComputers.Split(',');
        }

        public DirectoryEntry GetLDAPEntry(string domainName, string domainUser, string domainPassword)
        {
            if (domainUser != string.Empty)
                return new DirectoryEntry("LDAP://" + domainName, domainUser, domainPassword);
            else
                return new DirectoryEntry("LDAP://" + domainName);
        }
        
       #endregion
    }
}
