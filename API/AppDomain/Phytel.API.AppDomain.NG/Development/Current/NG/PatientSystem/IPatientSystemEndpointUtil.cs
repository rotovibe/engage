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
        List<string> InsertEngagePatientSystems(InsertEngagePatientSystemsDataRequest request);
        List<PatientSystemData> UpdatePatientSystems(UpdatePatientSystemsRequest request);
        void DeletePatientSystems(DeletePatientSystemsRequest request);
        List<PatientData> GetAllPatients(UpdatePatientsAndSystemsRequest request);
        bool HasHealthyWeightProgramAssigned(string patientId, UpdatePatientsAndSystemsRequest request);
    }
}