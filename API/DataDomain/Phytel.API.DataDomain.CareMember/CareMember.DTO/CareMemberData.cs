using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.CareMember.DTO
{
    public class CareMemberData
    {
        public string Id { get; set; }
        public string PatientId { get; set; }
        public string ContactId { get; set; }
        public string TypeId { get; set; }
        public bool Primary { get; set; }
    }
}
