using System;
using System.Net;
using Phytel.API.Common;
using Phytel.API.DataDomain.Scheduling;
using Phytel.API.DataDomain.Scheduling.DTO;
using Phytel.API.Common.Format;
using System.Configuration;

namespace Phytel.API.DataDomain.Scheduling.Service
{
    public class SchedulingService : ServiceStack.ServiceInterface.Service
    {
        public ISchedulingDataManager Manager { get; set; }
        public ICommonFormatterUtil FormatUtil { get; set; }
        public IHelpers Helpers { get; set; }

        #region ToDo
        public GetToDosDataResponse Post(GetToDosDataRequest request)
        {
            GetToDosDataResponse response = new GetToDosDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("SchedulingDD:Post()::Unauthorized Access");

                response = Manager.GetToDos(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                FormatUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public PutInsertToDoDataResponse Put(PutInsertToDoDataRequest request)
        {
            PutInsertToDoDataResponse response = new PutInsertToDoDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("SchedulingDD:Put()::Unauthorized Access");

                response = Manager.InsertToDo(request);
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

        public PutUpdateToDoDataResponse Put(PutUpdateToDoDataRequest request)
        {
            PutUpdateToDoDataResponse response = new PutUpdateToDoDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("SchedulingDD:Put()::Unauthorized Access");

                response = Manager.UpdateToDo(request);
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

        public InsertBatchPatientToDosDataResponse Post(InsertBatchPatientToDosDataRequest request)
        {
            InsertBatchPatientToDosDataResponse response = new InsertBatchPatientToDosDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("SchedulingDD:Post()::Unauthorized Access");

                response.Responses = Manager.InsertBatchPatientToDos(request);
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

        public RemoveProgramInToDosDataResponse Put(RemoveProgramInToDosDataRequest request)
        {
            RemoveProgramInToDosDataResponse response = new RemoveProgramInToDosDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("SchedulingDD:RemoveProgramInToDos()::Unauthorized Access");

                response = Manager.RemoveProgramInToDos(request);
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

        public DeleteToDoByPatientIdDataResponse Delete(DeleteToDoByPatientIdDataRequest request)
        {
            DeleteToDoByPatientIdDataResponse response = new DeleteToDoByPatientIdDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("SchedulingDD:Delete()::Unauthorized Access");

                response = Manager.DeleteToDoByPatientId(request);
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

        public UndoDeletePatientToDosDataResponse Put(UndoDeletePatientToDosDataRequest request)
        {
            UndoDeletePatientToDosDataResponse response = new UndoDeletePatientToDosDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("SchedulingDD:UndoDelete()::Unauthorized Access");

                response = Manager.UndoDeleteToDos(request);
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
        #endregion

        #region Schedule
        public GetScheduleDataResponse Get(GetScheduleDataRequest request)
        {
            GetScheduleDataResponse response = new GetScheduleDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("SchedulingDD:Get()::Unauthorized Access");

                response = Manager.GetSchedule(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                FormatUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }
        #endregion
    }
}