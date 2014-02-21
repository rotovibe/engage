using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO.Observation;
using ServiceStack.ServiceClient.Web;
using Phytel.API.AppDomain.NG.PlanSpecification;
using Phytel.API.AppDomain.NG.PlanCOR;
using ServiceStack.Service;
using DD = Phytel.API.DataDomain.Program.DTO;
using System.Configuration;
using Phytel.API.DataDomain.PatientGoal.DTO;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.PatientObservation.DTO;

namespace Phytel.API.AppDomain.NG.Observation
{
    public class ObservationsManager : ManagerBase
    {
        public GetInitializeObservationResponse GetInitialGoalRequest(GetInitializeObservationRequest request)
        {
            try
            {
                GetInitializeObservationResponse response = new GetInitializeObservationResponse();
                PatientObservationData po = (PatientObservationData)ObservationsUtil.GetInitialObservationRequest(request);
                response.Observation = ObservationsUtil.GetObservationForInitialize(request, po);
                response.Version = request.Version;
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
