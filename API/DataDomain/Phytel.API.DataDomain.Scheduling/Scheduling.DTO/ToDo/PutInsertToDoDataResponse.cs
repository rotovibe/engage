using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Scheduling.DTO
{
    public class PutInsertToDoDataResponse : IDomainResponse
    {
        public ToDoData ToDoData { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
