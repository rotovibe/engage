using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace C3.Data.Enum
{
    [Serializable]
    [DataContract]
    public enum UserTypes
    {
        [EnumMember]
        PhytelUser = 1,
        [EnumMember]
        ContractUser = 2
    }
}
