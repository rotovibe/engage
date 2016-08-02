using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.Security.DTO;
using Phytel.API.Common.Format;
using Phytel.API.DataAudit;
using ServiceStack.ServiceClient.Web;
using System;

namespace Phytel.API.AppDomain.NG.Service
{
    /// <summary>
    /// The CareMember endpoints are not longer used
    /// </summary>
    public partial class NGService //: ServiceStack.ServiceInterface.Service
    {

        //public PostCareMemberResponse Post(PostCareMemberRequest request)
        //{
        //    PostCareMemberResponse response = new PostCareMemberResponse();
        //    CareMembersManager ngm = new CareMembersManager();
        //    ValidateTokenResponse result = null;

        //    try
        //    {
        //        request.Token = base.Request.Headers["Token"] as string;
        //        result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
        //        if (result.UserId.Trim() != string.Empty)
        //        {
        //            request.UserId = result.UserId;
        //            response = ngm.InsertCareMember(request);
        //        }
        //        else
        //            throw new UnauthorizedAccessException();
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
        //        if ((ex is WebServiceException) == false)
        //            ngm.LogException(ex);
        //    }
        //    finally
        //    {
        //        if(result != null)
        //            AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
        //    }
        //    return response;
        //}

        //public PostUpdateCareMemberResponse Post(PostUpdateCareMemberRequest request)
        //{
        //    PostUpdateCareMemberResponse response = new PostUpdateCareMemberResponse();
        //    CareMembersManager ngm = new CareMembersManager();
        //    ValidateTokenResponse result = null;

        //    try
        //    {
        //        request.Token = base.Request.Headers["Token"] as string;
        //        result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
        //        if (result.UserId.Trim() != string.Empty)
        //        {
        //            request.UserId = result.UserId;
        //            response = ngm.UpdateCareMember(request);
        //        }
        //        else
        //            throw new UnauthorizedAccessException();
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
        //        if ((ex is WebServiceException) == false)
        //            ngm.LogException(ex);
        //    }
        //    finally
        //    {
        //        if(result != null)
        //            AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
        //    }
        //    return response;
        //}

        //public GetCareMemberResponse Get(GetCareMemberRequest request)
        //{
        //    GetCareMemberResponse response = new GetCareMemberResponse();
        //    CareMembersManager nManager = new CareMembersManager();
        //    ValidateTokenResponse result = null;

        //    try
        //    {
        //        request.Token = base.Request.Headers["Token"] as string;
        //        result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
        //        if (result.UserId.Trim() != string.Empty)
        //        {
        //            request.UserId = result.UserId;
        //            response.CareMember = nManager.GetCareMember(request);
        //        }
        //        else
        //            throw new UnauthorizedAccessException();
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
        //        if ((ex is WebServiceException) == false)
        //            nManager.LogException(ex);
        //    }
        //    finally
        //    {
        //        if(result != null)
        //            AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
        //    }
        //    return response;
        //}

        //public GetAllCareMembersResponse Get(GetAllCareMembersRequest request)
        //{
        //    GetAllCareMembersResponse response = new GetAllCareMembersResponse();
        //    CareMembersManager nManager = new CareMembersManager();
        //    ValidateTokenResponse result = null;

        //    try
        //    {
        //        request.Token = base.Request.Headers["Token"] as string;
        //        result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
        //        if (result.UserId.Trim() != string.Empty)
        //        {
        //            request.UserId = result.UserId;
        //            response.CareMembers = nManager.GetAllCareMembers(request);
        //        }
        //        else
        //            throw new UnauthorizedAccessException();
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
        //        if ((ex is WebServiceException) == false)
        //            nManager.LogException(ex);
        //    }
        //    finally
        //    {
        //        if(result != null)
        //            AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
        //    }
        //    return response;
        //}
    }
}