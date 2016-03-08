using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Cohort.DTO.Referrals
{
    public class PostReferralDefinitionResponse : IDomainResponse
   {


        public double Version { get; set; }

        // this will be deprecated. Only used because its inherited from the legacy IDomainResponse in Engage assembles.
        public ResponseStatus Status { get; set; } 
        public ResponseStatus ResponseStatus { get; set; }
   }
}
