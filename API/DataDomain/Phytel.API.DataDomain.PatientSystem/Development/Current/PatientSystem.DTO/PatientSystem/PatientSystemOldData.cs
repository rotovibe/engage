using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.PatientSystem.DTO
{
    public class PatientSystemOldData
    {
        public string Id { get; set; }
        public string PatientId { get; set; }
        public string SystemId { get; set; }
        public string Value { get; set; }
        public string SystemSource { get; set; }
        public int StatusId { get; set; }
        public bool Primary { get; set; }
        public string CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedById { get; set; }
        public DateTime? UpdatedOn { get; set; }

        #region DeprecatedPropertiesOfPatientSystem.
        public string OldSystemId { get; set; }
        public string DisplayLabel { get; set; }
        public string SystemName { get; set; }
        #endregion
    }
}
