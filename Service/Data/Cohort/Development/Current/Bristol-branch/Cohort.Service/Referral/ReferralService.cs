using System;
using Phytel.API.DataDomain.Cohort.DTO;
using Phytel.API.Common.Format;
using System.Configuration;
using Phytel.API.DataDomain.Cohort.DTO.Referrals;

namespace Phytel.API.DataDomain.Cohort.Service.Referral
{
    public class ReferralService : ServiceStack.ServiceInterface.Service
    {
        public IDataReferralManager Manager { get; set; }

        public PostReferralDefinitionResponse Post(PostReferralDefinitionRequest request)
        {
            var response = new PostReferralDefinitionResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("CohortDD:Post()::Unauthorized Access");
                if ((string.IsNullOrEmpty(request.Context)))
                    throw new ArgumentNullException("Request parameter context value cannot be NULL/EMPTY");
                if ((string.IsNullOrEmpty(request.ContractNumber)))
                    throw new ArgumentNullException("Request parameter contract number value cannot be NULL/EMPTY");
                response.ReferralId = Manager.InsertReferral(request.Referral);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                //Setting service stack response to bubble up the exception to app domain
                response.ResponseStatus = response.Status;
                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }


        public PostPatientsListReferralDefinitionResponse Post(PostPatientsListReferralDefinitionRequest request)
        {
            var response = new PostPatientsListReferralDefinitionResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("CohortDD:Post()::Unauthorized Access");

                response = Manager.InsertReferralsAll(request);
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }       // end method definition


        public GetReferralDataResponse Get(GetReferralDataRequest request)
        {
            GetReferralDataResponse response = new GetReferralDataResponse();
            try
            {
                response.Referral = Manager.GetReferralById(request.ReferralID);
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

        public GetAllReferralsDataResponse Get(GetAllReferralsDataRequest request)
        {
            GetAllReferralsDataResponse response = new GetAllReferralsDataResponse();
            try
            {
                response.Referrals = Manager.GetAllReferrals();
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