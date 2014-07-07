using Phytel.API.DataDomain.Program.DTO;
using System;
using System.Collections.Generic;
namespace Phytel.API.DataDomain.PatientSystem.MongoDB.DataManagement
{
    public interface IProceduresManager
    {
        GetMongoProceduresResponse ExecuteProcedure(GetMongoProceduresRequest request);
        GetMongoProceduresListResponse GetProceduresList(GetMongoProceduresListRequest request);
        List<T> GetProcedureConstValues<T>(Type type);
    }
}
