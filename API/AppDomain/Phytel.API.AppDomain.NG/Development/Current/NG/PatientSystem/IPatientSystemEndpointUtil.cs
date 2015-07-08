using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;

namespace Phytel.API.AppDomain.NG
{
    public interface IPatientSystemEndpointUtil
    {
        List<SystemSourceData> GetSystemSources(GetActiveSystemSourcesRequest request);
    }
}