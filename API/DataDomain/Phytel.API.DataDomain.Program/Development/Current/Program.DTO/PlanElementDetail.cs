using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;
using System;

namespace Phytel.API.DataDomain.Program.DTO
{
    public class PlanElementDetail
    {
        public string Id { get; set; }
        public string SourceId { get; set; }
        public int Order { get; set; }
        public bool Enabled { get; set; }
        public bool Completed { get; set; }
        public string Next { get; set; }
        public string Previous { get; set; }

        public SpawnElementDetail SpawnElement { get; set; }
    }

    public class SpawnElementDetail
    {
        public int ElementType { get; set; }
        public string ElementId { get; set; }
    }
}
