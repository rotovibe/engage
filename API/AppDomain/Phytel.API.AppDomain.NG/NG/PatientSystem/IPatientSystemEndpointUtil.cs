using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Context;
using Phytel.API.AppDomain.NG.DTO.Internal;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;

namespace Phytel.API.AppDomain.NG
{
    public interface IPatientSystemEndpointUtil
    {
        List<SystemData> GetSystems(IServiceContext context);
        List<PatientSystemData> GetPatientSystems(IServiceContext context, string patientId);
        List<PatientSystemOldData> GetAllPatientSystems(IServiceContext context);
        List<PatientData> GetAllPatients(IServiceContext request);
        List<PatientSystemData> InsertPatientSystems(IServiceContext context, string patientId);
        List<PatientSystemData> UpdatePatientSystems(IServiceContext context, string patientId);
        void DeletePatientSystems(DeletePatientSystemsRequest request);
    }
}