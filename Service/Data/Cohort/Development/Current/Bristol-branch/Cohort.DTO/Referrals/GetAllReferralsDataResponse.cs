using System.Collections.Generic;
using Phytel.API.DataDomain.Cohort.DTO.Model;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Cohort.DTO
{
    public class GetAllReferralsDataResponse : IDomainResponse
   {
        public List<ReferralData> Referrals { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
