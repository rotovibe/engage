using Phytel.API.AppDomain.NG.DTO;
using System;
using System.Net;

namespace Phytel.API.AppDomain.NG.Service
{
    public class NGService : ServiceStack.ServiceInterface.Service
    {
        public PatientResponse Post(PatientRequest request)
        {
            bool validated = false;
            PatientResponse response = new PatientResponse();
            try
            {
                //request.Token = base.Request.Headers["APIToken"] as string;

                // if the token is valid, go ahead and grant the request.
                NGManager ngm = new NGManager();
                response = ngm.GetPatientByID(request.Token, request.ID, request.Product, request.ContractNumber, out validated);

                if (!validated)
                {
                    base.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                }
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Excepton", ex.Message);
            }
            return response;
        }
    }
}
