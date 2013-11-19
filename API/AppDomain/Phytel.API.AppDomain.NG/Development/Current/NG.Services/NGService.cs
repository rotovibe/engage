using Phytel.API.AppDomain.NG.DTO;
using System;
using System.Collections.Generic;
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
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
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
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
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

        public ProblemsLookUpResponse Get(ProblemLookUpRequest request)
        {
            ProblemsLookUpResponse response = new ProblemsLookUpResponse();
            try
            {
                NGManager ngm = new NGManager();

                bool result = ngm.IsUserValidated(request.Version, request.Token);
                if (result)
                    response.Problems = ngm.GetProblems(request);
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

        public CohortResponse Post(CohortRequest request)
        {
            CohortResponse response = new CohortResponse();
            try
            {
                NGManager ngm = new NGManager();

                bool result = ngm.IsUserValidated(request.Version, request.Token);
                if (result)
                {
                    // implement
                    //response = ngm.GetCohorts(request);
                    List<Cohort> chrl = new List<Cohort>();
                    chrl.Add(new Cohort { Description = "Diabetics who are males and the like.", Name = "Diabetics who are males", ShortName = "DM - all Males" });
                    chrl.Add(new Cohort { Description = "Some messed up people.", Name = "People who need some serious help.", ShortName = "OMG - all humans" });
                    response.Cohorts = chrl;
                }
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

        public GetCohortPatientsResponse Get(GetCohortPatientsRequest request)
        {
            GetCohortPatientsResponse response = new GetCohortPatientsResponse();
            try
            {
                NGManager ngm = new NGManager();

                bool result = ngm.IsUserValidated(request.Version, request.Token);
                if (result)
                {
                    // implement
                    //response = ngm.GetCohorts(request);
                    //response.Patients = NGUtils.PopulateCohortPatientStubData();
                    response = ngm.GetCohortPatients(request);
                }
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
