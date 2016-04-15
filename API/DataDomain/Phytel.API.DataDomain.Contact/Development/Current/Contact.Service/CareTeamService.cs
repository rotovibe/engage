﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using Phytel.API.Common;
using Phytel.API.Common.Format;
using Phytel.API.DataDomain.Contact.CareTeam;
using Phytel.API.DataDomain.Contact.DTO.CareTeam;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.Contact.Service
{
    public class CareTeamService : ServiceStack.ServiceInterface.Service
    {
        public ICareTeamManager Manager { get; set; }
        public IHelpers Helpers { get; set; }
        public ICommonFormatterUtil CommonFormat { get; set; }

        public SaveCareTeamDataResponse Post(SaveCareTeamDataRequest request)
        {
            var response = new SaveCareTeamDataResponse() { Version = request.Version };
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ContactDD:CareTeamService:Post()::Unauthorized Access");

                Manager.InsertCareTeam(request);

            }
            catch (Exception ex)
            {
                CommonFormat.FormatExceptionResponse(response, base.Response, ex);

                var aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public UpdateCareTeamMemberDataResponse Put(UpdateCareTeamMemberDataRequest request)
        {
            var response = new UpdateCareTeamMemberDataResponse() { Version = request.Version };
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ContactDD:CareTeamService:Put()::Unauthorized Access");

                Manager.UpdateCareTeamMember(request);

            }           
            catch (Exception ex)
            {
                CommonFormat.FormatExceptionResponse(response, base.Response, ex);
                
                var aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);                                 
            }
            return response;
        }

        public GetCareTeamDataResponse Get(GetCareTeamDataRequest request)
        {
            var response = new GetCareTeamDataResponse() { Version = request.Version };
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ContactDD:CareTeamService:Get()::Unauthorized Access");

                response = Manager.GetCareTeam(request);

            }
            catch (Exception ex)
            {
                CommonFormat.FormatExceptionResponse(response, base.Response, ex);

                var aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public DeleteCareTeamMemberDataResponse Delete(DeleteCareTeamMemberDataRequest request)
        {
            var response = new DeleteCareTeamMemberDataResponse();

            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ContactDD:CareTeamService:Delete()::Unauthorized Access");

                Manager.DeleteCareTeamMember(request);
            }
            catch (Exception ex)
            {
                CommonFormat.FormatExceptionResponse(response, base.Response, ex);

                var aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;

        }
    }
}