using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO.Utilization;
using Phytel.API.AppDomain.NG.Notes;

namespace Phytel.API.AppDomain.NG.Test.Stubs
{
    public class StubUtilsManager : IUtilizationManager
    {
        public DTO.Utilization.GetPatientUtilizationResponse GetPatientUtilization(DTO.Utilization.GetPatientUtilizationRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.DeletePatientUtilizationResponse DeletePatientUtilization(DTO.DeletePatientUtilizationRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.Utilization.GetPatientUtilizationsResponse GetPatientUtilizations(DTO.Utilization.GetPatientUtilizationsRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.Utilization.PostPatientUtilizationResponse InsertPatientUtilization(DTO.Utilization.PostPatientUtilizationRequest request)
        {
            var resp = new PostPatientUtilizationResponse {
             Result = true};
            return resp;
        }

        public DTO.Utilization.PutPatientUtilizationResponse UpdatePatientUtilization(DTO.Utilization.PutPatientUtilizationRequest request)
        {
            throw new NotImplementedException();
        }

        public void LogException(Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
