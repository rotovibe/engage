using Phytel.API.AppDomain.NG.DTO.Search;
using Phytel.API.AppDomain.Security.DTO;
using Phytel.API.Common.Audit;
using Phytel.API.Common.Format;
using ServiceStack.ServiceClient.Web;
using System;
using System.Linq;

namespace Phytel.API.AppDomain.NG.Service
{
    public partial class SearchService : ServiceStack.ServiceInterface.Service
    {
        public ISearchManager SearchManager { get; set; }
        public ISecurityManager Security {get; set;}
        public IAuditUtil AuditUtil { get; set; }
        public ICommonFormatterUtil CommonFormatterUtil { get; set; }

        private const string unknownBrowserType = "Unknown browser";
        private const string unknownUserHostAddress = "Unknown IP";

        public GetSearchResultsResponse Get(GetSearchResultsRequest request)
        {
            GetSearchResultsResponse response = new GetSearchResultsResponse();
            ValidateTokenResponse result = null;

            try
            {
                if (base.Request != null)
                {
                    request.Token = base.Request.Headers["Token"] as string;
                }
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                     var res = SearchManager.GetSearchAllergyResults(request);
                     var count = res.Count;
                     if (request.Take > 0 && res.Count > request.Take)
                     {
                         res = res.Take(request.Take).ToList();
                         response.Message = request.Take + " out of " + count + ". Please refine your search.";
                     }
                    response.Allergies = res;
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    SearchManager.LogException(ex);
            }
            finally
            {
                if (result != null)
                {
                    string browser = (base.Request != null) ? base.Request.UserAgent : unknownBrowserType;
                    string hostAddress = (base.Request != null) ? base.Request.UserHostAddress : unknownUserHostAddress;
                    AuditUtil.LogAuditData(request, result.SQLUserId, null, browser, hostAddress, request.GetType().Name);
                }
            }

            return response;
        }

        public GetMedNamesResponse Get(GetMedNamesRequest request)
        {
            GetMedNamesResponse response = new GetMedNamesResponse();
            ValidateTokenResponse result = null;

            try
            {
                if (base.Request != null)
                {
                    request.Token = base.Request.Headers["Token"] as string;
                }
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    var res = SearchManager.GetSearchMedNameResults(request);
                    var count = res.Count;
                    if (request.Take > 0 && res.Count > request.Take)
                    {
                        res = res.Take(request.Take).ToList();
                        response.Message = request.Take + " out of " + count + ". Please refine your search.";
                    }

                    response.ProprietaryNames = res;
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    SearchManager.LogException(ex);
            }
            finally
            {
                if (result != null)
                {
                    string browser = (base.Request != null) ? base.Request.UserAgent : unknownBrowserType;
                    string hostAddress = (base.Request != null) ? base.Request.UserHostAddress : unknownUserHostAddress;
                    AuditUtil.LogAuditData(request, result.SQLUserId, null, browser, hostAddress, request.GetType().Name);
                }
            }

            return response;
        }

        public GetMedFieldsResponse Get(GetMedFieldsRequest request)
        {
            GetMedFieldsResponse response = new GetMedFieldsResponse();
            ValidateTokenResponse result = null;

            try
            {
                if (base.Request != null)
                {
                    request.Token = base.Request.Headers["Token"] as string;
                }
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    var results = SearchManager.GetSearchMedFieldsResults(request);

                    response.DosageForms = results.FormList;
                    response.Routes = results.RouteList;
                    response.Strengths = results.StrengthList;
                    response.Units = results.UnitsList;
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    SearchManager.LogException(ex);
            }
            finally
            {
                if (result != null)
                {
                    string browser = (base.Request != null) ? base.Request.UserAgent : unknownBrowserType;
                    string hostAddress = (base.Request != null) ? base.Request.UserHostAddress : unknownUserHostAddress;
                    AuditUtil.LogAuditData(request, result.SQLUserId, null, browser, hostAddress, request.GetType().Name);
                }
            }

            return response;
        }
    }
}