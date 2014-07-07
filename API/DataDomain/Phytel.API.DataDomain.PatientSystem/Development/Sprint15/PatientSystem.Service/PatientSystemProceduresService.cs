using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Phytel.API.Common.Format;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.DataDomain.PatientSystem.MongoDB.DataManagement;

namespace Phytel.API.DataDomain.PatientSystem.Service
{
    public partial class PatientSystemService
    {
        public IProceduresManager ProcsManager { get; set; }

        public GetMongoProceduresResponse Get(GetMongoProceduresRequest request)
        {
            GetMongoProceduresResponse response = new GetMongoProceduresResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:Post()::Unauthorized Access");

                response = ProcsManager.ExecuteProcedure(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetMongoProceduresListResponse Get(GetMongoProceduresListRequest request)
        {
            GetMongoProceduresListResponse response = new GetMongoProceduresListResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:Post()::Unauthorized Access");

                response = ProcsManager.GetProceduresList(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

    }
}