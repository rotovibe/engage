using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.PatientGoal.DTO;
using Phytel.API.Interface;
using ServiceStack.Service;

namespace Phytel.API.AppDomain.NG.Test.Stubs
{
    public class StubGoalsEndpointUtils : IGoalsEndpointUtils
    {
        public PatientGoalData GetInitialGoalRequest(GetInitializeGoalRequest request)
        {
            var pgd = new PatientGoalData
            {
                Id = "233336745678235647821232",
                Name = "Example Patient Goal Data",
                StartDate = DateTime.UtcNow
            };
            return pgd;
        }

        public string GetInitialBarrierRequest(GetInitializeBarrierRequest request)
        {
            throw new NotImplementedException();
        }

        public PatientTaskData GetInitialTaskRequest(GetInitializeTaskRequest request)
        {
            throw new NotImplementedException();
        }

        public PatientGoal GetPatientGoal(GetPatientGoalRequest request)
        {
            throw new NotImplementedException();
        }

        public List<PatientGoalView> GetAllPatientGoals(GetAllPatientGoalsRequest request)
        {
            throw new NotImplementedException();
        }

        public List<PatientIntervention> GetInterventions(GetInterventionsRequest request)
        {
            throw new NotImplementedException();
        }

        public List<PatientTask> GetTasks(GetTasksRequest request)
        {
            throw new NotImplementedException();
        }

        public bool DeleteGoalRequest(PostDeletePatientGoalRequest request)
        {
            throw new NotImplementedException();
        }

        public bool DeleteBarrierRequest(PostDeletePatientGoalRequest request, string id)
        {
            throw new NotImplementedException();
        }

        public bool DeleteTaskRequest(PostDeletePatientGoalRequest request, string id)
        {
            throw new NotImplementedException();
        }

        public bool DeleteInterventionRequest(PostDeletePatientGoalRequest request, string id)
        {
            throw new NotImplementedException();
        }

        public List<CustomAttribute> GetAttributesLibraryByType(IAppDomainRequest request, int typeId)
        {
            throw new NotImplementedException();
        }

        public bool PostUpdateInterventionRequest(PostPatientGoalRequest request, PatientInterventionData pi)
        {
            throw new NotImplementedException();
        }

        public bool PostUpdateBarrierRequest(PostPatientGoalRequest request, PatientBarrierData bd)
        {
            throw new NotImplementedException();
        }

        public bool PostUpdateTaskRequest(PostPatientGoalRequest request, PatientTaskData td)
        {
            throw new NotImplementedException();
        }

        public string GetInitialInterventionRequest(GetInitializeInterventionRequest request)
        {
            throw new NotImplementedException();
        }

        public PatientGoal PostUpdateGoalRequest(PostPatientGoalRequest request)
        {
            return new PatientGoal();
        }

        public PatientIntervention PostUpdateInterventionRequest(PostPatientInterventionRequest request)
        {
            throw new NotImplementedException();
        }

        public PatientTask PostUpdateTaskRequest(PostPatientTaskRequest request)
        {
            throw new NotImplementedException();
        }

        public PatientBarrier PostUpdateBarrierRequest(PostPatientBarrierRequest request)
        {
            throw new NotImplementedException();
        }

        public PatientGoalData convertToPatientGoalData(PatientGoal pg)
        {
            throw new NotImplementedException();
        }

        public List<CustomAttributeData> GetPatientGoalAttributes(List<CustomAttribute> list)
        {
            throw new NotImplementedException();
        }

        public List<string> GetTaskIdsForRequest(List<PatientTask> list)
        {
            throw new NotImplementedException();
        }

        public List<string> GetInterventionIdsForRequest(List<PatientIntervention> list)
        {
            throw new NotImplementedException();
        }

        public List<string> GetBarrierIdsForRequest(List<PatientBarrier> list)
        {
            throw new NotImplementedException();
        }

        public PatientDetails GetPatientDetails(double version, string contractNumber, string userId, IRestClient client,
            string patientId)
        {
            throw new NotImplementedException();
        }
    }
}
