using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using System;
using Phytel.API.DataDomain.Patient.DTO.Responses.Batch;

namespace Phytel.API.DataDomain.Patient.DTO
{
    public class InsertPatientsDataResponse : IDomainResponse
    {
        public List<HttpObjectResponse<PatientData>> Responses { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
