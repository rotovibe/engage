using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetCohortPatientsResponse
    {
        public List<PatientResponse> Patients { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
