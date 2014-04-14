using System;
using System.Net;
using Phytel.API.DataDomain.ProgramDesign;
using Phytel.API.DataDomain.ProgramDesign.DTO;
using Phytel.API.Common.Format;
using System.Configuration;

namespace Phytel.API.DataDomain.ProgramDesign.Service
{
    public class ProgramDesignService : ServiceStack.ServiceInterface.Service
    {
        public GetProgramDesignResponse Post(GetProgramDesignRequest request)
        {
            GetProgramDesignResponse response = new GetProgramDesignResponse();
            try
            {
                response = ProgramDesignDataManager.GetProgramDesignByID(request);
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

        public GetProgramDesignResponse Get(GetProgramDesignRequest request)
        {
            GetProgramDesignResponse response = new GetProgramDesignResponse();
            try
            {
                response = ProgramDesignDataManager.GetProgramDesignByID(request);
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

        public GetAllProgramDesignsResponse Post(GetAllProgramDesignsRequest request)
        {
            GetAllProgramDesignsResponse response = new GetAllProgramDesignsResponse();
            try
            {
                response = ProgramDesignDataManager.GetProgramDesignList(request);
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