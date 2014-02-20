using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;
using Phytel.API.Common.CustomObjects;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    public class GetAllCommTypesDataResponse : IDomainResponse
    {
        public List<CommTypeData> CommTypes { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class CommTypeData : IdNamePair           
    {
        public List<string> CommModes { get; set; }
    }
}
