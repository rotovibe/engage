﻿using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Contact.CareTeam
{
    public interface ICareTeamRepository : IRepository
    {
        object GetCareTeamByContactId(string contactId);
        bool UpdateCareTeamMember(object entity);
        bool DeleteCareTeamMember(object entity);
    }
}
