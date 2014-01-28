using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.Common
{
    public enum DispatchType
    {
        auditerror,
        auditaction,
        auditview
    }

    /// <summary>
    /// Enum that denotes the various statuses in a workflow for program, module, action, step, objective, etc.
    /// </summary>
    public enum Status
    {
        Active = 1,
        Inactive = 2,
        InReview = 3,
        Met = 4,
        NotMet = 5
    }

    /// <summary>
    /// Program state enums
    /// </summary>
    public enum ProgramState
    {
        Removed = -1,
        NotStarted = 0,
        Started = 1,
        Completed = 2,
        Closed = 3
    }

    public enum ElementState
    {
        Removed = -1,
        NotStarted = 0,
        Started = 1,
        Completed = 2,
        InProgress =3
    }

    public enum GenericStatus
    {
        NotSet = 0,
        Pending = 1,
        NotEligible = 2
    }

    public enum EligibilityStatus
    {
        NotEligible = 0,
        Eligible = 1
    }

    public enum GenericSetting
    {
        No = 0,
        Yes = 1
    }

    public enum StepType
    {
        YesNo = 1,
        Text = 2,
        Input = 3,
        Single = 4,
        Multi = 5,
        Date = 6,
        Complete = 7,
        DateTime = 8,
        Time = 9,
        InputMultiline = 10
    }
}
