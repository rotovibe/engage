using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.Data.ETL.BulkCopy
{
    public class EPhone
    {
        public int ContactId { get; set; }
        public string MongoContactId { get; set; }
        public string MongoId { get; set; }
        public int TypeId { get; set; }
        public string MongoCommTypeId { get; set; }
        public string Number { get; set; }
        public string IsText { get; set; }
        public string PhonePreferred { get; set; }
        public string TextPreferred { get; set; }
        public string OptOut { get; set; }
        public string Delete { get; set; }
        public int Version { get; set; }
        public string MongoUpdatedBy { get; set; }
        public int UpdatedById { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string MongoRecordCreatedBy { get; set; }
        public int RecordCreatedById { get; set; }
        public DateTime? RecordCreatedOn { get; set; }
        public DateTime? TTLDate { get; set; }
    }
}
