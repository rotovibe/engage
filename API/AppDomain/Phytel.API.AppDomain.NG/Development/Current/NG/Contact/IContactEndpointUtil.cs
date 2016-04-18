﻿using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.Contact.DTO.CareTeam;

namespace Phytel.API.AppDomain.NG
{
    public interface IContactEndpointUtil
    {
        #region Contact
        ContactData GetContactByContactId(GetContactByContactIdRequest request);
        List<ContactData> GetContactsByContactIds(List<string> contactIds, double version, string contractNumber,string userId);
        #endregion

        #region CareTeam
        CareTeamData GetCareTeam(GetCareTeamRequest request);
        SaveCareTeamDataResponse SaveCareTeam(SaveCareTeamRequest request);
        DeleteCareTeamMemberDataResponse DeleteCareTeamMember(DeleteCareTeamMemberRequest request);
        UpdateCareTeamMemberResponse UpdateCareTeamMember(UpdateCareTeamMemberRequest request);

        #endregion

    }
}
