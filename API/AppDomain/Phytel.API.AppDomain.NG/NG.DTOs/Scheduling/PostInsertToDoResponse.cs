using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class PostInsertToDoResponse : IDomainResponse
    {
        public ToDo ToDo { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
