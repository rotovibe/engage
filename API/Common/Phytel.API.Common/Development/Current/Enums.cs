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
    /// depricated. see ElementState
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
        Removed = 1,
        NotStarted = 2,
        Started = 3,
        InProgress = 4,
        Completed = 5
    }

    public enum GenericStatus
    {
        NotSet = 1,
        Pending = 2,
        NotEligible = 3
    }

    public enum EnrollmentStatus
    {
        NotSet = 1,
        Pending = 2,
        NotEligible = 3
    }

    public enum EligibilityStatus
    {
        NotEligible = 1,
        Eligible = 2
    }

    public enum Locked
    {
        No = 1,
        Yes = 2
    }

    public enum GenericSetting
    {
        No = 1,
        Yes = 2
    }

    public enum EligibilityOverride
    {
        No = 1,
        Yes = 2
    }

    public enum Graduated
    {
        No = 1,
        Yes = 2
    }

    public enum Completed
    {
        No = 1,
        Yes = 2
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
