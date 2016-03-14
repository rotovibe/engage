using Phytel.API.DataDomain.Cohort.DTO;
using Phytel.API.DataDomain.Cohort.DTO.Context;
using Phytel.API.DataDomain.Cohort.DTO.Model;
using Phytel.API.DataDomain.Cohort.DTO.Referrals;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Cohort
{
    public class DataReferralManager : IDataReferralManager
    {
        private readonly IServiceContext _context;
        private readonly IReferralRepository<IDataDomainRequest> _repository;
        
        public DataReferralManager(IServiceContext context, IReferralRepository<IDataDomainRequest> repository)
        {
            _context = context;
            _repository = repository;
        }

        public PostReferralDefinitionResponse InsertReferral(PostReferralDefinitionRequest request)
        {
            PostReferralDefinitionResponse result = _repository.Insert(request) as PostReferralDefinitionResponse;
            return result;
        }
    }
}   
