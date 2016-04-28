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

        public List<PatientData> GetPatients(DTO.GetPatientsDataRequest request)
        {
            IPatientRepository repo = Factory.GetRepository(request, RepositoryType.Patient);
            List<PatientData> response = repo.Select(request.PatientIds);
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


        public UndoDeletePatientDataResponse UndoDeletePatient(UndoDeletePatientDataRequest request)
        {
            throw new NotImplementedException();
        }

        public UndoDeletePatientUsersDataResponse UndoDeletePatientUserByPatientId(UndoDeletePatientUsersDataRequest request)
        {
            throw new NotImplementedException();
        }

        public UndoDeleteCohortPatientViewDataResponse UndoDeleteCohortPatientViewByPatientId(UndoDeleteCohortPatientViewDataRequest request)
        {
            throw new NotImplementedException();
        }

        GetCohortPatientsDataResponse IPatientDataManager.GetCohortPatients(GetCohortPatientsDataRequest request)
        {
            throw new NotImplementedException();
        }

        GetCohortPatientViewResponse IPatientDataManager.GetCohortPatientView(GetCohortPatientViewRequest request)
        {
            throw new NotImplementedException();
        }

        GetPatientDataResponse IPatientDataManager.GetPatientByID(GetPatientDataRequest request)
        {
            throw new NotImplementedException();
        }

        GetPatientsDataResponse IPatientDataManager.GetPatients(GetPatientsDataRequest request)
        {
            throw new NotImplementedException();
        }

        GetPatientSSNDataResponse IPatientDataManager.GetPatientSSN(GetPatientSSNDataRequest request)
        {
            throw new NotImplementedException();
        }

        PutCohortPatientViewDataResponse IPatientDataManager.InsertCohortPatientView(PutCohortPatientViewDataRequest request)
        {
            throw new NotImplementedException();
        }

        PutPatientDataResponse IPatientDataManager.InsertPatient(PutPatientDataRequest request)
        {
            throw new NotImplementedException();
        }

        PutUpdateCohortPatientViewResponse IPatientDataManager.UpdateCohortPatientViewProblem(PutUpdateCohortPatientViewRequest request)
        {
            throw new NotImplementedException();
        }

        PutUpdatePatientDataResponse IPatientDataManager.UpdatePatient(PutUpdatePatientDataRequest request)
        {
            throw new NotImplementedException();
        }

        PutPatientFlaggedResponse IPatientDataManager.UpdatePatientFlagged(PutPatientFlaggedRequest request)
        {
            throw new NotImplementedException();
        }

        PutPatientPriorityResponse IPatientDataManager.UpdatePatientPriority(PutPatientPriorityRequest request)
        {
            throw new NotImplementedException();
        }

        DeletePatientDataResponse IPatientDataManager.DeletePatient(DeletePatientDataRequest request)
        {
            throw new NotImplementedException();
        }

        DeletePatientUserByPatientIdDataResponse IPatientDataManager.DeletePatientUserByPatientId(DeletePatientUserByPatientIdDataRequest request)
        {
            throw new NotImplementedException();
        }

        DeleteCohortPatientViewDataResponse IPatientDataManager.DeleteCohortPatientViewByPatientId(DeleteCohortPatientViewDataRequest request)
        {
            throw new NotImplementedException();
        }

        UndoDeletePatientDataResponse IPatientDataManager.UndoDeletePatient(UndoDeletePatientDataRequest request)
        {
            throw new NotImplementedException();
        }

        UndoDeletePatientUsersDataResponse IPatientDataManager.UndoDeletePatientUser(UndoDeletePatientUsersDataRequest request)
        {
            throw new NotImplementedException();
        }

        UndoDeleteCohortPatientViewDataResponse IPatientDataManager.UndoDeleteCohortPatientView(UndoDeleteCohortPatientViewDataRequest request)
        {
            throw new NotImplementedException();
        }


        public PutInitializePatientDataResponse InitializePatient(PutInitializePatientDataRequest request)
        {
            throw new NotImplementedException();
        }


        public PutPatientSystemIdDataResponse UpdatePatientSystem(PutPatientSystemIdDataRequest request)
        {
            throw new NotImplementedException();
        }


        public List<PatientData> GetAllPatients(GetAllPatientsDataRequest request)
        {
            throw new NotImplementedException();
        }


        public InsertBatchPatientsDataResponse InsertBatchPatients(InsertBatchPatientsDataRequest request)
        {
            throw new NotImplementedException();
        }


        public SyncPatientInfoDataResponse SyncPatient(SyncPatientInfoDataRequest request)
        {
            throw new NotImplementedException();
        }


        public AddPCMToCohortPatientViewDataResponse AddPcmToCohortPatientView(AddPCMToCohortPatientViewDataRequest request)
        {
            throw new NotImplementedException();
        }


        public RemovePCMFromCohortPatientViewDataResponse RemovePcmFromCohortPatientView(RemovePCMFromCohortPatientViewDataRequest request)
        {
            throw new NotImplementedException();
        }


        public AssignContactsToCohortPatientViewDataResponse AssignContactsToCohortPatientView(AssignContactsToCohortPatientViewDataRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
