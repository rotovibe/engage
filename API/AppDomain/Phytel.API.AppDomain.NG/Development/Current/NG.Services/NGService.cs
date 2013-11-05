using Phytel.API.AppDomain.NG.DTO;
using System;

namespace Phytel.API.AppDomain.NG.Service
{
    public class NGService : ServiceStack.ServiceInterface.Service
    {
        public object Get(PatientRequest request)
        {
            PatientResponse response = new PatientResponse();
            try
            {
                request.Token = base.Request.Headers["APIToken"] as string;

                // validate user against apiuser datastore
                bool result = NGManager.IsUserValidated(request.Token);

                if (result)
                {
                    response = NGManager.GetPatientByID(request.ID, request.Product, request.ContractNumber);
                }
            }
            catch(Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Excepton", ex.Message);
            }
            return response;
        }
    }
}
