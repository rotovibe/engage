using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Patient
{
    public interface IPatientRepository : IRepository
    {
        // these need to be refactored to either conform to the IRepository interface or get refactored up to the appdomain.
        List<PatientData> Select(List<string> patientIds);
        List<PatientData> Select(string query, string[] filterData, string querySort, int skip, int take);
        PutPatientPriorityResponse UpdatePriority(PutPatientPriorityRequest request);
        PutPatientFlaggedResponse UpdateFlagged(PutPatientFlaggedRequest request);
        PutPatientSystemIdDataResponse UpdatePatientSystem(PutPatientSystemIdDataRequest request);
        object FindByID(string patientId, string userId);
        object Update(PutUpdatePatientDataRequest request);
        object GetSSN(string patientId);
        List<PatientUserData> FindPatientUsersByPatientId(string patientId);
        CohortPatientViewData FindCohortPatientViewByPatientId(string patientId);
        object Initialize(object newEntity);
        object FindDuplicatePatient(PutUpdatePatientDataRequest request);
        bool SyncPatient(SyncPatientInfoDataRequest request);
        bool AddPCMToPatientCohortView(AddPCMToCohortPatientViewDataRequest request);
        bool RemovePCMFromCohortPatientView(RemovePCMFromCohortPatientViewDataRequest request);
        bool AddContactsToCohortPatientView(AssignContactsToCohortPatientViewDataRequest request);
    }
}
