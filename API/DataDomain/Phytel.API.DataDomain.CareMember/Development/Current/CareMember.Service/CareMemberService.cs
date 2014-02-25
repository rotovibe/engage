using System;
using System.Net;
using Phytel.API.DataDomain.CareMember;
using Phytel.API.DataDomain.CareMember.DTO;
using Phytel.API.Common.Format;

namespace Phytel.API.DataDomain.CareMember.Service
{
    public class CareMemberService : ServiceStack.ServiceInterface.Service
    {
        public PutCareMemberDataResponse Put(PutCareMemberDataRequest request)
        {
            PutCareMemberDataResponse response = new PutCareMemberDataResponse();
            try
            {
                response.Id = CareMemberDataManager.InsertCareMember(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        public PutUpdateCareMemberDataResponse Put(PutUpdateCareMemberDataRequest request)
        {
            PutUpdateCareMemberDataResponse response = new PutUpdateCareMemberDataResponse();
            try
            {
                response.Updated = CareMemberDataManager.UpdateCareMember(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        public GetCareMemberDataResponse Get(GetCareMemberDataRequest request)
        {
            GetCareMemberDataResponse response = new GetCareMemberDataResponse();
            try
            {
                response.CareMember = CareMemberDataManager.GetCareMember(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        public GetAllCareMembersDataResponse Get(GetAllCareMembersDataRequest request)
        {
            GetAllCareMembersDataResponse response = new GetAllCareMembersDataResponse();
            try
            {
                response.CareMembers = CareMemberDataManager.GetAllCareMembers(request);
                response.Version = request.Version;
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