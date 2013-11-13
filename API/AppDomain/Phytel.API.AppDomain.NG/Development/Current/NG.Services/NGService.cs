using Phytel.API.AppDomain.NG.DTO;
using System;
using System.Net;

namespace Phytel.API.AppDomain.NG.Service
{
    public class NGService : ServiceStack.ServiceInterface.Service
    {
        public PatientResponse Post(PatientRequest request)
        {
            PatientResponse response = new PatientResponse();
            try
            {
                NGManager ngm = new NGManager();

                //request.Token = base.Request.Headers["APIToken"] as string;
                bool result = ngm.IsUserValidated(request.Version, request.Token);
                if (result)
                    response = ngm.GetPatientByID(request);
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Excepton", ex.Message);
            }
            return response;
        }

        public PatientResponse Get(PatientRequest request)
        {
            PatientResponse response = new PatientResponse();
            try
            {
                NGManager ngm = new NGManager();

                //request.Token = base.Request.Headers["APIToken"] as string;
                bool result = ngm.IsUserValidated(request.Version, request.Token);
                if (result)
                    response = ngm.GetPatientByID(request);
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Excepton", ex.Message);
            }
            return response;
        }

        /// <summary>
        ///     ServiceStack's GET endpoint for getting active chronic problems for a patient
        /// </summary>
        /// <param name="request">PatientProblemResponse object</param>
        /// <returns>PatientProblemResponse object</returns>
        public PatientProblemsResponse Get(PatientProblemRequest request)
        {
            PatientProblemsResponse response = new PatientProblemsResponse();
            try
            {
                NGManager ngm = new NGManager();

                bool result = ngm.IsUserValidated(request.Version, request.Token);
                if (result)
                    response.PatientProblems = ngm.GetProblemsByPatientID(request);
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        /// <summary>
        ///     ServiceStack's POST endpoint for getting active chronic problems for a patient
        /// </summary>
        /// <param name="request">PatientProblemResponse object</param>
        /// <returns>PatientProblemResponse object</returns>
        public PatientProblemsResponse POST(PatientProblemRequest request)
        {
            PatientProblemsResponse response = new PatientProblemsResponse();
            try
            {
                NGManager ngm = new NGManager();

                bool result = ngm.IsUserValidated(request.Version, request.Token);
                if (result)
                    response.PatientProblems = ngm.GetProblemsByPatientID(request);
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        public LookUpConditionsResponse Get(LookUpConditionRequest request)
        {
            LookUpConditionsResponse response = new LookUpConditionsResponse();
            try
            {
                NGManager ngm = new NGManager();

                bool result = ngm.IsUserValidated(request.Version, request.Token);
                if (result)
                    response.Conditions = ngm.GetConditions(request);
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }



    }
}
