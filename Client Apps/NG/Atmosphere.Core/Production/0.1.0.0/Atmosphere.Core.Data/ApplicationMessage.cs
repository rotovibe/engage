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
    public class ApplicationMessage
    {
        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public bool Audit { get; set; }

        public static ApplicationMessage Build(ITypeReader reader)
        {
            ApplicationMessage applicationMessage = new ApplicationMessage();
            applicationMessage.Code = reader.GetString("Code");
            applicationMessage.Title = reader.GetString("Title");
            applicationMessage.Text = reader.GetString("Text");
            applicationMessage.Audit = reader.GetBool("Audit");
            return applicationMessage;
        }
    }
}
