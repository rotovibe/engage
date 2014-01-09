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
        public string StepID { get; set; }
        public string Value { get; set; }
        public bool Nominal { get; set; }
        public bool Required { get; set; }
        public string NextStepId { get; set; }
    }
}
