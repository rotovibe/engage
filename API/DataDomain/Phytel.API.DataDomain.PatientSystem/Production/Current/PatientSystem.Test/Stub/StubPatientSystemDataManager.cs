using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.PatientSystem.DTO;

namespace Phytel.API.DataDomain.PatientSystem.Test
{
    class StubPatientSystemDataManager : IPatientSystemDataManager
    {
        public IPatientSystemRepositoryFactory Factory { get; set; }

        public DTO.GetPatientSystemDataResponse GetPatientSystem(DTO.GetPatientSystemDataRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.GetPatientSystemsDataResponse GetPatientSystems(DTO.GetPatientSystemsDataRequest request)
        {
            GetPatientSystemsDataResponse result = new GetPatientSystemsDataResponse();
            IPatientSystemRepository repo = Factory.GetRepository(request, RepositoryType.PatientSystem);
            result.PatientSystems = repo.FindByPatientId(request.PatientId) as List<PatientSystemData>;
            return result;
        }

        public DTO.PutPatientSystemDataResponse InsertPatientSystem(DTO.PutPatientSystemDataRequest request)
        {
            PutPatientSystemDataResponse result = new PutPatientSystemDataResponse();
            IPatientSystemRepository repo = Factory.GetRepository(request, RepositoryType.PatientSystem);
            result.PatientSystemId = repo.Insert(request) as string;
            return result;
        }

        public DTO.PutUpdatePatientSystemDataResponse UpdatePatientSystem(DTO.PutUpdatePatientSystemDataRequest request)
        {
            PutUpdatePatientSystemDataResponse result = new PutUpdatePatientSystemDataResponse();
            IPatientSystemRepository repo = Factory.GetRepository(request, RepositoryType.PatientSystem);
            result.Success = (bool)repo.Update(request);
            return result;
        }

        public DTO.DeletePatientSystemByPatientIdDataResponse DeletePatientSystemByPatientId(DTO.DeletePatientSystemByPatientIdDataRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.UndoDeletePatientSystemsDataResponse UndoDeletePatientSystems(DTO.UndoDeletePatientSystemsDataRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
