using Phytel.API.DataDomain.Cohort.DTO.Model;

namespace Phytel.API.DataDomain.Cohort
{
    public interface IDataReferralManager
    {
        void SaveReferralData(ReferralData referral);
    }
}