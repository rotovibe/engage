using Phytel.API.DataDomain.LookUp.DTO;
using System.Collections.Generic;
using System.Linq;

namespace Phytel.API.DataDomain.LookUp
{
    public static class LookUpDataManager
    {
        private static readonly string CONDITIONLOOKUP = "condition";

        public static ConditionResponse GetConditionByID(GetConditionRequest request)
        {
            ConditionResponse response = new ConditionResponse();

            ILookUpRepository<ConditionResponse> repo = Phytel.API.DataDomain.LookUp.LookUpRepositoryFactory<ConditionResponse>.GetLookUpRepository(request.ContractNumber, request.Context, CONDITIONLOOKUP);
            response = repo.FindByID(request.ConditionID) as ConditionResponse;
            return response;
        }

        public static ConditionsResponse FindConditions(FindConditionsRequest request)
        {
            ConditionsResponse response = new ConditionsResponse();

            ILookUpRepository<Condition> repo = Phytel.API.DataDomain.LookUp.LookUpRepositoryFactory<Condition>.GetLookUpRepository(request.ContractNumber, request.Context, CONDITIONLOOKUP);
            IQueryable<Condition> conditions = repo.SelectAll() as IQueryable<Condition>;

            if (conditions != null)
            {
                response.Conditions = conditions.ToList();
            }
            return response;
        }
    }
}   
