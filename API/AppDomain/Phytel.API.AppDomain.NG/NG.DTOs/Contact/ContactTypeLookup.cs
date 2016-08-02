using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class ContactTypeLookUp
    {
        public ContactTypeLookUp()
        {
           // Children = new List<ContactTypeLookUp>();
        }

        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public int Group { get; set; }        
       // public List<ContactTypeLookUp> Children { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
