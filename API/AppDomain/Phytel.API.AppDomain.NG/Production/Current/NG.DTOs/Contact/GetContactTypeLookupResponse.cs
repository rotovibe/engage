using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetContactTypeLookupResponse : IDomainResponse
    {
        public GetContactTypeLookupResponse()
        {
            ContactTypeLookUps = new List<ContactTypeLookUp>();
        }

        public List<ContactTypeLookUp> ContactTypeLookUps { get; set; }

        public double Version { get; set; }

        public ResponseStatus Status { get; set; }
    }
}
