using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Phytel.API.Common;
using Phytel.API.Common.Format;
using Phytel.API.DataDomain.Contact.CareTeam;
using Phytel.API.DataDomain.Contact.DTO.CareTeam;

namespace Phytel.API.DataDomain.Contact.Service
{
    public class CareTeamService : ServiceStack.ServiceInterface.Service
    {
        public ICareTeamManager Manager { get; set; }
        public IHelpers Helpers { get; set; }
        public ICommonFormatterUtil CommonFormat { get; set; }

        public InsertCareTeamDataResponse Post(InsertCareTeamDataRequest request)
        {
            var response = new InsertCareTeamDataResponse() { Version = request.Version };
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
    }
}