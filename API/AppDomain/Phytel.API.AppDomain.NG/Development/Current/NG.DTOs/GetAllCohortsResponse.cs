using Phytel.API.Interface;
using ServiceStack;
using System;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetAllCohortsResponse : IDomainResponse
    {
        public List<Cohort> Cohorts { get; set; }
        public ResponseStatus Status { get; set; }
        public string Version { get; set; }
    }

    public class Cohort
    {
        public string ID { get; set; }
        public string SName { get; set; }
    }
}
