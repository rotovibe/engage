using System;
using Phytel.API.DataDomain.Cohort.DTO;
using Phytel.API.Common.Format;
using System.Configuration;
using Phytel.API.DataDomain.Cohort.DTO.Referrals;

namespace Phytel.API.DataDomain.Cohort.Service.Referral
{
    public class ReferralService : ServiceStack.ServiceInterface.Service
    {
                IDataReferralManager Manager { get; set; }
                public PostReferralDefinitionResponse Get(PostReferralDefinitionRequest request)
                {
                    var response = new PostReferralDefinitionResponse();
                    try
                    {
                        if (string.IsNullOrEmpty(request.UserId))
                            throw new UnauthorizedAccessException("CohortDD:Get()::Unauthorized Access");

                        Manager.SaveReferralData(request.Referral);
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