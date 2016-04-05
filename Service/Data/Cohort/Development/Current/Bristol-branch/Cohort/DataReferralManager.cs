using Phytel.API.DataDomain.Cohort.DTO;
using Phytel.API.DataDomain.Cohort.DTO.Context;
using Phytel.API.DataDomain.Cohort.DTO.Model;
using Phytel.API.DataDomain.Cohort.DTO.Referrals;
using Phytel.API.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public string InsertReferral(ReferralData request)
        {
            try
            {
         
                if ((request == null))
                    throw new ArgumentNullException("Request parameter referral cannot be NULL");
                if (string.IsNullOrEmpty(request.CohortId ))
                    throw new ArgumentNullException("Request parameter referral.cohortId cannot be NULL/EMPTY");
                if (string.IsNullOrEmpty(request.Name))
                    throw new ArgumentNullException("Request parameter referral.name cannot be NULL/EMPTY");
                if (string.IsNullOrEmpty(request.DataSource))
                    throw new ArgumentNullException("Request parameter referral.datasource cannot be NULL/EMPTY");

                return _repository.Insert(request, _context.Version, _context.UserId) as string;
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        public ReferralData GetReferralById(string referralId)
        {
            try
            {
                if (string.IsNullOrEmpty(referralId))
                    throw new ArgumentNullException("Request parameter ReferralID cannot be NULL/EMPTY");
                return _repository.FindByID(referralId) as ReferralData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<ReferralData> GetAllReferrals()
        {
            try
            {
                return _repository.SelectAll() as List<ReferralData>;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
  
