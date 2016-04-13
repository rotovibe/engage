using System.Collections.Generic;
using Phytel.API.DataDomain.Contact.DTO.CareTeam;

namespace Phytel.API.DataDomain.Contact.CareTeam
{
    public interface ICareTeamManager
    {
        SaveCareTeamDataResponse InsertCareTeam(SaveCareTeamDataRequest request);
        UpdateCareTeamMemberDataResponse UpdateCareTeamMember(UpdateCareTeamMemberDataRequest request);
        GetCareTeamDataResponse GetCareTeam(GetCareTeamDataRequest request);

    }
}
