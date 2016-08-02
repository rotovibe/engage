using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace C3.Data.Enum
{
    public enum GroupEntityTypes
    {
        [EnumMember]
        Schedule = 1,
        [EnumMember]
        Subscriber = 2,
    }
}
