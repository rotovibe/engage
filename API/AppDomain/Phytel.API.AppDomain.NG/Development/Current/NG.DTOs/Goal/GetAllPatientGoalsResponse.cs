using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;
using System;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetAllPatientGoalsResponse : IDomainResponse
    {
        public List<PatientGoalView> GoalsView { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class PatientGoalView
    {
        public string Id { get; set; }
        public List<string> FocusAreaIds { get; set; }
        public string Name { get; set; }
        public int StatusId { get; set; }

        public List<ChildView> BarriersView { get; set; }
        public List<ChildView> TasksView { get; set; }
        public List<ChildView> InterventionsView { get; set; }
    }

    public class ChildView
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int StatusId { get; set; }
    }
}
