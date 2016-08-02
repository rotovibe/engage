using System;
using System.Collections.Generic;
using Atmosphere.Core.Data;
using C3.Data;

namespace C3.Business.Interfaces
{
    public interface IOrgFunctionService
    {
        List<OrgFunction> GetContractOrgFunctions(int contractId);

        List<OrgFunction> GetUserContractOrgFunctions(Guid userId, int contractId);

        void SaveUserContractOrgFunctions(Guid userId, int contractId, List<OrgFunction> orgFunctions);
    }
}