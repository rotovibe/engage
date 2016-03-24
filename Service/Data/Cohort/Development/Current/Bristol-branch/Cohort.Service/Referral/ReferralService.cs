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
                        response  = Manager.InsertReferral(request);
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
    }
}