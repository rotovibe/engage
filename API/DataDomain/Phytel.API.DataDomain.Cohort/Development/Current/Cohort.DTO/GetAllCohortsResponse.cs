using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.DataDomain.Cohort.DTO
{
    public class GetAllCohortsResponse
   {
        public List<Cohort> Cohorts { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
