using System.Collections.Generic;
using Phytel.API.Common;
using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.DataDomain.Patient.DTO
{
    public class InsertBatchPatientsDataResponse : IDomainResponse
    {
        public List<HttpObjectResponse<PatientData>> Responses { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
