using System;
using Phytel.API.AppDomain.NG.DTO;

namespace Phytel.API.AppDomain.NG.Command
{
    public class DereferencePatientInContactCommand : INGCommand
    {
        private readonly PostDeletePatientRequest _request;
        public IContactEndpointUtil contactEndpointUtil { get; set; }

        public DereferencePatientInContactCommand(PostDeletePatientRequest request)
        {
            _request = request ;

        }

        public void Execute()
        {
            try
            {
                var patientId = _request.Id;
                var response = contactEndpointUtil.DereferencePatientInContact(patientId, _request.Version, _request.ContractNumber, _request.UserId);

            }
            catch (Exception ex)
            {

                throw new Exception("AD: DereferencePatientInContactCommand Execute::" + ex.Message, ex.InnerException);
            }
            
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
