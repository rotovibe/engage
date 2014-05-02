using System;
using System.Net;
using Phytel.API.DataDomain.ProgramDesign;
using Phytel.API.DataDomain.ProgramDesign.DTO;
using Phytel.API.Common.Format;
using System.Configuration;

namespace Phytel.API.DataDomain.ProgramDesign.Service
{
    public class ProgramDesignService : ServiceStack.ServiceInterface.Service
    {
        //public IProgramDesignDataManager ProgramDesignDataManager { get; set; }

        public GetProgramDesignResponse Post(GetProgramDesignRequest request)
        {
            GetProgramDesignResponse response = new GetProgramDesignResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:Post()::Unauthorized Access");

                response = ProgramDesignDataManager.GetProgramDesignByID(request);
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

        public GetProgramDesignResponse Get(GetProgramDesignRequest request)
        {
            GetProgramDesignResponse response = new GetProgramDesignResponse();
            try
            {
                response = ProgramDesignDataManager.GetProgramDesignByID(request);
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

        public GetAllProgramDesignsResponse Post(GetAllProgramDesignsRequest request)
        {
            GetAllProgramDesignsResponse response = new GetAllProgramDesignsResponse();
            try
            {
                response = ProgramDesignDataManager.GetProgramDesignList(request);
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

        public PutProgramDataResponse Put(PutProgramDataRequest request)
        {
            PutProgramDataResponse response = new PutProgramDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDesignDD:Put()::Unauthorized Access");
                response = ProgramDesignDataManager.InsertProgram(request);
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

        public PutUpdateProgramDataResponse Put(PutUpdateProgramDataRequest request)
        {
            PutUpdateProgramDataResponse response = new PutUpdateProgramDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDesignDD:Put()::Unauthorized Access");
                response = ProgramDesignDataManager.UpdateProgram(request);
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

        public PutModuleDataResponse Put(PutModuleDataRequest request)
        {
            PutModuleDataResponse response = new PutModuleDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDesignDD:Put()::Unauthorized Access");

                response = ProgramDesignDataManager.InsertModule(request);
                response.Version = request.Version;
                //throw new Exception("Just a test error");
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }

            return response;
        }

        public PutUpdateModuleDataResponse Put(PutUpdateModuleDataRequest request)
        {
            PutUpdateModuleDataResponse response = new PutUpdateModuleDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDesignDD:Put()::Unauthorized Access");
                response = ProgramDesignDataManager.UpdateModule(request);
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

        public PutActionDataResponse Put(PutActionDataRequest request)
        {
            PutActionDataResponse response = new PutActionDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDesignDD:Put()::Unauthorized Access");

                response = ProgramDesignDataManager.InsertAction(request);
                response.Version = request.Version;
                //throw new Exception("Just a test error");
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }

            return response;
        }

        public PutUpdateActionDataResponse Put(PutUpdateActionDataRequest request)
        {
            PutUpdateActionDataResponse response = new PutUpdateActionDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDesignDD:Put()::Unauthorized Access");
                response = ProgramDesignDataManager.UpdateAction(request);
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

        public PutTextStepDataResponse Put(PutTextStepDataRequest request)
        {
            PutTextStepDataResponse response = new PutTextStepDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDesignDD:Put()::Unathorized Access");
                response = ProgramDesignDataManager.InsertTextStep(request);
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

        public PutUpdateTextStepDataResponse Put(PutUpdateTextStepDataRequest request)
        {
            PutUpdateTextStepDataResponse response = new PutUpdateTextStepDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDesignDD:Put()::Unauthorized Access");
                response = ProgramDesignDataManager.UpdateTextStep(request);
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

        public PutYesNoStepDataResponse Put(PutYesNoStepDataRequest request)
        {
            PutYesNoStepDataResponse response = new PutYesNoStepDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDesignDD:Put()::Unathorized Access");
                response = ProgramDesignDataManager.InsertYesNoStep(request);
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

        public PutUpdateYesNoStepDataResponse Put(PutUpdateYesNoStepDataRequest request)
        {
            PutUpdateYesNoStepDataResponse response = new PutUpdateYesNoStepDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDesignDD:Put()::Unauthorized Access");
                response = ProgramDesignDataManager.UpdateYesNoStep(request);
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

        public DeleteProgramDataResponse Delete(DeleteProgramDataRequest request)
        {
            DeleteProgramDataResponse response = new DeleteProgramDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientGoalDD:Get()::Unauthorized Access");

                response = ProgramDesignDataManager.DeleteProgram(request);
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

        public DeleteModuleDataResponse Delete(DeleteModuleDataRequest request)
        {
            DeleteModuleDataResponse response = new DeleteModuleDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientGoalDD:Get()::Unauthorized Access");

                response = ProgramDesignDataManager.DeleteModule(request);
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

        public DeleteActionDataResponse Delete(DeleteActionDataRequest request)
        {
            DeleteActionDataResponse response = new DeleteActionDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDesignDD:Get()::Unauthorized Access");

                response = ProgramDesignDataManager.DeleteAction(request);
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

        public DeleteTextStepDataResponse Delete(DeleteTextStepDataRequest request)
        {
            DeleteTextStepDataResponse response = new DeleteTextStepDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDesignDD:Get()::Unauthorized Access");

                response = ProgramDesignDataManager.DeleteTextStep(request);
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

        public DeleteYesNoStepDataResponse Delete(DeleteYesNoStepDataRequest request)
        {
            DeleteYesNoStepDataResponse response = new DeleteYesNoStepDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDesignDD:Get()::Unauthorized Access");

                response = ProgramDesignDataManager.DeleteYesNoStep(request);
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