using System;
using System.Collections.Generic;
namespace Phytel.API.AppDomain.NG.DTO
{
    public interface IPlanElement
    {
        string Id { get; set; }
        bool Completed { get; set; }
        bool Enabled { get; set; }
        string Next { get; set; }
        int Order { get; set; }
        string Previous { get; set; }
        string SourceId { get; set; }
        int ElementState { get; set; }
        DateTime? AssignDate { get; set; }
        string AssignById { get; set; }
        DateTime? StateUpdatedOn { get; set; }
        string AssignToId { get; set; } // Sprint 12
        DateTime? AttrStartDate { get; set; } // Sprint 12
        DateTime? AttrEndDate { get; set; } // Sprint 12
        List<SpawnElement> SpawnElement { get; set; }
    }
}
