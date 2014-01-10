using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;
using System;

namespace Phytel.API.DataDomain.Program.DTO
{
    public class StepsDetail : PlanElementDetail
    {
        public string Id { get; set; }
        public string ActionId { get; set; }
        public string SelectedResponseId { get; set; }
        public int StepTypeId { get; set; }
        public string Header { get; set; }
        public string Question { get; set; }
        public string T { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string Text { get; set; }
        public string Ex { get; set; }
        public int Status { get; set; }
        public int ControlType { get; set; }
        public List<ResponseDetail> Responses { get; set; }
    }
}
