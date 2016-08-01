using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Program.DTO
{
    public enum RepositoryType
    {
        Program,
        PatientProgram,
        ContractProgram,
        Response,
        PatientProgramResponse,
        PatientProgramAttribute
    }

    public enum AssignToType
    {
        Unassigned = 1,
        PCM = 2
    }

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
        Graduated = 19,
        Locked = 20,
        Problem = 101,
        ToDo = 111
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
    //public enum ProgramState
    //{
    //    Removed = -1,
    //    NotStarted = 0,
    //    Started = 1,
    //    Completed = 2,
    //    Closed = 3
    //}

    public enum ElementState
    {
        NotStarted = 2,
        Started = 3,
        InProgress = 4,
        Completed = 5,
        Closed = 6
    }

    public enum EnrollmentStatus
    {
        Enrolled = 1,
        Pending = 2,
        DidNotEnroll = 3,
        Disenrolled = 4,
        CompletedProgram  = 5
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
        None = 0,
        No = 1,
        Yes = 2
    }
}
