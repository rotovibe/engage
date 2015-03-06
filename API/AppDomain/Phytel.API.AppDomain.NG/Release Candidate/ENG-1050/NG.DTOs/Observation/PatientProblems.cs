using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class PatientProblems
    {
        public string Id { get; set; }
        public string ObservationId { get; set; }
        public string Name { get; set; }
        public bool Standard { get; set; }
        public int State { get; set; }
        public int Display { get; set; }
    }
}
