using AppDomain.Engage.Population.DataDomainClient;
using AppDomain.Engage.Population.DTO.Context;
using Phytel.API.DataDomain.Patient.DTO;

namespace AppDomain.Engage.Population
{
    public class DemographicsManager : IDemographicsManager
    {
        private readonly IServiceContext _context;
        private readonly IPatientDataDomainClient _client;

        public DemographicsManager(IServiceContext context, IPatientDataDomainClient client)
        {
            _context = context;
            _client = client;
        }

        //example implementation
        public string DoSomething()
        {
            _client.PostPatientDetails(new PatientData());
            return _context.Contract;
        }
    }
}
