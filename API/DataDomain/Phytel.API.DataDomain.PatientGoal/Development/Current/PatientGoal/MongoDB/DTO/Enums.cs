using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    public enum GoalTaskStatus
    {
        Pending,
        Open,
        Met,
        NotMet,
        Abandoned
    }

    internal enum BarrierStatus
    {
        Pending,
        Open,
        Resolved
    }

    internal enum InterventionStatus
    {
        Pending,
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
