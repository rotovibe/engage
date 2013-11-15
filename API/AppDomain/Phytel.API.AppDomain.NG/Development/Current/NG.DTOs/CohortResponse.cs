using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class CohortResponse
    {
        public List<Cohort> Cohorts { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class Cohort
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
    }
}
