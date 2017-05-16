using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO;
using System;
using Phytel.API.AppDomain.NG.DTO.Goal;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.Interface;
using Program = Phytel.API.AppDomain.NG.DTO.Program;

namespace Phytel.API.AppDomain.NG
{
    public interface IPlanElementUtils
    {
        event ProcessedElementInUtilEventHandlers _processedElementEvent;
        PlanElement ActivatePlanElement(string p, Program program);
        Program CloneProgram(Program pr);
        void DisableCompleteButtonForAction(List<Step> list);
        T FindElementById<T>(List<T> list, string id);
        void FindSpawnIdInActions(Program program, string p, Module m);
        void FindSpawnIdInSteps(Program program, string p, Actions a);
        CohortPatientViewData GetCohortPatientViewRecord(string patientId, IAppDomainRequest request);
        Actions GetProcessingAction(List<Module> list, string actionId);
        void HydratePlanElementLists(List<object> processedElements, PlanElements planElems);
        PlanElement InitializePlanElementSettings(PlanElement pe, PlanElement p, Program program);
        bool IsActionInitial(Program p);
        bool IsProgramCompleted(Program p, string userId);
        bool ModifyProgramAttributePropertiesForUpdate(ProgramAttributeData pAtt, ProgramAttributeData _pAtt);
        void OnProcessIdEvent(object pe);
        void RegisterCohortPatientViewProblemToPatient(string problemId, string patientId, IAppDomainRequest request);
        bool ResponseSpawnAllowed(Step s, Response r);
        void SaveReportingAttributes(ProgramAttributeData _programAttributes, IAppDomainRequest request);
        bool SetCompletionStatus<T>(List<T> list);
        void SetElementEnabledState(string p, Program program);
        void SetEnabledState<T>(List<T> list, T x, string assignToId, bool pEnabled);
        void SetEnabledStatusByPrevious<T>(List<T> planElements, string assignToId, bool pEnabled);
        void SetInitialProperties(string assignToId, IPlanElement m, bool dependent);
        void SetProgramAttributes(SpawnElement r, Program program, string userId, ProgramAttributeData progAttr);
        void SetInitialActions(object x, string assignToId);
        void SetInitialValues(string assignToId, IPlanElement pe);
        void SetProgramInformation(ProgramAttributeData _programAttributes, Program p);
        void SetStartDateForProgramAttributes(string programId, IAppDomainRequest request);
        void SpawnElementsInList(List<SpawnElement> list, Program program, string userId, ProgramAttributeData progAttr);
        ProgramAttribute GetAttributes(ProgramAttributeData programAttributeData);
        Module CloneModule(Module md);
        Actions CloneAction(Actions act);

        bool UpdatePlanElementAttributes(Program pg, PlanElement planElement, string userId, PlanElements planElems);
        void ProcessPlanElementChanges(PlanElements planElems, PlanElement samplePe, PlanElement fPe, string userId);

        Actions CloneRepeatAction(Actions action, string assignedById);
        PatientGoal InsertPatientGoal(PlanElementEventArg arg, Goal goalTemplate);

        PatientIntervention InsertPatientIntervention(PlanElementEventArg arg, PatientGoal patientGoal, Intervention interventionTemplate);

        PatientTask InsertPatientTask(PlanElementEventArg arg, PatientGoal pGoal, Task taskTemplate);

        DateTime? HandleDueDate(int p);
       
        bool GetCompletionStatus<T>(List<T> list);
    }
}
