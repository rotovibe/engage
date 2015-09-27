using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.PatientSystem.DTO
{
    public class InsertBatchPatientSystemsDataResponse : IDomainResponse
    {
        public List<HttpObjectResponse<PatientSystemData>> Responses { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
