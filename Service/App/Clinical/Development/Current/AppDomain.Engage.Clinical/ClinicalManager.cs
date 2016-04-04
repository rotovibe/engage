using AppDomain.Engage.Clinical.DataDomainClient;
using AppDomain.Engage.Clinical.DTO.Context;
using AppDomain.Engage.Clinical.DTO.Medications;

namespace AppDomain.Engage.Clinical
{
    public class ClinicalManager : IClinicalManager
    {
        private readonly IServiceContext _context;
        private readonly IMedicationDataDomainClient _client;
        public UserContext UserContext { get; set; }

        public ClinicalManager(IServiceContext context, IMedicationDataDomainClient client)
        {
            _context = context;
            _client = client;
        }

        public PostPatientMedicationsResponse SavePatientMedications(MedicationData medsData)
        {
            return new PostPatientMedicationsResponse(); // add new dd client method call.  ie _client.SavePatientMedications.
        }
    }
}