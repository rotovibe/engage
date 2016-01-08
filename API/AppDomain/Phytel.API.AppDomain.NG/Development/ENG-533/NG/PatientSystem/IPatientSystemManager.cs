using System;
using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Context;
using Phytel.API.AppDomain.NG.DTO.Internal;

namespace Phytel.API.AppDomain.NG
{
    public interface IPatientSystemManager
    {
        void LogException(Exception ex);
        List<DTO.System> GetActiveSystems(IServiceContext context);
        List<DTO.PatientSystem> GetPatientSystems(IServiceContext context, string patientId);
        List<DTO.PatientSystem> InsertPatientSystems(IServiceContext context, string patientId);
        List<DTO.PatientSystem> UpdatePatientSystems(IServiceContext context, string patientId);
        void SetPrimaryPatientSystem(IServiceContext context, string patientId, List<DTO.PatientSystem> cList);
        void DeletePatientSystems(DeletePatientSystemsRequest request);
    }
}