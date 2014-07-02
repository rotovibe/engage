using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.CareMember.DTO
{
    public class DeleteCareMemberByPatientIdDataResponse : IDomainResponse
    {
        public bool Deleted { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
