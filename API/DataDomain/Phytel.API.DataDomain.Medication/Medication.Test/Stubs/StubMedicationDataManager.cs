using System.Collections.Generic;
using System.Linq;
using DataDomain.Medication.Repo;
using Phytel.API.DataDomain.Medication.DTO;
using System;
using Phytel.API.DataDomain.Medication.Test;

namespace Phytel.API.DataDomain.Medication
{
    public class StubMedicationDataManager : IMedicationDataManager
    {

        public List<MedicationData> GetMedicationList(GetAllMedicationsRequest request)
        {
            throw new NotImplementedException();
        }

        public bool BulkInsertMedications(List<MedicationData> meds, PutBulkInsertMedicationsRequest request)
        {
            throw new NotImplementedException();
        }

        public GetMedicationNDCsDataResponse GetMedicationNDCs(GetMedicationNDCsDataRequest request)
        {
            GetMedicationNDCsDataResponse result = new GetMedicationNDCsDataResponse();
            var repo = StubRepositoryFactory.GetMedicationRepository(request, RepositoryType.Medication);
            result.NDCcodes = repo.FindNDCCodes(request) as List<string>;
            return result;
        }
    }
}   
