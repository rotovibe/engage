using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.DataDomain.Patient.Responses.Batch;

namespace Phytel.API.DataDomain.Patient.DTO
{
    public class InsertBatchPatientsDataResponse : IDomainResponse
    {
        public List<HttpObjectResponse<PatientData>> Responses { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
