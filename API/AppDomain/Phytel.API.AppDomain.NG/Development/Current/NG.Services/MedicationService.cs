using AutoMapper;
using Phytel.API.AppDomain.NG.Medication;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.Security.DTO;
using Phytel.API.Common.Audit;
using Phytel.API.Common.CustomObject;
using Phytel.API.Common.Format;
using Phytel.API.DataAudit;
using ServiceStack.ServiceClient.Web;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Phytel.API.AppDomain.NG.Service
{
    public partial class MedicationService : ServiceStack.ServiceInterface.Service
    {
        public ISecurityManager Security {get; set;}
        public IMedicationManager MedicationManager {get; set;}
        public IAuditUtil AuditUtil { get; set; }
        public ICommonFormatterUtil CommonFormatterUtil { get; set; }

        private const string unknownBrowserType = "Unknown browser";
        private const string unknownUserHostAddress = "Unknown IP";

        #region MedicationMap - Posts
        public PostInitializeMedicationMapResponse Post(PostInitializeMedicationMapRequest request)
        {
            PostInitializeMedicationMapResponse response = new PostInitializeMedicationMapResponse();
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
                    response.MedicationMap = MedicationManager.InitializeMedicationMap(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    MedicationManager.LogException(ex);
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
        #endregion

        #region PatientMedSupps - Posts
        public GetPatientMedSuppsResponse Post(GetPatientMedSuppsRequest request)
        {
            GetPatientMedSuppsResponse response = new GetPatientMedSuppsResponse();
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
                    response.PatientMedSupps = MedicationManager.GetPatientMedSupps(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    MedicationManager.LogException(ex);
            }
            finally
            {
                List<string> patientIds = new List<string>();
                patientIds.Add(request.PatientId);
                if (result != null)
                {
                    string browser = (base.Request != null) ? base.Request.UserAgent : unknownBrowserType;
                    string hostAddress = (base.Request != null) ? base.Request.UserHostAddress : unknownUserHostAddress;
                    AuditUtil.LogAuditData(request, result.SQLUserId, patientIds, browser, hostAddress, request.GetType().Name);
                }
            }
            return response;
        }

        public PostPatientMedSuppResponse Post(PostPatientMedSuppRequest request)
        {
            PostPatientMedSuppResponse response = new PostPatientMedSuppResponse();
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
                    response.PatientMedSupp = MedicationManager.SavePatientMedSupp(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    MedicationManager.LogException(ex);
            }
            finally
            {
                List<string> patientIds = null;
                if (request.PatientMedSupp != null)
                {
                    patientIds = new List<string>();
                    patientIds.Add(request.PatientMedSupp.PatientId);
                }
                if (result != null)
                {
                    string browser = (base.Request != null) ? base.Request.UserAgent : unknownBrowserType;
                    string hostAddress = (base.Request != null) ? base.Request.UserHostAddress : unknownUserHostAddress;
                    AuditUtil.LogAuditData(request, result.SQLUserId, patientIds, browser, hostAddress, request.GetType().Name);
                }
            }
            return response;
        }
        #endregion

        #region PatientMedSupps - Delete
        public DeletePatientMedSuppResponse Delete(DeletePatientMedSuppRequest request)
        {
            DeletePatientMedSuppResponse response = new DeletePatientMedSuppResponse();
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
                    MedicationManager.DeletePatientMedSupp(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    MedicationManager.LogException(ex);
            }
            finally
            {
                List<string> patientIds = null;
                if (request.PatientId != null)
                {
                    patientIds = new List<string>();
                    patientIds.Add(request.PatientId);
                }
                if (result != null)
                {
                    string browser = (base.Request != null) ? base.Request.UserAgent : unknownBrowserType;
                    string hostAddress = (base.Request != null) ? base.Request.UserHostAddress : unknownUserHostAddress;
                    AuditUtil.LogAuditData(request, result.SQLUserId, patientIds, browser, hostAddress, request.GetType().Name);
                }
            }
            return response;
        }
        #endregion  
        
        #region PatientMedFrequency
        public GetPatientMedFrequenciesResponse Get(GetPatientMedFrequenciesRequest request)
        {
            GetPatientMedFrequenciesResponse response = new GetPatientMedFrequenciesResponse();
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
                    response.PatientMedFrequencies = MedicationManager.GetPatientMedFrequencies(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    MedicationManager.LogException(ex);
            }
            finally
            {
                List<string> patientIds = new List<string>();
                patientIds.Add(request.PatientId);
                if (result != null)
                {
                    string browser = (base.Request != null) ? base.Request.UserAgent : unknownBrowserType;
                    string hostAddress = (base.Request != null) ? base.Request.UserHostAddress : unknownUserHostAddress;
                    AuditUtil.LogAuditData(request, result.SQLUserId, patientIds, browser, hostAddress, request.GetType().Name);
                }
            }
            return response;
        }

        public PostPatientMedFrequencyResponse Post(PostPatientMedFrequencyRequest request)
        {
            PostPatientMedFrequencyResponse response = new PostPatientMedFrequencyResponse();
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
                    response.Id = MedicationManager.InsertPatientMedFrequency(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    MedicationManager.LogException(ex);
            }
            finally
            {
                List<string> patientIds = null;
                if (request.PatientMedFrequency != null)
                {
                    patientIds = new List<string>();
                    patientIds.Add(request.PatientMedFrequency.PatientId);
                }
                if (result != null)
                {
                    string browser = (base.Request != null) ? base.Request.UserAgent : unknownBrowserType;
                    string hostAddress = (base.Request != null) ? base.Request.UserHostAddress : unknownUserHostAddress;
                    AuditUtil.LogAuditData(request, result.SQLUserId, patientIds, browser, hostAddress, request.GetType().Name);
                }
            }
            return response;
        }
        #endregion
    }
}