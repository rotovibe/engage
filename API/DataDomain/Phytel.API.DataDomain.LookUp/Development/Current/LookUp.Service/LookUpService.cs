using System.Collections.Generic;
using Phytel.API.DataDomain.LookUp;
using Phytel.API.DataDomain.LookUp.DTO;

namespace Phytel.API.DataDomain.Patient.Service
{
    public class LookUpService : ServiceStack.ServiceInterface.Service
    {
        public ConditionResponse Get(GetConditionRequest request)
        {
            ConditionResponse response =  LookUpDataManager.GetConditionByID(request);
            return response;
        }

        public List<ConditionResponse> Get(FindConditionsRequest request)
        {
            List<ConditionResponse> response = LookUpDataManager.GetConditions(request);
            return response;
        }
    }
}