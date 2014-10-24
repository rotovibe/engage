using System;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Allergy.DTO
{
    public class DdAllergy
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
