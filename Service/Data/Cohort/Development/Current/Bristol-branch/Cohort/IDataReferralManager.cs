using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Phytel.API.DataDomain.Cohort.DTO;
using Phytel.API.DataDomain.Cohort.DTO.Model;
using Phytel.API.DataDomain.Cohort.DTO.Referrals;

namespace Phytel.API.DataDomain.Cohort
{
    public interface IDataReferralManager
    {
       string InsertReferral(ReferralData request);
        ReferralData GetReferralById(string referralId);
        List<ReferralData> GetAllReferrals();
        PostPatientsListReferralDefinitionResponse InsertReferralsAll(PostPatientsListReferralDefinitionRequest request);
    }
}