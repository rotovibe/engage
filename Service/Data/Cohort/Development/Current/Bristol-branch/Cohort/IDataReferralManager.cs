using System.Security.Cryptography.X509Certificates;
using Phytel.API.DataDomain.Cohort.DTO.Model;
using Phytel.API.DataDomain.Cohort.DTO.Referrals;

namespace Phytel.API.DataDomain.Cohort
{
    public interface IDataReferralManager
    {
       PostReferralDefinitionResponse InsertReferral(PostReferralDefinitionRequest request);
    }
}