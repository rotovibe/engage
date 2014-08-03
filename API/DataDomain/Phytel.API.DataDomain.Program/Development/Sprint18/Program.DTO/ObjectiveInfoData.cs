using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;
using System;

namespace Phytel.API.DataDomain.Program.DTO
{
    public class ObjectiveInfoData
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
        public int Status { get; set; }
    }
}
