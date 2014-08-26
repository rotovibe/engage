using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.Programs;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientObservation.DTO;
using Phytel.API.DataDomain.Program.DTO;

namespace Phytel.API.AppDomain.NG.Test.Stubs
{
    public class StubEndpointUtils : IEndpointUtils
    {
        public DTO.PatientObservation GetPatientProblem(string probId, PlanCOR.PlanElementEventArg e, string userId)
        {
            return new DTO.PatientObservation { Id = "101010101010101010101010", Name="fake Patient Observation"};
        }

        public ProgramAttributeData GetProgramAttributes(string planElemId, Interface.IAppDomainRequest request)
        {
            throw new NotImplementedException();
        }

        public List<StepResponse> GetResponsesForStep(string stepId, Interface.IAppDomainRequest request)
        {
            throw new NotImplementedException();
        }

        public bool InsertNewProgramAttribute(ProgramAttributeData pa, Interface.IAppDomainRequest request)
        {
            throw new NotImplementedException();
        }

        public PutRegisterPatientObservationResponse PutNewPatientProblem(string patientId, string userId, string elementId, Interface.IAppDomainRequest request)
        {
            return new PutRegisterPatientObservationResponse { };
        }

        public CohortPatientViewData RequestCohortPatientViewData(string patientId, Interface.IAppDomainRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.Program RequestPatientProgramDetail(DTO.IProcessActionRequest request)
        {
            throw new NotImplementedException();
        }

        public GetProgramDetailsSummaryResponse RequestPatientProgramDetailsSummary(DTO.GetPatientProgramDetailsSummaryRequest request)
        {
            throw new NotImplementedException();
        }

        public ProgramDetail SaveAction(DTO.IProcessActionRequest request, string actionId, DTO.Program p, bool repeat)
        {
            throw new NotImplementedException();
        }

        public void UpdateCohortPatientViewProblem(CohortPatientViewData cpvd, string patientId, Interface.IAppDomainRequest request)
        {
            throw new NotImplementedException();
        }

        public PutUpdateObservationDataResponse UpdatePatientProblem(string patientId, string userId, string elementId, DTO.PatientObservation pod, bool _active, Interface.IAppDomainRequest request)
        {
            return new PutUpdateObservationDataResponse { Result = true};
        }

        public bool UpdateProgramAttributes(DataDomain.Program.DTO.ProgramAttributeData pAtt, Interface.IAppDomainRequest request)
        {
            throw new NotImplementedException();
        }

        public DataDomain.Program.DTO.PutProgramToPatientResponse AssignPatientToProgram(DTO.PostPatientToProgramsRequest request, string careManagerId)
        {
            throw new NotImplementedException();
        }

        public string GetPrimaryCareManagerForPatient(DTO.PostPatientToProgramsRequest request)
        {
            throw new NotImplementedException();
        }

        public void SaveResponses(DTO.Actions action, DTO.IProcessActionRequest request, bool repeat)
        {
            throw new NotImplementedException();
        }

        public DTO.Outcome SaveProgramAttributeChanges(DTO.PostProgramAttributesChangeRequest request, DataDomain.Program.DTO.ProgramDetail pg)
        {
            throw new NotImplementedException();
        }
    }
}
