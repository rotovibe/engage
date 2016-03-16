using Phytel.API.DataDomain.Cohort.DTO;
using Phytel.API.DataDomain.Cohort.DTO.Context;
using Phytel.API.DataDomain.Cohort.DTO.Model;
using Phytel.API.DataDomain.Cohort.DTO.Referrals;
using Phytel.API.Interface;
using System;

namespace Phytel.API.DataDomain.Cohort
{
    public class DataPatientReferralManager : IDataPatientReferralManager
    {
        private readonly IServiceContext _context;
        private readonly IPatientReferralRepository<IDataDomainRequest> _repository;

        public DataPatientReferralManager(IServiceContext context, IPatientReferralRepository<IDataDomainRequest> repository)
        {
            _context = context;
            _repository = repository;
        }

        public PostPatientReferralDefinitionResponse InsertPatientReferral(PostPatientReferralDefinitionRequest request)
        {
            PostPatientReferralDefinitionResponse result = new PostPatientReferralDefinitionResponse();
            try
            {
               
                if (request == null)
                    throw new ArgumentNullException("Request parameter cannot be NULL");
                if ((String.IsNullOrEmpty(request.Context)))
                    throw new ArgumentNullException("Request parameter context value cannot be NULL/EMPTY");
                if ((String.IsNullOrEmpty(request.ContractNumber)))
                    throw new ArgumentNullException("Request parameter contract number value cannot be NULL/EMPTY");
                if ((String.IsNullOrEmpty(request.UserId)))
                    throw new ArgumentNullException("Request parameter user id value cannot be NULL/EMPTY");
                if ((request.PatientReferral == null))
                    throw new ArgumentNullException("Request parameter patientReferral cannot be NULL");
                if (String.IsNullOrEmpty(request.PatientReferral.Id))
                    throw new ArgumentNullException("Request parameter patientReferral.referralId cannot be NULL/EMPTY");
                if (String.IsNullOrEmpty(request.PatientReferral.PatientId))
                    throw new ArgumentNullException("Request parameter patientReferral.patientId cannot be NULL/EMPTY");
                if (String.IsNullOrEmpty(request.PatientReferral.ReferralId))
                    throw new ArgumentNullException("Request parameter patientReferral.referralId cannot be NULL/EMPTY");
                if (String.IsNullOrEmpty(request.PatientReferral.CreatedBy))
                    throw new ArgumentNullException("Request parameter patientReferral.createdBy cannot be NULL/EMPTY");

                result = _repository.Insert(request) as PostPatientReferralDefinitionResponse;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}   
