using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    public class GetAllPatientGoalsDataResponse : IDomainResponse
   {
        public List<PatientGoalViewData> PatientGoalsViewData { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
   }

    public class PatientGoalViewData
    {
        public string Id { get; set; }
        public List<string> FocusAreaIds { get; set; }
        public string Name { get; set; }
        public int StatusId { get; set; }

        public List<ChildViewData> BarriersViewData { get; set; }
        public List<ChildViewData> TasksViewData { get; set; }
        public List<ChildViewData> InterventionsViewData { get; set; }
    }

    public class ChildViewData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int StatusId { get; set; }
    }

}
