using System;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Contact.DTO
{
    public class ContactTypeLookUpData
    {
        public ContactTypeLookUpData()
        {
            Children = new List<ContactTypeLookUpData>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string ParentId { get; set; }
        public ContactLookUpGroupType Group { get; set; }
        public List<ContactTypeLookUpData> Children { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
