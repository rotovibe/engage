using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;
using System;

namespace Phytel.API.DataDomain.Program.DTO
{
    public class GetPatientActionDetailsDataResponse : IDomainResponse
    {
        public ActionsDetail ActionData { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
