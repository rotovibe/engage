using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Cohort.DTO.Referrals
{
    public class PostPatientsListReferralDefinitionResponse : IDomainResponse
    {
        public List<string> NewPatientIds { get; set; }
        public List<string> ExistingPatientIds { get; set; }

        public double Version { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        // this will be deprecated. Only used because its inherited from the legacy IDomainResponse in Engage assembles.
        public ResponseStatus Status { get; set; }
    }       // end class definition
}           // end namespace defintion
