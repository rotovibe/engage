using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetAllTimesOfDaysResponse : IDomainResponse
    {
        public List<LookUp> TimesOfDays { get; set; }
        public ResponseStatus Status { get; set; }
        public string Version { get; set; }
    }
}
