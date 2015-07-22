using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class PostRepeatActionResponse : IDomainResponse
    {
        public PlanElements PlanElems { get; set; }
        public Program Program { get; set; }
        public List<string> RelatedChanges { get; set; }
        public List<PatientGoal> Goals { get; set; }
        public string PatientId{get; set;}
        public ResponseStatus Status { get; set; }
        public double Version { get; set; }
    }
}
