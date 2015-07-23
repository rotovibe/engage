using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.PatientSystem.DTO;

namespace Phytel.API.DataDomain.PatientSystem
{
    public interface IPatientSystemDataManager
    {
        GetPatientSystemDataResponse GetPatientSystem(GetPatientSystemDataRequest request);
        List<PatientSystemData> GetPatientSystems(GetPatientSystemsDataRequest request);
        List<PatientSystemOldData> GetAllPatientSystems();
        List<PatientSystemData> InsertPatientSystems(InsertPatientSystemsDataRequest request);
        string InsertPatientSystem(InsertPatientSystemDataRequest request);
        List<PatientSystemData> UpdatePatientSystems(UpdatePatientSystemsDataRequest request);
        DeletePatientSystemByPatientIdDataResponse DeletePatientSystemByPatientId(DeletePatientSystemByPatientIdDataRequest request);
        UndoDeletePatientSystemsDataResponse UndoDeletePatientSystems(UndoDeletePatientSystemsDataRequest request);
        void DeletePatientSystems(DeletePatientSystemsDataRequest request);
        bool UpdatePatientSystem(UpdatePatientSystemDataRequest request);
        List<string> InsertEngagePatientSystems(InsertEngagePatientSystemsDataRequest request);
    }
}
