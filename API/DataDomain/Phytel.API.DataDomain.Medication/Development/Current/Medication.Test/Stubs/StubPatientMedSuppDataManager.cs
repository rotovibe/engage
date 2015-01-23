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
            DeleteMedSuppsByPatientIdDataResponse response = null;
            try
            {
                response = new DeleteMedSuppsByPatientIdDataResponse();

                var repo = StubRepositoryFactory.GetMedicationRepository(request, RepositoryType.PatientMedSupp);
                GetPatientMedSuppsDataRequest getAllPatientMedSuppsDataRequest = new GetPatientMedSuppsDataRequest
                {
                    Context = request.Context,
                    ContractNumber = request.ContractNumber,
                    PatientId = request.PatientId,
                    UserId = request.UserId,
                    Version = request.Version
                };
                List<PatientMedSuppData> patientMedSupps = repo.FindByPatientId(getAllPatientMedSuppsDataRequest) as List<PatientMedSuppData>;
                List<string> deletedIds = null;
                if (patientMedSupps != null)
                {
                    deletedIds = new List<string>();
                    patientMedSupps.ForEach(u =>
                    {
                        DeleteMedSuppsByPatientIdDataRequest deletePMSDataRequest = new DeleteMedSuppsByPatientIdDataRequest
                        {
                            Context = request.Context,
                            ContractNumber = request.ContractNumber,
                            Id = u.Id,
                            PatientId = request.PatientId,
                            UserId = request.UserId,
                            Version = request.Version
                        };
                        repo.Delete(deletePMSDataRequest);
                        deletedIds.Add(deletePMSDataRequest.Id);
                    });
                    response.DeletedIds = deletedIds;
                }
                response.Success = true;
                return response;
            }
            catch (Exception ex) { throw ex; }
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
    }
}
