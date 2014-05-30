﻿using Phytel.API.AppDomain.NG.Programs;
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

        public DataDomain.Program.DTO.ProgramAttributeData GetProgramAttributes(string planElemId, Interface.IAppDomainRequest request)
        {
            throw new NotImplementedException();
        }

        public List<DataDomain.Program.DTO.StepResponse> GetResponsesForStep(string stepId, Interface.IAppDomainRequest request)
        {
            throw new NotImplementedException();
        }

        public bool InsertNewProgramAttribute(DataDomain.Program.DTO.ProgramAttributeData pa, Interface.IAppDomainRequest request)
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
            DTO.Program prg = new DTO.Program
            {
                Id = "000000000000000000000000",
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
                Modules = new List<DTO.Module>() { 
                    new DTO.Module { Id = "000000000000000000000000", 
                        Name = "Test stub module 1",
                         Description = "BSHSI - Outreach & Enrollment",
                          SourceId ="532b5585a381168abe00042c",
                        Actions = new List<DTO.Actions>(){ 
                            new DTO.Actions{  Id = "000000000000000000000000", 
                                                ElementState = 4, 
                                                Name ="test action from stub",
                                                Description = "action Description",
                                                Text = "test action 1",
                                                AttrEndDate =  DateTime.UtcNow.AddDays(10),
                                                AttrStartDate =  DateTime.UtcNow,
                                                AssignDate = System.DateTime.UtcNow
                            } }
                    }
                },
                Text = "This is a sample patient program for the request patient details summary test stub",
                Attributes = new DTO.ProgramAttribute
                {
                    //AssignedBy = "me",
                    //AssignedOn = System.DateTime.UtcNow,
                    Id = "000000000000000000000000",
                    PlanElementId = "000000000000000000000000"
                }
            };

            return prg;
        }

        public GetProgramDetailsSummaryResponse RequestPatientProgramDetailsSummary(DTO.GetPatientProgramDetailsSummaryRequest request)
        {
            GetProgramDetailsSummaryResponse response = new GetProgramDetailsSummaryResponse();

            response.Program = new ProgramDetail
            {
                Id = "000000000000000000000000",
                AssignBy = request.UserId,
                Description = "this is a test program from the stub.",
                AssignDate = System.DateTime.UtcNow,
                Client = "NG",
                Completed = false,
                ContractProgramId = "123456789098765432167846",
                StateUpdatedOn = DateTime.UtcNow,
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
                         Description = "BSHSI - Outreach & Enrollment",
                          SourceId ="532b5585a381168abe00042c",
                        Actions = new List<ActionsDetail>(){ 
                            new ActionsDetail{  Id = "000000000000000000000000", 
                                                ElementState = 4, 
                                                Name ="test action from stub",
                                                Description = "action Description",
                                                Text = "test action 1",
                                                AttrEndDate =  DateTime.UtcNow.AddDays(10),
                                                AttrStartDate =  DateTime.UtcNow,
                                                AssignTo = "5325d9f7d6a4850adcbba4da",
                                                AssignBy = "5325d9f7d6a4850adcbb2323",
                                                AssignDate = System.DateTime.UtcNow,
                                                Objectives = getObjectives()
                            } } 
                    }
                },
                Text = "This is a sample patient program for the request patient details summary test stub",
                Attributes = new ProgramAttributeData
                {
                    //AssignedBy = "me",
                    //AssignedOn = System.DateTime.UtcNow,
                    Id = "000000000000000000000000",
                    PlanElementId = "000000000000000000000000"
                }
            };

            return response;
        }

        private List<ObjectiveInfoData> getObjectives()
        {
            List<ObjectiveInfoData> list = new List<ObjectiveInfoData>();
            list.Add(new ObjectiveInfoData { Id = "5325da08d6a4850adcbba50e", Status = 1, Unit = "oz", Value = "21" });
            list.Add(new ObjectiveInfoData { Id = "5325da11d6a4850adcbba522", Status = 1, Unit = "lbs", Value = "455" });
            return list;
        }

        public DataDomain.Program.DTO.ProgramDetail SaveAction(DTO.IProcessActionRequest request, string actionId, DTO.Program p)
        {
            return new ProgramDetail();
        }

        public void UpdateCohortPatientViewProblem(DataDomain.Patient.DTO.CohortPatientViewData cpvd, string patientId, Interface.IAppDomainRequest request)
        {
            throw new NotImplementedException();
        }

        public DataDomain.PatientObservation.DTO.PutUpdateObservationDataResponse UpdatePatientProblem(string patientId, string userId, string elementId, DTO.Observation.PatientObservation pod, bool _active, Interface.IAppDomainRequest request)
        {
            throw new NotImplementedException();
        }

        public bool UpdateProgramAttributes(DataDomain.Program.DTO.ProgramAttributeData pAtt, Interface.IAppDomainRequest request)
        {
            throw new NotImplementedException();
        }


        public PutProgramToPatientResponse AssignPatientToProgram(DTO.PostPatientToProgramsRequest request, string careManagerId)
        {
            PutProgramToPatientResponse response = new PutProgramToPatientResponse
            {
                program = new ProgramInfo
                {
                    Id = "111111111111111111111111",
                    ShortName = "shortname",
                    Status = 1,
                    Name = "programname",
                    PatientId = request.PatientId,
                    ElementState = 1,
                    ProgramState = 1
                },
                Outcome = new Outcome
                {
                    Result = 1,
                    Reason = "Success"
                },
                Version = 1.0
            };
            return response;
        }

        public string GetPrimaryCareManagerForPatient(DTO.PostPatientToProgramsRequest request)
        {
            string pcmId = "123456789012345678901234";
            return pcmId;
        }
    }
}
