using System;
using System.Net;
using Phytel.API.DataDomain.Program;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.Common.Format;
using System.Configuration;
using System.Web;
using Phytel.API.Common;
using Phytel.API.DataDomain.Program.MongoDB.DataManagement;

namespace Phytel.API.DataDomain.Program.Service
{
    public class ProgramService : ServiceStack.ServiceInterface.Service
    {
        public ICommonFormatterUtil CommonFormatterUtil { get; set; }
        public IHelpers Helpers { get; set; }
        public IProgramDataManager ProgramDataManager { get; set; }

        public GetProgramResponse Post(GetProgramRequest request)
        {
            GetProgramResponse response = new GetProgramResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:Post()::Unauthorized Access");

                response = ProgramDataManager.GetProgramByID(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetAllActiveProgramsResponse Get(GetAllActiveProgramsRequest request)
        {
            GetAllActiveProgramsResponse response = new GetAllActiveProgramsResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:Get()::Unauthorized Access");

                response = ProgramDataManager.GetAllActiveContractPrograms(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public PutProgramToPatientResponse Put(PutProgramToPatientRequest request)
        {
            PutProgramToPatientResponse response = new PutProgramToPatientResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:Put()::Unauthorized Access");

                response = ProgramDataManager.PutPatientToProgram(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public PutInsertResponseResponse Put(PutInsertResponseRequest request)
        {
            PutInsertResponseResponse response = new PutInsertResponseResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:Put()::Unauthorized Access");

                response = ProgramDataManager.PutInsertResponse(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public PutUpdateResponseResponse Put(PutUpdateResponseRequest request)
        {
            PutUpdateResponseResponse response = new PutUpdateResponseResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:Put()::Unauthorized Access");

                response = ProgramDataManager.PutUpdateResponse(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public PutProgramActionProcessingResponse Put(PutProgramActionProcessingRequest request)
        {
            PutProgramActionProcessingResponse response = new PutProgramActionProcessingResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:Put()::Unauthorized Access");

                response = ProgramDataManager.PutProgramActionUpdate(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetProgramResponse Get(GetProgramRequest request)
        {
            GetProgramResponse response = new GetProgramResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:Get()::Unauthorized Access");

                response = ProgramDataManager.GetProgramByID(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetProgramDetailsSummaryResponse Get(GetProgramDetailsSummaryRequest request)
        {
            GetProgramDetailsSummaryResponse response = new GetProgramDetailsSummaryResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:Get()::Unauthorized Access");

                response = ProgramDataManager.GetPatientProgramDetailsById(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetPatientProgramsDataResponse Get(GetPatientProgramsDataRequest request)
        {
            GetPatientProgramsDataResponse response = new GetPatientProgramsDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:Get()::Unauthorized Access");

                response = ProgramDataManager.GetPatientPrograms(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetStepResponseResponse Get(GetStepResponseRequest request)
        {
            GetStepResponseResponse response = new GetStepResponseResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:Get()::Unauthorized Access");

                response = ProgramDataManager.GetStepResponse(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetStepResponseListResponse Get(GetStepResponseListRequest request)
        {
            GetStepResponseListResponse response = new GetStepResponseListResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:Get()::Unauthorized Access");

                response = ProgramDataManager.GetStepResponse(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetProgramAttributeResponse Get(GetProgramAttributeRequest request)
        {
            GetProgramAttributeResponse response = new GetProgramAttributeResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:Get()::Unauthorized Access");

                response = ProgramDataManager.GetProgramAttributes(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public PutUpdateProgramAttributesResponse Put(PutUpdateProgramAttributesRequest request)
        {
            PutUpdateProgramAttributesResponse response = new PutUpdateProgramAttributesResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:Put()::Unauthorized Access");

                response = ProgramDataManager.PutUpdateProgramAttributes(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public PutProgramAttributesResponse Put(PutProgramAttributesRequest request)
        {
            PutProgramAttributesResponse response = new PutProgramAttributesResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:Put()::Unauthorized Access");

                response = ProgramDataManager.InsertProgramAttributes(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetPatientActionDetailsDataResponse Get(GetPatientActionDetailsDataRequest request)
        {
            GetPatientActionDetailsDataResponse response = new GetPatientActionDetailsDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:Get()::Unauthorized Access");

                response = ProgramDataManager.GetActionDetails(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetMongoProceduresResponse Get(GetMongoProceduresRequest request)
        {
            GetMongoProceduresResponse response = new GetMongoProceduresResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:Post()::Unauthorized Access");

                IProceduresManager pm = new ProceduresManager();
                response = pm.ExecuteProcedure(request);
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

        public GetMongoProceduresListResponse Get(GetMongoProceduresListRequest request)
        {
            GetMongoProceduresListResponse response = new GetMongoProceduresListResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:Post()::Unauthorized Access");

                IProceduresManager pm = new ProceduresManager();
                response = pm.GetProceduresList(request);
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

        public DeletePatientProgramByPatientIdDataResponse Delete(DeletePatientProgramByPatientIdDataRequest request)
        {
            DeletePatientProgramByPatientIdDataResponse response = new DeletePatientProgramByPatientIdDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:PatientProgramDelete()::Unauthorized Access");

                response = ProgramDataManager.DeletePatientProgramByPatientId(request);
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

        public UndoDeletePatientProgramDataResponse Put(UndoDeletePatientProgramDataRequest request)
        {
            UndoDeletePatientProgramDataResponse response = new UndoDeletePatientProgramDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:PatientProgramUndoDelete()::Unauthorized Access");

                response = ProgramDataManager.UndoDeletePatientPrograms(request);
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

        public DeletePatientProgramDataResponse Delete(DeletePatientProgramDataRequest request)
        {
            DeletePatientProgramDataResponse response = new DeletePatientProgramDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:PatientProgramDelete()::Unauthorized Access");

                response = ProgramDataManager.DeletePatientProgram(request);
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