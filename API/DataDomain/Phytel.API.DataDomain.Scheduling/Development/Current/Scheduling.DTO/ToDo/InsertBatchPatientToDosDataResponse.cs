using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.Scheduling.DTO
{
    public class InsertBatchPatientToDosDataResponse : IDomainResponse
    {
        public List<HttpObjectResponse<ToDoData>> Responses { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
