using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace C3.Data.Enum
{
    [Serializable]
    [DataContract]
    public enum PermissionTypes
    {
        [EnumMember]
        View = 1,
        [EnumMember]
        Edit = 2,
        [EnumMember]
        Delete = 3,
        [EnumMember]
        Print = 4,
        [EnumMember]
        Export = 5, 
    }

    [Serializable]
    [DataContract]
    public enum Permissions
    {
        VIEW_APPOINTMENTS = 1,
        VIEW_OUTREACH = 2,
        VIEW_REPORTS = 3,
        VIEW_ADMIN = 4,
        VIEW_PATIENTSUMMARY = 5,
        VIEW_IMPERSONATION = 6,
        EDIT_IMPERSONATION = 7,
        VIEW_USERS = 8,
        VIEW_PROFILE = 10,
        EDIT_PROFILE = 11,
        VIEW_PRINT = 12,
        EDIT_PROFILE_PERMISSIONS = 13
    }
}
