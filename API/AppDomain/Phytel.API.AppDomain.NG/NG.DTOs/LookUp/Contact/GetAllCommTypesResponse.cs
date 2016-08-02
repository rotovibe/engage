using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;
using Phytel.API.Common.CustomObject;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetAllCommTypesResponse : IDomainResponse
    {
        public List<CommTypeLookUp> CommTypes { get; set; }
        public ResponseStatus Status { get; set; }
        public double Version { get; set; }

    }

    public class CommTypeLookUp : IdNamePair
    {
        public List<string> CommModes { get; set; }
    }
}
