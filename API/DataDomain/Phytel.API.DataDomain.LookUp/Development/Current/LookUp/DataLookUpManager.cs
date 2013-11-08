using Phytel.API.DataDomain.LookUp.DTO;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.LookUp
{
    public static class LookUpDataManager
    {
        private static readonly string CONDITIONLOOKUP = "condition";
        
        public static ConditionResponse GetConditionByID(GetConditionRequest request)
        {
            ConditionResponse result = new ConditionResponse();

            ILookUpRepository<ConditionResponse> repo = Phytel.API.DataDomain.LookUp.LookUpRepositoryFactory<ConditionResponse>.GetLookUpRepository(request.ContractNumber, request.Context, CONDITIONLOOKUP);
            MECondition meCondition =  repo.FindByID(request.ConditionID) as MECondition;

            if (meCondition != null)
            {
                result.ConditionID = meCondition.Id.ToString();
                result.DisplayName = meCondition.DisplayName;
                result.IsActive = meCondition.IsActive;
            }

            return result;
        }

        public static List<ConditionResponse> GetConditions(FindConditionsRequest request)
        {
            List<ConditionResponse> results = new List<ConditionResponse>();

            ILookUpRepository<ConditionResponse> repo = Phytel.API.DataDomain.LookUp.LookUpRepositoryFactory<ConditionResponse>.GetLookUpRepository(request.ContractNumber, request.Context, CONDITIONLOOKUP);
            List<MECondition> meConditions = (List<MECondition>)repo.SelectAll();

            foreach(MECondition m in meConditions)
            {
                ConditionResponse meCondition = new ConditionResponse
                { 
                    ConditionID = m.Id.ToString(),
                    DisplayName = m.DisplayName,
                    IsActive = m.IsActive
                };
                results.Add(meCondition);
            }

            return results;
        }
    }
}   
