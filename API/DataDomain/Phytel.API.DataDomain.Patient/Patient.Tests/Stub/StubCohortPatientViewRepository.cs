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

        public List<DTO.PatientData> Select(List<string> patientIds)
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

        public DTO.PutPatientSystemIdDataResponse UpdatePatientSystem(DTO.PutPatientSystemIdDataRequest request)
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

        public List<DTO.PatientUserData> FindPatientUsersByPatientId(string patientId)
        {
            throw new NotImplementedException();
        }

        public DTO.CohortPatientViewData FindCohortPatientViewByPatientId(string patientId)
        {
            throw new NotImplementedException();
        }

        public object Initialize(object newEntity)
        {
            throw new NotImplementedException();
        }

        public object FindDuplicatePatient(DTO.PutUpdatePatientDataRequest request)
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

        public void UndoDelete(object entity)
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
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public bool SyncContact(DTO.SyncPatientInfoDataRequest request)
        {
            throw new NotImplementedException();
        }


        public bool SyncPatient(DTO.SyncPatientInfoDataRequest request)
        {
            throw new NotImplementedException();
        }


        public bool AddPCMToPatientCohortView(DTO.AddPCMToCohortPatientViewDataRequest request)
        {
            throw new NotImplementedException();
        }


        public bool RemovePCMFromCohortPatientView(DTO.RemovePCMFromCohortPatientViewDataRequest request)
        {
            throw new NotImplementedException();
        }


        public bool AddContactsToCohortPatientView(DTO.AssignContactsToCohortPatientViewDataRequest request)
        {
            throw new NotImplementedException();
        }

        public object FindByNameDOB(string firstName, string lastName, string dob)
        {
            throw new NotImplementedException();
        }
    }
}
