using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    public class GetAllPatientGoalsDataResponse : IDomainResponse
   {
        public List<PatientGoalDataView> PatientGoalsViewData { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
   }

    public class PatientGoalDataView
    {
        public string Id { get; set; }
        public List<string> FocusAreaIds { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }

        public List<ChildViewData> BarriersViewData { get; set; }
        public List<ChildViewData> TasksViewData { get; set; }
        public List<ChildViewData> InterventionsViewData { get; set; }
    }

    public class ChildViewData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
    }

}
