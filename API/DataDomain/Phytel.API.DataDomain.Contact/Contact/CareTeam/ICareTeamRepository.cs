using System.Collections.Generic;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.Contact.DTO.CareTeam;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Contact.CareTeam
{
    public interface ICareTeamRepository : IRepository
    {
        object GetCareTeamByContactId(string contactId);
        bool UpdateCareTeamMember(object entity);
        void DeleteCareTeamMember(object entity);
        bool CareTeamExist(string careTeamId);
        bool ContactCareTeamExist(string contactId);
        bool CareTeamMemberExist(string careTeamId, string memberId);
        string AddCareTeamMember(AddCareTeamMemberDataRequest request);
        bool CareTeamMemberContactExist(string careTeamId, string contactId);
        IEnumerable<CareTeamData> GetCareTeamsByContactIds(List<string> contactIds);
    }
}
