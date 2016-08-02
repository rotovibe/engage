using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.API.DataDomain.Contact.DTO
{
    public class PhoneData
    {
        public string Id { get; set; }
        public long Number { get; set; }
        public string ExtNumber { get; set; }
        public string TypeId { get; set; }
        public bool IsText { get; set; }
        public bool PhonePreferred { get; set; }
        public bool TextPreferred { get; set; }
        public bool OptOut { get; set; }
        public string ExternalRecordId { get; set; }
        public string DataSource { get; set; }
    }
}
