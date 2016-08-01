using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;
using System;

namespace Phytel.API.DataDomain.Program.DTO
{
    public class ResponseDetail
    {
        public string Id { get; set; }
        public int Order { get; set; }
        public string Text { get; set; }
        public string StepId { get; set; }
        public string Value { get; set; }
        public bool Nominal { get; set; }
        public bool Required { get; set; }
        public string NextStepId { get; set; }
        public List<SpawnElementDetail> SpawnElement { get; set; }
        public bool Selected { get; set; }
        public bool Delete { get; set; }
    }
}
