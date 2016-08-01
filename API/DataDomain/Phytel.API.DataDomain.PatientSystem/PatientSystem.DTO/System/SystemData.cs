using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.API.DataDomain.PatientSystem.DTO
{
    public class SystemData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Field { get; set; }
        public string DisplayLabel { get; set; }
        public int StatusId { get; set; }
        public bool Primary { get; set; }
    }
}
