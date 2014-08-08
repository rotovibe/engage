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
        public string PatientID { get; set; }
        public string SystemID { get; set; }
        public string SystemName { get; set; }
        public string DisplayLabel { get; set; }
    }

}
