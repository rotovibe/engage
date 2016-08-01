using System.Collections.Generic;
using Phytel.API.DataDomain.PatientNote.DTO;
using Phytel.API.DataDomain.PatientNote.Repo;

namespace Phytel.API.DataDomain.PatientNote
{
    public interface IDataPatientUtilizationManager
    {
        PatientUtilizationData InsertPatientUtilization(PatientUtilizationData data);
        PatientUtilizationData UpdatePatientUtilization(PatientUtilizationData data);
        List<PatientUtilizationData> GetPatientUtilizations(string patientId);
        PatientUtilizationData GetPatientUtilization(string utilId);
        bool DeletePatientUtilization(string utilId);
        UndoDeletePatientUtilizationsDataResponse UndoDeletePatientPatientUtilizations(UndoDeletePatientUtilizationsDataRequest request);
        DeleteUtilizationByPatientIdDataResponse DeletePatientUtilizationsByPatientId(DeleteUtilizationsByPatientIdDataRequest request);
    }
}