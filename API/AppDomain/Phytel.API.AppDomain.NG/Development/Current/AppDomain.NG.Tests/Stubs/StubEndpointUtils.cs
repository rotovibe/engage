﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using Phytel.API.AppDomain.NG.Programs;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientObservation.DTO;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.DataDomain.Scheduling.DTO;
using Phytel.API.Interface;

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


        public DTO.Scheduling.Schedule GetScheduleToDoById(string p, string userId, IAppDomainRequest request)
        {
            return new DTO.Scheduling.Schedule
            {
                Id = "888888888888888888888888",
                DueDateRange = 5,
                AssignedToId = ObjectId.GenerateNewId().ToString(),
                CategoryId = ObjectId.GenerateNewId().ToString(),
                CreatedById = ObjectId.GenerateNewId().ToString(),
                Description = "test description",
                ProgramIds = new List<string> {ObjectId.GenerateNewId().ToString()},
                Title = "Test Title for sample",
                PriorityId = 3,
                StatusId = 1,
                PatientId = ObjectId.GenerateNewId().ToString(),
                CreatedOn = DateTime.UtcNow,
                TypeId = 2
            };
        }


        public object PutInsertToDo(ToDoData todo, string p)
        {
            // insert implementation here
            return new object();
        }
    }
}
