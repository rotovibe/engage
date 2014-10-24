using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class Allergy
    {
        public string Id { get; set; }
        public List<string> SubType { get; set; }
        public string Description { get; set; }
        public string CodingSystem { get; set; }
        public string CodingSystemCode { get; set; }
        public int Status { get; set; }
        public double Version { get; set; }
        public string UpdatedBy { get; set; }
        public bool DeleteFlag { get; set; }
        public DateTime? TTLDate { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string RecordCreatedBy { get; set; }
        public DateTime RecordCreatedOn { get; set; }
    }
}
