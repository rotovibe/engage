using System;
using System.Net;
using Phytel.API.DataDomain.Program;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.Common.Format;
using System.Configuration;

namespace Phytel.API.DataDomain.Program.Service
{
    public class ProgramService : ServiceStack.ServiceInterface.Service
    {
        public GetProgramResponse Post(GetProgramRequest request)
        {
            GetProgramResponse response = new GetProgramResponse();
            try
            {
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