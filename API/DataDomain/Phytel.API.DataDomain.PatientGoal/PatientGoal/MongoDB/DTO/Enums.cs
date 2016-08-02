using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    public enum GoalTaskStatus
    {
        Open = 1,
        Met,
        NotMet,
        Abandoned
    }

    public enum BarrierStatus
    {
        Open = 1,
        Resolved
    }

    public enum InterventionStatus
    {
        Open = 1,
        Completed,
        Removed
    }

    public enum GoalType
    {
        Longterm = 1,
        Shortterm = 2
    }
}
