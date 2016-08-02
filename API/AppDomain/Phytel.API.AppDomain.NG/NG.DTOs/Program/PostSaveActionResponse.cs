using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class PostSaveActionResponse : IDomainResponse
    {
        public bool Saved { get; set; }
        public List<string> RelatedChanges { get; set; }
        public List<PatientGoal> Goals { get; set; }
        public string PatientId{get; set;}
        public ResponseStatus Status { get; set; }
        public double Version { get; set; }
    }
}
