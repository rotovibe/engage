using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.PatientSystem.DTO
{
    public class PatientSystemData
    {
        public string Id { get; set; }
        public string PatientId { get; set; }
        public string SystemSourceId { get; set; }
        public string Value { get; set; }
        public int StatusId { get; set; }
        public bool Primary { get; set; }

        #region DeprecatedPropertiesOfPatientSystem.
        public string SystemID { get; set; }
        public string DisplayLabel { get; set; }
        public string SystemName { get; set; }
        #endregion
    }
}
