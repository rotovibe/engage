using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    public class GetAllCommTypesDataResponse : IDomainResponse
    {
        public List<CommTypeData> CommTypes { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class CommTypeData : LookUpData           
    {
        public List<string> CommModes { get; set; }
    }
}
