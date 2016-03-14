using System;
using Phytel.API.DataDomain.Cohort.DTO;
using Phytel.API.Common.Format;
using System.Configuration;
using Phytel.API.DataDomain.Cohort.DTO.Referrals;

namespace Phytel.API.DataDomain.Cohort.Service.Referral
{
    public class PatientReferralService : ServiceStack.ServiceInterface.Service
    {
                public IDataPatientReferralManager Manager { get; set; }
                //public PostPatientReferralDefinitionResponse Post(PostPatientReferralDefinitionRequest request)
                //{
                //    var response = new PostPatientReferralDefinitionResponse();
                //    try
                //    {
                //        if (string.IsNullOrEmpty(request.UserId))
                //            throw new UnauthorizedAccessException("CohortDD:Post()::Unauthorized Access");

                //        Manager.InsertPatientReferral(request);
                //        response.Version = request.Version;
                //    }
                //    catch (Exception ex)
                //    {
                //        CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                //        string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                //        Common.Helper.LogException(int.Parse(aseProcessID), ex);
                //    }
                //    return response;

                //}
    }
}