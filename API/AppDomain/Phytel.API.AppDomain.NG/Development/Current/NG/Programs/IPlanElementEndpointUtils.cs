using System;
namespace Phytel.API.AppDomain.NG.Programs
{
    public interface IEndpointUtils
    {
        global::Phytel.API.AppDomain.NG.DTO.Observation.PatientObservation GetPatientProblem(string probId, global::Phytel.API.AppDomain.NG.PlanCOR.PlanElementEventArg e, string userId);
        global::Phytel.API.DataDomain.Program.DTO.ProgramAttribute GetProgramAttributes(string planElemId, global::Phytel.API.Interface.IAppDomainRequest request);
        global::System.Collections.Generic.List<global::Phytel.API.DataDomain.Program.DTO.StepResponse> GetResponsesForStep(string stepId, global::Phytel.API.Interface.IAppDomainRequest request);
        bool InsertNewProgramAttribute(global::Phytel.API.DataDomain.Program.DTO.ProgramAttribute pa, global::Phytel.API.Interface.IAppDomainRequest request);
        global::Phytel.API.DataDomain.PatientObservation.DTO.PutRegisterPatientObservationResponse PutNewPatientProblem(string patientId, string userId, string elementId, global::Phytel.API.Interface.IAppDomainRequest request);
        global::Phytel.API.DataDomain.Patient.DTO.CohortPatientViewData RequestCohortPatientViewData(string patientId, global::Phytel.API.Interface.IAppDomainRequest request);
        global::Phytel.API.AppDomain.NG.DTO.Program RequestPatientProgramDetail(global::Phytel.API.AppDomain.NG.DTO.IProcessActionRequest request);
        global::Phytel.API.DataDomain.Program.DTO.GetProgramDetailsSummaryResponse RequestPatientProgramDetailsSummary(global::Phytel.API.AppDomain.NG.DTO.GetPatientProgramDetailsSummaryRequest request);
        global::Phytel.API.DataDomain.Program.DTO.ProgramDetail SaveAction(global::Phytel.API.AppDomain.NG.DTO.IProcessActionRequest request, string actionId, global::Phytel.API.AppDomain.NG.DTO.Program p);
        void UpdateCohortPatientViewProblem(global::Phytel.API.DataDomain.Patient.DTO.CohortPatientViewData cpvd, string patientId, global::Phytel.API.Interface.IAppDomainRequest request);
        global::Phytel.API.DataDomain.PatientObservation.DTO.PutUpdateObservationDataResponse UpdatePatientProblem(string patientId, string userId, string elementId, global::Phytel.API.AppDomain.NG.DTO.Observation.PatientObservation pod, bool _active, global::Phytel.API.Interface.IAppDomainRequest request);
        bool UpdateProgramAttributes(global::Phytel.API.DataDomain.Program.DTO.ProgramAttribute pAtt, global::Phytel.API.Interface.IAppDomainRequest request);
    }
}
