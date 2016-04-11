using System.Collections.Generic;
using Phytel.API.DataDomain.Contact.DTO.CareTeam;

namespace Phytel.API.DataDomain.Contact.CareTeam
{
    public interface ICareTeamManager
    {
        InsertCareTeamDataResponse InsertCareTeam(InsertCareTeamDataRequest request);
    }
}
