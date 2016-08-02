using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class Address
    {
        public string Id { get; set; }
        public string TypeId { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string City { get; set; }
        public string StateId { get; set; }
        public string PostalCode { get; set; }
        public bool Preferred { get; set; }
        public bool OptOut { get; set; }
        public string ExternalRecordId { get; set; }
        public string DataSource { get; set; }
    }
}
