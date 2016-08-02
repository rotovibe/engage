using System;
using Phytel.API.DataDomain.PatientSystem.DTO;

namespace Phytel.API.DataDomain.PatientSystem.Service
{
    public class SystemService : ServiceBase
    {
        public ISystemDataManager Manager { get; set; }

        public GetSystemsDataResponse Get(GetSystemsDataRequest request)
        {
            GetSystemsDataResponse response = new GetSystemsDataResponse();
            try
            {
                RequireUserId(request);
                response.SystemsData = Manager.GetSystems(request);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }
    }
}