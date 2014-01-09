using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;
using System;

namespace Phytel.API.DataDomain.Program.DTO
{
    public class ModuleDetail : PlanElementDetail
    {
        public string Id { get; set; }
        public string ProgramId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ObjectivesDetail> Objectives { get; set; }
        public List<ActionsDetail> Actions { get; set; }
        public int Status { get; set; }
    }
}
