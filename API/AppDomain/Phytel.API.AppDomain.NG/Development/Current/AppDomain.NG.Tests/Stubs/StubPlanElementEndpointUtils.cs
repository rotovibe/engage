using Phytel.API.AppDomain.NG.Programs;
using Phytel.API.DataDomain.Program.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.Test.Stubs
{
    public class StubPlanElementEndpointUtils : IEndpointUtils
    {
        public IRestClient Client { get; set; }

        public DTO.Observation.PatientObservation GetPatientProblem(string probId, PlanCOR.PlanElementEventArg e, string userId)
        {
            throw new NotImplementedException();
        }

        public DataDomain.Program.DTO.ProgramAttribute GetProgramAttributes(string planElemId, Interface.IAppDomainRequest request)
        {
            throw new NotImplementedException();
        }

        public List<DataDomain.Program.DTO.StepResponse> GetResponsesForStep(string stepId, Interface.IAppDomainRequest request)
        {
            throw new NotImplementedException();
        }

        public bool InsertNewProgramAttribute(DataDomain.Program.DTO.ProgramAttribute pa, Interface.IAppDomainRequest request)
        {
            throw new NotImplementedException();
        }

        public DataDomain.PatientObservation.DTO.PutRegisterPatientObservationResponse PutNewPatientProblem(string patientId, string userId, string elementId, Interface.IAppDomainRequest request)
        {
            throw new NotImplementedException();
        }

        public DataDomain.Patient.DTO.CohortPatientViewData RequestCohortPatientViewData(string patientId, Interface.IAppDomainRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.Program RequestPatientProgramDetail(DTO.IProcessActionRequest request)
        {
            throw new NotImplementedException();
        }

        public GetProgramDetailsSummaryResponse RequestPatientProgramDetailsSummary(DTO.GetPatientProgramDetailsSummaryRequest request)
        {
            GetProgramDetailsSummaryResponse response = new GetProgramDetailsSummaryResponse();

            response.Program = new ProgramDetail
            {
                Id = "000000000000000000000000",
                AssignBy = "test",
                Description = "this is a test program from the stub.",
                AssignDate = System.DateTime.UtcNow,
                Client = "NG",
                Completed = false,
                ContractProgramId = "123456789098765432167846",
                ElementState = 4,
                Enabled = true,
                Name = "test stub program",
                ShortName = "t s p",
                EligibilityRequirements = "Individual must be a part of the health plan and have completed HRA and other requirements.",
                EligibilityStartDate = DateTime.UtcNow.AddDays(1),
                EligibilityEndDate = DateTime.UtcNow.AddDays(20),
                Modules = new List<ModuleDetail>() { 
                    new ModuleDetail { Id = "000000000000000000000000", 
                        Name = "Test stub module 1", 
                        Actions = new List<ActionsDetail>(){ 
                            new ActionsDetail{ Id = "000000000000000000000000", ElementState = 4, Name ="test action from stub", Text = "test action 1"} } 
                    }
                },
                Text = "This is a sample patient program for the request patient details summary test stub"
            };

            return response;
        }

        public DataDomain.Program.DTO.ProgramDetail SaveAction(DTO.IProcessActionRequest request, string actionId, DTO.Program p)
        {
            throw new NotImplementedException();
        }

        public void UpdateCohortPatientViewProblem(DataDomain.Patient.DTO.CohortPatientViewData cpvd, string patientId, Interface.IAppDomainRequest request)
        {
            throw new NotImplementedException();
        }

        public DataDomain.PatientObservation.DTO.PutUpdateObservationDataResponse UpdatePatientProblem(string patientId, string userId, string elementId, DTO.Observation.PatientObservation pod, bool _active, Interface.IAppDomainRequest request)
        {
            throw new NotImplementedException();
        }

        public bool UpdateProgramAttributes(DataDomain.Program.DTO.ProgramAttribute pAtt, Interface.IAppDomainRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
