using System.Collections.Generic;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.Contact.DTO.CareTeam;

namespace Phytel.API.DataDomain.Contact.CareTeam
{
    public interface ICareTeamManager
    {
        SaveCareTeamDataResponse InsertCareTeam(SaveCareTeamDataRequest request);
        UpdateCareTeamMemberDataResponse UpdateCareTeamMember(UpdateCareTeamMemberDataRequest request);
        GetCareTeamDataResponse GetCareTeam(GetCareTeamDataRequest request);
        void DeleteCareTeamMember(DeleteCareTeamMemberDataRequest request);
        DeleteCareTeamDataResponse DeleteCareTeam(DeleteCareTeamDataRequest request);
        UndoDeleteCareTeamDataResponse UndoDeleteCareTeam(UndoDeleteCareTeamDataRequest request);
        AddCareTeamMemberDataResponse AddCareTeamMember(AddCareTeamMemberDataRequest request);

    }
}
