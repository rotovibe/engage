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
        public string SystemId { get; set; }
        public string SystemName { get; set; }
        public string DisplayLabel { get; set; }
        public bool DeleteFlag { get; set; }
    }

}
