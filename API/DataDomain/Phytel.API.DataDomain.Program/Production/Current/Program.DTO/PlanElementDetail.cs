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
        public string ArchiveOriginId { get; set; }
        public bool Archived { get; set; }
        public DateTime? ArchivedDate { get; set; }
        public int Order { get; set; }
        public bool Enabled { get; set; }
        public bool Completed { get; set; }
        public string Next { get; set; }
        public string Previous { get; set; }
        public int ElementState { get; set; }
        public DateTime? StateUpdatedOn { get; set; }
        public DateTime? AssignDate { get; set; }
        public string AssignBy { get; set; }
        public string AssignTo { get; set; }
        public string CompletedBy { get; set; }
        public DateTime? DateCompleted { get; set; }
        public DateTime? AttrStartDate { get; set; }
        public DateTime? AttrEndDate { get; set; }
        public List<SpawnElementDetail> SpawnElement { get; set; }
    }

    public class SpawnElementDetail
    {
        public int ElementType { get; set; }
        public string ElementId { get; set; }
        public string Tag { get; set; }
    }
}
