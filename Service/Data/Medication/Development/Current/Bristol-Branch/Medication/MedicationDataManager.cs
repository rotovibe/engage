using System.Collections.Generic;
using System.Linq;
using DataDomain.Medication.Repo;
using Phytel.API.DataDomain.Medication.DTO;
using System;

namespace Phytel.API.DataDomain.Medication
{
    public class MedicationDataManager : IMedicationDataManager
    {
        //protected readonly IMongoMedicationRepository MedicationRepository;
        public IMongoMedicationRepository MedicationRepository { get; set; }

        //public MedicationDataManager(IMongoMedicationRepository repository)
        //{
        //    MedicationRepository = repository;
        //}

        public List<DTO.MedicationData> GetMedicationList(GetAllMedicationsRequest request)
        {
            try
            {
                List<DTO.MedicationData> result = null;
                var repo = MedicationRepositoryFactory.GetMedicationRepository(request, RepositoryType.Medication);
                result = repo.SelectAll().Cast<DTO.MedicationData>().ToList<DTO.MedicationData>();
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        public bool BulkInsertMedications(List<DTO.MedicationData> meds, PutBulkInsertMedicationsRequest request)
        {
            try
            {
                var repo = MedicationRepositoryFactory.GetMedicationRepository(request, RepositoryType.Medication);
                var result = (Boolean)repo.InsertAll(meds.ToList<object>());
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        public GetMedicationNDCsDataResponse GetMedicationNDCs(GetMedicationNDCsDataRequest request)
        {
            try
            {
                GetMedicationNDCsDataResponse result = new GetMedicationNDCsDataResponse();
                var repo = MedicationRepositoryFactory.GetMedicationRepository(request, RepositoryType.Medication);
                List<MEMedication> meMeds = repo.FindNDCCodes(request) as List<MEMedication>;
                if (meMeds != null && meMeds.Count > 0)
                {
                    List<string> ndcs = new List<string>();
                    // Get list of unique NDC codes.
                    meMeds.ForEach(m =>
                    {
                        if (!ndcs.Contains(m.NDC))
                        {
                            ndcs.Add(m.NDC);
                        }
                    });
                    result.NDCcodes = ndcs;
                }
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

    }
}   
