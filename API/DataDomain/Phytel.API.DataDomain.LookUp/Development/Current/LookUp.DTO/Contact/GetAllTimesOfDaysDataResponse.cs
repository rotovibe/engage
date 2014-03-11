using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;
using Phytel.API.Common.CustomObject;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    public class GetAllTimesOfDaysDataResponse : IDomainResponse
   {
        public List<IdNamePair> TimesOfDays { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
