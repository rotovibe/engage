using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Cohort.DTO
{
    public class GetCohortDataResponse : IDomainResponse
   {
        public CohortData Cohort { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
