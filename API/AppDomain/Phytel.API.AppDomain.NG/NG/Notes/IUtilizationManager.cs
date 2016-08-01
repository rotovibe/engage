using System;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Utilization;

namespace Phytel.API.AppDomain.NG.Notes
{
    public interface IUtilizationManager
    {
        GetPatientUtilizationResponse GetPatientUtilization(GetPatientUtilizationRequest request);
        DeletePatientUtilizationResponse DeletePatientUtilization(DeletePatientUtilizationRequest request);
        GetPatientUtilizationsResponse GetPatientUtilizations(GetPatientUtilizationsRequest request);
        PostPatientUtilizationResponse InsertPatientUtilization(PostPatientUtilizationRequest request);
        PutPatientUtilizationResponse UpdatePatientUtilization(PutPatientUtilizationRequest request);
        void LogException(Exception ex);
    }
}