using System.Collections.Generic;

namespace Phytel.API.DataDomain.Cohort.DTO
{
    public class CohortListResponse
   {
        public List<CohortResponse> Cohorts { get; set; }
        public string Version { get; set; }
    }

}
