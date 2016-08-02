using System;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Allergy.DTO
{
    public class AllergyData
    {
        public string Id { get; set; }
        public List<string> TypeIds { get; set; }
        public string Name { get; set; }
        public string CodingSystemId { get; set; }
        public string CodingSystemCode { get; set; }
        public double Version { get; set; }
        public bool DeleteFlag { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
