using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Common;
using Phytel.API.DataDomain.PatientSystem.DTO;

namespace Phytel.API.DataDomain.PatientSystem.Test
{
    class StubPatientSystemDataManager : IPatientSystemDataManager
    {
        public IPatientSystemRepositoryFactory Factory { get; set; }

        public DTO.GetPatientSystemDataResponse GetPatientSystem(DTO.GetPatientSystemDataRequest request)
        {
            GetPatientSystemDataResponse result = new GetPatientSystemDataResponse();
            var repo = Factory.GetRepository(RepositoryType.PatientSystem);
            result.PatientSystemData = repo.FindByID(request.Id) as PatientSystemData;
            return result;
        }

        public DTO.GetPatientSystemsDataResponse GetPatientSystems(DTO.GetPatientSystemsDataRequest request)
        {
            GetPatientSystemsDataResponse result = new GetPatientSystemsDataResponse();
            var repo = Factory.GetRepository(RepositoryType.PatientSystem);
            result.PatientSystemsData = repo.FindByPatientId(request.PatientId) as List<PatientSystemData>;
            return result;
        }

        public DTO.InsertPatientSystemsDataRequest InsertPatientSystem(DTO.InsertPatientSystemsDataRequest request)
        {
            InsertPatientSystemsDataRequest result = new InsertPatientSystemsDataRequest();
            var repo = Factory.GetRepository(RepositoryType.PatientSystem);
          //  result.PatientSystemId = repo.Insert(request) as string;
            return result;
        }

        public DTO.UpdatePatientSystemsDataRequest UpdatePatientSystem(DTO.UpdatePatientSystemsDataRequest request)
        {
            UpdatePatientSystemsDataRequest result = new UpdatePatientSystemsDataRequest();
            var repo = Factory.GetRepository(RepositoryType.PatientSystem);
          //  result.Success = (bool)repo.Update(request);
            return result;
        }

        public List<PatientSystemOldData> GetAllPatientSystems()
        {
            List<PatientSystemOldData> result = new List<PatientSystemOldData>();
            var repo = Factory.GetRepository(RepositoryType.PatientSystem);
            result = repo.SelectAll() as List<PatientSystemOldData>;
            return result;
        }


        List<PatientSystemData> IPatientSystemDataManager.GetPatientSystems(GetPatientSystemsDataRequest request)
        {
            throw new NotImplementedException();
        }

        public List<PatientSystemData> InsertPatientSystems(InsertPatientSystemsDataRequest request)
        {
            throw new NotImplementedException();
        }

        public PatientSystemData InsertPatientSystem(InsertPatientSystemDataRequest request)
        {
            throw new NotImplementedException();
        }

        public List<PatientSystemData> UpdatePatientSystems(UpdatePatientSystemsDataRequest request)
        {
            throw new NotImplementedException();
        }

        public DeletePatientSystemByPatientIdDataResponse DeletePatientSystemByPatientId(DeletePatientSystemByPatientIdDataRequest request)
        {
            throw new NotImplementedException();
        }

        public UndoDeletePatientSystemsDataResponse UndoDeletePatientSystems(UndoDeletePatientSystemsDataRequest request)
        {
            throw new NotImplementedException();
        }

        public void DeletePatientSystems(DeletePatientSystemsDataRequest request)
        {
            throw new NotImplementedException();
        }

        public bool UpdatePatientSystem(UpdatePatientSystemDataRequest request)
        {
            throw new NotImplementedException();
        }

        public BulkInsertResult InsertBatchEngagePatientSystems(InsertBatchEngagePatientSystemsDataRequest request)
        {
            throw new NotImplementedException();
        }

        public List<HttpObjectResponse<PatientSystemData>> InsertBatchPatientSystems(InsertBatchPatientSystemsDataRequest request)
        {
            throw new NotImplementedException();
        }


        public List<PatientSystemData> GetPatientSystemsByIds(GetPatientSystemByIdsDataRequest request)
        {
            throw new NotImplementedException();
        }
    }
}

