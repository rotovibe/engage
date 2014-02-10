using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    internal enum GoalTaskStatus
    {
        Open,
        Met,
        NotMet,
        Abandoned
    }

    internal enum BarrierStatus
    {
        Open,
        Resolved
    }

    internal enum InterventionStatus
    {
        Open,
        Completed,
        Removed
    }

    internal enum GoalType
    {
        Longterm = 1,
        Shortterm = 2
    }
}
