using System;
using System.Net;
using Phytel.API.DataDomain.CareMember;
using Phytel.API.DataDomain.CareMember.DTO;
using Phytel.API.Common.Format;
using System.Configuration;
using System.Web;

namespace Phytel.API.DataDomain.CareMember.Service
{
    public class CareMemberService : ServiceStack.ServiceInterface.Service
    {
        public ICareMemberDataManager Manager { get; set; }
        
        public PutCareMemberDataResponse Put(PutCareMemberDataRequest request)
        {
            PutCareMemberDataResponse response = new PutCareMemberDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("CareMemberDD:Put()::Unauthorized Access");

                response.Id = Manager.InsertCareMember(request);
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

        public PutUpdateCareMemberDataResponse Put(PutUpdateCareMemberDataRequest request)
        {
            PutUpdateCareMemberDataResponse response = new PutUpdateCareMemberDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("CareMemberDD:Put()::Unauthorized Access");

                response.Updated = Manager.UpdateCareMember(request);
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

        public GetCareMemberDataResponse Get(GetCareMemberDataRequest request)
        {
            GetCareMemberDataResponse response = new GetCareMemberDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("CareMemberDD:Get()::Unauthorized Access");

                response.CareMember = Manager.GetCareMember(request);
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

        public GetAllCareMembersDataResponse Get(GetAllCareMembersDataRequest request)
        {
            GetAllCareMembersDataResponse response = new GetAllCareMembersDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("CareMemberDD:Get()::Unauthorized Access");

                response.CareMembers = Manager.GetAllCareMembers(request);
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

        public GetPrimaryCareManagerDataResponse Get(GetPrimaryCareManagerDataRequest request)
        {
            GetPrimaryCareManagerDataResponse response = new GetPrimaryCareManagerDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("CareMemberDD:Get()::Unauthorized Access");

                response.CareMember = Manager.GetPrimaryCareManager(request);
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

        public DeleteCareMemberByPatientIdDataResponse Delete(DeleteCareMemberByPatientIdDataRequest request)
        {
            DeleteCareMemberByPatientIdDataResponse response = new DeleteCareMemberByPatientIdDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("CareMemberDD:CareMemberDelete()::Unauthorized Access");

                response = Manager.DeleteCareMemberByPatientId(request);
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

        public UndoDeleteCareMembersDataResponse Put(UndoDeleteCareMembersDataRequest request)
        {
            UndoDeleteCareMembersDataResponse response = new UndoDeleteCareMembersDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("CareMemberDD:CareMemberUndoDelete()::Unauthorized Access");

                response = Manager.UndoDeleteCareMembers(request);
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