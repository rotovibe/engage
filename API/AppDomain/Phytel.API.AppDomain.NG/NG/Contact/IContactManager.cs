using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;

namespace Phytel.API.AppDomain.NG
{
    public interface IContactManager
    {
        void LogException(Exception ex);

        #region Contact
        Contact GetContactByContactId(GetContactByContactIdRequest request);
        List<Contact> GetCareManagers(GetAllCareManagersRequest request);

            #endregion

        #region CareTeam
        CareTeam GetCareTeam(GetCareTeamRequest request);
        SaveCareTeamResponse SaveCareTeam(SaveCareTeamRequest request);
        UpdateCareTeamMemberResponse UpdateCareTeamMember(UpdateCareTeamMemberRequest request);
        DeleteCareTeamMemberResponse DeleteCareTeamMember(DeleteCareTeamMemberRequest request);
        AddCareTeamMemberResponse AddCareTeamMember(AddCareTeamMemberRequest request);

        #endregion
    }
}
