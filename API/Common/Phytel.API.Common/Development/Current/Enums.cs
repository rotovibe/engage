using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.Common
{


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
        Delete,
        UndoDelete
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
        Response,
        Program,
        Module,
        Action,
        Step,
        ToDo,
        Schedule,
        Allergy,
        PatientAllergy,
        Medication,
        PatientMedSupp,
        MedicationMap,
        PatientMedFrequency,
        PatientUtilization,
        CareTeam
    }
}
