using System;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Service;

namespace Phytel.API.AppDomain.NG.Command
{
    public class DereferencePatientInContactCommand : INGCommand
    {
        private readonly DereferencePatientRequest _request;
        private readonly IRestClient _restClient;
        public IContactEndpointUtil EndpointUtil { get; set; }

        public DereferencePatientInContactCommand(DereferencePatientRequest request, IRestClient restClient)
        {
            _request = request ;
            _restClient = restClient;
        }

        public void Execute()
        {
            var patientId = _request.PatientId;
            
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
