using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Phytel.Framework.SQL.Data;

namespace C3.Data
{
    [Serializable]
    [DataContract]
    public class TermsOfService
    {
        public int TermsOfServiceID { get; set; }
        public string TermsOfServiceText{ get; set; }
        public string Version { get; set; }

        public static TermsOfService Build(ITypeReader reader)
        {
            TermsOfService tos = new TermsOfService();
            tos.TermsOfServiceID = reader.GetInt("TermsOfServiceID"); 
            tos.TermsOfServiceText = reader.GetString("Text");
            tos.Version = reader.GetString("Version");
            return tos;
        }
    }
}
