using System;
using System.Collections.Generic;
using System.Linq;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.Security.DTO;
using Phytel.API.Common.Audit;
using Phytel.API.Common.Format;

namespace Phytel.API.AppDomain.NG.Service
{
    public partial class NGCareMembersService : ServiceStack.ServiceInterface.Service
    {
        public PostCareMemberResponse Post(PostCareMemberRequest request)
        {
            PostCareMemberResponse response = new PostCareMemberResponse();
            try
            {
                CareMembersManager ngm = new CareMembersManager();

                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = ngm.InsertCareMember(request);
                }
                else
                    throw new UnauthorizedAccessException();

                AuditHelper.LogAuditData(request, null, System.Web.HttpContext.Current.Request, request.GetType().Name);

                return response;
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                return response;
            }
        }

        public PostUpdateCareMemberResponse Post(PostUpdateCareMemberRequest request)
        {
            PostUpdateCareMemberResponse response = new PostUpdateCareMemberResponse();
            try
            {
                CareMembersManager ngm = new CareMembersManager();

                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = ngm.UpdateCareMember(request);
                }
                else
                    throw new UnauthorizedAccessException();

                AuditHelper.LogAuditData(request, null, System.Web.HttpContext.Current.Request, request.GetType().Name);

                return response;
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                return response;
            }
        }

        public GetCareMemberResponse Get(GetCareMemberRequest request)
        {
            GetCareMemberResponse response = new GetCareMemberResponse();
            try
            {
                CareMembersManager nManager = new CareMembersManager();
                ValidateTokenResponse result = nManager.IsUserValidated(request.Version, request.Token);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.CareMember = nManager.GetCareMember(request);
                }
                else
                    throw new UnauthorizedAccessException();

                return response;
            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                return response;
            }
        }

        public GetAllCareMembersResponse Get(GetAllCareMembersRequest request)
        {
            GetAllCareMembersResponse response = new GetAllCareMembersResponse();
            try
            {
                CareMembersManager nManager = new CareMembersManager();
                ValidateTokenResponse result = nManager.IsUserValidated(request.Version, request.Token);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.CareMembers = nManager.GetAllCareMembers(request);
                }
                else
                    throw new UnauthorizedAccessException();

                return response;
            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                return response;
            }
        }
    }
}