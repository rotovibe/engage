using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;
using System;

namespace Phytel.API.DataDomain.Program.DTO
{
    public class ActionsDetail : PlanElementDetail
    {
        public string ModuleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ObjectiveInfoData> Objectives { get; set; }
        public List<StepsDetail> Steps { get; set; }
        public int Status { get; set; }
        public string Text { get; set; }
        public bool DeleteFlag { get; set; }
    }
}
