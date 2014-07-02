using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Patient.Test.Stub
{
    class StubCohortPatientViewRepository : IPatientRepository
    {
        string userId = "testuser";

        public StubCohortPatientViewRepository(string contract)
        {
        }

        public DTO.GetPatientsDataResponse Select(string[] patientIds)
        {
            throw new NotImplementedException();
        }

        public List<DTO.PatientData> Select(string query, string[] filterData, string querySort, int skip, int take)
        {
            throw new NotImplementedException();
        }

        public DTO.PutPatientPriorityResponse UpdatePriority(DTO.PutPatientPriorityRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.PutPatientFlaggedResponse UpdateFlagged(DTO.PutPatientFlaggedRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.PutPatientBackgroundDataResponse UpdateBackground(DTO.PutPatientBackgroundDataRequest request)
        {
            throw new NotImplementedException();
        }

        public object FindByID(string patientId, string userId)
        {
            throw new NotImplementedException();
        }

        public object Update(DTO.PutUpdatePatientDataRequest request)
        {
            throw new NotImplementedException();
        }

        public object GetSSN(string patientId)
        {
            throw new NotImplementedException();
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }

        public void Delete(object entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public object FindByID(string entityID)
        {
            throw new NotImplementedException();
        }

        public object Insert(object newEntity)
        {
            throw new NotImplementedException();
        }

        public object InsertAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> SelectAll()
        {
            throw new NotImplementedException();
        }

        public object Update(object entity)
        {
            throw new NotImplementedException();
        }

        public string UserId
        {
            get
            {
                return this.userId;
            }
            set
            {
                this.userId = value;
            }
        }


        public List<DTO.PatientUserData> FindPatientUsersByPatientId(string patientId)
        {
            throw new NotImplementedException();
        }

        public DTO.CohortPatientViewData FindCohortPatientViewByPatientId(string patientId)
        {
            throw new NotImplementedException();
        }

        DTO.GetPatientsDataResponse IPatientRepository.Select(string[] patientIds)
        {
            throw new NotImplementedException();
        }

        List<DTO.PatientData> IPatientRepository.Select(string query, string[] filterData, string querySort, int skip, int take)
        {
            throw new NotImplementedException();
        }

        DTO.PutPatientPriorityResponse IPatientRepository.UpdatePriority(DTO.PutPatientPriorityRequest request)
        {
            throw new NotImplementedException();
        }

        DTO.PutPatientFlaggedResponse IPatientRepository.UpdateFlagged(DTO.PutPatientFlaggedRequest request)
        {
            throw new NotImplementedException();
        }

        DTO.PutPatientBackgroundDataResponse IPatientRepository.UpdateBackground(DTO.PutPatientBackgroundDataRequest request)
        {
            throw new NotImplementedException();
        }

        object IPatientRepository.FindByID(string patientId, string userId)
        {
            throw new NotImplementedException();
        }

        object IPatientRepository.Update(DTO.PutUpdatePatientDataRequest request)
        {
            throw new NotImplementedException();
        }

        object IPatientRepository.GetSSN(string patientId)
        {
            throw new NotImplementedException();
        }

        List<DTO.PatientUserData> IPatientRepository.FindPatientUsersByPatientId(string patientId)
        {
            throw new NotImplementedException();
        }

        DTO.CohortPatientViewData IPatientRepository.FindCohortPatientViewByPatientId(string patientId)
        {
            throw new NotImplementedException();
        }
    }
}
