using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;
using Phytel.API.Common.CustomObject;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetAllowedStatesResponse : IDomainResponse
    {
        public List<State> States { get; set; }
        public ResponseStatus Status { get; set; }
        public double Version { get; set; }
    }

    public class State
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> TypeIds { get; set; }
    }
}
