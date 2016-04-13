using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class UpdateCareTeamMemberResponse : IDomainResponse
    {
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
