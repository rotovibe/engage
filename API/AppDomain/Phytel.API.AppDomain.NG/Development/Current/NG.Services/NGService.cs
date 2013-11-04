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
                // validate user against apiuser datastore
                response = NGManager.GetPatientByID(request.ID);
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
