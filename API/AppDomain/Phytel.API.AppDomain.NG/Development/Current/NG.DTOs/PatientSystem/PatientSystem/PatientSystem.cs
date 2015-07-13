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
        public string SystemSourceId { get; set; }
        public string Value { get; set; }
        public int StatusId { get; set; }
        public bool Primary { get; set; }
    }
}
