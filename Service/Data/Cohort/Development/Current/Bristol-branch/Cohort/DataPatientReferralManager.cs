using Phytel.API.DataDomain.Cohort.DTO;
using Phytel.API.DataDomain.Cohort.DTO.Context;
using Phytel.API.DataDomain.Cohort.DTO.Model;
using Phytel.API.Interface;

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

        public void SavePatientReferralData(PatientReferralData patientReferral)
        {
           // GetCohortDataResponse response = new GetCohortDataResponse();

            //_repository.Insert()
            //response = repository method call;

        }
    }
}   
