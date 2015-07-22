using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Internal;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;

namespace Phytel.API.AppDomain.NG
{
    public interface IPatientSystemEndpointUtil
    {
        List<SystemData> GetSystems(GetActiveSystemsRequest request);
        List<PatientSystemData> GetPatientSystems(GetPatientSystemsRequest request);
        List<PatientSystemOldData> GetAllPatientSystems(UpdatePatientsAndSystemsRequest request);
        List<PatientSystemData> InsertPatientSystems(InsertPatientSystemsRequest request);
        string InsertPatientSystem(InsertPatientSystemDataRequest request);
        List<PatientSystemData> UpdatePatientSystems(UpdatePatientSystemsRequest request);
        void DeletePatientSystems(DeletePatientSystemsRequest request);
        List<PatientData> GetAllPatients(UpdatePatientsAndSystemsRequest request);
    }
}