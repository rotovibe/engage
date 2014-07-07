using Phytel.API.DataDomain.PatientSystem.DTO;
using System;

namespace Phytel.API.DataDomain.PatientSystem
{
    public interface IPatientSystemDataManager
    {
        IPatientSystemRepositoryFactory Factory { get; set; }
        GetAllPatientSystemsDataResponse GetAllPatientSystems(GetAllPatientSystemsDataRequest request);
        GetPatientSystemDataResponse GetPatientSystem(GetPatientSystemDataRequest request);
        PutPatientSystemDataResponse PutPatientSystem(PutPatientSystemDataRequest request);
    }
}
