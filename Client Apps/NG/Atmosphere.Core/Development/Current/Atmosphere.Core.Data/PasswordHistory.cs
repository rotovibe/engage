using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using Phytel.Framework.SQL.Data;

namespace C3.Data
{
    [Serializable]
    [DataContract]
    public class PasswordHistory
    {
        [DataMember]
        public Guid UserId { get; set; }

        [DataMember]
        public string Password { get; set; }

        public static PasswordHistory Build(ITypeReader reader)
        {
            PasswordHistory PasswordHistory = new PasswordHistory();
            PasswordHistory.UserId = reader.GetGuid("UserId");
            PasswordHistory.Password = reader.GetString("Password");
            return PasswordHistory;
        }
    }
}
