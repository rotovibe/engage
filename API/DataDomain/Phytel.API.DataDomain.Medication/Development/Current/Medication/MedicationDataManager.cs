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

        public List<DTO.MedicationData> GetMedicationList(string userid, string contract)
        {
            try
            {
                List<DTO.MedicationData> result = null;
                //MedicationRepository.UserId = request.UserId;
                //result = MedicationRepository.SelectAll().Cast<DTO.Medication>().ToList<DTO.Medication>();
                var repo = MedicationRepositoryFactory.GetMedicationRepository(userid, contract, RepositoryType.Medication);
                result = repo.SelectAll().Cast<DTO.MedicationData>().ToList<DTO.MedicationData>();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("MedicationDD:GetMedicationList()::" + ex.Message, ex.InnerException);
            }
        }

        public bool BulkInsertMedications(List<DTO.MedicationData> meds, string userId, string contract)
        {
            try
            {
                //MedicationRepository.UserId = request.UserId;
                //result = MedicationRepository.SelectAll().Cast<DTO.Medication>().ToList<DTO.Medication>();
                var repo = MedicationRepositoryFactory.GetMedicationRepository(userId, contract, RepositoryType.Medication);

                var result = (Boolean)repo.InsertAll(meds.ToList<object>());
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("MedicationDD:GetMedicationList()::" + ex.Message, ex.InnerException);
            }
        }
    }
}   
