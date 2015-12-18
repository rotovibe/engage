using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Scheduling.DTO
{
    public class GetToDosDataResponse : IDomainResponse, ICountableResponse
   {
        public List<ToDoData> ToDos { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
        public long TotalCount { get; set; }
   }

}
