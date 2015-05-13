using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace C3.Data
{
    [Serializable]
    [DataContract]
    public class Password
    {
        [DataMember]
        public DateTime? PasswordExpiration { get; set; }

        public bool IsExpired()
        {
            return PasswordExpiration == null || PasswordExpiration <= DateTime.Now;
        }
    }
}
