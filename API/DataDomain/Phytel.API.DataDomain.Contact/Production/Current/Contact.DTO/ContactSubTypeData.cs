using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Contact.DTO
{
    public class ContactSubTypeData
    {
        public string Id { get; set; }
        public string SubTypeId { get; set; }
        public string SpecialtyId { get; set; }
        public List<string> SubSpecialtyIds { get; set; }
    }
}
