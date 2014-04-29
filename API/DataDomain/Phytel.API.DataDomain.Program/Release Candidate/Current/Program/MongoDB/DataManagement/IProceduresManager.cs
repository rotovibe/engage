using Phytel.API.DataDomain.Program.DTO;
using System;
namespace Phytel.API.DataDomain.Program.MongoDB.DataManagement
{
    public interface IProceduresManager
    {
        PostMongoProceduresResponse ExecuteProcedure(PostMongoProceduresRequest request);
    }
}
