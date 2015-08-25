using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDomain.Medication.Repo;
using Phytel.API.DataDomain.Medication.DTO;
using Phytel.API.DataDomain.Medication.Test;

namespace Phytel.API.DataDomain.Medication
{
    public class StubPatientMedSuppDataManager : IPatientMedSuppDataManager
    {

        public List<PatientMedSuppData> GetPatientMedSupps(GetPatientMedSuppsDataRequest request)
        {
            var repo = StubRepositoryFactory.GetMedicationRepository(request, RepositoryType.PatientMedSupp);
            return repo.FindByPatientId(request) as List<PatientMedSuppData>;
        }

        public PatientMedSuppData SavePatientMedSupps(PutPatientMedSuppDataRequest request)
        {
            var repo = StubRepositoryFactory.GetMedicationRepository(request, RepositoryType.PatientMedSupp);
            return  (PatientMedSuppData)repo.Insert(request);
        }

        public DeleteMedSuppsByPatientIdDataResponse DeletePatientMedSupps(DeleteMedSuppsByPatientIdDataRequest request)
        {
            DeleteMedSuppsByPatientIdDataResponse response = new DeleteMedSuppsByPatientIdDataResponse();
            var repo = StubRepositoryFactory.GetMedicationRepository(request, RepositoryType.PatientMedSupp);
            repo.Delete(request);
            response.DeletedIds = new List<string> { request.Id};
            response.Success = true;
            return response;
        }

        public UndoDeletePatientMedSuppsDataResponse UndoDeletePatientMedSupps(UndoDeletePatientMedSuppsDataRequest request)
        {
            UndoDeletePatientMedSuppsDataResponse response = new UndoDeletePatientMedSuppsDataResponse();
            var repo = StubRepositoryFactory.GetMedicationRepository(request, RepositoryType.PatientMedSupp);
            if (request.Ids != null && request.Ids.Count > 0)
            {
                request.Ids.ForEach(u =>
                {
                    request.PatientMedSuppId = u;
                    repo.UndoDelete(request);
                });
            }
            response.Success = true;
            return response;
        }


        public DeletePatientMedSuppDataResponse Delete(DeletePatientMedSuppDataRequest request)
        {
            throw new NotImplementedException();
        }

        public int GetPatientMedSuppsCount(GetPatientMedSuppsCountDataRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
