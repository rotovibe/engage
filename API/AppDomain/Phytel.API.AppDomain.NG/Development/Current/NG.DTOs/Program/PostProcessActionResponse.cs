using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class PostProcessActionResponse : IDomainResponse
    {
        //public List<Program> Programs { get; set; }
        //public List<Module> Modules { get; set; }
        //public List<Actions> Actions { get; set; }
        //public List<Step> Steps { get; set; }
        public PlanElements PlanElems { get; set; }
        public Program Program { get; set; }
        public List<string> RelatedChanges { get; set; }
        public string PatientId{get; set;}
        public ResponseStatus Status { get; set; }
        public double Version { get; set; }
    }

    public class PlanElements
    {
        public List<Program> Programs { get; set; }
        public List<Module> Modules { get; set; }
        public List<Actions> Actions { get; set; }
        public List<Step> Steps { get; set; }

        public PlanElements()
        {
            Programs = new List<Program>();
            Modules = new List<Module>();
            Actions = new List<Actions>();
            Steps = new List<Step>();
        }
    }
}
