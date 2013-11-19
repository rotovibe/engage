using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetCohortPatientsResponse
    {
        public List<GetPatientResponse> Patients { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
