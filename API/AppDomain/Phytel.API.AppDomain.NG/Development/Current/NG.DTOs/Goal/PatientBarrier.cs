using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class PatientBarrier
    {
        public string Id { get; set; }
        public string PatientGoalId { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public DateTime? StatusDate { get; set; }
    }
}
