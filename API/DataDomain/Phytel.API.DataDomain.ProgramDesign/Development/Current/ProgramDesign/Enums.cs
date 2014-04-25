using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.ProgramDesign
{
    public enum Status
    {
        Active = 1,
        Inactive = 2,
        InReview = 3,
        Met = 4,
        NotMet = 5
    } 
    
    public enum SpawnElementTypeCode
    {
        Program = 1,
        Module = 2,
        Action = 3,
        Step = 4,
        Eligibility = 10,
        IneligibleReason = 11,
        ProgramState = 12,
        ProgramStartdate = 13,
        ProgramEnddate = 14,
        EnrollmentStatus = 15,
        OptOut = 16,
        //OptOutReason = 17,
        //OptOutDate = 18,
        Graduated = 19,
        Locked = 20,
        //EligibilityOverride = 21,
        Diabetes = 101
    }

    public enum SelectType
    {
        Single = 1,
        Multi = 2
    }

    public enum ControlType
    {
        CheckBox = 1,
        List = 2,
        Radio = 3
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

    public enum EnrollmentStatus
    {
        Enrolled = 1,
        Pending = 2,
        DidNotEnroll = 3,
        Disenrolled = 4
    }

    public enum EligibilityStatus
    {
        NotEligible = 1,
        Eligible = 2,
        Pending = 3
    }

    public enum Locked
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
}
