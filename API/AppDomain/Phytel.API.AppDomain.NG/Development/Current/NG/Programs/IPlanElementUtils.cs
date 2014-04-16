using System;
namespace Phytel.API.AppDomain.NG
{
    public interface IPlanElementUtils
    {
        event ProcessedElementInUtilEventHandlers _processedElementEvent;
        Phytel.API.AppDomain.NG.DTO.PlanElement ActivatePlanElement(string p, Phytel.API.AppDomain.NG.DTO.Program program);
        Phytel.API.AppDomain.NG.DTO.Program CloneProgram(Phytel.API.AppDomain.NG.DTO.Program pr);
        void DisableCompleteButtonForAction(System.Collections.Generic.List<Phytel.API.AppDomain.NG.DTO.Step> list);
        T FindElementById<T>(System.Collections.Generic.List<T> list, string id);
        void FindIdInActions(string p, Phytel.API.AppDomain.NG.DTO.Module m);
        void FindIdInSteps(string p, Phytel.API.AppDomain.NG.DTO.Actions a);
        Phytel.API.DataDomain.Patient.DTO.CohortPatientViewData GetCohortPatientViewRecord(string patientId, Phytel.API.Interface.IAppDomainRequest request);
        Phytel.API.AppDomain.NG.DTO.Actions GetProcessingAction(System.Collections.Generic.List<Phytel.API.AppDomain.NG.DTO.Module> list, string actionId);
        void HydratePlanElementLists(System.Collections.Generic.List<object> ProcessedElements, Phytel.API.AppDomain.NG.DTO.PostProcessActionResponse response);
        Phytel.API.AppDomain.NG.DTO.PlanElement InitializePlanElementSettings(Phytel.API.AppDomain.NG.DTO.PlanElement pe, Phytel.API.AppDomain.NG.DTO.PlanElement p);
        bool IsActionInitial(Phytel.API.AppDomain.NG.DTO.Program p);
        bool IsProgramCompleted(Phytel.API.AppDomain.NG.DTO.Program p, string userId);
        bool ModifyProgramAttributePropertiesForUpdate(Phytel.API.DataDomain.Program.DTO.ProgramAttribute pAtt, Phytel.API.DataDomain.Program.DTO.ProgramAttribute _pAtt);
        void OnProcessIdEvent(object pe);
        void RegisterCohortPatientViewProblemToPatient(string problemId, string patientId, Phytel.API.Interface.IAppDomainRequest request);
        bool ResponseSpawnAllowed(Phytel.API.AppDomain.NG.DTO.Step s, Phytel.API.AppDomain.NG.DTO.Response r);
        void SaveReportingAttributes(Phytel.API.DataDomain.Program.DTO.ProgramAttribute _programAttributes, Phytel.API.Interface.IAppDomainRequest request);
        bool SetCompletionStatus<T>(System.Collections.Generic.List<T> list);
        void SetElementEnabledState(string p, Phytel.API.AppDomain.NG.DTO.Program program);
        void SetEnabledState<T>(System.Collections.Generic.List<T> list, T x);
        void SetEnabledStatusByPrevious<T>(System.Collections.Generic.List<T> actions);
        void SetInitialProperties(Phytel.API.AppDomain.NG.DTO.IPlanElement m);
        void SetProgramAttributes(Phytel.API.AppDomain.NG.DTO.SpawnElement r, Phytel.API.AppDomain.NG.DTO.Program program, string userId, Phytel.API.DataDomain.Program.DTO.ProgramAttribute progAttr);
        void SetProgramInformation(Phytel.API.DataDomain.Program.DTO.ProgramAttribute _programAttributes, Phytel.API.AppDomain.NG.DTO.Program p);
        void SetStartDateForProgramAttributes(string programId, Phytel.API.Interface.IAppDomainRequest request);
        void SpawnElementsInList(System.Collections.Generic.List<Phytel.API.AppDomain.NG.DTO.SpawnElement> list, Phytel.API.AppDomain.NG.DTO.Program program, string userId, Phytel.API.DataDomain.Program.DTO.ProgramAttribute progAttr);
    }
}
