using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.PatientSystem.DTO
{
    public class PatientSystemData : IAppData
    {
        public string Id { get; set; }
        public string PatientId { get; set; }
        public string SystemId { get; set; }
        public string Value { get; set; }
        public string DataSource { get; set; }
        public int StatusId { get; set; }
        public bool Primary { get; set; }
        public string CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedById { get; set; }
        public DateTime? UpdatedOn { get; set; }
        // added for atmosphere integration
        public string ExternalRecordId { get; set; }
    }
}
