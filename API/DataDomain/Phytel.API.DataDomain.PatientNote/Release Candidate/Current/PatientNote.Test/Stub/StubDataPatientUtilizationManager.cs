using System;
using System.Collections.Generic;
using Phytel.API.DataDomain.PatientNote.DTO;
using Phytel.API.DataDomain.PatientNote.Repo;

namespace Phytel.API.DataDomain.PatientNote.Test.Stub
{
    public class StubDataPatientUtilizationManager : IDataPatientUtilizationManager
    {
        public PatientUtilizationData InsertPatientUtilization(PatientUtilizationData data)
        {
            return new PatientUtilizationData();
        }

        public PatientUtilizationData UpdatePatientUtilization(PatientUtilizationData data)
        {
            return new PatientUtilizationData();
        }

        public List<PatientUtilizationData> GetPatientUtilizations(string userId)
        {
            return new List<PatientUtilizationData>{ TestInit.PatientUtilizationData };
        }

        public PatientUtilizationData GetPatientUtilization(string utilId)
        {
            return TestInit.PatientUtilizationData;
        }

        public bool DeletePatientUtilization(string utilId)
        {
            return true;
        }

        public IMongoPatientNoteRepository Repository
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



        public UndoDeletePatientUtilizationsDataResponse UndoDeletePatientPatientUtilizations(UndoDeletePatientUtilizationsDataRequest request)
        {
            throw new NotImplementedException();
        }

        public DeleteUtilizationByPatientIdDataResponse DeletePatientUtilizationsByPatientId(DeleteUtilizationsByPatientIdDataRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
