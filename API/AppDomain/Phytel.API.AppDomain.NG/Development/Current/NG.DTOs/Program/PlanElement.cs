using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class PlanElement : IPlanElement
    {
        /// <summary>
        /// Id of element
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// SourceId of element
        /// </summary>
        public string SourceId { get; set; }

        /// <summary>
        /// Ordinal of the current item in a list
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Will show disabled or not
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Completed summary
        /// </summary>
        public bool Completed { get; set; }

        /// <summary>
        /// Next like dependent element Id.
        /// </summary>
        public string Next { get; set; }

        /// <summary>
        /// Previous like dependent element Id.
        /// </summary>
        public string Previous { get; set; }

        public string CompletedBy { get; set; }
        public DateTime? DateCompleted { get; set; }
        public int ElementState { get; set; }
        public DateTime? AssignDate { get; set; }
        public string AssignBy { get; set; }
        public string AssignTo { get; set; } // Sprint 12
        public DateTime? AttrStartDate { get; set; } // Sprint 12
        public DateTime? AttrEndDate { get; set; } // Sprint 12

        public List<SpawnElement> SpawnElement { get; set; }
    }
}
