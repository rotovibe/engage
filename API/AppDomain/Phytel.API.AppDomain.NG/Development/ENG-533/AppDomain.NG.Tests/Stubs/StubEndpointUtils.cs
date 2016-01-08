using System;
using System.Collections.Generic;
using AutoMapper;
using MongoDB.Bson;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Goal;
using Phytel.API.AppDomain.NG.DTO.Scheduling;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.AppDomain.NG.Programs;
using Phytel.API.Common;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientGoal.DTO;
using Phytel.API.DataDomain.PatientObservation.DTO;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.DataDomain.Scheduling.DTO;
using Phytel.API.Interface;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using Outcome = Phytel.API.AppDomain.NG.DTO.Outcome;
using Program = Phytel.API.AppDomain.NG.DTO.Program;

namespace Phytel.API.AppDomain.NG.Test.Stubs
{
    public class StubEndpointUtils : IEndpointUtils
    {
        public PatientObservation GetPatientProblem(string probId, PlanElementEventArg e, string userId)
        {
            return new PatientObservation { Id = "101010101010101010101010", Name="fake Patient Observation"};
        }

        public ProgramAttributeData GetProgramAttributes(string planElemId, IAppDomainRequest request)
        {
            throw new NotImplementedException();
        }

        public List<StepResponse> GetResponsesForStep(string stepId, IAppDomainRequest request)
        {
            throw new NotImplementedException();
        }

        public bool InsertNewProgramAttribute(ProgramAttributeData pa, IAppDomainRequest request)
        {
            throw new NotImplementedException();
        }

        public PutRegisterPatientObservationResponse PutNewPatientProblem(string patientId, string userId, string elementId, IAppDomainRequest request)
        {
            return new PutRegisterPatientObservationResponse { };
        }

        public CohortPatientViewData RequestCohortPatientViewData(string patientId, IAppDomainRequest request)
        {
            throw new NotImplementedException();
        }

        public Program RequestPatientProgramDetail(IProcessActionRequest request)
        {
            throw new NotImplementedException();
        }

        public GetProgramDetailsSummaryResponse RequestPatientProgramDetailsSummary(GetPatientProgramDetailsSummaryRequest request)
        {
            throw new NotImplementedException();
        }

        public ProgramDetail SaveAction(IProcessActionRequest request, string actionId, Program p, bool repeat)
        {
            throw new NotImplementedException();
        }

        public void UpdateCohortPatientViewProblem(CohortPatientViewData cpvd, string patientId, IAppDomainRequest request)
        {
            throw new NotImplementedException();
        }

        public PutUpdateObservationDataResponse UpdatePatientProblem(string patientId, string userId, string elementId, PatientObservation pod, bool _active, IAppDomainRequest request)
        {
            return new PutUpdateObservationDataResponse { Result = true};
        }

        public bool UpdateProgramAttributes(ProgramAttributeData pAtt, IAppDomainRequest request)
        {
            throw new NotImplementedException();
        }

        public PutProgramToPatientResponse AssignPatientToProgram(PostPatientToProgramsRequest request, string careManagerId)
        {
            throw new NotImplementedException();
        }

        public string GetPrimaryCareManagerForPatient(PostPatientToProgramsRequest request)
        {
            throw new NotImplementedException();
        }

        public void SaveResponses(Actions action, IProcessActionRequest request, bool repeat)
        {
            throw new NotImplementedException();
        }

        public Outcome SaveProgramAttributeChanges(PostProgramAttributesChangeRequest request, ProgramDetail pg)
        {
            throw new NotImplementedException();
        }


        public Schedule GetScheduleToDoById(string p, string userId, IAppDomainRequest request)
        {
            return new Schedule
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
                StatusId = 2,
                PatientId = ObjectId.GenerateNewId().ToString(),
                CreatedOn = DateTime.UtcNow,
                TypeId = 2
            };
        }


        public object PutInsertToDo(ToDoData todo, string p, IAppDomainRequest req)
        {
            // insert implementation here
            return new object();
        }


        public Goal GetGoalById(string sid, string userId, IAppDomainRequest req)
        {
            //var goal = new Goal
            //{
            //    Id = "123456789012345678901234",
            //    EndDate = DateTime.UtcNow,
            //    Name = "sample goal name",
            //    UtilizationSourceId = "5325db9cd6a4850adcbba9ca",
            //    StartDate = DateTime.UtcNow,
            //    StatusId = 1,
            //    TargetDate = DateTime.UtcNow,
            //    TargetValue = "12",
            //    TypeId = 2,
            //    CustomAttributes = new List<CustomAttribute> {new CustomAttribute {Id = "1234", Name = "testing value"}},
            //    FocusAreaIds = new List<string> {"123456789098676745763"}
            //};

            //return goal;
            try
            {
                var request = new GetGoalDataRequest();

                IRestClient client = new JsonServiceClient();

                //"/{Context}/{Version}/{ContractNumber}/Goal/{Id}"
                var url = Helper.BuildURL(string.Format(@"{0}/{1}/{2}/{3}/Goal/{4}",
                    "http://localhost:8888/PatientGoal",
                    "NG",
                    req.Version,
                    req.ContractNumber,
                    sid), userId);

                var response = client.Get<GetGoalDataResponse>(url);
                if (response == null) throw new Exception("Schedule template was not found or initialized.");
                var goal = Mapper.Map<Goal>(response.GoalData); //new Goal();

                return goal;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:GetGoalById()::" + ex.Message,
                    ex.InnerException);
            }

        }

        public PatientGoal GetOpenNotMetPatientGoalByTemplateId(string sid, string patientId, string userId, IAppDomainRequest req)
        {
            //PatientGoal pg = new PatientGoal
            //{
            //    StatusId = 1,
            //    Name = "test patient goal",
            //    Id = "123456789012345612345678",
            //    PatientId = "5325da9ed6a4850adcbba6ce"
            //};
            //return pg;

            try
            {
                var request = new GetGoalDataRequest();

                IRestClient client = new JsonServiceClient();

                //"/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Goal/"
                var url = Helper.BuildURL(string.Format(@"{0}/{1}/{2}/{3}/Patient/{4}/Goal/?TemplateId={5}",
                    "http://localhost:8888/PatientGoal",
                    "NG",
                    req.Version,
                    req.ContractNumber,
                     "5424628084ac050e3806e8e2",
                    "545a91a1fe7a59218cef2d6d"), userId);

                var response = client.Get<GetPatientGoalByTemplateIdResponse>(url);
                if (response == null) throw new Exception("Patient goal was not found.");
                var goal = Mapper.Map<PatientGoal>(response.GoalData);
                //var patientGoal = Mapper.Map<Goal>(response.GoalData); //new Goal();

                return goal;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:GetGoalById()::" + ex.Message,
                    ex.InnerException);
            }
        }


        public Intervention GetInterventionById(string sid, string userId, IAppDomainRequest req)
        {
            try
            {
                var request = new GetInterventionDataRequest();

                IRestClient client = new JsonServiceClient();

                //"/{Context}/{Version}/{ContractNumber}/Goal/Interventions?Id={Id}"
                var url = Helper.BuildURL(string.Format(@"{0}/{1}/{2}/{3}/Goal/Interventions?Id={4}",
                    "http://localhost:8888/PatientGoal",
                    "NG",
                    req.Version,
                    req.ContractNumber,
                    sid), userId);

                var response = client.Get<GetInterventionDataResponse>(url);
                if (response == null) throw new Exception("Intervention template was not found or initialized.");
                var intervention = Mapper.Map<Intervention>(response.InterventionsData);

                return intervention;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:GetInterventionById()::" + ex.Message,
                    ex.InnerException);
            }
        }


        public PatientIntervention GetOpenNotMetPatientInterventionByTemplateId(string gid, string templateId, string patientId, string userId, IAppDomainRequest req)
        {
            try
            {
                var request = new GetPatientInterventionByTemplateIdRequest();
                PatientIntervention intervention = null;

                IRestClient client = new JsonServiceClient();

                var url =
                    Helper.BuildURL(
                        string.Format(@"{0}/{1}/{2}/{3}/Patient/{4}/Goal/Interventions?TemplateId={5}&GoalId={6}",
                            "http://localhost:8888/PatientGoal",
                            "NG",
                            req.Version,
                            req.ContractNumber,
                            patientId,
                            "5461bb4ffe7a59064cd074c0",
                            "54652e3a84ac050f50239ded"), userId);

                var response = client.Get<GetPatientInterventionByTemplateIdResponse>(url);
                if (response.InterventionData != null)
                    intervention = Mapper.Map<PatientIntervention>(response.InterventionData);

                return intervention;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:GetGoalById()::" + ex.Message,
                    ex.InnerException);
            }
        }


        public Task GetTaskById(string sid, string userId, IAppDomainRequest req)
        {
            try
            {
                IRestClient client = new JsonServiceClient();
                var url = Helper.BuildURL(string.Format(@"{0}/{1}/{2}/{3}/Goal/Tasks?Id={4}",
                    "http://localhost:8888/PatientGoal",
                    "NG",
                    req.Version,
                    req.ContractNumber,
                    sid), userId);

                var response = client.Get<GetTaskDataResponse>(url);
                if (response == null) throw new Exception("Task template was not found or initialized.");
                var task = Mapper.Map<Task>(response.TaskData);

                return task;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:GetTaskById()::" + ex.Message,
                    ex.InnerException);
            }
        }

        public PatientTask GetOpenNotMetPatientTaskByTemplateId(string taskid, string tempId, string patientId, string userId, IAppDomainRequest req)
        {
            try
            {
                var request = new GetPatientTaskByTemplateIdRequest();
                PatientTask task = null;

                IRestClient client = new JsonServiceClient();

                var url =
                    Helper.BuildURL(
                        string.Format(@"{0}/{1}/{2}/{3}/Patient/{4}/Goal/Tasks?TemplateId={5}&GoalId={6}",
                            "http://localhost:8888/PatientGoal",
                            "NG",
                            req.Version,
                            req.ContractNumber,
                            patientId,
                            tempId,
                            taskid), userId);

                var response = client.Get<GetPatientTaskByTemplateIdResponse>(url);
                if (response.TaskData != null)
                    task = Mapper.Map<PatientTask>(response.TaskData);

                return task;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:GetGoalById()::" + ex.Message,
                    ex.InnerException);
            }
        }


        public List<ToDoData> GetPatientToDos(string patientId, string userId, IAppDomainRequest req)
        {
            throw new NotImplementedException();
        }
    }
}
