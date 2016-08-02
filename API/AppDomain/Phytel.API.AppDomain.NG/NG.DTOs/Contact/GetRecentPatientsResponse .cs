using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetRecentPatientsResponse : IDomainResponse
    {
        public string ContactId { get; set; }
        public int Limit { get; set; }
        public List<CohortPatient> Patients { get; set; }
        public ResponseStatus Status { get; set; }
        public double Version { get; set; }
    }
}
