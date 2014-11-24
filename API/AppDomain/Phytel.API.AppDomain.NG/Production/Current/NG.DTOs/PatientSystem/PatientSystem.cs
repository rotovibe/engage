using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class PatientSystem
    {
        public string Id { get; set; }
        public string PatientId { get; set; }
        public string SystemId { get; set; }
        public string SystemName { get; set; }
        public string DisplayLabel { get; set; }
        public bool DeleteFlag { get; set; }
    }
}
