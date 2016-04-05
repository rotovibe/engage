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

                result = _repository.Insert(request.Referral) as PostReferralDefinitionResponse;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public GetReferralDataResponse GetReferralByID(GetReferralDataRequest request)
        {
            try
            {
                if (request == null)
                    throw new ArgumentNullException("Request parameter cannot be NULL");
                if (string.IsNullOrEmpty(request.ReferralID))
                    throw new ArgumentNullException("Request parameter ReferralID cannot be NULL/EMPTY");
                GetReferralDataResponse response = new GetReferralDataResponse();
                response.Referral = _repository.FindByID(request.ReferralID) as ReferralData;
                response.Version = request.Version;
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public GetAllReferralsDataResponse GetReferrals(GetAllReferralsDataRequest request)
        {
            try
            {
                if (request == null)
                    throw new ArgumentNullException("Request parameter cannot be NULL");

                GetAllReferralsDataResponse response = new GetAllReferralsDataResponse();
                List<API.DataDomain.Cohort.DTO.Model.ReferralData> referrals =
                    _repository.SelectAll() as List<API.DataDomain.Cohort.DTO.Model.ReferralData>;
                response.Referrals = referrals.ToList();
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
  
