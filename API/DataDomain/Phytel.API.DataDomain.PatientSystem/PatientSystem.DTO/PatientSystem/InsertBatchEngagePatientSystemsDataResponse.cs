using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.PatientSystem.DTO
{
    public class InsertBatchEngagePatientSystemsDataResponse : IDomainResponse
    {
        public BulkInsertResult Result { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
