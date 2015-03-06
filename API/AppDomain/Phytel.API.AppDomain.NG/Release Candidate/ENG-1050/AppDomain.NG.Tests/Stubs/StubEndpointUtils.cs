using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Bson;
using Moq;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Goal;
using Phytel.API.AppDomain.NG.Programs;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientGoal.DTO;
using Phytel.API.DataDomain.PatientObservation.DTO;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.DataDomain.Scheduling.DTO;
using Phytel.API.Interface;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using Task = Phytel.API.AppDomain.NG.DTO.Goal.Task;

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
            //    SourceId = "5325db9cd6a4850adcbba9ca",
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
                var url = Common.Helper.BuildURL(string.Format(@"{0}/{1}/{2}/{3}/Goal/{4}",
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
                var url = Common.Helper.BuildURL(string.Format(@"{0}/{1}/{2}/{3}/Patient/{4}/Goal/?TemplateId={5}",
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
                var url = Common.Helper.BuildURL(string.Format(@"{0}/{1}/{2}/{3}/Goal/Interventions?Id={4}",
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
                    Common.Helper.BuildURL(
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


        public DTO.Goal.Task GetTaskById(string sid, string userId, IAppDomainRequest req)
        {
            try
            {
                IRestClient client = new JsonServiceClient();
                var url = Common.Helper.BuildURL(string.Format(@"{0}/{1}/{2}/{3}/Goal/Tasks?Id={4}",
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
                    Common.Helper.BuildURL(
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
    }
}
