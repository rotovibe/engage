using System;
using System.Net;
using Phytel.API.DataDomain.Program;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.Common.Format;
using System.Configuration;
using System.Web;

namespace Phytel.API.DataDomain.Program.Service
{
    public class ProgramService : ServiceStack.ServiceInterface.Service
    {
        private const string _phytelUserIDToken = "x-Phytel-UserID";

        public GetProgramResponse Post(GetProgramRequest request)
        {
            GetProgramResponse response = new GetProgramResponse();
            try
            {
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:Post()");

                response = ProgramDataManager.GetProgramByID(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetAllActiveProgramsResponse Get(GetAllActiveProgramsRequest request)
        {
            GetAllActiveProgramsResponse response = new GetAllActiveProgramsResponse();
            try
            {
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:Get()");

                response = ProgramDataManager.GetAllActiveContractPrograms(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public PutProgramToPatientResponse Put(PutProgramToPatientRequest request)
        {
            PutProgramToPatientResponse response = new PutProgramToPatientResponse();
            try
            {
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:Put()");

                response = ProgramDataManager.PutPatientToProgram(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public PutUpdateResponseResponse Put(PutUpdateResponseRequest request)
        {
            PutUpdateResponseResponse response = new PutUpdateResponseResponse();
            try
            {
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:Put()");

                response = ProgramDataManager.PutUpdateResponse(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public PutProgramActionProcessingResponse Put(PutProgramActionProcessingRequest request)
        {
            PutProgramActionProcessingResponse response = new PutProgramActionProcessingResponse();
            try
            {
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:Put()");

                response = ProgramDataManager.PutProgramActionUpdate(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetProgramResponse Get(GetProgramRequest request)
        {
            GetProgramResponse response = new GetProgramResponse();
            try
            {
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:Get()");

                response = ProgramDataManager.GetProgramByID(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetProgramDetailsSummaryResponse Get(GetProgramDetailsSummaryRequest request)
        {
            GetProgramDetailsSummaryResponse response = new GetProgramDetailsSummaryResponse();
            try
            {
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:Get()");

                response = ProgramDataManager.GetPatientProgramDetailsById(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetPatientProgramsResponse Get(GetPatientProgramsRequest request)
        {
            GetPatientProgramsResponse response = new GetPatientProgramsResponse();
            try
            {
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:Get()");

                response = ProgramDataManager.GetPatientPrograms(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetStepResponseResponse Get(GetStepResponseRequest request)
        {
            GetStepResponseResponse response = new GetStepResponseResponse();
            try
            {
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:Get()");

                response = ProgramDataManager.GetStepResponse(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetStepResponseListResponse Get(GetStepResponseListRequest request)
        {
            GetStepResponseListResponse response = new GetStepResponseListResponse();
            try
            {
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:Get()");

                response = ProgramDataManager.GetStepResponse(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetProgramAttributeResponse Get(GetProgramAttributeRequest request)
        {
            GetProgramAttributeResponse response = new GetProgramAttributeResponse();
            try
            {
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:Get()");

                response = ProgramDataManager.GetProgramAttributes(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public PutUpdateProgramAttributesResponse Put(PutUpdateProgramAttributesRequest request)
        {
            PutUpdateProgramAttributesResponse response = new PutUpdateProgramAttributesResponse();
            try
            {
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:Put()");

                response = ProgramDataManager.PutUpdateProgramAttributes(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public PutProgramAttributesResponse Put(PutProgramAttributesRequest request)
        {
            PutProgramAttributesResponse response = new PutProgramAttributesResponse();
            try
            {
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:Put()");

                response = ProgramDataManager.InsertProgramAttributes(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }
    }
}