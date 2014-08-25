using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetToDosResponse : IDomainResponse
    {
        public List<ToDo> ToDos { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
