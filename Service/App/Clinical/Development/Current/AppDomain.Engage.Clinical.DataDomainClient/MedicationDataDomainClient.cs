using AppDomain.Engage.Clinical.DTO.Context;
using AutoMapper;
using Phytel.API.Common;
using ServiceStack.Service;

namespace AppDomain.Engage.Clinical.DataDomainClient
{
    public class MedicationDataDomainClient : IMedicationDataDomainClient
    {
        private readonly IMappingEngine _mappingEngine;
        private readonly string _domainUrl;
        private readonly IHelpers _urlHelper;
        private readonly IRestClient _client;
        private readonly IServiceContext _context;

        public MedicationDataDomainClient(IMappingEngine mappingEngine, string domainUrl, IHelpers urlHelper, IRestClient client, IServiceContext context)
        {
            _mappingEngine = mappingEngine;
            _domainUrl = domainUrl;
            _urlHelper = urlHelper;
            _client = client;
            _context = context;
        }

        // implement method to call data domain service to save data
        public string PostPatientMedications(Phytel.API.DataDomain.Medication.DTO.PatientMedSuppData data)
        {
            throw new System.NotImplementedException();
        }
    }
}