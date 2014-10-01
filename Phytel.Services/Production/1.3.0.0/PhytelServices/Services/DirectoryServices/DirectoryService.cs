using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.IO;
using System.DirectoryServices;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Phytel.Services
{
    public sealed class DirectoryService
    {
        public enum AccessMaskTypes { FullControl, Change, Read };

        private static volatile DirectoryService instance;
        private static object syncRoot = new Object();
 
        #region Instance Methods
        private DirectoryService() { }

        public static DirectoryService Instance
        {
            get 
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DirectoryService();
                    }
                }

                return instance;
            }
        }
        #endregion

        private object[] GetACEPermissions(string[] userAccountNames, AccessMaskTypes accessMask, AceFlags aflag, AceType atype)
        {
            object[] returnObjects = new object[userAccountNames.Length];
            int i = 0;
            foreach (string userAccountName in userAccountNames)
            {
                NTAccount ntAccount = new NTAccount(userAccountName); // or whatever account you will grant access
                SecurityIdentifier sid = (SecurityIdentifier)ntAccount.Translate(typeof(SecurityIdentifier));
                byte[] sidArray = new byte[sid.BinaryLength];
                sid.GetBinaryForm(sidArray, 0);

                ManagementObject userTrustee = new ManagementClass(new ManagementPath("Win32_Trustee"), null);
                userTrustee["SID"] = sidArray;

                //==1. Create win32_ace
                ManagementObject userACE = new ManagementClass(new ManagementPath("Win32_Ace"), null);
                string accessMaskValue = string.Empty;
                switch (accessMask)
                {
                    case AccessMaskTypes.FullControl:
                        accessMaskValue = "2032127";
                        break;
                    case AccessMaskTypes.Change:
                        accessMaskValue = "1245631";
                        break;
                    default:
                        accessMaskValue = "1179785";
                        break;
                }
                userACE["AccessMask"] = accessMaskValue;
                userACE["AceFlags"] = aflag;
                userACE["AceType"] = atype;
                userACE["Trustee"] = userTrustee;

                returnObjects[i] = userACE;
                i++;
            }
            return returnObjects;
        }

        public void CreateShare(string serverName, string path, string name, string description, string[] userPermissions, AccessMaskTypes accessMaskType)
        {
            ManagementObject secDescriptor = new ManagementClass(new ManagementPath("Win32_SecurityDescriptor"), null);
            secDescriptor["ControlFlags"] = 4;
            secDescriptor["DACL"] = this.GetACEPermissions(userPermissions, accessMaskType, AceFlags.ObjectInherit | AceFlags.ContainerInherit, AceType.AccessAllowed);

            ManagementClass classObj = new ManagementClass("\\\\" + serverName + "\\root\\cimv2", "Win32_Share", null);
            ManagementBaseObject inParams = classObj.GetMethodParameters("Create");
            inParams["Access"] = secDescriptor;
            inParams["Description"] = description;
            inParams["Name"] = name;
            inParams["Path"] = path;
            inParams["Type"] = 0; //0: Disk Drive

            ManagementBaseObject outParams = classObj.InvokeMethod("Create", inParams, null);
            uint ret = (uint)(outParams.Properties["ReturnValue"].Value);
            if (ret != 0)
                throw new Exception(string.Format("Failed to create Share '{0}'.  Return Code: {1}", name, ret.ToString()));
        }

        public void AddDirectorySecurity(string FileName, string[] identity, FileSystemRights flSystemRights, InheritanceFlags iFlags, PropagationFlags pFlags, AccessControlType type, bool setInheritance)
        {
            DirectoryInfo dInfo = new DirectoryInfo(FileName);
            DirectorySecurity ds = new DirectorySecurity();

            foreach (string userIdentity in identity)
            {
                ds.SetAccessRule(new FileSystemAccessRule(new NTAccount(userIdentity), flSystemRights, iFlags, pFlags, type));

                //The add statement applies to the subordinate folders/files for each user listed 
                ds.AddAccessRule(new FileSystemAccessRule(new NTAccount(userIdentity), flSystemRights, iFlags, pFlags, type));
            }
            ds.SetAccessRuleProtection(setInheritance, setInheritance);

            dInfo.SetAccessControl(ds);
        }

        public void CopyDirectory(string src, string dest, bool setPermissions, bool applyInheritance, bool applyChildInheritance, string[] identity, FileSystemRights flSystemRights, InheritanceFlags iFlags, PropagationFlags pFlags, AccessControlType type)
        {
            if (!Directory.Exists(dest))
                Directory.CreateDirectory(dest);

            if (setPermissions)
                AddDirectorySecurity(dest, identity, flSystemRights, iFlags, pFlags, type, applyInheritance);

            if (Directory.Exists(src) & Directory.Exists(dest))
            {
                DirectoryInfo di = new DirectoryInfo(src);

                foreach (FileSystemInfo fsi in di.GetFileSystemInfos())
                {
                    try
                    {
                        string destName = Path.Combine(dest, fsi.Name);
                        if (fsi is FileInfo)
                        {
                            if (fsi.Attributes == FileAttributes.ReadOnly)
                                fsi.Attributes = FileAttributes.Normal;

                            File.Copy(fsi.FullName, destName, true);
                        }
                        else
                        {
                            Directory.CreateDirectory(destName);
                            CopyDirectory(fsi.FullName, destName, setPermissions, applyChildInheritance, applyChildInheritance, identity, flSystemRights, iFlags, pFlags, type);
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            else
            {
                throw new Exception("Source or target directory doesn't exist.");
            }
        }

        /// <summary>
        /// Overloaded method to Copy directory
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="identity"></param>
        /// <param name="flSystemRights"></param>
        /// <param name="type"></param>
        public void CopyDirectory(string src, string dest, bool setPermissions, InheritanceFlags iFlags, PropagationFlags pFlags,
                               string[] identity, FileSystemRights flSystemRights, AccessControlType type)
        {
            if (!Directory.Exists(dest))
            {
                Directory.CreateDirectory(dest);
            }
            if (setPermissions)
            {
                AddDirectorySecurity(dest, identity, flSystemRights, iFlags, pFlags, type);
            }
            if (Directory.Exists(src) & Directory.Exists(dest))
            {
                DirectoryInfo di = new DirectoryInfo(src);

                foreach (FileSystemInfo fsi in di.GetFileSystemInfos())
                {
                    try
                    {
                        string destName = Path.Combine(dest, fsi.Name);
                        if (fsi is FileInfo)
                        {
                            if (fsi.Attributes == FileAttributes.ReadOnly)
                                fsi.Attributes = FileAttributes.Normal;
                            {

                                File.Copy(fsi.FullName, destName, true);

                            }
                        }
                        else
                        {
                            Directory.CreateDirectory(destName);
                            CopyDirectory(fsi.FullName, destName, setPermissions, iFlags, pFlags, identity, flSystemRights, type);
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            else
            {
                throw new Exception("Source or target directory doesn't exist.");
            }
        }

        /// <summary>
        /// Overloaded method to create security permissions for folders
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="identity"></param>
        /// <param name="flSystemRights"></param>
        /// <param name="type"></param>
        public void AddDirectorySecurity(string FileName, string[] identity, FileSystemRights flSystemRights, InheritanceFlags iFlags, PropagationFlags pFlags,
                                                AccessControlType type)
        {
            DirectoryInfo dInfo = new DirectoryInfo(FileName);
            DirectorySecurity ds = new DirectorySecurity();
            foreach (string userIdentity in identity)
            {
                ds.AddAccessRule(new FileSystemAccessRule(new NTAccount(userIdentity), flSystemRights, iFlags, pFlags, type));
            }
            ds.SetAccessRuleProtection(false, false);
            dInfo.SetAccessControl(ds);
        }

    }
}
