using System;
using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO;
namespace Phytel.API.AppDomain.NG
{
    public interface IPatientSystemManager
    {
        void LogException(Exception ex);
        List<SystemSource> GetActiveSystemSources(GetActiveSystemSourcesRequest request);
    }
}