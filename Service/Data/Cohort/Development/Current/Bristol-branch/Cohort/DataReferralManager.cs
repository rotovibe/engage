using Phytel.API.DataDomain.Cohort.DTO;
using Phytel.API.DataDomain.Cohort.DTO.Context;
using Phytel.API.DataDomain.Cohort.DTO.Model;
using Phytel.API.DataDomain.Cohort.DTO.Referrals;
using Phytel.API.Interface;
using System;

namespace Phytel.API.DataDomain.Cohort
{
    public class DataReferralManager : IDataReferralManager
    {
        //private readonly IServiceContext _context;
        private readonly IReferralRepository<IDataDomainRequest> _repository;
        
        public DataReferralManager(/*IServiceContext context,*/ IReferralRepository<IDataDomainRequest> repository)
        {
            //_context = context;
            _repository = repository;
        }

        public PostReferralDefinitionResponse InsertReferral(PostReferralDefinitionRequest request)
        {
            var result = new PostReferralDefinitionResponse();
            try
            {
                if (request == null)
                    throw new ArgumentNullException("Request parameter cannot be NULL");
                if((string.IsNullOrEmpty(request.Context)))
                    throw new ArgumentNullException("Request parameter context value cannot be NULL/EMPTY");
                if ((string.IsNullOrEmpty(request.ContractNumber)))
                    throw new ArgumentNullException("Request parameter contract number value cannot be NULL/EMPTY");
                if ((request.Referral == null))
                    throw new ArgumentNullException("Request parameter referral cannot be NULL");
                if (string.IsNullOrEmpty(request.Referral.CohortId ))
                    throw new ArgumentNullException("Request parameter referral.cohortId cannot be NULL/EMPTY");
                if (string.IsNullOrEmpty(request.Referral.Name))
                    throw new ArgumentNullException("Request parameter referral.name cannot be NULL/EMPTY");
                if (string.IsNullOrEmpty(request.Referral.DataSource))
                    throw new ArgumentNullException("Request parameter referral.datasource cannot be NULL/EMPTY");

                result = _repository.Insert(request) as PostReferralDefinitionResponse;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}   
