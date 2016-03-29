using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Contact.DTO
{
    public class ContactTypeData
    {
        public ContactTypeData()
        {
            Children = new List<ContactTypeData>();
        }
        
        public string ContactTypeLookupId { get; set; }
        public List<ContactTypeData> Children { get; set; }
    }
}
