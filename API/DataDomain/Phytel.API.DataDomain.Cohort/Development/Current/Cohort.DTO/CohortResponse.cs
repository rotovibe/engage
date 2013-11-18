using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.DataDomain.Cohort.DTO
{
    public class CohortResponse
   {
        public Cohort Cohort { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class Cohort
    {
        public string ID { get; set; }
        public string SName { get; set; }
        public string Query { get; set; }
        public string Sort { get; set; }
    }
}
