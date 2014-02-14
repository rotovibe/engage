using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class PatientTask
    {
        public string Id { get; set; }
        public string TargetValue { get; set; }
        public int StatusId { get; set; }
        public DateTime? TargetDate { get; set; }
        public List<Attribute> Attributes { get; set; }
        public List<string> BarrierIds { get; set; }
        public string Description { get; set; }
        public DateTime? StatusDate { get; set; }
        public DateTime? StartDate { get; set; }
    }
}
