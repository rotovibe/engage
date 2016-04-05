using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Cohort
{
    public interface IReferralRepository<T> : IRepository<T>
    {
        object Insert(object newEntity, double version, string userid);
    }
}
