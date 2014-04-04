using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.Common
{
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
        OptOutReason = 17,
        OptOutDate = 18,
        Graduated = 19,
        Locked = 20,
        EligibilityOverride = 21,
        Diabetes = 101
    }

    public enum DispatchType
    {
        auditerror,
        auditaction,
        auditview
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

    public enum AttributeControlType
    {
        Single = 1,
        Multi = 2,
        Date = 3,
        DateTime = 4,
        Text = 5
    }

    public enum AuditType
    {
        GetPatient = 375,
        GetActionsInfo = 376,
        GetActivePrograms = 377,
        GetAllCommModes = 378,
        GetAllCommTypes = 379,
        GetAllLanguages = 380,
        GetAllSettings = 381,
        GetAllStates = 382,
        GetAllTimesOfDays = 383,
        GetAllTimeZones = 384,
        GetCohortPatients = 385,
        GetCohorts = 386,
        GetContact = 387,
        GetModuleInfo = 388,
        GetAllPatientProblems = 389,
        GetPatientProgramDetailsSummary = 390,
        GetProblems = 391,
        GetResponses = 392,
        GetSpamElement = 393,
        GetStepsInfo = 394,
        PostPatientToProgram = 395,
        PutPatientDetailsUpdate = 396,
        PutPatientFlaggedUpdate = 397,
        GetContactByPatientID = 398,
        PutContact = 399
    }

    public enum DataAuditType
    { 
        Insert,
        Update,
        Delete
    }

    public enum ObservationState
    {
        Complete = 1,
        Active = 2,
        Inactive = 3,
        Resolved = 4,
        Decline = 5
    }

    public enum MongoCollectionName
    {
        Patient,
        PatientBarrier,
        PatientIntervention,
        PatientTask,
        PatientGoal,
        PatientNote,
        PatientObservation,
        PatientProblem,
        PatientProgram,
        PatientSystem,
        PatientUser,
        PatientProgramAttribute,
        PatientProgramResponse,
        Contact,
        CareMember,
        CohortPatientView,
        Response
    }
}
