using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Goal;
using Phytel.API.AppDomain.NG.DTO.Scheduling;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientObservation.DTO;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.DataDomain.Scheduling.DTO;
using Phytel.API.Interface;
using System;
using System.Collections.Generic;
using AD = Phytel.API.AppDomain.NG.DTO;
using DD = Phytel.API.DataDomain.Program.DTO;

namespace Phytel.API.AppDomain.NG.Programs
{
    public interface IEndpointUtils
    {
        PatientObservation GetPatientProblem(string probId, PlanElementEventArg e, string userId);
        DD.ProgramAttributeData GetProgramAttributes(string planElemId, IAppDomainRequest request);
        List<DD.StepResponse> GetResponsesForStep(string stepId, IAppDomainRequest request);
        bool InsertNewProgramAttribute(DD.ProgramAttributeData pa, IAppDomainRequest request);
        PutRegisterPatientObservationResponse PutNewPatientProblem(string patientId, string userId, string elementId, IAppDomainRequest request);
        CohortPatientViewData RequestCohortPatientViewData(string patientId, IAppDomainRequest request);
        AD.Program RequestPatientProgramDetail(IProcessActionRequest request);
        DD.GetProgramDetailsSummaryResponse RequestPatientProgramDetailsSummary(AD.GetPatientProgramDetailsSummaryRequest request);
        DD.ProgramDetail SaveAction(IProcessActionRequest request, string actionId, AD.Program p, bool repeat);
        void UpdateCohortPatientViewProblem(CohortPatientViewData cpvd, string patientId, IAppDomainRequest request);
        PutUpdateObservationDataResponse UpdatePatientProblem(string patientId, string userId, string elementId, PatientObservation pod, bool _active, IAppDomainRequest request);
        bool UpdateProgramAttributes(DD.ProgramAttributeData pAtt, IAppDomainRequest request);
        DD.PutProgramToPatientResponse AssignPatientToProgram(AD.PostPatientToProgramsRequest request, string careManagerId);
        string GetPrimaryCareManagerForPatient(PostPatientToProgramsRequest request);
        void SaveResponses(Actions action, IProcessActionRequest request, bool repeat);
        AD.Outcome SaveProgramAttributeChanges(PostProgramAttributesChangeRequest request, ProgramDetail pg);

        Schedule GetScheduleToDoById(string p, string userId, IAppDomainRequest request);

        object PutInsertToDo(ToDoData todo, string p, IAppDomainRequest request);
        Goal GetGoalById(string sid, string userId, IAppDomainRequest req);
        PatientGoal GetOpenNotMetPatientGoalByTemplateId(string sid, string patientId, string userId, IAppDomainRequest req);
        Intervention GetInterventionById(string sid, string userId, IAppDomainRequest req);

        PatientIntervention GetOpenNotMetPatientInterventionByTemplateId(string gid, string tempId, string patientId, string userId, IAppDomainRequest req);
        Task GetTaskById(string sid, string userId, IAppDomainRequest req);
        PatientTask GetOpenNotMetPatientTaskByTemplateId(string taskid, string tempId, string patientId, string userId, IAppDomainRequest req);
        List<ToDoData> GetPatientToDos(string patientId, string userId, IAppDomainRequest req);
    }
}
