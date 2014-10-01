using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Patient.DTO;

namespace Phytel.API.DataDomain.Patient.Test.Stub
{
    class StubPatientRespository : IPatientRepository
    {
        string userId = "testuser";
        
        public StubPatientRespository(string contract)
        {
        }

        public DTO.GetPatientsDataResponse Select(string[] patientIds)
        {
            GetPatientsDataResponse response = new GetPatientsDataResponse();
            List<PatientData> patientData = new List<PatientData>();
            patientData.Add( new PatientData { Id = "abc", FirstName = "mark", LastName = "anderson", PreferredName = "MA", MiddleName = "jay", Gender = "M", DOB = "01/01/1945"});
            patientData.Add( new PatientData { Id = "efg", FirstName = "lisa", LastName = "anderson", PreferredName = "LA", MiddleName = "olivia", Gender = "F", DOB = "02/02/1932"});
            patientData.Add( new PatientData { Id = "hij", FirstName = "mark1", LastName = "anderson2", PreferredName = "MA", MiddleName = "jay", Gender = "M", DOB = "01/01/1945"});
            patientData.Add( new PatientData { Id = "xyz", FirstName = "mark2", LastName = "anderson3", PreferredName = "MA", MiddleName = "jay", Gender = "M", DOB = "01/01/1945"});
            response.Version = 1;
            response.Patients = patientData;

            return response;
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


        public List<PatientUserData> FindPatientUsersByPatientId(string patientId)
        {
            throw new NotImplementedException();
        }

        public CohortPatientViewData FindCohortPatientViewByPatientId(string patientId)
        {
            throw new NotImplementedException();
        }


        public void UndoDelete(object entity)
        {
            throw new NotImplementedException();
        }


        public object Initialize(object newEntity)
        {
            throw new NotImplementedException();
        }


        public object FindDuplicatePatient(PutUpdatePatientDataRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
