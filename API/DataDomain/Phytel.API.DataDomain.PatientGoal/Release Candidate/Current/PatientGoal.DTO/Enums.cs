using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    public enum RepositoryType
    {
        PatientGoal =1,
        PatientBarrier = 2,
        PatientTask = 3,
        PatientIntervention = 4,
        AttributeLibrary = 5,
        Goal = 6,
        Task = 7,
        Intervention = 8
    }
    
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
