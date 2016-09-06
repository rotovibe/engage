using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;
using System;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetAllPatientGoalsResponse : IDomainResponse
    {
        public List<PatientGoalView> Goals { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class PatientGoalView
    {
        public string Id { get; set; }
        public string PatientId { get; set; }
        public List<string> FocusAreaIds { get; set; }
        public string Name { get; set; }
        public int StatusId { get; set; }
        public string TemplateId { get; set; }
        public List<ChildView> Barriers { get; set; }
        public List<ChildView> Tasks { get; set; }
        public List<ChildView> Interventions { get; set; }
    }

    public class ChildView
    {
        public string Id { get; set; }
        public string PatientGoalId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StatusId { get; set; }
    }
}
