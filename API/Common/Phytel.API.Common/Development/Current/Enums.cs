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
        Completed = 2
    }

    public enum GenericStatus
    {
        NotSet = 0,
        Pending = 1
    }

    public enum GenericSetting
    {
        No = 0,
        Yes = 1
    }
}
