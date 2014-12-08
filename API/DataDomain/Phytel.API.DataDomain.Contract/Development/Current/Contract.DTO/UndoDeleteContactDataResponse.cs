using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Contact.DTO
{
    public class UndoDeleteContactDataResponse : IDomainResponse
    {
        public bool Success { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
