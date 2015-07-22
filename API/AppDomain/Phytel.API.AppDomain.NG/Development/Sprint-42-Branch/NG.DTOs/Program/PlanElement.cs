using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Serializable]
    public class PlanElement : IPlanElement
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
        public string CompletedBy { get; set; }
        public DateTime? DateCompleted { get; set; }
        public int ElementState { get; set; }
        public DateTime? StateUpdatedOn { get; set; }
        public DateTime? AssignDate { get; set; }
        public string AssignById { get; set; }
        public string AssignToId { get; set; } // Sprint 12
        public DateTime? AttrStartDate { get; set; } // Sprint 12
        public DateTime? AttrEndDate { get; set; } // Sprint 12

        public List<SpawnElement> SpawnElement { get; set; }
    }
}
