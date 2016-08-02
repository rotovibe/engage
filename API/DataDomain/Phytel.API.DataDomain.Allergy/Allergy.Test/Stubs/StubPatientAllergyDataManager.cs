using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDomain.Allergy.Repo;
using Phytel.API.DataDomain.Allergy.DTO;

namespace Phytel.API.DataDomain.Allergy.Test
{
    public class StubPatientAllergyDataManager : IPatientAllergyDataManager
    {
        public List<DTO.PatientAllergyData> GetPatientAllergies(DTO.GetPatientAllergiesDataRequest request)
        {
            List<PatientAllergyData> result = null;
            if (request.PatientId != null)
            {
                var repo = StubRepositoryFactory.GetAllergyRepository(request, RepositoryType.PatientAllergy);
                result = repo.FindByPatientId(request) as List<PatientAllergyData>;
            }
            return result;
        }

        public DTO.PatientAllergyData InitializePatientAllergy(DTO.PutInitializePatientAllergyDataRequest request)
        {
            var repo = StubRepositoryFactory.GetAllergyRepository(request, RepositoryType.PatientAllergy);
            return (PatientAllergyData)repo.Initialize(request);
        }

        public List<DTO.PatientAllergyData> UpdatePatientAllergies(DTO.PutPatientAllergiesDataRequest request)
        {
            List<PatientAllergyData> result = new List<PatientAllergyData>();
            var repo = StubRepositoryFactory.GetAllergyRepository(request, RepositoryType.PatientAllergy);
            bool status = (bool)repo.Update(request);
            if (status)
            {
                PatientAllergyData data = (PatientAllergyData)repo.FindByID(request.PatientAllergiesData[0].Id);
                result.Add(data);
            }
            return result;
        }

        public DTO.DeleteAllergiesByPatientIdDataResponse DeletePatientAllergies(DTO.DeleteAllergiesByPatientIdDataRequest request)
        {
            DeleteAllergiesByPatientIdDataResponse response = new DeleteAllergiesByPatientIdDataResponse();
            var repo = StubRepositoryFactory.GetAllergyRepository(request, RepositoryType.PatientAllergy);
            repo.Delete(request);
            response.Success = true;
            return response;
        }

        public DTO.UndoDeletePatientAllergiesDataResponse UndoDeletePatientAllergies(DTO.UndoDeletePatientAllergiesDataRequest request)
        {
            UndoDeletePatientAllergiesDataResponse response = new UndoDeletePatientAllergiesDataResponse();
            var repo = StubRepositoryFactory.GetAllergyRepository(request, RepositoryType.PatientAllergy);
            repo.UndoDelete(request);
            response.Success = true;
            return response;
        }


        public DeletePatientAllergyDataResponse Delete(DeletePatientAllergyDataRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
