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
        GetPatientSystemsDataResponse GetPatientSystems(GetPatientSystemsDataRequest request);
        PutPatientSystemDataResponse InsertPatientSystem(PutPatientSystemDataRequest request);
        PutUpdatePatientSystemDataResponse UpdatePatientSystem(PutUpdatePatientSystemDataRequest request);
        DeletePatientSystemByPatientIdDataResponse DeletePatientSystemByPatientId(DeletePatientSystemByPatientIdDataRequest request);
        UndoDeletePatientSystemsDataResponse UndoDeletePatientSystems(UndoDeletePatientSystemsDataRequest request);
    }
}
