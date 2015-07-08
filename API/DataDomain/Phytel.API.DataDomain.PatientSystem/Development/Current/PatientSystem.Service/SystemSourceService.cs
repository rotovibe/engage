using System;
using Phytel.API.DataDomain.PatientSystem.DTO;

namespace Phytel.API.DataDomain.PatientSystem.Service
{
    public class SystemSource : ServiceBase
    {
        public ISystemSourceDataManager Manager { get; set; }

        public GetSystemSourcesDataResponse Get(GetSystemSourcesDataRequest request)
        {
            GetSystemSourcesDataResponse response = new GetSystemSourcesDataResponse();
            try
            {
                RequireUserId(request);
                response.SystemSourcesData = Manager.GetSystemSources(request);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }
    }
}