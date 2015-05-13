using System;
using System.Data;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Phytel.Framework.SQL.Data;

namespace C3.Data
{
    [Serializable]
    [DataContract]
    public class AccordionMember
    {
        [DataMember]
        public int ControlId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Path { get; set; }

        [DataMember]
        public string ParentControlName { get; set; }

        [DataMember]
        public int ParentControlId { get; set; }

        [DataMember]
        public int DisplayOrder { get; set; }

        public static AccordionMember Build(ITypeReader reader)
        {
            AccordionMember accordionMember = new AccordionMember();

            accordionMember.ControlId = reader.GetInt("ControlId");
            accordionMember.Name = reader.GetString("Name");
            accordionMember.Path = reader.GetString("Path");
            accordionMember.ParentControlName = reader.GetString("ParentControlName");
            accordionMember.ParentControlId = reader.GetInt("ParentControlId");
            accordionMember.DisplayOrder = reader.GetInt("DisplayOrder");

            return accordionMember;
        }

        /// <summary>
        /// The list that contains the hierarchy to populate an accordion control.
        /// </summary>
        [Serializable]
        public class AccordionMemberList : List<AccordionMember>
        {
            public AccordionMemberList()
            {
            }
        }


    }
}
