using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class PatientIntervention
    {
        public string Id { get; set; }
        public string PatientGoalId { get; set; }
        public int Category { get; set; }
        public string AssignedTo { get; set; }
        public int Order { get; set; }
        public List<string> Barriers { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public DateTime? StatusDate { get; set; }
        public DateTime? StartDate { get; set; }
        public List<Attribute> Attributes { get; set; }
    }
}
