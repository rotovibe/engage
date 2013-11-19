using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetAllCohortsResponse
    {
        public List<Cohort> Cohorts { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class Cohort
    {
        public string ID { get; set; }
        public string SName { get; set; }
    }
}
