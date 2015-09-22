using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using System;

namespace Phytel.API.DataDomain.Patient.DTO
{
    public class InsertPatientsDataResponse : IDomainResponse
    {
        public Dictionary<string, string> Ids { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
