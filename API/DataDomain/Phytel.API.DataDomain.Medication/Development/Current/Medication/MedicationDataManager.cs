using System.Collections.Generic;
using System.Linq;
using DataDomain.Medication.Repo;
using Phytel.API.DataDomain.Medication.DTO;
using System;

namespace Phytel.API.DataDomain.Medication
{
    public class MedicationDataManager : IMedicationDataManager
    {
        protected readonly IMongoMedicationRepository MedicationRepository;

        public MedicationDataManager(IMongoMedicationRepository repository)
        {
            MedicationRepository = repository;
        }

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

        public GetMedicationDetailsDataResponse GetMedicationDetails(GetMedicationDetailsDataRequest request)
        {
            try
            {
                GetMedicationDetailsDataResponse result = new GetMedicationDetailsDataResponse();
                var repo = MedicationRepositoryFactory.GetMedicationRepository(request, RepositoryType.PatientMedSupp);
                List<MEMedication> meMeds = repo.SearchMedications(request) as List<MEMedication>;
                if (meMeds != null && meMeds.Count > 0)
                {
                    List<string> ndcs = new List<string>();
                    List<string> pharmclasses = new List<string>();
                    // Get list of unique ndc codes and pharmclasses.
                    meMeds.ForEach(m =>
                    {
                        if (!ndcs.Contains(m.NDC))
                        {
                            ndcs.Add(m.NDC);
                        }
                        if (m.PharmClass != null && m.PharmClass.Count > 0)
                        {
                            m.PharmClass.ForEach(p =>
                                {
                                    if (!pharmclasses.Contains(p))
                                    {
                                        pharmclasses.Add(p);
                                    }
                                });
                        }
                    });
                    result.NDCcodes = ndcs;
                    result.PharmClasses = pharmclasses;
                }
                return result;
            }
            catch (Exception ex) { throw ex; }
        }
    }
}   
