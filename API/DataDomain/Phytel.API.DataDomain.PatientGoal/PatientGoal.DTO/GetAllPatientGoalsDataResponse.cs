using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    public class GetAllPatientGoalsDataResponse : IDomainResponse
   {
        public List<PatientGoalViewData> PatientGoalsData { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
   }

    public class PatientGoalViewData
    {
        public string Id { get; set; }
        public string PatientId { get; set; }
        public List<string> FocusAreaIds { get; set; }
        public string Name { get; set; }
        public int StatusId { get; set; }
        public string TemplateId { get; set; }
        public List<ChildViewData> BarriersData { get; set; }
        public List<ChildViewData> TasksData { get; set; }
        public List<ChildViewData> InterventionsData { get; set; }
    }

    public class ChildViewData
    {
        public string Id { get; set; }
        public string PatientGoalId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StatusId { get; set; }
    }

}
