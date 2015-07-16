using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;

namespace Phytel.API.AppDomain.NG
{
    public interface IPatientSystemEndpointUtil
    {
        List<SystemData> GetSystems(GetActiveSystemsRequest request);
        List<PatientSystemData> GetPatientSystems(GetPatientSystemsRequest request);
        List<PatientSystemData> InsertPatientSystems(InsertPatientSystemsRequest request);
        List<PatientSystemData> UpdatePatientSystems(UpdatePatientSystemsRequest request);
        void DeletePatientSystems(DeletePatientSystemsRequest request);
    }
}