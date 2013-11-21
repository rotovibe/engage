using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetCohortPatientsResponse : IDomainResponse
    {
        public List<Patient> Patients { get; set; }
        public ResponseStatus Status { get; set; }
        public string Version { get; set; }
    }
}
