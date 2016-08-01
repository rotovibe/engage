using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    public class PatientBarrierData
    {
        public string Id { get; set; }
        public string PatientGoalId { get; set; }
        public string CategoryId { get; set; }
        public string Name { get; set; }
        public int StatusId { get; set; }
        public DateTime? StatusDate { get; set; }
        public bool DeleteFlag { get; set; }
        public string Details { get; set; }
    }
}
