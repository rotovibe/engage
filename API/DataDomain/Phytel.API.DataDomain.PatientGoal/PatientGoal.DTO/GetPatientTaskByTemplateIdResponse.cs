using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;
using System;

namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    public class GetPatientTaskByTemplateIdResponse : IDomainResponse
    {
        public PatientTaskData TaskData { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
