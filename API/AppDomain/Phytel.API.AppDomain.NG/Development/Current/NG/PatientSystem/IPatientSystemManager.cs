using System;
using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO;
namespace Phytel.API.AppDomain.NG
{
    public interface IPatientSystemManager
    {
        void LogException(Exception ex);
        List<DTO.System> GetActiveSystems(GetActiveSystemsRequest request);
        List<PatientSystem> GetPatientSystems(GetPatientSystemsRequest request);
        List<PatientSystem> InsertPatientSystems(InsertPatientSystemsRequest request);
        List<PatientSystem> UpdatePatientSystems(UpdatePatientSystemsRequest request);
        void DeletePatientSystems(DeletePatientSystemsRequest request);
    }
}