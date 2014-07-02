using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Patient.DTO;

namespace Phytel.API.DataDomain.Patient.Test.Stub
{
    public class StubDataPatientManager : IPatientDataManager
    {
        public IPatientRepositoryFactory Factory { get; set; }
        
        public DTO.GetCohortPatientsDataResponse GetCohortPatients(DTO.GetCohortPatientsDataRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.GetCohortPatientViewResponse GetCohortPatientView(DTO.GetCohortPatientViewRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.GetPatientDataResponse GetPatientByID(DTO.GetPatientDataRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.GetPatientsDataResponse GetPatients(DTO.GetPatientsDataRequest request)
        {
            GetPatientsDataResponse response = new GetPatientsDataResponse();
            IPatientRepository repo = Factory.GetRepository(request, RepositoryType.Patient);
            response = repo.Select(request.PatientIds);
            return response;
        }

        public DTO.GetPatientSSNDataResponse GetPatientSSN(DTO.GetPatientSSNDataRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.PutCohortPatientViewDataResponse InsertCohortPatientView(DTO.PutCohortPatientViewDataRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.PutPatientDataResponse InsertPatient(DTO.PutPatientDataRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.PutUpdateCohortPatientViewResponse UpdateCohortPatientViewProblem(DTO.PutUpdateCohortPatientViewRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.PutUpdatePatientDataResponse UpdatePatient(DTO.PutUpdatePatientDataRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.PutPatientBackgroundDataResponse UpdatePatientBackground(DTO.PutPatientBackgroundDataRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.PutPatientFlaggedResponse UpdatePatientFlagged(DTO.PutPatientFlaggedRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.PutPatientPriorityResponse UpdatePatientPriority(DTO.PutPatientPriorityRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.DeletePatientDataResponse DeletePatient(DTO.DeletePatientDataRequest request)
        {
            DeletePatientDataResponse response = new DeletePatientDataResponse();
            IPatientRepository repo = Factory.GetRepository(request, RepositoryType.Patient);
            repo.Delete(request.Id);
            return response;
        }

        public DTO.DeletePatientUserByPatientIdDataResponse DeletePatientUserByPatientId(DTO.DeletePatientUserByPatientIdDataRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.DeleteCohortPatientViewDataResponse DeleteCohortPatientViewByPatientId(DTO.DeleteCohortPatientViewDataRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
