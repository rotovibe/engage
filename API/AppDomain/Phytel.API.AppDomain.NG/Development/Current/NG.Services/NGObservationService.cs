using System;
using System.Net;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.ServiceHost;
using Phytel.API.Common.Format;
using Phytel.API.AppDomain.Security.DTO;
using ServiceStack.ServiceClient.Web;
using ServiceStack.ServiceInterface.Cors;
using System.Text;
using System.Linq;
using Phytel.API.Common.Audit;
using System.Collections.Generic;
using Phytel.API.AppDomain.NG.Observation;
using System.Web;
using Phytel.API.DataAudit;

namespace Phytel.API.AppDomain.NG.Service
{
    public partial class NGService : ServiceStack.ServiceInterface.Service
    {
        public GetStandardObservationItemsResponse Get(GetStandardObservationItemsRequest request)
        {
            GetStandardObservationItemsResponse response = new GetStandardObservationItemsResponse();
            try
            {
                ObservationsManager om = new ObservationsManager();
                ValidateTokenResponse result = om.IsUserValidated(request.Version, request.Token, HttpContext.Current.Request);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = om.GetStandardObservationsRequest(request);
                }
                else
                    throw new UnauthorizedAccessException();


            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
            }
            finally
            {
                List<string> patientIds = new List<string>();
                patientIds.Add(request.PatientId);
                AuditHelper.LogAuditData(request, patientIds, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            
            return response; 
        }

        public GetAdditionalObservationItemResponse Get(GetAdditionalObservationItemRequest request)
        {
            GetAdditionalObservationItemResponse response = new GetAdditionalObservationItemResponse();
            try
            {
                ObservationsManager om = new ObservationsManager();
                ValidateTokenResponse result = om.IsUserValidated(request.Version, request.Token, HttpContext.Current.Request);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = om.GetAdditionalObservationsRequest(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
            }
            finally
            {
                List<string> patientIds = new List<string>();
                patientIds.Add(request.PatientId);
                AuditHelper.LogAuditData(request, patientIds, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }

            return response;
        }

        public GetAdditionalObservationLibraryResponse Get(GetAdditionalObservationLibraryRequest request)
        {
            GetAdditionalObservationLibraryResponse response = new GetAdditionalObservationLibraryResponse();
            try
            {
                ObservationsManager om = new ObservationsManager();
                ValidateTokenResponse result = om.IsUserValidated(request.Version, request.Token, HttpContext.Current.Request);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = om.GetAdditionalObservationsLibraryRequest(request);
                }
                else
                    throw new UnauthorizedAccessException();


            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
            }
            finally
            {
                List<string> patientIds = new List<string>();
                patientIds.Add(request.PatientId);
                AuditHelper.LogAuditData(request, patientIds, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }

            return response;
        }

        public PostUpdateObservationItemsResponse Post(PostUpdateObservationItemsRequest request)
        {
            PostUpdateObservationItemsResponse response = new PostUpdateObservationItemsResponse();
            try
            {
                ObservationsManager om = new ObservationsManager();
                ValidateTokenResponse result = om.IsUserValidated(request.Version, request.Token, HttpContext.Current.Request);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = om.SavePatientObservations(request);
                }
                else
                    throw new UnauthorizedAccessException();


            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
            }
            finally
            {
                List<string> patientIds = new List<string>();
                patientIds.Add(request.PatientId);
                AuditHelper.LogAuditData(request, patientIds, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }

            return response;
        }
    }
}